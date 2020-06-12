#region Usings

using System.Collections.Generic;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public interface IDetectionService
    {
        string FindConnectedCams();
        void PrepareCamsAndTryCapture(List<CamService> camsList, CamServiceWorkingMode workingMode);
        void RunDetection();
        void StopDetection();

        public event DetectionService.ExceptionOccurredDelegate OnErrorOccurred;
    }
}