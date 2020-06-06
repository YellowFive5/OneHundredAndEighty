#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Windows.Main;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public enum DetectionServiceStatus
    {
        WaitingThrow,
        ProcessingThrow,
        DartsExtraction
    }

    public class DetectionService
    {
        private readonly MainWindow mainWindow;
        private readonly DrawService drawService;
        private readonly ConfigService configService;
        private readonly ThrowService throwService;
        private readonly Logger logger;
        private List<CamService> cams;
        private CancellationTokenSource cts;
        private CancellationToken cancelToken;
        private double moveDetectedSleepTime;
        private double extractionSleepTime;
        private double thresholdSleepTime;
        private bool withDetection;
        private CamServiceWorkingMode workingMode;

        public DetectionService(MainWindow mainWindow,
                                DrawService drawService,
                                ConfigService configService,
                                ThrowService throwService,
                                Logger logger)
        {
            this.mainWindow = mainWindow;
            this.drawService = drawService;
            this.configService = configService;
            this.throwService = throwService;
            this.logger = logger;
        }

        public delegate void ThrowDetectedDelegate(DetectedThrow thrw);

        public event ThrowDetectedDelegate OnThrowDetected;

        public delegate void ExceptionOccurredDelegate(Exception ex);

        public event ExceptionOccurredDelegate OnErrorOccurred;

        public delegate void StatusDelegate(DetectionServiceStatus status);

        public event StatusDelegate OnStatusChanged;

        public void PrepareCamsAndTryCapture(CamServiceWorkingMode workingMode = CamServiceWorkingMode.Detection)
        {
            this.workingMode = workingMode;
            cams = new List<CamService>();
            var cam1Active = mainWindow.Cam1CheckBox.IsChecked.Value && !App.NoCams;
            var cam2Active = mainWindow.Cam2CheckBox.IsChecked.Value && !App.NoCams;
            var cam3Active = mainWindow.Cam3CheckBox.IsChecked.Value && !App.NoCams;
            var cam4Active = mainWindow.Cam4CheckBox.IsChecked.Value && !App.NoCams;

            if (cam1Active)
            {
                cams.Add(new CamService(mainWindow, mainWindow.Cam1Grid.Name, workingMode)); // todo extenstion method... or not
            }

            if (cam2Active)
            {
                cams.Add(new CamService(mainWindow, mainWindow.Cam2Grid.Name, workingMode));
            }

            if (cam3Active)
            {
                cams.Add(new CamService(mainWindow, mainWindow.Cam3Grid.Name, workingMode));
            }

            if (cam4Active)
            {
                cams.Add(new CamService(mainWindow, mainWindow.Cam4Grid.Name, workingMode));
            }

            cts = new CancellationTokenSource();
            cancelToken = cts.Token;

            extractionSleepTime = configService.Read<double>(SettingsType.ExtractionSleepTime);
            thresholdSleepTime = configService.Read<double>(SettingsType.ThresholdSleepTime);
            moveDetectedSleepTime = configService.Read<double>(SettingsType.MoveDetectedSleepTime);
            withDetection = configService.Read<bool>(SettingsType.WithDetectionCheckBox) && !App.NoCams;

            foreach (var cam in cams)
            {
                cam.TryQueryFrame();
            }
        }

        public async void RunDetection()
        {
            try
            {
                await Task.Run(() =>
                               {
                                   OnStatusChanged?.Invoke(DetectionServiceStatus.WaitingThrow);

                                   Thread.CurrentThread.Name = $"Recognition_workerThread";

                                   cams.ForEach(c => c.DoCapture(true));

                                   while (!cancelToken.IsCancellationRequested)
                                   {
                                       foreach (var cam in cams)
                                       {
                                           logger.Debug($"Cam_{cam.camNumber} detection start");

                                           ResponseType response;
                                           if (workingMode == CamServiceWorkingMode.Crossing)
                                           {
                                               response = ResponseType.Move;
                                           }
                                           else
                                           {
                                               response = withDetection
                                                              ? cam.DetectMove()
                                                              : ResponseType.Nothing;
                                           }

                                           if (response == ResponseType.Move)
                                           {
                                               OnStatusChanged?.Invoke(DetectionServiceStatus.ProcessingThrow);

                                               if (workingMode != CamServiceWorkingMode.Crossing)
                                               {
                                                   Thread.Sleep(TimeSpan.FromSeconds(moveDetectedSleepTime));
                                               }

                                               response = cam.DetectThrow();

                                               if (response == ResponseType.Trow)
                                               {
                                                   cam.FindAndProcessDartContour();

                                                   FindThrowOnRemainingCams(cam);

                                                   logger.Debug($"Cam_{cam.camNumber} detection end with response type '{ResponseType.Trow}'. Cycle break");

                                                   OnStatusChanged?.Invoke(DetectionServiceStatus.WaitingThrow);
                                                   break;
                                               }

                                               if (response == ResponseType.Extraction)
                                               {
                                                   OnStatusChanged?.Invoke(DetectionServiceStatus.DartsExtraction);
                                                   Thread.Sleep(TimeSpan.FromSeconds(extractionSleepTime));

                                                   drawService.ProjectionClear();
                                                   cams.ForEach(c => c.DoCapture(true));

                                                   logger.Debug($"Cam_{cam.camNumber} detection end with response type '{ResponseType.Extraction}'. Cycle break");
                                                   OnStatusChanged?.Invoke(DetectionServiceStatus.WaitingThrow);
                                                   break;
                                               }
                                           }

                                           Thread.Sleep(TimeSpan.FromSeconds(thresholdSleepTime));

                                           logger.Debug($"Cam_{cam.camNumber} detection end with response type '{ResponseType.Nothing}'");
                                       }
                                   }

                                   cams.ForEach(c =>
                                                {
                                                    c.Dispose();
                                                    c.ClearImageBoxes();
                                                });

                                   logger.Info($"Detection for {cams.Count} cams end. Cancellation requested");
                               });
            }
            catch (Exception e)
            {
                OnErrorOccurred?.Invoke(e);
                StopDetection();
            }
        }

        public void StopDetection()
        {
            cts?.Cancel();
        }

        public void InvokeOnThrowDetected(DetectedThrow thrw)
        {
            OnThrowDetected?.Invoke(thrw);
        }

        private void FindThrowOnRemainingCams(CamService succeededCam)
        {
            logger.Info($"Finding throws from remaining cams start. Succeeded cam: {succeededCam.camNumber}");

            foreach (var cam in cams.Where(cam => cam != succeededCam))
            {
                cam.FindThrow();
                cam.FindAndProcessDartContour();
            }

            var thrw = throwService.GetThrow();
            if (thrw != null)
            {
                InvokeOnThrowDetected(thrw);
            }

            logger.Info($"Finding throws from remaining cams end");
        }
    }
}