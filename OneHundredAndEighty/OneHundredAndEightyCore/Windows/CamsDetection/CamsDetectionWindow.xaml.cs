#region Usings

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Windows.CamsDetection
{
    public partial class CamsDetectionWindow
    {
        private readonly CamsDetectionBoard camsDetectionViewModel;
        private bool ForceClose { get; set; }

        public CamsDetectionWindow(WindowSettings settings, CamsDetectionBoard camsDetectionViewModel)

        {
            this.camsDetectionViewModel = camsDetectionViewModel;
            InitializeComponent();

            Left = settings.PositionLeft;
            Top = settings.PositionTop;
            Height = settings.Height;
            Width = settings.Width;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (!ForceClose)
            {
                e.Cancel = true;
            }
        }

        public new void Close()
        {
            ForceClose = true;
            base.Close();
        }

        private void UndoThrowButtonClick(object sender, RoutedEventArgs e)
        {
            camsDetectionViewModel.FireThrowUndo();
        }

        private void CorrectThrowButtonClick(object sender, RoutedEventArgs e)
        {
            camsDetectionViewModel.FireThrowCorrect();
        }

        public void SetImages(CamNumber camNumber,
                              BitmapImage image,
                              BitmapImage roiImage,
                              BitmapImage lastRoiImage)
        {
            switch (camNumber)
            {
                case CamNumber._1:
                    Cam1Image.Source = image;
                    Cam1RoiImage.Source = roiImage;
                    Cam1LastRoiImage.Source = lastRoiImage;
                    break;
                case CamNumber._2:
                    Cam2Image.Source = image;
                    Cam2RoiImage.Source = roiImage;
                    Cam2LastRoiImage.Source = lastRoiImage;
                    break;
                case CamNumber._3:
                    Cam3Image.Source = image;
                    Cam3RoiImage.Source = roiImage;
                    Cam3LastRoiImage.Source = lastRoiImage;
                    break;
                case CamNumber._4:
                    Cam4Image.Source = image;
                    Cam4RoiImage.Source = roiImage;
                    Cam4LastRoiImage.Source = lastRoiImage;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(camNumber), camNumber, null);
            }
        }
    }
}