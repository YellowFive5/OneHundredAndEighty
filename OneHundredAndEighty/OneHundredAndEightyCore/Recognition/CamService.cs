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
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public class CamService
    {
        private readonly Logger logger;
        private readonly DrawService drawService;
        private readonly VideoCapture videoCapture;
        private readonly IConfigService configService;
        private Image<Bgr, byte> OriginFrame { get; set; }
        private Image<Bgr, byte> LinedFrame { get; set; }
        public Image<Gray, byte> RoiFrame { get; private set; }
        public Image<Gray, byte> RoiPreviousFrame { get; private set; }
        public Image<Gray, byte> ThrowExtractedRoiFrame { get; private set; }

        private readonly int resolutionHeight;
        private readonly int smoothGauss;
        private readonly double thresholdSlider;
        private readonly double roiHeightSlider;

        public readonly CamNumber camNumber;
        public readonly PointF camSetupPoint;
        public readonly double surfaceCenterSlider;
        public readonly double camFovAngle;
        public readonly int resolutionWidth;
        public readonly double surfaceSlider;
        public readonly double roiPosYSlider;
        public readonly double toBullAngle;

        public CamService(CamNumber camNumber,
                          Logger logger,
                          DrawService drawService,
                          IConfigService configService)
        {
            this.camNumber = camNumber;
            this.logger = logger;
            this.drawService = drawService;
            this.configService = configService;

            var camIndex = -1;
            switch (camNumber)
            {
                case CamNumber._1:
                    camIndex = GetCamIndexById(configService.Cam1Id);
                    thresholdSlider = configService.Cam1ThresholdSliderValue;
                    roiPosYSlider = configService.Cam1RoiPosYSliderValue;
                    roiHeightSlider = configService.Cam1RoiHeightSliderValue;
                    surfaceSlider = configService.Cam1SurfaceSliderValue;
                    surfaceCenterSlider = configService.Cam1SurfaceCenterSliderValue;
                    camSetupPoint = new PointF(configService.Cam1XSetupValue,
                                               configService.Cam1YSetupValue);
                    break;
                case CamNumber._2:
                    camIndex = GetCamIndexById(configService.Cam2Id);
                    thresholdSlider = configService.Cam2ThresholdSliderValue;
                    roiPosYSlider = configService.Cam2RoiPosYSliderValue;
                    roiHeightSlider = configService.Cam2RoiHeightSliderValue;
                    surfaceSlider = configService.Cam2SurfaceSliderValue;
                    surfaceCenterSlider = configService.Cam2SurfaceCenterSliderValue;
                    camSetupPoint = new PointF(configService.Cam2XSetupValue,
                                               configService.Cam2YSetupValue);
                    break;
                case CamNumber._3:
                    camIndex = GetCamIndexById(configService.Cam3Id);
                    thresholdSlider = configService.Cam3ThresholdSliderValue;
                    roiPosYSlider = configService.Cam3RoiPosYSliderValue;
                    roiHeightSlider = configService.Cam3RoiHeightSliderValue;
                    surfaceSlider = configService.Cam3SurfaceSliderValue;
                    surfaceCenterSlider = configService.Cam3SurfaceCenterSliderValue;
                    camSetupPoint = new PointF(configService.Cam3XSetupValue,
                                               configService.Cam3YSetupValue);

                    break;
                case CamNumber._4:
                    camIndex = GetCamIndexById(configService.Cam4Id);
                    thresholdSlider = configService.Cam4ThresholdSliderValue;
                    roiPosYSlider = configService.Cam4RoiPosYSliderValue;
                    roiHeightSlider = configService.Cam4RoiHeightSliderValue;
                    surfaceSlider = configService.Cam4SurfaceSliderValue;
                    surfaceCenterSlider = configService.Cam4SurfaceCenterSliderValue;
                    camSetupPoint = new PointF(configService.Cam4XSetupValue,
                                               configService.Cam4YSetupValue);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(camNumber), camNumber, null);
            }

            resolutionWidth = configService.CamsResolutionWidth;
            resolutionHeight = configService.CamsResolutionHeight;
            smoothGauss = configService.SmoothGaussValue;
            camFovAngle = configService.CamsFovAngle;
            videoCapture = new VideoCapture(camIndex, VideoCapture.API.DShow);
            videoCapture.SetCaptureProperty(CapProp.FrameWidth, resolutionWidth);
            videoCapture.SetCaptureProperty(CapProp.FrameHeight, resolutionHeight);
            var projectionCenterPoint = new PointF((float) DrawService.ProjectionFrameSide / 2,
                                                   (float) DrawService.ProjectionFrameSide / 2);
            toBullAngle = MeasureService.FindAngle(camSetupPoint, projectionCenterPoint);
        }

        private int GetCamIndexById(string camId)
        {
            var allCams = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice).ToList();
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

        public BitmapImage GetThrowExtractedRoiFrameImage()
        {
            return ThrowExtractedRoiFrame?.Data != null
                       ? Converter.EmguImageToBitmapImage(ThrowExtractedRoiFrame)
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

        public void DoSetupCaptures()
        {
            double thresholdSliderTemp;
            double surfaceSliderTemp;
            double surfaceCenterSliderTemp;
            double roiPosYSliderTemp;
            double roiHeightSliderTemp;

            switch (camNumber)
            {
                case CamNumber._1:
                    thresholdSliderTemp = configService.Cam1ThresholdSliderValue;
                    surfaceSliderTemp = configService.Cam1SurfaceSliderValue;
                    surfaceCenterSliderTemp = configService.Cam1SurfaceCenterSliderValue;
                    roiPosYSliderTemp = configService.Cam1RoiPosYSliderValue;
                    roiHeightSliderTemp = configService.Cam1RoiHeightSliderValue;
                    break;
                case CamNumber._2:
                    thresholdSliderTemp = configService.Cam2ThresholdSliderValue;
                    surfaceSliderTemp = configService.Cam2SurfaceSliderValue;
                    surfaceCenterSliderTemp = configService.Cam2SurfaceCenterSliderValue;
                    roiPosYSliderTemp = configService.Cam2RoiPosYSliderValue;
                    roiHeightSliderTemp = configService.Cam2RoiHeightSliderValue;
                    break;
                case CamNumber._3:
                    thresholdSliderTemp = configService.Cam3ThresholdSliderValue;
                    surfaceSliderTemp = configService.Cam3SurfaceSliderValue;
                    surfaceCenterSliderTemp = configService.Cam3SurfaceCenterSliderValue;
                    roiPosYSliderTemp = configService.Cam3RoiPosYSliderValue;
                    roiHeightSliderTemp = configService.Cam3RoiHeightSliderValue;
                    break;
                case CamNumber._4:
                    thresholdSliderTemp = configService.Cam4ThresholdSliderValue;
                    surfaceSliderTemp = configService.Cam4SurfaceSliderValue;
                    surfaceCenterSliderTemp = configService.Cam4SurfaceCenterSliderValue;
                    roiPosYSliderTemp = configService.Cam4RoiPosYSliderValue;
                    roiHeightSliderTemp = configService.Cam4RoiHeightSliderValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var slidersData = new List<double>
                              {
                                  surfaceSliderTemp,
                                  surfaceCenterSliderTemp,
                                  roiPosYSliderTemp,
                                  roiHeightSliderTemp,
                                  resolutionWidth,
                                  thresholdSliderTemp
                              };

            DoCapturesInternal(slidersData);
        }

        public void DoDetectionCaptures()
        {
            var slidersData = new List<double>
                              {
                                  surfaceSlider,
                                  surfaceCenterSlider,
                                  roiPosYSlider,
                                  roiHeightSlider,
                                  resolutionWidth,
                                  thresholdSlider
                              };

            DoCapturesInternal(slidersData);
        }

        private void DoCapturesInternal(List<double> slidersData)
        {
            var roiRectangleTemp = new Rectangle(0,
                                                 (int) slidersData.ElementAt(2),
                                                 (int) slidersData.ElementAt(4),
                                                 (int) slidersData.ElementAt(3));

            OriginFrame = videoCapture.QueryFrame().ToImage<Bgr, byte>();
            LinedFrame = drawService.DrawSetupLines(OriginFrame.Clone(), slidersData);
            RoiFrame = OriginFrame.Clone().Convert<Gray, byte>().Not();
            RoiFrame.ROI = roiRectangleTemp;
            RoiFrame._SmoothGaussian(smoothGauss);

            CvInvoke.Threshold(RoiFrame, RoiFrame, slidersData.ElementAt(5), 255, ThresholdType.Binary);
        }

        public void RoiToPreviousRoi()
        {
            RoiPreviousFrame = RoiFrame.Clone();
        }

        public void RoiToThrowExtractedRoiFrame()
        {
            ThrowExtractedRoiFrame = RoiFrame.Clone();
        }

        public void ExtractFromRoi()
        {
            var diffImage = RoiPreviousFrame?.AbsDiff(RoiFrame);
            ThrowExtractedRoiFrame = diffImage?.Clone();
            diffImage?.Dispose();
        }

        public void Dispose()
        {
            videoCapture.Dispose();
        }
    }
}