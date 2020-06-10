﻿#region Usings

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
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Windows.CamsDetection;
using OneHundredAndEightyCore.Windows.Main;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public enum CamServiceWorkingMode
    {
        Setup,
        Check,
        Crossing,
        Detection
    }

    public class CamService
    {
        private readonly IMainWindow mainWindow;
        public readonly CamNumber camNumber;
        private readonly CamServiceWorkingMode workingMode;
        private readonly DrawService drawService;
        private readonly VideoCapture videoCapture;
        private readonly ConfigService configService;
        private readonly MeasureService measureService;
        private readonly CamsDetectionBoard camsDetectionBoard;
        private readonly Logger logger;
        public PointF surfacePoint1;
        public PointF surfacePoint2;
        public PointF surfaceCenterPoint1;
        private PointF surfaceCenterPoint2;
        public PointF surfaceLeftPoint1;
        public PointF surfaceRightPoint1;
        private double thresholdSlider;
        public double roiPosYSlider;
        private double roiHeightSlider;
        private double surfaceSlider;
        private double surfaceCenterSlider;
        public PointF setupPoint;
        public readonly double toBullAngle;
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

        public CamService(IMainWindow mainWindow,
                          CamNumber camNumber,
                          CamServiceWorkingMode workingMode)
        {
            this.workingMode = workingMode;
            this.mainWindow = mainWindow;
            this.camNumber = camNumber;
            logger = mainWindow.ServiceContainer.Resolve<Logger>();
            drawService = mainWindow.ServiceContainer.Resolve<DrawService>();
            configService = mainWindow.ServiceContainer.Resolve<ConfigService>();
            camsDetectionBoard = mainWindow.ServiceContainer.Resolve<CamsDetectionBoard>();
            measureService = new MeasureService(this, mainWindow); // todo not need mainWindow only because container

            var camIndex = -1;
            switch (camNumber)
            {
                case CamNumber._1:
                    setupPoint = new PointF(configService.Read<float>(SettingsType.Cam1X),
                                            configService.Read<float>(SettingsType.Cam1Y));
                    camIndex = GetCamIndexById(SettingsType.Cam1Id);
                    break;
                case CamNumber._2:
                    setupPoint = new PointF(configService.Read<float>(SettingsType.Cam2X),
                                            configService.Read<float>(SettingsType.Cam2Y));
                    camIndex = GetCamIndexById(SettingsType.Cam2Id);
                    break;
                case CamNumber._3:
                    setupPoint = new PointF(configService.Read<float>(SettingsType.Cam3X),
                                            configService.Read<float>(SettingsType.Cam3Y));
                    camIndex = GetCamIndexById(SettingsType.Cam3Id);
                    break;
                case CamNumber._4:
                    setupPoint = new PointF(configService.Read<float>(SettingsType.Cam4X),
                                            configService.Read<float>(SettingsType.Cam4Y));
                    camIndex = GetCamIndexById(SettingsType.Cam4Id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(camNumber), camNumber, null);
            }

            resolutionWidth = configService.Read<int>(SettingsType.ResolutionWidth);
            resolutionHeight = configService.Read<int>(SettingsType.ResolutionHeight);
            movesExtraction = configService.Read<int>(SettingsType.MovesExtraction);
            movesDart = configService.Read<int>(SettingsType.MovesDart);
            movesNoise = configService.Read<int>(SettingsType.MovesNoise);
            smoothGauss = configService.Read<int>(SettingsType.SmoothGauss);
            toBullAngle = MeasureService.FindAngle(setupPoint, DrawService.projectionCenterPoint);
            videoCapture = new VideoCapture(camIndex, VideoCapture.API.DShow);
            videoCapture.SetCaptureProperty(CapProp.FrameWidth, resolutionWidth);
            videoCapture.SetCaptureProperty(CapProp.FrameHeight, resolutionHeight);
            GetSlidersData();
            RefreshImageBoxes();
        }

        private int GetCamIndexById(SettingsType camIdSetting)
        {
            var allCams = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice).ToList();
            var camId = configService.Read<string>(camIdSetting);
            var index = allCams.FindIndex(x => x.DevicePath.Contains(camId));
            if (index == -1)
            {
                throw new Exception($"Camera with specified id '{camId}' not found in connected camera devices for Camera#{camNumber}");
            }

            return index;
        }

        private void GetSlidersData()
        {
            var sliderData = mainWindow.GetCamsSetupSlidersData(camNumber);

            thresholdSlider = sliderData.ElementAt(0);
            roiPosYSlider = sliderData.ElementAt(1);
            roiHeightSlider = sliderData.ElementAt(2);
            surfaceSlider = sliderData.ElementAt(3);
            surfaceCenterSlider = sliderData.ElementAt(4);
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
                                      DrawService.CamRoiRectThickness);

            surfacePoint1 = new PointF(0, (float) surfaceSlider);
            surfacePoint2 = new PointF(resolutionWidth,
                                       (float) surfaceSlider);
            drawService.DrawLine(LinedFrame,
                                 surfacePoint1,
                                 surfacePoint2,
                                 drawService.camSurfaceLineColor.MCvScalar,
                                 DrawService.CamSurfaceLineThickness);

            surfaceCenterPoint1 = new PointF((float) surfaceCenterSlider,
                                             (float) surfaceSlider);

            surfaceCenterPoint2 = new PointF(surfaceCenterPoint1.X,
                                             surfaceCenterPoint1.Y - 50);
            drawService.DrawLine(LinedFrame,
                                 surfaceCenterPoint1,
                                 surfaceCenterPoint2,
                                 drawService.camSurfaceLineColor.MCvScalar,
                                 DrawService.CamSurfaceLineThickness);

            surfaceLeftPoint1 = new PointF((float) surfaceCenterSlider - LinedFrame.Cols / 3,
                                           (float) surfaceSlider);

            surfaceRightPoint1 = new PointF((float) surfaceCenterSlider + LinedFrame.Cols / 3,
                                            (float) surfaceSlider);
        }

        private void ThresholdRoi(Image<Gray, byte> roiFrame)
        {
            roiFrame.ROI = roiRectangle;
            roiFrame._SmoothGaussian(smoothGauss);
            CvInvoke.Threshold(roiFrame, roiFrame, thresholdSlider, 255, ThresholdType.Binary);
        }

        public void DoCapture(bool withRoiBackgroundRefresh = false)
        {
            logger.Debug($"Doing capture for cam_{camNumber} start. RoiBackgroundRefresh = {withRoiBackgroundRefresh}");

            GetSlidersData();
            DrawSetupLines();

            if (workingMode == CamServiceWorkingMode.Crossing || withRoiBackgroundRefresh)
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

        public void RefreshImageBoxes(bool clear = false)
        {
            logger.Debug($"Refreshing imageboxes for cam_{camNumber} start");

            switch (workingMode)
            {
                case CamServiceWorkingMode.Setup:
                    mainWindow.Dispatcher.Invoke(() =>
                                                 {
                                                     var image = LinedFrame?.Data != null && !clear
                                                                     ? drawService.ToBitmap(LinedFrame)
                                                                     : new BitmapImage();

                                                     var roiImage = RoiFrame?.Data != null && !clear
                                                                        ? drawService.ToBitmap(RoiFrame)
                                                                        : new BitmapImage();

                                                     mainWindow.SetCamImages(camNumber, image, roiImage);
                                                 });
                    break;
                case CamServiceWorkingMode.Crossing:
                case CamServiceWorkingMode.Detection:
                    camsDetectionBoard.dispatcher.Invoke(() =>
                                                         {
                                                             var image = LinedFrame?.Data != null && !clear
                                                                             ? drawService.ToBitmap(LinedFrame)
                                                                             : new BitmapImage();

                                                             var roiImage = RoiFrame?.Data != null && !clear
                                                                                ? drawService.ToBitmap(RoiFrame)
                                                                                : new BitmapImage();

                                                             var lastRoiImage = RoiLastThrowFrame?.Data != null && !clear
                                                                                    ? drawService.ToBitmap(RoiLastThrowFrame)
                                                                                    : new BitmapImage();
                                                             camsDetectionBoard.SetCamImages(camNumber, image, roiImage, lastRoiImage);
                                                         });
                    break;
                case CamServiceWorkingMode.Check:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
            RefreshImageBoxes(true);
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

        public void TryQueryFrame()
        {
            var testImage = videoCapture.QueryFrame();
            if (testImage == null)
            {
                throw new Exception($"Error query image from Cam#{camNumber}");
            }
        }
    }
}