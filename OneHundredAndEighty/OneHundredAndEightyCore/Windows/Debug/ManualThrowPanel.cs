#region Usings

using System.Drawing;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.Windows.DebugPanel
{
    public class ManualThrowPanel
    {
        private readonly DetectionService detectionService;
        private readonly Logger logger;
        private readonly ManualThrowPanelWindow manualThrowPanelWindow;

        public ManualThrowPanel(Logger logger, DetectionService detectionService)
        {
            this.logger = logger;
            this.detectionService = detectionService;

            if (App.ThrowPanel)
            {
                manualThrowPanelWindow = new ManualThrowPanelWindow(this);
                manualThrowPanelWindow.DartboardImage.Source = Converter.BitmapToBitmapImage(Resources.Dartboard);

                manualThrowPanelWindow.Show();
            }
        }

        public void Close()
        {
            manualThrowPanelWindow?.Close();
        }

        public void ThrowDartTo(string multiplier, int sector)
        {
            var point = new PointF(0.0f, 0.0f);
            var side = 0;
            ThrowType type;

            if (multiplier.Contains("Tremble"))
            {
                type = ThrowType.Tremble;
            }
            else if (multiplier.Contains("Double"))
            {
                type = ThrowType.Double;
            }
            else if (multiplier.Contains("Single"))
            {
                type = ThrowType.Single;
            }
            else if (multiplier == "_25")
            {
                type = ThrowType._25;
            }
            else if (multiplier == "Bulleye")
            {
                type = ThrowType.Bulleye;
            }
            else
            {
                type = ThrowType.Zero;
            }

            detectionService.InvokeOnThrowDetected(new DetectedThrow(point,
                                                                     sector,
                                                                     type,
                                                                     side));
        }
    }
}