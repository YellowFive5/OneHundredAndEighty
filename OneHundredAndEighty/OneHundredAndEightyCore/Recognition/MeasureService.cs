#region Usings

using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using NLog;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public class MeasureService
    {
        private readonly IConfigService configService;
        private readonly ThrowService throwService;
        private readonly CamNumber camNumber;
        private readonly Logger logger;
        private DartContour dartContour;

        private readonly int minContourArc;
        private readonly int resolutionWidth;
        private readonly double camFovAngle;
        private readonly PointF camSetupPoint;
        private readonly double roiPosYSlider;
        private readonly double surfaceSlider;
        private readonly double surfaceCenterSlider;
        private readonly double toBullAngle;

        public const double StartRadSector_11 = -3.14159;
        public const double StartRadSector_1114 = -2.9845105;
        public const double SectorStepRad = 0.314159;
        public const double SemiSectorStepRad = SectorStepRad / 2;
        private const int DartboardDiameterInPixels = 1020;
        private const int DartboardDiameterInCm = 34;

        public MeasureService(CamNumber camNumber,
                              Logger logger,
                              IConfigService configService,
                              ThrowService throwService)
        {
            this.camNumber = camNumber;
            this.logger = logger;
            this.configService = configService;
            this.throwService = throwService;

            minContourArc = configService.Read<int>(SettingsType.MinContourArc);
            camFovAngle = configService.Read<double>(SettingsType.CamFovAngle);
            resolutionWidth = configService.Read<int>(SettingsType.ResolutionWidth);

            switch (camNumber)
            {
                case CamNumber._1:
                    camSetupPoint = new PointF(configService.Read<float>(SettingsType.Cam1X),
                                               configService.Read<float>(SettingsType.Cam1Y));
                    roiPosYSlider = configService.Read<double>(SettingsType.Cam1RoiPosYSlider);
                    surfaceSlider = configService.Read<double>(SettingsType.Cam1SurfaceSlider);
                    surfaceCenterSlider = configService.Read<double>(SettingsType.Cam1SurfaceCenterSlider);
                    break;
                case CamNumber._2:
                    camSetupPoint = new PointF(configService.Read<float>(SettingsType.Cam2X),
                                               configService.Read<float>(SettingsType.Cam2Y));
                    roiPosYSlider = configService.Read<double>(SettingsType.Cam2RoiPosYSlider);
                    surfaceSlider = configService.Read<double>(SettingsType.Cam2SurfaceSlider);
                    surfaceCenterSlider = configService.Read<double>(SettingsType.Cam2SurfaceCenterSlider);
                    break;
                case CamNumber._3:
                    camSetupPoint = new PointF(configService.Read<float>(SettingsType.Cam3X),
                                               configService.Read<float>(SettingsType.Cam3Y));
                    roiPosYSlider = configService.Read<double>(SettingsType.Cam3RoiPosYSlider);
                    surfaceSlider = configService.Read<double>(SettingsType.Cam3SurfaceSlider);
                    surfaceCenterSlider = configService.Read<double>(SettingsType.Cam3SurfaceCenterSlider);
                    break;
                case CamNumber._4:
                    camSetupPoint = new PointF(configService.Read<float>(SettingsType.Cam4X),
                                               configService.Read<float>(SettingsType.Cam4Y));
                    roiPosYSlider = configService.Read<double>(SettingsType.Cam4RoiPosYSlider);
                    surfaceSlider = configService.Read<double>(SettingsType.Cam4SurfaceSlider);
                    surfaceCenterSlider = configService.Read<double>(SettingsType.Cam4SurfaceCenterSlider);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(camNumber), camNumber, null);
            }

            var projectionCenterPoint = new PointF((float) DrawService.ProjectionFrameSide / 2,
                                                   (float) DrawService.ProjectionFrameSide / 2);
            toBullAngle = FindAngle(camSetupPoint, projectionCenterPoint);
        }

        public bool FindDartContour(Image<Gray, byte> roiLastThrowFrame)
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

        public void ProcessDartContour()
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
            var contourHeight = FindDistance(contourBoxPoint1, contourBoxPoint2);
            var contourWidth = FindDistance(contourBoxPoint4, contourBoxPoint1);
            PointF contourBoxMiddlePoint1;
            PointF contourBoxMiddlePoint2;

            if (contourWidth > contourHeight)
            {
                contourBoxMiddlePoint1 = FindMiddle(contourBoxPoint1, contourBoxPoint2);
                contourBoxMiddlePoint2 = FindMiddle(contourBoxPoint4, contourBoxPoint3);
            }
            else
            {
                contourBoxMiddlePoint1 = FindMiddle(contourBoxPoint4, contourBoxPoint1);
                contourBoxMiddlePoint2 = FindMiddle(contourBoxPoint3, contourBoxPoint2);
            }

            // Find spikeLine to surface
            var spikeLinePoint1 = contourBoxMiddlePoint1;
            var spikeLinePoint2 = contourBoxMiddlePoint2;
            var spikeLineLength = surfaceSlider - contourBoxMiddlePoint2.Y;
            var spikeAngle = FindAngle(contourBoxMiddlePoint2, contourBoxMiddlePoint1);
            spikeLinePoint1.X = (float) (contourBoxMiddlePoint2.X + Math.Cos(spikeAngle) * spikeLineLength);
            spikeLinePoint1.Y = (float) (contourBoxMiddlePoint2.Y + Math.Sin(spikeAngle) * spikeLineLength);

            // Find point of impact with surface
            PointF? camPoi = FindLinesIntersection(spikeLinePoint1,
                                                   spikeLinePoint2,
                                                   new PointF(0,
                                                              (float) surfaceSlider),
                                                   new PointF(resolutionWidth,
                                                              (float) surfaceSlider));

            // Translate cam surface POI to dartboard projection
            var frameSemiWidth = resolutionWidth / 2;
            var camFovSemiAngle = camFovAngle / 2;
            var projectionToCenter = new PointF();
            var surfacePoiToCenterDistance = FindDistance(new PointF((float) surfaceCenterSlider,
                                                                     (float) surfaceSlider),
                                                          camPoi.GetValueOrDefault());
            var surfaceLeftToPoiDistance = FindDistance(new PointF((float) surfaceCenterSlider - resolutionWidth / 3,
                                                                   (float) surfaceSlider),
                                                        camPoi.GetValueOrDefault());
            var surfaceRightToPoiDistance = FindDistance(new PointF((float) surfaceCenterSlider + resolutionWidth / 3,
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
            var angle = FindAngle(camSetupPoint, rayPoint);
            rayPoint.X = (float) (camSetupPoint.X + Math.Cos(angle) * 2000);
            rayPoint.Y = (float) (camSetupPoint.Y + Math.Sin(angle) * 2000);

            var ray = new Ray(camNumber, camSetupPoint, rayPoint, dartContour.Arc);

            throwService.SaveRay(ray);
        }

        public static PointF FindLinesIntersection(PointF line1Point1,
                                                   PointF line1Point2,
                                                   PointF line2Point1,
                                                   PointF line2Point2)
        {
            var tolerance = 0.001;
            var x1 = line1Point1.X;
            var y1 = line1Point1.Y;
            var x2 = line1Point2.X;
            var y2 = line1Point2.Y;
            var x3 = line2Point1.X;
            var y3 = line2Point1.Y;
            var x4 = line2Point2.X;
            var y4 = line2Point2.Y;
            float x;
            float y;

            if (Math.Abs(x1 - x2) < tolerance)
            {
                var m2 = (y4 - y3) / (x4 - x3);
                var c2 = -m2 * x3 + y3;
                x = x1;
                y = c2 + m2 * x1;
            }
            else if (Math.Abs(x3 - x4) < tolerance)
            {
                var m1 = (y2 - y1) / (x2 - x1);
                var c1 = -m1 * x1 + y1;
                x = x3;
                y = c1 + m1 * x3;
            }
            else
            {
                var m1 = (y2 - y1) / (x2 - x1);
                var c1 = -m1 * x1 + y1;
                var m2 = (y4 - y3) / (x4 - x3);
                var c2 = -m2 * x3 + y3;
                x = (c1 - c2) / (m2 - m1);
                y = c2 + m2 * x;
            }

            return new PointF(x, y);
        }

        private static PointF FindMiddle(PointF point1,
                                         PointF point2)
        {
            var mpX = (point1.X + point2.X) / 2;
            var mpY = (point1.Y + point2.Y) / 2;
            return new PointF(mpX, mpY);
        }

        public static float FindDistance(PointF point1,
                                         PointF point2)
        {
            return (float) Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        public static float FindAngle(PointF point1,
                                      PointF point2)
        {
            return (float) Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
        }

        public static PointF CalculateCamSetupPoint(double toCamCmDistance, string camSetupSector)
        {
            var toCamPixels = DartboardDiameterInPixels * toCamCmDistance / DartboardDiameterInCm;
            var multiplier = Converter.CamSetupSectorSettingValueToComboboxSelectedIndex(camSetupSector); // todo because useful

            var projectionCenterPoint = new PointF((float) DrawService.ProjectionFrameSide / 2,
                                                   (float) DrawService.ProjectionFrameSide / 2);

            var calibratedCamSetupPoint = new PointF
                                          {
                                              X = (int) (projectionCenterPoint.X + Math.Cos(StartRadSector_11 + multiplier * SemiSectorStepRad) * toCamPixels),
                                              Y = (int) (projectionCenterPoint.Y + Math.Sin(StartRadSector_11 + multiplier * SemiSectorStepRad) * toCamPixels)
                                          };
            return calibratedCamSetupPoint;
        }
    }
}