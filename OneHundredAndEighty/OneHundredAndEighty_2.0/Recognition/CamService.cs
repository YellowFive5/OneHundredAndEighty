#region Usings

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using Autofac;
using DirectShowLib;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using NLog;

#endregion

namespace OneHundredAndEighty_2._0.Recognition
{
    public enum CamServiceWorkingMode
    {
        Setup,
        Work
    }

    public class CamService
    {
        private readonly MainWindow mainWindow;
        private readonly string parentGridName;
        private readonly CamServiceWorkingMode mode;
        private readonly DrawService drawService;
        private readonly VideoCapture videoCapture;
        private readonly ConfigService configService;
        private readonly MeasureService measureService;
        private readonly Logger logger;
        public PointF surfacePoint1;
        public PointF surfacePoint2;
        public PointF surfaceCenterPoint1;
        private PointF surfaceCenterPoint2;
        public PointF surfaceLeftPoint1;
        public PointF surfaceRightPoint1;
        private double tresholdSlider;
        public double roiPosYSlider;
        private double roiHeightSlider;
        private double surfaceSlider;
        private double surfaceCenterSlider;
        public PointF setupPoint;
        public readonly double toBullAngle;
        public readonly int camNumber;
        private Rectangle roiRectangle;
        private Image<Bgr, byte> OriginFrame { get; set; }
        private Image<Bgr, byte> LinedFrame { get; set; }
        private Image<Gray, byte> RoiFrame { get; set; }
        private Image<Gray, byte> RoiFrameBackground { get; set; }
        public Image<Gray, byte> RoiLastThrowFrame { get; private set; }

        private readonly int resolutionWidth;
        private readonly int resolutionHeight;
        private readonly int movesExtraction;
        private readonly int movesDart;
        private readonly int movesNoise;
        private readonly int smoothGauss;

        public CamService(MainWindow mainWindow,
                          string parentGridName,
                          CamServiceWorkingMode mode = CamServiceWorkingMode.Work)
        {
            this.mode = mode;
            this.mainWindow = mainWindow;
            this.parentGridName = parentGridName;
            logger = MainWindow.ServiceContainer.Resolve<Logger>();
            drawService = MainWindow.ServiceContainer.Resolve<DrawService>();
            configService = MainWindow.ServiceContainer.Resolve<ConfigService>();
            measureService = new MeasureService(this);

            var camIndex = -1;
            switch (parentGridName)
            {
                case "Cam1Grid":
                    camNumber = 1;
                    setupPoint = new PointF(configService.Read<float>(SettingsType.Cam1X),
                                            configService.Read<float>(SettingsType.Cam1Y));
                    camIndex = GetCamIndex(SettingsType.Cam1Id);
                    break;
                case "Cam2Grid":
                    camNumber = 2;
                    setupPoint = new PointF(configService.Read<float>(SettingsType.Cam2X),
                                            configService.Read<float>(SettingsType.Cam2Y));
                    camIndex = GetCamIndex(SettingsType.Cam2Id);
                    break;
                case "Cam3Grid":
                    camNumber = 3;
                    setupPoint = new PointF(configService.Read<float>(SettingsType.Cam3X),
                                            configService.Read<float>(SettingsType.Cam3Y));
                    camIndex = GetCamIndex(SettingsType.Cam3Id);
                    break;
                case "Cam4Grid":
                    camNumber = 4;
                    setupPoint = new PointF(configService.Read<float>(SettingsType.Cam4X),
                                            configService.Read<float>(SettingsType.Cam4Y));
                    camIndex = GetCamIndex(SettingsType.Cam4Id);
                    break;
            }

            surfacePoint1 = new PointF();
            surfacePoint2 = new PointF();
            resolutionWidth = configService.Read<int>(SettingsType.ResolutionWidth);
            resolutionHeight = configService.Read<int>(SettingsType.ResolutionHeight);
            movesExtraction = configService.Read<int>(SettingsType.MovesExtraction);
            movesDart = configService.Read<int>(SettingsType.MovesDart);
            movesNoise = configService.Read<int>(SettingsType.MovesNoise);
            smoothGauss = configService.Read<int>(SettingsType.SmoothGauss);
            toBullAngle = MeasureService.FindAngle(setupPoint, drawService.projectionCenterPoint);
            videoCapture = new VideoCapture(camIndex, VideoCapture.API.DShow);
            videoCapture.SetCaptureProperty(CapProp.FrameWidth, resolutionWidth);
            videoCapture.SetCaptureProperty(CapProp.FrameHeight, resolutionHeight);
            GetSlidersData();
            RefreshImageBoxes();
        }

        private int GetCamIndex(SettingsType camIdSetting)
        {
            var allCams = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice).ToList();
            var camId = configService.Read<string>(camIdSetting);
            var index = allCams.FindIndex(x => x.DevicePath.Contains(camId));
            return index;
        }

        private void GetSlidersData()
        {
            switch (parentGridName)
            {
                case "Cam1Grid":
                    mainWindow.Dispatcher.Invoke(new Action(() => tresholdSlider = mainWindow.Cam1TresholdSlider.Value));
                    mainWindow.Dispatcher.Invoke(new Action(() => roiPosYSlider = mainWindow.Cam1RoiPosYSlider.Value));
                    mainWindow.Dispatcher.Invoke(new Action(() => roiHeightSlider = mainWindow.Cam1RoiHeightSlider.Value));
                    mainWindow.Dispatcher.Invoke(new Action(() => surfaceSlider = mainWindow.Cam1SurfaceSlider.Value));
                    mainWindow.Dispatcher.Invoke(new Action(() => surfaceCenterSlider = mainWindow.Cam1SurfaceCenterSlider.Value));
                    break;
                case "Cam2Grid":
                    mainWindow.Dispatcher.Invoke(new Action(() => tresholdSlider = mainWindow.Cam2TresholdSlider.Value));
                    mainWindow.Dispatcher.Invoke(new Action(() => roiPosYSlider = mainWindow.Cam2RoiPosYSlider.Value));
                    mainWindow.Dispatcher.Invoke(new Action(() => roiHeightSlider = mainWindow.Cam2RoiHeightSlider.Value));
                    mainWindow.Dispatcher.Invoke(new Action(() => surfaceSlider = mainWindow.Cam2SurfaceSlider.Value));
                    mainWindow.Dispatcher.Invoke(new Action(() => surfaceCenterSlider = mainWindow.Cam2SurfaceCenterSlider.Value));
                    break;
                case "Cam3Grid":
                    mainWindow.Dispatcher.Invoke(new Action(() => tresholdSlider = mainWindow.Cam3TresholdSlider.Value));
                    mainWindow.Dispatcher.Invoke(new Action(() => roiPosYSlider = mainWindow.Cam3RoiPosYSlider.Value));
                    mainWindow.Dispatcher.Invoke(new Action(() => roiHeightSlider = mainWindow.Cam3RoiHeightSlider.Value));
                    mainWindow.Dispatcher.Invoke(new Action(() => surfaceSlider = mainWindow.Cam3SurfaceSlider.Value));
                    mainWindow.Dispatcher.Invoke(new Action(() => surfaceCenterSlider = mainWindow.Cam3SurfaceCenterSlider.Value));
                    break;
                case "Cam4Grid":
                    mainWindow.Dispatcher.Invoke(new Action(() => tresholdSlider = mainWindow.Cam4TresholdSlider.Value));
                    mainWindow.Dispatcher.Invoke(new Action(() => roiPosYSlider = mainWindow.Cam4RoiPosYSlider.Value));
                    mainWindow.Dispatcher.Invoke(new Action(() => roiHeightSlider = mainWindow.Cam4RoiHeightSlider.Value));
                    mainWindow.Dispatcher.Invoke(new Action(() => surfaceSlider = mainWindow.Cam4SurfaceSlider.Value));
                    mainWindow.Dispatcher.Invoke(new Action(() => surfaceCenterSlider = mainWindow.Cam4SurfaceCenterSlider.Value));
                    break;
            }
        }

        private void DrawSetupLines()
        {
            OriginFrame = videoCapture.QueryFrame().ToImage<Bgr, byte>();
            LinedFrame = OriginFrame.Clone();

            roiRectangle = new Rectangle(0,
                                         (int) roiPosYSlider,
                                         resolutionWidth,
                                         (int) roiHeightSlider);

            drawService.DrawRectangle(LinedFrame,
                                      roiRectangle,
                                      drawService.camRoiRectColor.MCvScalar,
                                      drawService.camRoiRectThickness);

            surfacePoint1 = new PointF(0, (float) surfaceSlider);
            surfacePoint2 = new PointF(resolutionWidth,
                                       (float) surfaceSlider);
            drawService.DrawLine(LinedFrame,
                                 surfacePoint1,
                                 surfacePoint2,
                                 drawService.camSurfaceLineColor.MCvScalar,
                                 drawService.camSurfaceLineThickness);

            surfaceCenterPoint1 = new PointF((float) surfaceCenterSlider,
                                             (float) surfaceSlider);

            surfaceCenterPoint2 = new PointF(surfaceCenterPoint1.X,
                                             surfaceCenterPoint1.Y - 50);
            drawService.DrawLine(LinedFrame,
                                 surfaceCenterPoint1,
                                 surfaceCenterPoint2,
                                 drawService.camSurfaceLineColor.MCvScalar,
                                 drawService.camSurfaceLineThickness);

            surfaceLeftPoint1 = new PointF((float) surfaceCenterSlider - LinedFrame.Cols / 3,
                                           (float) surfaceSlider);

            surfaceRightPoint1 = new PointF((float) surfaceCenterSlider + LinedFrame.Cols / 3,
                                            (float) surfaceSlider);
        }

        private void ThresholdRoi(Image<Gray, byte> roiFrame)
        {
            roiFrame.ROI = roiRectangle;
            roiFrame._SmoothGaussian(smoothGauss);
            CvInvoke.Threshold(roiFrame, roiFrame, tresholdSlider, 255, ThresholdType.Binary);
        }

        public void DoCapture(bool withRoiBackgroundRefresh = false)
        {
            logger.Debug($"Doing capture for cam_{camNumber} start. RoiBackgroundRefresh = {withRoiBackgroundRefresh}");

            GetSlidersData();
            DrawSetupLines();

            if (withRoiBackgroundRefresh)
            {
                OriginFrame = videoCapture.QueryFrame().ToImage<Bgr, byte>();
                RoiFrame = OriginFrame.Clone().Convert<Gray, byte>().Not();
                ThresholdRoi(RoiFrame);
                RoiFrameBackground = RoiFrame.Clone();
                RoiLastThrowFrame = RoiFrame.Clone();
                RefreshImageBoxes();
            }

            logger.Debug($"Doing capture for cam_{camNumber} end");
        }

        public void RefreshImageBoxes()
        {
            logger.Debug($"Refreshing imageboxes for cam_{camNumber} start");

            switch (parentGridName)
            {
                case "Cam1Grid":
                    switch (mode)
                    {
                        case CamServiceWorkingMode.Setup:
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam1ImageBox.Source = LinedFrame?.Data != null
                                                                                                               ? drawService.ToBitmap(LinedFrame)
                                                                                                               : new BitmapImage()));
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam1ImageBoxRoi.Source = RoiFrame?.Data != null
                                                                                                                  ? drawService.ToBitmap(RoiFrame)
                                                                                                                  : new BitmapImage()));
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam1ImageBoxRoi.Source = RoiLastThrowFrame?.Data != null
                                                                                                                  ? drawService.ToBitmap(RoiLastThrowFrame)
                                                                                                                  : new BitmapImage()));
                            break;
                        case CamServiceWorkingMode.Work:
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam1ImageBoxRecognitionTab.Source = LinedFrame?.Data != null
                                                                                                                             ? drawService.ToBitmap(LinedFrame)
                                                                                                                             : new BitmapImage()));
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam1ImageBoxRoiRecognitionTab.Source = RoiFrame?.Data != null
                                                                                                                                ? drawService.ToBitmap(RoiFrame)
                                                                                                                                : new BitmapImage()));
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam1ImageBoxRoiLastThrowRecognitionTab.Source = RoiLastThrowFrame?.Data != null
                                                                                                                                         ? drawService.ToBitmap(RoiLastThrowFrame)
                                                                                                                                         : new BitmapImage()));
                            break;
                    }

                    break;
                case "Cam2Grid":
                    switch (mode)
                    {
                        case CamServiceWorkingMode.Setup:
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam2ImageBox.Source = LinedFrame?.Data != null
                                                                                                               ? drawService.ToBitmap(LinedFrame)
                                                                                                               : new BitmapImage()));
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam2ImageBoxRoi.Source = RoiFrame?.Data != null
                                                                                                                  ? drawService.ToBitmap(RoiFrame)
                                                                                                                  : new BitmapImage()));
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam2ImageBoxRoi.Source = RoiLastThrowFrame?.Data != null
                                                                                                                  ? drawService.ToBitmap(RoiLastThrowFrame)
                                                                                                                  : new BitmapImage()));
                            break;
                        case CamServiceWorkingMode.Work:
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam2ImageBoxRecognitionTab.Source = LinedFrame?.Data != null
                                                                                                                             ? drawService.ToBitmap(LinedFrame)
                                                                                                                             : new BitmapImage()));
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam2ImageBoxRoiRecognitionTab.Source = RoiFrame?.Data != null
                                                                                                                                ? drawService.ToBitmap(RoiFrame)
                                                                                                                                : new BitmapImage()));
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam2ImageBoxRoiLastThrowRecognitionTab.Source = RoiLastThrowFrame?.Data != null
                                                                                                                                         ? drawService.ToBitmap(RoiLastThrowFrame)
                                                                                                                                         : new BitmapImage()));
                            break;
                    }

                    break;
                case "Cam3Grid":
                    switch (mode)
                    {
                        case CamServiceWorkingMode.Setup:
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam3ImageBox.Source = LinedFrame?.Data != null
                                                                                                               ? drawService.ToBitmap(LinedFrame)
                                                                                                               : new BitmapImage()));
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam3ImageBoxRoi.Source = RoiFrame?.Data != null
                                                                                                                  ? drawService.ToBitmap(RoiFrame)
                                                                                                                  : new BitmapImage()));
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam3ImageBoxRoi.Source = RoiLastThrowFrame?.Data != null
                                                                                                                  ? drawService.ToBitmap(RoiLastThrowFrame)
                                                                                                                  : new BitmapImage()));
                            break;
                        case CamServiceWorkingMode.Work:
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam3ImageBoxRecognitionTab.Source = LinedFrame?.Data != null
                                                                                                                             ? drawService.ToBitmap(LinedFrame)
                                                                                                                             : new BitmapImage()));
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam3ImageBoxRoiRecognitionTab.Source = RoiFrame?.Data != null
                                                                                                                                ? drawService.ToBitmap(RoiFrame)
                                                                                                                                : new BitmapImage()));
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam3ImageBoxRoiLastThrowRecognitionTab.Source = RoiLastThrowFrame?.Data != null
                                                                                                                                         ? drawService.ToBitmap(RoiLastThrowFrame)
                                                                                                                                         : new BitmapImage()));
                            break;
                    }

                    break;
                case "Cam4Grid":
                    switch (mode)
                    {
                        case CamServiceWorkingMode.Setup:
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam4ImageBox.Source = LinedFrame?.Data != null
                                                                                                               ? drawService.ToBitmap(LinedFrame)
                                                                                                               : new BitmapImage()));
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam4ImageBoxRoi.Source = RoiFrame?.Data != null
                                                                                                                  ? drawService.ToBitmap(RoiFrame)
                                                                                                                  : new BitmapImage()));
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam4ImageBoxRoi.Source = RoiLastThrowFrame?.Data != null
                                                                                                                  ? drawService.ToBitmap(RoiLastThrowFrame)
                                                                                                                  : new BitmapImage()));
                            break;
                        case CamServiceWorkingMode.Work:
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam4ImageBoxRecognitionTab.Source = LinedFrame?.Data != null
                                                                                                                             ? drawService.ToBitmap(LinedFrame)
                                                                                                                             : new BitmapImage()));
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam4ImageBoxRoiRecognitionTab.Source = RoiFrame?.Data != null
                                                                                                                                ? drawService.ToBitmap(RoiFrame)
                                                                                                                                : new BitmapImage()));
                            mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam4ImageBoxRoiLastThrowRecognitionTab.Source = RoiLastThrowFrame?.Data != null
                                                                                                                                         ? drawService.ToBitmap(RoiLastThrowFrame)
                                                                                                                                         : new BitmapImage()));
                            break;
                    }

                    break;
            }

            logger.Debug($"Refreshing imageboxes for cam_{camNumber} end");
        }

        public ResponseType DetectMove()
        {
            var response = ResponseType.Nothing;

            var diffImage = CaptureAndDiff();
            var moves = diffImage.CountNonzero()[0];

            logger.Debug($"Moves is '{moves}'");

            if (moves > movesNoise)
            {
                response = ResponseType.Move;
            }

            logger.Debug($"Response is '{response}'");
            return response;
        }

        public ResponseType DetectThrow()
        {
            var response = ResponseType.Nothing;
            var diffImage = CaptureAndDiff();
            var moves = diffImage.CountNonzero()[0];
            logger.Debug($"Moves is '{moves}'");
            var extractProcess = moves > movesExtraction;
            var throwDetected = !extractProcess && moves > movesDart;

            if (throwDetected)
            {
                OriginFrame = videoCapture.QueryFrame().ToImage<Bgr, byte>();
                RoiFrame = OriginFrame.Clone().Convert<Gray, byte>().Not();
                ThresholdRoi(RoiFrame);
                RefreshImages(diffImage);

                response = ResponseType.Trow;
            }

            if (extractProcess)
            {
                response = ResponseType.Extraction;
            }

            logger.Debug($"Response is '{response}'");
            return response;
        }

        public void FindThrow()
        {
            logger.Debug($"Find throw for cam_{camNumber} start");

            OriginFrame = videoCapture.QueryFrame().ToImage<Bgr, byte>();
            RoiFrame = OriginFrame.Clone().Convert<Gray, byte>().Not();
            ThresholdRoi(RoiFrame);
            var diffImage = CaptureAndDiff();
            RefreshImages(diffImage);

            logger.Debug($"Find throw for cam_{camNumber} end");
        }

        private void RefreshImages(Image<Gray, byte> diffImage)
        {
            logger.Debug($"Refreshing images for cam_{camNumber} start");

            RoiFrameBackground = RoiFrame.Clone();
            RoiLastThrowFrame = diffImage.Clone();
            DoCapture();
            RefreshImageBoxes();

            logger.Debug($"Refreshing images for cam_{camNumber} end");
        }

        private Image<Gray, byte> CaptureAndDiff()
        {
            logger.Debug($"Capture and diff for cam_{camNumber} start");

            var newImage = videoCapture.QueryFrame().ToImage<Gray, byte>().Not();
            ThresholdRoi(newImage);
            var diffImage = RoiFrameBackground.AbsDiff(newImage);

            logger.Debug($"Capture and diff for cam_{camNumber} end");
            return diffImage;
        }

        public void ClearImageBoxes()
        {
            switch (parentGridName)
            {
                case "Cam1Grid":
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam1ImageBox.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam1ImageBoxRoi.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam1ImageBoxRoi.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam1ImageBoxRecognitionTab.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam1ImageBoxRoiRecognitionTab.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam1ImageBoxRoiLastThrowRecognitionTab.Source = new BitmapImage()));
                    break;
                case "Cam2Grid":
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam2ImageBox.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam2ImageBoxRoi.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam2ImageBoxRoi.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam2ImageBoxRecognitionTab.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam2ImageBoxRoiRecognitionTab.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam2ImageBoxRoiLastThrowRecognitionTab.Source = new BitmapImage()));

                    break;
                case "Cam3Grid":
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam3ImageBox.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam3ImageBoxRoi.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam3ImageBoxRoi.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam3ImageBoxRecognitionTab.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam3ImageBoxRoiRecognitionTab.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam3ImageBoxRoiLastThrowRecognitionTab.Source = new BitmapImage()));

                    break;
                case "Cam4Grid":
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam4ImageBox.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam4ImageBoxRoi.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam4ImageBoxRoi.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam4ImageBoxRecognitionTab.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam4ImageBoxRoiRecognitionTab.Source = new BitmapImage()));
                    mainWindow.Dispatcher.Invoke(new Action(() => mainWindow.Cam4ImageBoxRoiLastThrowRecognitionTab.Source = new BitmapImage()));

                    break;
            }
        }

        public void FindAndProcessDartContour()
        {
            var found = measureService.FindDartContour();
            if (found)
            {
                measureService.ProcessDartContour();
            }
        }

        public void Dispose()
        {
            videoCapture.Dispose();
        }
    }
}