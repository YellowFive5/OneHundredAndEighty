#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DirectShowLib;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Windows.CamsDetection;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public enum DetectionServiceStatus
    {
        WaitingThrow,
        ProcessingThrow,
        DartsExtraction
    }

    public enum DetectionServiceWorkingMode
    {
        Setup,
        Check,
        Crossing,
        Detection
    }

    public class DetectionService : IDetectionService
    {
        private readonly DrawService drawService;
        private readonly ConfigService configService;
        private readonly ThrowService throwService;
        private readonly Logger logger;
        private readonly CamsDetectionBoard camsDetectionBoard;

        private List<CamService> cams;
        private CancellationTokenSource cts;
        private CancellationToken cancelToken;
        private double moveDetectedSleepTime;
        private double extractionSleepTime;
        private double thresholdSleepTime;
        private bool withDetection;
        private DetectionServiceWorkingMode workingMode;

        public DetectionService(DrawService drawService,
                                ConfigService configService,
                                ThrowService throwService,
                                Logger logger,
                                CamsDetectionBoard camsDetectionBoard)
        {
            this.drawService = drawService;
            this.configService = configService;
            this.throwService = throwService;
            this.logger = logger;
            this.camsDetectionBoard = camsDetectionBoard;
        }

        public delegate void ThrowDetectedDelegate(DetectedThrow thrw);

        public event ThrowDetectedDelegate OnThrowDetected;

        public delegate void ExceptionOccurredDelegate(Exception ex);

        public event ExceptionOccurredDelegate OnErrorOccurred;

        public delegate void StatusDelegate(DetectionServiceStatus status);

        public event StatusDelegate OnStatusChanged;

        public void CheckCamsAndTryCapture(List<CamService> camsList)
        {
            foreach (var cam in camsList)
            {
                cam.TryQueryFrame();
            }
        }

        public async void RunDetection(List<CamService> camsList,
                                       DetectionServiceWorkingMode workingMode)
        {
            cams = camsList;
            this.workingMode = workingMode;
            cts = new CancellationTokenSource();
            cancelToken = cts.Token;

            extractionSleepTime = configService.Read<double>(SettingsType.ExtractionSleepTime);
            thresholdSleepTime = configService.Read<double>(SettingsType.ThresholdSleepTime);
            moveDetectedSleepTime = configService.Read<double>(SettingsType.MoveDetectedSleepTime);
            withDetection = configService.Read<bool>(SettingsType.WithDetectionCheckBox) && !App.NoCams;

            try
            {
                await Task.Run(() =>
                               {
                                   OnStatusChanged?.Invoke(DetectionServiceStatus.WaitingThrow);

                                   Thread.CurrentThread.Name = $"Recognition_workerThread";

                                   cams.ForEach(c => c.DoCaptures());

                                   while (!cancelToken.IsCancellationRequested)
                                   {
                                       foreach (var cam in cams)
                                       {
                                           logger.Debug($"Cam_{cam.camNumber} detection start");

                                           ResponseType response;
                                           if (workingMode == DetectionServiceWorkingMode.Crossing)
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

                                               if (workingMode != DetectionServiceWorkingMode.Crossing)
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

                                                   // drawService.ProjectionClear();
                                                   cams.ForEach(c => c.DoCaptures());

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
                                                    // c.ClearImageBoxes();
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

        public void InvokeOnThrowDetected(DetectedThrow thrw)
        {
            OnThrowDetected?.Invoke(thrw);
        }

        public string FindConnectedCams()
        {
            var allCams = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice).ToList();
            var str = new StringBuilder();
            for (var i = 0; i < allCams.Count; i++)
            {
                var cam = allCams[i];
                var camId = cam.DevicePath.Substring(44, 10);
                str.AppendLine($"[{cam.Name}]-[ID:'{camId}']");
            }

            return allCams.Count == 0
                       ? "No cameras found"
                       : str.ToString();
        }
    }
}