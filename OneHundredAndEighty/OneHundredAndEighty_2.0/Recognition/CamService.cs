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
    public class CamService
    {
        private readonly MainWindow camWindow; // todo window
        private readonly DrawService drawService;
        public readonly VideoCapture videoCapture;
        private readonly ConfigService configService;
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
        private readonly bool runtimeCapturing;
        private readonly bool withDetection;
        private readonly double toCamDistance;
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

        public CamService(MainWindow camWindow) // todo window
        {
            this.camWindow = camWindow;
            logger = MainWindow.ServiceContainer.Resolve<Logger>();
            drawService = MainWindow.ServiceContainer.Resolve<DrawService>();
            configService = MainWindow.ServiceContainer.Resolve<ConfigService>();
            runtimeCapturing = configService.Read<bool>(SettingsType.RuntimeCapturingCheckBox);
            withDetection = configService.Read<bool>(SettingsType.WithDetectionCheckBox);
            surfacePoint1 = new PointF();
            surfacePoint2 = new PointF();
            // camNumber = camWindow.camNumber;
            // setupPoint = new PointF(configService.Read<float>($"Cam{camNumber}X"),
            //                         configService.Read<float>($"Cam{camNumber}Y"));
            resolutionWidth = configService.Read<int>(SettingsType.ResolutionWidth);
            resolutionHeight = configService.Read<int>(SettingsType.ResolutionHeight);
            movesExtraction = configService.Read<int>(SettingsType.MovesExtraction);
            movesDart = configService.Read<int>(SettingsType.MovesDart);
            movesNoise = configService.Read<int>(SettingsType.MovesNoise);
            smoothGauss = configService.Read<int>(SettingsType.SmoothGauss);
            // toCamDistance = configService.Read<double>($"ToCam{camNumber}Distance");
            toBullAngle = MeasureService.FindAngle(setupPoint, drawService.projectionCenterPoint);
            videoCapture = new VideoCapture(GetCamIndex(camNumber), VideoCapture.API.DShow);
            videoCapture.SetCaptureProperty(CapProp.FrameWidth, resolutionWidth);
            videoCapture.SetCaptureProperty(CapProp.FrameHeight, resolutionHeight);
            GetSlidersData();
            RefreshImageBoxes();
        }

        private int GetCamIndex(int camNumber)
        {
            var allCams = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice).ToList();
            // var camId = configService.Read<string>($"Cam{camNumber}Id");
            // var index = allCams.FindIndex(x => x.DevicePath.Contains(camId));
            var index = -1;
            return index;
        }

        private void GetSlidersData()
        {
            // camWindow.Dispatcher.Invoke(new Action(() => tresholdSlider = camWindow.TresholdSlider.Value));
            // camWindow.Dispatcher.Invoke(new Action(() => roiPosYSlider = camWindow.RoiPosYSlider.Value));
            // camWindow.Dispatcher.Invoke(new Action(() => roiHeightSlider = camWindow.RoiHeightSlider.Value));
            // camWindow.Dispatcher.Invoke(new Action(() => surfaceSlider = camWindow.SurfaceSlider.Value));
            // camWindow.Dispatcher.Invoke(new Action(() => surfaceCenterSlider = camWindow.SurfaceCenterSlider.Value));
        }

        private void DrawSetupLines()
        {
            OriginFrame = videoCapture.QueryFrame().ToImage<Bgr, byte>();
            LinedFrame = OriginFrame.Clone();

            roiRectangle = new Rectangle(0,
                                         (int)roiPosYSlider,
                                         resolutionWidth,
                                         (int)roiHeightSlider);

            drawService.DrawRectangle(LinedFrame,
                                      roiRectangle,
                                      drawService.camRoiRectColor.MCvScalar,
                                      drawService.camRoiRectThickness);

            surfacePoint1 = new PointF(0, (float)surfaceSlider);
            surfacePoint2 = new PointF(resolutionWidth,
                                       (float)surfaceSlider);
            drawService.DrawLine(LinedFrame,
                                 surfacePoint1,
                                 surfacePoint2,
                                 drawService.camSurfaceLineColor.MCvScalar,
                                 drawService.camSurfaceLineThickness);

            surfaceCenterPoint1 = new PointF((float)surfaceCenterSlider,
                                             (float)surfaceSlider);

            surfaceCenterPoint2 = new PointF(surfaceCenterPoint1.X,
                                             surfaceCenterPoint1.Y - 50);
            drawService.DrawLine(LinedFrame,
                                 surfaceCenterPoint1,
                                 surfaceCenterPoint2,
                                 drawService.camSurfaceLineColor.MCvScalar,
                                 drawService.camSurfaceLineThickness);

            surfaceLeftPoint1 = new PointF((float)surfaceCenterSlider - LinedFrame.Cols / 3,
                                           (float)surfaceSlider);

            surfaceRightPoint1 = new PointF((float)surfaceCenterSlider + LinedFrame.Cols / 3,
                                            (float)surfaceSlider);
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

        private void RefreshImageBoxes()
        {
            logger.Debug($"Refreshing imageboxes for cam_{camNumber} start");

            // camWindow.Dispatcher.Invoke(new Action(() => camWindow.ImageBox.Source = LinedFrame?.Data != null
            //                                                                              ? drawService.ToBitmap(LinedFrame)
            //                                                                              : new BitmapImage()));
            // camWindow.Dispatcher.Invoke(new Action(() => camWindow.ImageBoxRoi.Source = RoiFrame?.Data != null
            //                                                                                 ? drawService.ToBitmap(RoiFrame)
            //                                                                                 : new BitmapImage()));
            // camWindow.Dispatcher.Invoke(new Action(() => camWindow.ImageBoxRoiLastThrow.Source = RoiLastThrowFrame?.Data != null
            //                                                                                          ? drawService.ToBitmap(RoiLastThrowFrame)
            //                                                                                          : new BitmapImage()));
            logger.Debug($"Refreshing imageboxes for cam_{camNumber} end");
        }

        public ResponseType DetectMove()
        {
            var response = ResponseType.Nothing;

            if (withDetection)
            {
                var diffImage = CaptureAndDiff();
                var moves = diffImage.CountNonzero()[0];

                logger.Debug($"Moves is '{moves}'");

                if (moves > movesNoise)
                {
                    response = ResponseType.Move;
                }
            }

            if (runtimeCapturing)
            {
                DoCapture(true);
                RefreshImageBoxes();
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

        public void CalibrateCamSetupPoint()
        {
            const double startRadSector = -3.14159;
            const double radSectorStep = 0.314159;
            const int dartboardDiameterInPixels = 1020;
            const int dartboardDiameterInCm = 34;
            var toCamPixels = dartboardDiameterInPixels * toCamDistance / dartboardDiameterInCm;
            var i = 1;
            switch (camNumber)
            {
                case 1:
                    i = 2;
                    break;
                case 2:
                    i = 4;
                    break;
                case 3:
                    i = 6;
                    break;
                case 4:
                    i = 8;
                    break;
            }

            var calibratedCamSetupPoint = new PointF
            {
                X = (int)(drawService.projectionCenterPoint.X + Math.Cos(startRadSector + i * radSectorStep) * toCamPixels),
                Y = (int)(drawService.projectionCenterPoint.Y + Math.Sin(startRadSector + i * radSectorStep) * toCamPixels)
            };

            // camWindow.XTextBox.Text = calibratedCamSetupPoint.X.ToString();
            // camWindow.YTextBox.Text = calibratedCamSetupPoint.Y.ToString();

            // configService.Write($"Cam{camNumber}X", calibratedCamSetupPoint.X);
            // configService.Write($"Cam{camNumber}Y", calibratedCamSetupPoint.Y);

            setupPoint = calibratedCamSetupPoint;
        }
    }
}