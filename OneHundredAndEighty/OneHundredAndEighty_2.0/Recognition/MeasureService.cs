#region Usings

using System;
using System.Drawing;
using Autofac;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using NLog;

#endregion

namespace OneHundredAndEighty_2._0.Recognition
{
    public class MeasureService
    {
        private readonly CamService camService;
        private readonly DrawService drawService;
        private readonly ThrowService throwService;
        private readonly ConfigService configService;
        private readonly Logger logger;
        private DartContour dartContour;

        private readonly int minContourArc;
        private readonly double camFovAngle;

        public MeasureService(CamService camService)
        {
            this.camService = camService;
            logger = MainWindow.ServiceContainer.Resolve<Logger>();
            drawService = MainWindow.ServiceContainer.Resolve<DrawService>();
            throwService = MainWindow.ServiceContainer.Resolve<ThrowService>();
            configService = MainWindow.ServiceContainer.Resolve<ConfigService>();
            minContourArc = configService.Read<int>(SettingsType.MinContourArc);
            camFovAngle = configService.Read<double>(SettingsType.CamFovAngle);
        }

        public bool FindDartContour()
        {
            logger.Debug($"Find dartContour for cam_{camService.camNumber} start");

            var allContours = new VectorOfVectorOfPoint();
            var matHierarсhy = new Mat();
            CvInvoke.FindContours(camService.RoiLastThrowFrame,
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

            logger.Debug($"Find dartContour for cam_{camService.camNumber} end. Found:{found}. Contour arc:{contourArс}");
            return found;
        }

        public void ProcessDartContour()
        {
            logger.Debug($"Process dartContour for cam_{camService.camNumber} start");
            // Moments and centerpoint
            // var contourMoments = CvInvoke.Moments(processedContour);
            // var contourCenterPoint = new PointF((float) (contourMoments.M10 / contourMoments.M00),
            //                                     (float) camService.roiPosYSlider + (float) (contourMoments.M01 / contourMoments.M00));

            // Find contour rectangle
            var rect = CvInvoke.MinAreaRect(dartContour.ContourPoints);
            var box = CvInvoke.BoxPoints(rect);

            var contourBoxPoint1 = new PointF(box[0].X, (float)camService.roiPosYSlider + box[0].Y);
            var contourBoxPoint2 = new PointF(box[1].X, (float)camService.roiPosYSlider + box[1].Y);
            var contourBoxPoint3 = new PointF(box[2].X, (float)camService.roiPosYSlider + box[2].Y);
            var contourBoxPoint4 = new PointF(box[3].X, (float)camService.roiPosYSlider + box[3].Y);

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
            var spikeLineLength = camService.surfacePoint2.Y - contourBoxMiddlePoint2.Y;
            var spikeAngle = FindAngle(contourBoxMiddlePoint2, contourBoxMiddlePoint1);
            spikeLinePoint1.X = (float)(contourBoxMiddlePoint2.X + Math.Cos(spikeAngle) * spikeLineLength);
            spikeLinePoint1.Y = (float)(contourBoxMiddlePoint2.Y + Math.Sin(spikeAngle) * spikeLineLength);

            // Find point of impact with surface
            PointF? camPoi = FindLinesIntersection(spikeLinePoint1, spikeLinePoint2, camService.surfacePoint1, camService.surfacePoint2);

            // Translate cam surface POI to dartboard projection
            var frameWidth = camService.RoiLastThrowFrame.Cols;
            var frameSemiWidth = frameWidth / 2;
            var camFovSemiAngle = camFovAngle / 2;
            var projectionToCenter = new PointF();
            var surfacePoiToCenterDistance = FindDistance(camService.surfaceCenterPoint1, camPoi.GetValueOrDefault());
            var surfaceLeftToPoiDistance = FindDistance(camService.surfaceLeftPoint1, camPoi.GetValueOrDefault());
            var surfaceRightToPoiDistance = FindDistance(camService.surfaceRightPoint1, camPoi.GetValueOrDefault());
            var projectionCamToCenterDistance = frameSemiWidth / Math.Sin(Math.PI * camFovSemiAngle / 180.0) * Math.Cos(Math.PI * camFovSemiAngle / 180.0);
            var projectionCamToPoiDistance = Math.Sqrt(Math.Pow(projectionCamToCenterDistance, 2) + Math.Pow(surfacePoiToCenterDistance, 2));
            var projectionPoiToCenterDistance = Math.Sqrt(Math.Pow(projectionCamToPoiDistance, 2) - Math.Pow(projectionCamToCenterDistance, 2));
            var poiCamCenterAngle = Math.Asin(projectionPoiToCenterDistance / projectionCamToPoiDistance);

            projectionToCenter.X = (float)(camService.setupPoint.X - Math.Cos(camService.toBullAngle) * projectionCamToCenterDistance);
            projectionToCenter.Y = (float)(camService.setupPoint.Y - Math.Sin(camService.toBullAngle) * projectionCamToCenterDistance);

            if (surfaceLeftToPoiDistance < surfaceRightToPoiDistance)
            {
                poiCamCenterAngle *= -1;
            }

            var projectionPoi = new PointF
            {
                X = (float)(camService.setupPoint.X + Math.Cos(camService.toBullAngle + poiCamCenterAngle) * 2000),
                Y = (float)(camService.setupPoint.Y + Math.Sin(camService.toBullAngle + poiCamCenterAngle) * 2000)
            };

            // Draw line from cam through projection POI
            var rayPoint = projectionPoi;
            var angle = FindAngle(camService.setupPoint, rayPoint);
            rayPoint.X = (float)(camService.setupPoint.X + Math.Cos(angle) * 2000);
            rayPoint.Y = (float)(camService.setupPoint.Y + Math.Sin(angle) * 2000);

            drawService.ProjectionDrawLine(camService.setupPoint, rayPoint, new Bgr(Color.DodgerBlue).MCvScalar);

            var ray = new Ray(camService.camNumber, camService.setupPoint, rayPoint, dartContour.Arc);

            throwService.SaveRay(ray);

            logger.Debug($"Process dartContour for cam_{camService.camNumber} end. Ray saved:{ray}");
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
            return (float)Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        public static float FindAngle(PointF point1,
                                      PointF point2)
        {
            return (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
        }
    }
}