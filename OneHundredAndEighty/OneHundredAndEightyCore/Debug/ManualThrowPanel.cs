#region Usings

using NLog;
using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.Debug
{
    public class ManualThrowPanel
    {
        private readonly DetectionService detectionService;
        private readonly Logger logger;
        private ManualThrowPanelWindow manualThrowPanelWindow;

        public ManualThrowPanel(Logger logger, DetectionService detectionService)
        {
            this.logger = logger;
            this.detectionService = detectionService;

            if (App.Panel)
            {
                manualThrowPanelWindow = new ManualThrowPanelWindow();
                manualThrowPanelWindow.Show();
            }
        }

        public void Close()
        {
            manualThrowPanelWindow?.Close();
        }
    }
}