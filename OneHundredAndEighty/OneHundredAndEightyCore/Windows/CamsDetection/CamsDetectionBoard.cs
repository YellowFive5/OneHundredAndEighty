#region Usings

using System.Windows.Media.Imaging;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.Windows.CamsDetection
{
    public class CamsDetectionBoard
    {
        private readonly ConfigService configService;
        private readonly Logger logger;
        private CamsDetectionWindow camsDetectionWindow;
        public System.Windows.Threading.Dispatcher dispatcher; // todo weird and temp

        public delegate void UndoThrowButtonDelegate();

        public event UndoThrowButtonDelegate OnUndoThrowButtonPressed;

        public delegate void CorrectThrowButtonDelegate();

        public event CorrectThrowButtonDelegate OnCorrectThrowButtonPressed;

        public CamsDetectionBoard(ConfigService configService, Logger logger)
        {
            this.configService = configService;
            this.logger = logger;
        }

        public void SetCamImages(CamNumber camNumber, BitmapImage image, BitmapImage roiImage, BitmapImage lastRoiImage)
        {
            camsDetectionWindow.SetImages(camNumber, image, roiImage, lastRoiImage);
        }

        public void SetProjectionImage(BitmapImage image)
        {
            camsDetectionWindow.Dispatcher.Invoke(() => { camsDetectionWindow.ProjectionImage.Source = image; });
        }

        public void PrintThrow(DetectedThrow thrw)
        {
            camsDetectionWindow.PointsText.Text = thrw.ToString();
            camsDetectionWindow.PointsHistoryBox.Text = $"{camsDetectionWindow.PointsHistoryBox.Text}\n{thrw}";
            camsDetectionWindow.PointsHistoryBox.ScrollToEnd();
        }

        public void ClearPointsBox()
        {
            camsDetectionWindow.PointsText.Text = string.Empty;
        }

        public void ClearHistoryPointsBox()
        {
            camsDetectionWindow.PointsHistoryBox.Text = string.Empty;
        }

        public void Open()
        {
            var windowSettings = new WindowSettings(configService.Read<double>(SettingsType.CamsDetectionWindowHeight),
                                                    configService.Read<double>(SettingsType.CamsDetectionWindowWidth),
                                                    configService.Read<double>(SettingsType.CamsDetectionWindowPositionLeft),
                                                    configService.Read<double>(SettingsType.CamsDetectionWindowPositionTop));

            camsDetectionWindow = new CamsDetectionWindow(windowSettings, this);

            camsDetectionWindow.Show();
            dispatcher = camsDetectionWindow.Dispatcher;
        }

        public void Close()
        {
            if (camsDetectionWindow != null)
            {
                SaveSettings();
                camsDetectionWindow.Close();
            }
        }

        private void SaveSettings()
        {
            configService.Write(SettingsType.CamsDetectionWindowPositionLeft, camsDetectionWindow.Left);
            configService.Write(SettingsType.CamsDetectionWindowPositionTop, camsDetectionWindow.Top);
            configService.Write(SettingsType.CamsDetectionWindowHeight, camsDetectionWindow.Height);
            configService.Write(SettingsType.CamsDetectionWindowWidth, camsDetectionWindow.Width);
        }

        public void FireThrowUndo()
        {
            OnUndoThrowButtonPressed?.Invoke();
        }

        public void FireThrowCorrect()
        {
            OnCorrectThrowButtonPressed?.Invoke();
        }
    }
}