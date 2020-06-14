#region Usings

using System.Collections.Generic;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public interface IDetectionService
    {
        string FindConnectedCams();
        void CheckCamsAndTryCapture(List<CamService> camsList);
        void RunDetection(List<CamService> camsList,DetectionServiceWorkingMode workingMode);
        void StopDetection();

        public event DetectionService.ExceptionOccurredDelegate OnErrorOccurred;
    }
}