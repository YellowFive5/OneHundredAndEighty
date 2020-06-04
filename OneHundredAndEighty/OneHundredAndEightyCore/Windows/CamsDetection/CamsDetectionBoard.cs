#region Usings

using System;
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

        public CamsDetectionBoard(ConfigService configService, Logger logger)
        {
            this.configService = configService;
            this.logger = logger;
        }

        public void SetCamImages(CamNumber camNumber, BitmapImage image, BitmapImage roiImage, BitmapImage lastRoiImage)
        {
            switch (camNumber)
            {
                case CamNumber._1:
                    camsDetectionWindow.Cam1Image.Source = image;
                    camsDetectionWindow.Cam1RoiImage.Source = roiImage;
                    camsDetectionWindow.Cam1LastRoiImage.Source = lastRoiImage;
                    break;
                case CamNumber._2:
                    camsDetectionWindow.Cam2Image.Source = image;
                    camsDetectionWindow.Cam2RoiImage.Source = roiImage;
                    camsDetectionWindow.Cam2LastRoiImage.Source = lastRoiImage;
                    break;
                case CamNumber._3:
                    camsDetectionWindow.Cam3Image.Source = image;
                    camsDetectionWindow.Cam3RoiImage.Source = roiImage;
                    camsDetectionWindow.Cam3LastRoiImage.Source = lastRoiImage;
                    break;
                case CamNumber._4:
                    camsDetectionWindow.Cam4Image.Source = image;
                    camsDetectionWindow.Cam4RoiImage.Source = roiImage;
                    camsDetectionWindow.Cam4LastRoiImage.Source = lastRoiImage;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(camNumber), camNumber, null);
            }
        }

        public void SetProjectionImage(BitmapImage image)
        {
            camsDetectionWindow.Dispatcher.Invoke(() => { camsDetectionWindow.ProjectionImage.Source = image; });
        }

        public void PrintThrow(DetectedThrow thrw)
        {
            camsDetectionWindow.PointsBox.Text = thrw.ToString();
            camsDetectionWindow.PointsHistoryBox.Text = $"{camsDetectionWindow.PointsHistoryBox.Text}\n{thrw}";
            camsDetectionWindow.PointsHistoryBox.ScrollToEnd();
        }

        public void ClearPointsBox()
        {
            camsDetectionWindow.PointsBox.Text = string.Empty;
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
    }
}