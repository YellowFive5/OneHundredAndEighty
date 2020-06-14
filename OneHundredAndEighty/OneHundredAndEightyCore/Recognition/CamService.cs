#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using DirectShowLib;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using NLog;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public class CamService
    {
        public readonly CamNumber camNumber;
        private readonly DrawService drawService;
        private readonly VideoCapture videoCapture;
        private readonly IConfigService configService;
        private readonly MeasureService measureService;
        private readonly Logger logger;
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
        private readonly double thresholdSlider;
        private readonly double surfaceSlider;
        private readonly double surfaceCenterSlider;
        private readonly double roiPosYSlider;
        private readonly double roiHeightSlider;
        private readonly Rectangle roiRectangle;

        public CamService(CamNumber camNumber,
                          Logger logger,
                          DrawService drawService,
                          IConfigService configService,
                          ThrowService throwService)
        {
            this.camNumber = camNumber;
            this.logger = logger;
            this.drawService = drawService;
            this.configService = configService;
            measureService = new MeasureService(camNumber, logger, configService, throwService);

            var camIndex = -1;
            switch (camNumber)
            {
                case CamNumber._1:
                    camIndex = GetCamIndexById(SettingsType.Cam1Id);
                    thresholdSlider = configService.Read<double>(SettingsType.Cam1ThresholdSlider);
                    roiPosYSlider = configService.Read<double>(SettingsType.Cam1RoiPosYSlider);
                    roiHeightSlider = configService.Read<double>(SettingsType.Cam1RoiHeightSlider);
                    surfaceSlider = configService.Read<double>(SettingsType.Cam1SurfaceSlider);
                    surfaceCenterSlider = configService.Read<double>(SettingsType.Cam1SurfaceCenterSlider);
                    break;
                case CamNumber._2:
                    camIndex = GetCamIndexById(SettingsType.Cam2Id);
                    thresholdSlider = configService.Read<double>(SettingsType.Cam2ThresholdSlider);
                    roiPosYSlider = configService.Read<double>(SettingsType.Cam2RoiPosYSlider);
                    roiHeightSlider = configService.Read<double>(SettingsType.Cam2RoiHeightSlider);
                    surfaceSlider = configService.Read<double>(SettingsType.Cam2SurfaceSlider);
                    surfaceCenterSlider = configService.Read<double>(SettingsType.Cam2SurfaceCenterSlider);
                    break;
                case CamNumber._3:
                    camIndex = GetCamIndexById(SettingsType.Cam3Id);
                    thresholdSlider = configService.Read<double>(SettingsType.Cam3ThresholdSlider);
                    roiPosYSlider = configService.Read<double>(SettingsType.Cam3RoiPosYSlider);
                    roiHeightSlider = configService.Read<double>(SettingsType.Cam3RoiHeightSlider);
                    surfaceSlider = configService.Read<double>(SettingsType.Cam3SurfaceSlider);
                    surfaceCenterSlider = configService.Read<double>(SettingsType.Cam3SurfaceCenterSlider);
                    break;
                case CamNumber._4:
                    camIndex = GetCamIndexById(SettingsType.Cam4Id);
                    thresholdSlider = configService.Read<double>(SettingsType.Cam4ThresholdSlider);
                    roiPosYSlider = configService.Read<double>(SettingsType.Cam4RoiPosYSlider);
                    roiHeightSlider = configService.Read<double>(SettingsType.Cam4RoiHeightSlider);
                    surfaceSlider = configService.Read<double>(SettingsType.Cam4SurfaceSlider);
                    surfaceCenterSlider = configService.Read<double>(SettingsType.Cam4SurfaceCenterSlider);
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
            videoCapture = new VideoCapture(camIndex, VideoCapture.API.DShow);
            videoCapture.SetCaptureProperty(CapProp.FrameWidth, resolutionWidth);
            videoCapture.SetCaptureProperty(CapProp.FrameHeight, resolutionHeight);
            roiRectangle = new Rectangle(0,
                                         (int) roiPosYSlider,
                                         resolutionWidth,
                                         (int) roiHeightSlider);
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

        public BitmapImage GetImage()
        {
            return LinedFrame?.Data != null
                       ? Converter.EmguImageToBitmapImage(LinedFrame)
                       : new BitmapImage();
        }

        public BitmapImage GetRoiImage()
        {
            return RoiFrame?.Data != null
                       ? Converter.EmguImageToBitmapImage(RoiFrame)
                       : new BitmapImage();
        }

        public BitmapImage GetLastRoiImage()
        {
            return RoiLastThrowFrame?.Data != null
                       ? Converter.EmguImageToBitmapImage(RoiLastThrowFrame)
                       : new BitmapImage();
        }

        public void TryQueryFrame()
        {
            var testImage = videoCapture.QueryFrame();
            if (testImage == null)
            {
                throw new Exception($"Error query image from Cam#{camNumber}");
            }
        }

        public void DoSetupCaptures(List<double> slidersData)
        {
            var thresholdSlider = slidersData.ElementAt(0);
            var roiPosYSlider = slidersData.ElementAt(3);
            var roiHeightSlider = slidersData.ElementAt(4);
            var resolutionWidth = slidersData.ElementAt(5);
            var roiRectangle = new Rectangle(0,
                                             (int) roiPosYSlider,
                                             (int) resolutionWidth,
                                             (int) roiHeightSlider);

            OriginFrame = videoCapture.QueryFrame().ToImage<Bgr, byte>();
            LinedFrame = drawService.DrawSetupLines(OriginFrame.Clone(), slidersData);
            RoiFrame = OriginFrame.Clone().Convert<Gray, byte>().Not();
            RoiFrame.ROI = roiRectangle;
            RoiFrame._SmoothGaussian(smoothGauss);
            CvInvoke.Threshold(RoiFrame, RoiFrame, thresholdSlider, 255, ThresholdType.Binary);
        }

        public void DoCaptures()
        {
            var slidersData = new List<double>()
                              {
                                  surfaceSlider,
                                  surfaceCenterSlider,
                                  roiPosYSlider,
                                  roiHeightSlider,
                                  resolutionWidth
                              };

            OriginFrame = videoCapture.QueryFrame().ToImage<Bgr, byte>();
            LinedFrame = drawService.DrawSetupLines(OriginFrame.Clone(), slidersData);
            RoiFrame = OriginFrame.Clone().Convert<Gray, byte>().Not();
            RoiFrame.ROI = roiRectangle;
            RoiFrame._SmoothGaussian(smoothGauss);
            CvInvoke.Threshold(RoiFrame, RoiFrame, thresholdSlider, 255, ThresholdType.Binary);

            RoiFrameBackground = RoiFrame.Clone();
            RoiLastThrowFrame = RoiFrame.Clone();
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
                // ThresholdRoi(RoiFrame);
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
            // ThresholdRoi(RoiFrame);
            var diffImage = CaptureAndDiff();
            RefreshImages(diffImage);

            logger.Debug($"Find throw for cam_{camNumber} end");
        }

        private void RefreshImages(Image<Gray, byte> diffImage)
        {
            logger.Debug($"Refreshing images for cam_{camNumber} start");

            RoiFrameBackground = RoiFrame.Clone();
            RoiLastThrowFrame = diffImage.Clone();
            DoCaptures();
            // GetImage();

            logger.Debug($"Refreshing images for cam_{camNumber} end");
        }

        private Image<Gray, byte> CaptureAndDiff()
        {
            logger.Debug($"Capture and diff for cam_{camNumber} start");

            var newImage = videoCapture.QueryFrame().ToImage<Gray, byte>().Not();
            // ThresholdRoi(newImage);
            var diffImage = RoiFrameBackground.AbsDiff(newImage);
            logger.Debug($"Capture and diff for cam_{camNumber} end");
            return diffImage;
        }

        public void FindAndProcessDartContour()
        {
            var found = measureService.FindDartContour(RoiLastThrowFrame);
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