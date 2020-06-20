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
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Enums;
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

            extractionSleepTime = configService.ExtractionSleepTimeValue;
            thresholdSleepTime = configService.ThresholdSleepTimeValue;
            moveDetectedSleepTime = configService.MovesDetectedSleepTimeValue;
            withDetection = configService.DetectionEnabled && !App.NoCams;

            try
            {
                await Task.Run(() =>
                               {
                                   OnStatusChanged?.Invoke(DetectionServiceStatus.WaitingThrow);

                                   cams.ForEach(c => c.DoDetectionCaptures());

                                   while (!cancelToken.IsCancellationRequested)
                                   {
                                       Thread.Sleep(TimeSpan.FromSeconds(thresholdSleepTime));

                                       foreach (var cam in cams)
                                       {
                                           // todo new logic
                                       }
                                   }
                               });
            }
            catch (Exception e)
            {
                OnErrorOccurred?.Invoke(e);
            }
            finally
            {
                cts?.Cancel();
                throwService.ClearRays();
                cams.ForEach(c => { c.Dispose(); });
            }
        }

        public void StopDetection()
        {
            cts?.Cancel();
        }

        private void FindThrowOnRemainingCams(CamService succeededCam)
        {
            foreach (var cam in cams.Where(cam => cam != succeededCam))
            {
                // cam.FindThrow();
                cam.FindAndProcessDartContour();
            }

            var thrw = throwService.GetThrow();
            if (thrw != null)
            {
                InvokeOnThrowDetected(thrw);
            }
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