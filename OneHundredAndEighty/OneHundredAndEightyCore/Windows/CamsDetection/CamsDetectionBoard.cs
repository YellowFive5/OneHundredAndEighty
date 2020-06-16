#region Usings

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.Windows.CamsDetection
{
    public class CamsDetectionBoard : INotifyPropertyChanged
    {
        private CamsDetectionWindow camsDetectionWindow;
        private readonly Logger logger;
        private readonly DrawService drawService;
        private readonly ConfigService configService;

        public delegate void UndoThrowButtonDelegate();

        public event UndoThrowButtonDelegate OnUndoThrowButtonPressed;

        public delegate void CorrectThrowButtonDelegate();

        public event CorrectThrowButtonDelegate OnCorrectThrowButtonPressed;

        #region BindableProps

        #region Window position

        private double windowPositionLeft;

        public double WindowPositionLeft
        {
            get => windowPositionLeft;
            set
            {
                windowPositionLeft = value;
                OnPropertyChanged(nameof(WindowPositionLeft));
            }
        }

        private double windowPositionTop;

        public double WindowPositionTop
        {
            get => windowPositionTop;
            set
            {
                windowPositionTop = value;
                OnPropertyChanged(nameof(WindowPositionTop));
            }
        }

        private double windowHeight;

        public double WindowHeight
        {
            get => windowHeight;
            set
            {
                windowHeight = value;
                OnPropertyChanged(nameof(WindowHeight));
            }
        }

        private double windowWidth;

        public double WindowWidth
        {
            get => windowWidth;
            set
            {
                windowWidth = value;
                OnPropertyChanged(nameof(WindowWidth));
            }
        }

        #endregion

        #region Images

        private BitmapImage cam1Image;

        public BitmapImage Cam1Image
        {
            get => cam1Image;
            set
            {
                cam1Image = value;
                OnPropertyChanged(nameof(Cam1Image));
            }
        }

        private BitmapImage cam2Image;

        public BitmapImage Cam2Image
        {
            get => cam2Image;
            set
            {
                cam2Image = value;
                OnPropertyChanged(nameof(Cam2Image));
            }
        }

        private BitmapImage cam3Image;

        public BitmapImage Cam3Image
        {
            get => cam3Image;
            set
            {
                cam3Image = value;
                OnPropertyChanged(nameof(Cam3Image));
            }
        }

        private BitmapImage cam4Image;

        public BitmapImage Cam4Image
        {
            get => cam4Image;
            set
            {
                cam4Image = value;
                OnPropertyChanged(nameof(Cam4Image));
            }
        }

        private BitmapImage cam1RoiImage;

        public BitmapImage Cam1RoiImage
        {
            get => cam1RoiImage;
            set
            {
                cam1RoiImage = value;
                OnPropertyChanged(nameof(Cam1RoiImage));
            }
        }

        private BitmapImage cam2RoiImage;

        public BitmapImage Cam2RoiImage
        {
            get => cam2RoiImage;
            set
            {
                cam2RoiImage = value;
                OnPropertyChanged(nameof(Cam2RoiImage));
            }
        }

        private BitmapImage cam3RoiImage;

        public BitmapImage Cam3RoiImage
        {
            get => cam3RoiImage;
            set
            {
                cam3RoiImage = value;
                OnPropertyChanged(nameof(Cam3RoiImage));
            }
        }

        private BitmapImage cam4RoiImage;

        public BitmapImage Cam4RoiImage
        {
            get => cam4RoiImage;
            set
            {
                cam4RoiImage = value;
                OnPropertyChanged(nameof(Cam4RoiImage));
            }
        }

        private BitmapImage cam1LastRoiImage;

        public BitmapImage Cam1LastRoiImage
        {
            get => cam1LastRoiImage;
            set
            {
                cam1LastRoiImage = value;
                OnPropertyChanged(nameof(Cam1LastRoiImage));
            }
        }

        private BitmapImage cam2LastRoiImage;

        public BitmapImage Cam2LastRoiImage
        {
            get => cam2LastRoiImage;
            set
            {
                cam2LastRoiImage = value;
                OnPropertyChanged(nameof(Cam2LastRoiImage));
            }
        }

        private BitmapImage cam3LastRoiImage;

        public BitmapImage Cam3LastRoiImage
        {
            get => cam3LastRoiImage;
            set
            {
                cam3LastRoiImage = value;
                OnPropertyChanged(nameof(Cam3LastRoiImage));
            }
        }

        private BitmapImage cam4LastRoiImage;

        public BitmapImage Cam4LastRoiImage
        {
            get => cam4LastRoiImage;
            set
            {
                cam4LastRoiImage = value;
                OnPropertyChanged(nameof(Cam4LastRoiImage));
            }
        }

        private BitmapImage projectionImage;

        public BitmapImage ProjectionImage
        {
            get => projectionImage;
            set
            {
                projectionImage = value;
                OnPropertyChanged(nameof(ProjectionImage));
            }
        }

        #endregion

        private string throwText;

        public string ThrowText
        {
            get => throwText;
            set
            {
                throwText = value;
                OnPropertyChanged(nameof(ThrowText));
            }
        }

        private string throwsHistoryText;

        public string ThrowsHistoryText
        {
            get => throwsHistoryText;
            set
            {
                throwsHistoryText = value;
                OnPropertyChanged(nameof(ThrowsHistoryText));
            }
        }

        #endregion

        public CamsDetectionBoard()
        {
        }

        public CamsDetectionBoard(ConfigService configService,
                                  Logger logger,
                                  DrawService drawService)
        {
            this.logger = logger;
            this.drawService = drawService;
            this.configService = configService;
        }

        public void Open()
        {
            camsDetectionWindow = new CamsDetectionWindow(this);
            ClearProjectionImage();
            camsDetectionWindow.Show();
        }

        public bool ForceClose { get; private set; }

        public void Close()
        {
            if (camsDetectionWindow == null)
            {
                return;
            }

            configService.CamsDetectionWindowPositionLeft = WindowPositionLeft;
            configService.CamsDetectionWindowPositionTop = WindowPositionTop;
            configService.CamsDetectionWindowHeight = WindowHeight;
            configService.CamsDetectionWindowWidth = WindowWidth;

            ForceClose = true;
            camsDetectionWindow?.Close();
            camsDetectionWindow = null;
            ForceClose = false;
        }

        public void OnWindowLoaded()
        {
            WindowHeight = configService.CamsDetectionWindowHeight;
            WindowWidth = configService.CamsDetectionWindowWidth;
            WindowPositionLeft = configService.CamsDetectionWindowPositionLeft;
            WindowPositionTop = configService.CamsDetectionWindowPositionTop;
        }

        public void SetCamImages(CamNumber camNumber,
                                 BitmapImage image,
                                 BitmapImage roiImage,
                                 BitmapImage lastRoiImage)
        {
            switch (camNumber)
            {
                case CamNumber._1:
                    Cam1Image = image;
                    Cam1RoiImage = roiImage;
                    Cam1LastRoiImage = lastRoiImage;
                    break;
                case CamNumber._2:
                    Cam2Image = image;
                    Cam2RoiImage = roiImage;
                    Cam2LastRoiImage = lastRoiImage;
                    break;
                case CamNumber._3:
                    Cam3Image = image;
                    Cam3RoiImage = roiImage;
                    Cam3LastRoiImage = lastRoiImage;
                    break;
                case CamNumber._4:
                    Cam4Image = image;
                    Cam4RoiImage = roiImage;
                    Cam4LastRoiImage = lastRoiImage;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(camNumber), camNumber, null);
            }
        }

        public void SetProjectionImage(BitmapImage image)
        {
            ProjectionImage = image;
        }

        public void ClearProjectionImage()
        {
            ProjectionImage = Converter.EmguImageToBitmapImage(drawService.ProjectionBackgroundImage);
        }

        public void PrintThrow(DetectedThrow thrw)
        {
            ThrowText = thrw.ToString();
            ThrowsHistoryText = $"{ThrowsHistoryText}\n{thrw}";
        }

        public void ClearPointsBox()
        {
            ThrowText = string.Empty;
        }

        public void ClearHistoryPointsBox()
        {
            ThrowsHistoryText = string.Empty;
        }

        public void FireThrowUndo()
        {
            OnUndoThrowButtonPressed?.Invoke();
        }

        public void FireThrowCorrect()
        {
            OnCorrectThrowButtonPressed?.Invoke();
        }

        #region PropertyChangingFire

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}