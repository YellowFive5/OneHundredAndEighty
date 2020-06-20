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
using Emgu.CV.Util;
using NLog;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public class CamService
    {
        private readonly Logger logger;
        private readonly DrawService drawService;
        private readonly VideoCapture videoCapture;
        private readonly IConfigService configService;
        public readonly CamNumber camNumber;
        private readonly ThrowService throwService;
        private Image<Bgr, byte> OriginFrame { get; set; }
        private Image<Bgr, byte> LinedFrame { get; set; }
        private Image<Gray, byte> RoiFrame { get; set; }
        private Image<Gray, byte> RoiFrameBackground { get; set; }
        private Image<Gray, byte> RoiLastThrowFrame { get; set; }

        private readonly PointF camSetupPoint;

        private readonly int resolutionWidth;
        private readonly int resolutionHeight;
        private readonly int movesExtraction;
        private readonly int movesDart;
        private readonly int movesNoise;
        private readonly int smoothGauss;
        private readonly int minContourArc;
        private readonly double camFovAngle;
        private readonly double thresholdSlider;
        private readonly double surfaceSlider;
        private readonly double surfaceCenterSlider;
        private readonly double roiPosYSlider;
        private readonly double roiHeightSlider;
        private readonly Rectangle roiRectangle;
        private readonly double toBullAngle;

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
            this.throwService = throwService;

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
            movesExtraction = configService.MovesExtractionValue;
            movesDart = configService.MovesDartValue;
            movesNoise = configService.MovesNoiseValue;
            smoothGauss = configService.SmoothGaussValue;
            minContourArc = configService.MinContourArcValue;
            camFovAngle = configService.CamsFovAngle;
            videoCapture = new VideoCapture(camIndex, VideoCapture.API.DShow);
            videoCapture.SetCaptureProperty(CapProp.FrameWidth, resolutionWidth);
            videoCapture.SetCaptureProperty(CapProp.FrameHeight, resolutionHeight);
            roiRectangle = new Rectangle(0,
                                         (int) roiPosYSlider,
                                         resolutionWidth,
                                         (int) roiHeightSlider);
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

            var roiRectangleTemp = new Rectangle(0,
                                                 (int) roiPosYSliderTemp,
                                                 resolutionWidth,
                                                 (int) roiHeightSliderTemp);
            var slidersData = new List<double>
                              {
                                  surfaceSliderTemp,
                                  surfaceCenterSliderTemp,
                                  roiPosYSliderTemp,
                                  roiHeightSliderTemp,
                                  resolutionWidth
                              };

            OriginFrame = videoCapture.QueryFrame().ToImage<Bgr, byte>();
            LinedFrame = drawService.DrawSetupLines(OriginFrame.Clone(), slidersData);
            RoiFrame = OriginFrame.Clone().Convert<Gray, byte>().Not();
            RoiFrame.ROI = roiRectangleTemp;
            RoiFrame._SmoothGaussian(smoothGauss);
            CvInvoke.Threshold(RoiFrame, RoiFrame, thresholdSliderTemp, 255, ThresholdType.Binary);
        }

        public void DoCaptures()
        {
            var slidersData = new List<double>
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

            if (moves > movesNoise)
            {
                response = ResponseType.Move;
            }

            return response;
        }

        public ResponseType DetectThrow()
        {
            var response = ResponseType.Nothing;
            var diffImage = CaptureAndDiff();
            var moves = diffImage.CountNonzero()[0];
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

            return response;
        }

        public void FindThrow()
        {
            OriginFrame = videoCapture.QueryFrame().ToImage<Bgr, byte>();
            RoiFrame = OriginFrame.Clone().Convert<Gray, byte>().Not();
            // ThresholdRoi(RoiFrame);
            var diffImage = CaptureAndDiff();
            RefreshImages(diffImage);
        }

        private void RefreshImages(Image<Gray, byte> diffImage)
        {
            RoiFrameBackground = RoiFrame.Clone();
            RoiLastThrowFrame = diffImage.Clone();
            DoCaptures();
            // GetImage();
        }

        private Image<Gray, byte> CaptureAndDiff()
        {
            var newImage = videoCapture.QueryFrame().ToImage<Gray, byte>().Not();
            // ThresholdRoi(newImage);
            var diffImage = RoiFrameBackground.AbsDiff(newImage);
            return diffImage;
        }

        public void FindAndProcessDartContour()
        {
            var dartContour = TryFindDartContour(RoiLastThrowFrame);
            if (dartContour != null)
            {
                ProcessDartContour(dartContour);
            }
        }

        private DartContour TryFindDartContour(Image<Gray, byte> roiLastThrowFrame)
        {
            var allContours = new VectorOfVectorOfPoint();
            var matHierarсhy = new Mat();
            CvInvoke.FindContours(roiLastThrowFrame,
                                  allContours,
                                  matHierarсhy,
                                  RetrType.External,
                                  ChainApproxMethod.ChainApproxNone);

            var contour = new VectorOfPoint();
            var contourArс = 0.0;

            for (var i = 0; i < allContours.Size; i++)
            {
                var tempContour = allContours[i];
                var tempContourArс = CvInvoke.ArcLength(tempContour, true);
                if (tempContourArс > minContourArc &&
                    tempContourArс > contourArс)
                {
                    contourArс = tempContourArс;
                    contour = tempContour;
                }
            }

            DartContour dartContour = null;
            if (contourArс > 0)
            {
                dartContour = new DartContour(contour, contourArс);
            }

            return dartContour;
        }

        private void ProcessDartContour(DartContour dartContour)
        {
            // Moments and centerpoint
            // var contourMoments = CvInvoke.Moments(processedContour);
            // var contourCenterPoint = new PointF((float) (contourMoments.M10 / contourMoments.M00),
            //                                     (float) camService.roiPosYSlider + (float) (contourMoments.M01 / contourMoments.M00));

            // Find contour rectangle
            var rect = CvInvoke.MinAreaRect(dartContour.ContourPoints);
            var box = CvInvoke.BoxPoints(rect);

            var contourBoxPoint1 = new PointF(box[0].X, (float) roiPosYSlider + box[0].Y);
            var contourBoxPoint2 = new PointF(box[1].X, (float) roiPosYSlider + box[1].Y);
            var contourBoxPoint3 = new PointF(box[2].X, (float) roiPosYSlider + box[2].Y);
            var contourBoxPoint4 = new PointF(box[3].X, (float) roiPosYSlider + box[3].Y);

            // Setup vertical contour middlepoints
            var contourHeight = MeasureService.FindDistance(contourBoxPoint1, contourBoxPoint2);
            var contourWidth = MeasureService.FindDistance(contourBoxPoint4, contourBoxPoint1);
            PointF contourBoxMiddlePoint1;
            PointF contourBoxMiddlePoint2;

            if (contourWidth > contourHeight)
            {
                contourBoxMiddlePoint1 = MeasureService.FindMiddle(contourBoxPoint1, contourBoxPoint2);
                contourBoxMiddlePoint2 = MeasureService.FindMiddle(contourBoxPoint4, contourBoxPoint3);
            }
            else
            {
                contourBoxMiddlePoint1 = MeasureService.FindMiddle(contourBoxPoint4, contourBoxPoint1);
                contourBoxMiddlePoint2 = MeasureService.FindMiddle(contourBoxPoint3, contourBoxPoint2);
            }

            // Find spikeLine to surface
            var spikeLinePoint1 = contourBoxMiddlePoint1;
            var spikeLinePoint2 = contourBoxMiddlePoint2;
            var spikeLineLength = surfaceSlider - contourBoxMiddlePoint2.Y;
            var spikeAngle = MeasureService.FindAngle(contourBoxMiddlePoint2, contourBoxMiddlePoint1);
            spikeLinePoint1.X = (float) (contourBoxMiddlePoint2.X + Math.Cos(spikeAngle) * spikeLineLength);
            spikeLinePoint1.Y = (float) (contourBoxMiddlePoint2.Y + Math.Sin(spikeAngle) * spikeLineLength);

            // Find point of impact with surface
            PointF? camPoi = MeasureService.FindLinesIntersection(spikeLinePoint1,
                                                                  spikeLinePoint2,
                                                                  new PointF(0,
                                                                             (float) surfaceSlider),
                                                                  new PointF(resolutionWidth,
                                                                             (float) surfaceSlider));

            // Translate cam surface POI to dartboard projection
            var frameSemiWidth = resolutionWidth / 2;
            var camFovSemiAngle = camFovAngle / 2;
            var projectionToCenter = new PointF();
            var surfacePoiToCenterDistance = MeasureService.FindDistance(new PointF((float) surfaceCenterSlider,
                                                                                    (float) surfaceSlider),
                                                                         camPoi.GetValueOrDefault());
            var surfaceLeftToPoiDistance = MeasureService.FindDistance(new PointF((float) surfaceCenterSlider - resolutionWidth / 3,
                                                                                  (float) surfaceSlider),
                                                                       camPoi.GetValueOrDefault());
            var surfaceRightToPoiDistance = MeasureService.FindDistance(new PointF((float) surfaceCenterSlider + resolutionWidth / 3,
                                                                                   (float) surfaceSlider),
                                                                        camPoi.GetValueOrDefault());
            var projectionCamToCenterDistance = frameSemiWidth / Math.Sin(Math.PI * camFovSemiAngle / 180.0) * Math.Cos(Math.PI * camFovSemiAngle / 180.0);
            var projectionCamToPoiDistance = Math.Sqrt(Math.Pow(projectionCamToCenterDistance, 2) + Math.Pow(surfacePoiToCenterDistance, 2));
            var projectionPoiToCenterDistance = Math.Sqrt(Math.Pow(projectionCamToPoiDistance, 2) - Math.Pow(projectionCamToCenterDistance, 2));
            var poiCamCenterAngle = Math.Asin(projectionPoiToCenterDistance / projectionCamToPoiDistance);

            projectionToCenter.X = (float) (camSetupPoint.X - Math.Cos(toBullAngle) * projectionCamToCenterDistance);
            projectionToCenter.Y = (float) (camSetupPoint.Y - Math.Sin(toBullAngle) * projectionCamToCenterDistance);

            if (surfaceLeftToPoiDistance < surfaceRightToPoiDistance)
            {
                poiCamCenterAngle *= -1;
            }

            var projectionPoi = new PointF
                                {
                                    X = (float) (camSetupPoint.X + Math.Cos(toBullAngle + poiCamCenterAngle) * 2000),
                                    Y = (float) (camSetupPoint.Y + Math.Sin(toBullAngle + poiCamCenterAngle) * 2000)
                                };

            // Draw line from cam through projection POI
            var rayPoint = projectionPoi;
            var angle = MeasureService.FindAngle(camSetupPoint, rayPoint);
            rayPoint.X = (float) (camSetupPoint.X + Math.Cos(angle) * 2000);
            rayPoint.Y = (float) (camSetupPoint.Y + Math.Sin(angle) * 2000);

            var ray = new Ray(camNumber, camSetupPoint, rayPoint, dartContour.Arc);

            throwService.SaveRay(ray);
        }

        public void Dispose()
        {
            videoCapture.Dispose();
        }
    }
}