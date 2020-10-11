#region Usings

using System.Drawing;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.Windows.Debug
{
    public class ManualThrowPanel
    {
        private readonly DetectionService detectionService;
        private readonly Logger logger;
        private readonly ManualThrowPanelWindow manualThrowPanelWindow;
        public MakeManualThrowCommand MakeManualThrowCommand { get; }

        public ManualThrowPanel()
        {
        }

        public ManualThrowPanel(Logger logger, DetectionService detectionService)
        {
            this.logger = logger;
            this.detectionService = detectionService;
            MakeManualThrowCommand = new MakeManualThrowCommand(ThrowDartTo);
            manualThrowPanelWindow = new ManualThrowPanelWindow
                                     {
                                         DartboardImage = {Source = Converter.BitmapToBitmapImage(Resources.Resources.Dartboard)},
                                         DataContext = this
                                     };
        }

        public void HidePanel()
        {
            manualThrowPanelWindow.Hide();
        }

        public void ShowPanel()
        {
            manualThrowPanelWindow.Show();
        }

        public void ClosePanel()
        {
            manualThrowPanelWindow?.Close();
        }

        private void ThrowDartTo(string str)
        {
            var point = new PointF(0.0f, 0.0f);
            var side = 0;
            ThrowType type;
            var sector = Converter.ToInt(str.Substring(str.IndexOf("_") + 1));
            if (str.Contains("Tremble"))
            {
                type = ThrowType.Tremble;
            }
            else if (str.Contains("Double"))
            {
                type = ThrowType.Double;
            }
            else if (str.Contains("Single"))
            {
                type = ThrowType.Single;
            }
            else if (str.Contains("_25"))
            {
                type = ThrowType._25;
            }
            else if (str.Contains("Bull"))
            {
                type = ThrowType.Bull;
            }
            else
            {
                type = ThrowType.Zero;
            }

            detectionService.InvokeOnThrowDetected(new DetectedThrow(point,
                                                                     null,
                                                                     null,
                                                                     sector,
                                                                     type,
                                                                     side),
                                                   true);
            if (!App.ThrowPanel)
            {
                HidePanel();
            }
        }
    }
}