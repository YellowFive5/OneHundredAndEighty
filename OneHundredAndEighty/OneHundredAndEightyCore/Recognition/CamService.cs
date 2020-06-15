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

        private DartContour dartContour;
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
                    camIndex = GetCamIndexById(SettingsType.Cam1Id);
                    thresholdSlider = configService.Read<double>(SettingsType.Cam1ThresholdSlider);
                    roiPosYSlider = configService.Read<double>(SettingsType.Cam1RoiPosYSlider);
                    roiHeightSlider = configService.Read<double>(SettingsType.Cam1RoiHeightSlider);
                    surfaceSlider = configService.Read<double>(SettingsType.Cam1SurfaceSlider);
                    surfaceCenterSlider = configService.Read<double>(SettingsType.Cam1SurfaceCenterSlider);
                    camSetupPoint = new PointF(configService.Read<float>(SettingsType.Cam1X),
                                               configService.Read<float>(SettingsType.Cam1Y));
                    break;
                case CamNumber._2:
                    camIndex = GetCamIndexById(SettingsType.Cam2Id);
                    thresholdSlider = configService.Read<double>(SettingsType.Cam2ThresholdSlider);
                    roiPosYSlider = configService.Read<double>(SettingsType.Cam2RoiPosYSlider);
                    roiHeightSlider = configService.Read<double>(SettingsType.Cam2RoiHeightSlider);
                    surfaceSlider = configService.Read<double>(SettingsType.Cam2SurfaceSlider);
                    surfaceCenterSlider = configService.Read<double>(SettingsType.Cam2SurfaceCenterSlider);
                    camSetupPoint = new PointF(configService.Read<float>(SettingsType.Cam2X),
                                               configService.Read<float>(SettingsType.Cam2Y));
                    break;
                case CamNumber._3:
                    camIndex = GetCamIndexById(SettingsType.Cam3Id);
                    thresholdSlider = configService.Read<double>(SettingsType.Cam3ThresholdSlider);
                    roiPosYSlider = configService.Read<double>(SettingsType.Cam3RoiPosYSlider);
                    roiHeightSlider = configService.Read<double>(SettingsType.Cam3RoiHeightSlider);
                    surfaceSlider = configService.Read<double>(SettingsType.Cam3SurfaceSlider);
                    surfaceCenterSlider = configService.Read<double>(SettingsType.Cam3SurfaceCenterSlider);
                    camSetupPoint = new PointF(configService.Read<float>(SettingsType.Cam3X),
                                               configService.Read<float>(SettingsType.Cam3Y));

                    break;
                case CamNumber._4:
                    camIndex = GetCamIndexById(SettingsType.Cam4Id);
                    thresholdSlider = configService.Read<double>(SettingsType.Cam4ThresholdSlider);
                    roiPosYSlider = configService.Read<double>(SettingsType.Cam4RoiPosYSlider);
                    roiHeightSlider = configService.Read<double>(SettingsType.Cam4RoiHeightSlider);
                    surfaceSlider = configService.Read<double>(SettingsType.Cam4SurfaceSlider);
                    surfaceCenterSlider = configService.Read<double>(SettingsType.Cam4SurfaceCenterSlider);
                    camSetupPoint = new PointF(configService.Read<float>(SettingsType.Cam4X),
                                               configService.Read<float>(SettingsType.Cam4Y));

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
            minContourArc = configService.Read<int>(SettingsType.MinContourArc);
            camFovAngle = configService.Read<double>(SettingsType.CamFovAngle);
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
            var found = FindDartContour(RoiLastThrowFrame);
            if (found)
            {
                ProcessDartContour();
            }
        }

        private bool FindDartContour(Image<Gray, byte> roiLastThrowFrame)
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

            var found = contourArс > 0;

            if (found)
            {
                dartContour = new DartContour(contour, contourArс);
            }

            return found;
        }

        private void ProcessDartContour()
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