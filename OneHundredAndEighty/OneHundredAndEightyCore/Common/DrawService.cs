#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using NLog;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Recognition;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public class DrawService
    {
        private readonly Logger logger;

        private readonly Bgr camRoiRectColor = new Bgr(Color.LawnGreen);
        private readonly int camRoiRectThickness = 5;
        private readonly Bgr camSurfaceLineColor = new Bgr(Color.Red);
        private readonly int camSurfaceLineThickness = 5;

        private readonly MCvScalar projectionGridColor = new Bgr(Color.DarkGray).MCvScalar;
        private readonly Bgr projectionDigitsColor = new Bgr(Color.White);

        private readonly MCvScalar projectionRayColor = new Bgr(Color.DeepSkyBlue).MCvScalar;
        private readonly MCvScalar projectionThrowRayColor = new Bgr(Color.Blue).MCvScalar;
        private readonly MCvScalar poiColor = new Bgr(Color.MediumVioletRed).MCvScalar;

        public readonly SolidColorBrush redBrush = new SolidColorBrush(Colors.Red);
        public readonly SolidColorBrush yellowBrush = new SolidColorBrush(Colors.Yellow);
        public readonly SolidColorBrush greenBrush = new SolidColorBrush(Colors.ForestGreen);
        public readonly SolidColorBrush blackBrush = new SolidColorBrush(Colors.Black);
        public readonly SolidColorBrush transparentBrush = new SolidColorBrush() {Opacity = 0};

        public Image<Bgr, byte> ProjectionBackgroundImage { get; private set; }
        public const int ProjectionFrameSide = 1300;
        private const int PoiRadius = 6;
        private const int PoiThickness = 6;
        private const int ProjectionGridThickness = 2;
        private const double ProjectionDigitsScale = 2;
        private const int ProjectionDigitsThickness = 2;
        public const int ProjectionCoefficient = 3;

        public DrawService(Logger logger)
        {
            this.logger = logger;
            ProjectionPrepare();
        }

        private void ProjectionPrepare()
        {
            ProjectionBackgroundImage = new Image<Bgr, byte>(ProjectionFrameSide,
                                                             ProjectionFrameSide);

            var projectionCenterPoint = new PointF((float) ProjectionBackgroundImage.Width / 2,
                                                   (float) ProjectionBackgroundImage.Height / 2);

            // Draw dartboard projection
            DrawCircle(ProjectionBackgroundImage, projectionCenterPoint, ProjectionCoefficient * 7, projectionGridColor, ProjectionGridThickness);
            DrawCircle(ProjectionBackgroundImage, projectionCenterPoint, ProjectionCoefficient * 17, projectionGridColor, ProjectionGridThickness);
            DrawCircle(ProjectionBackgroundImage, projectionCenterPoint, ProjectionCoefficient * 95, projectionGridColor, ProjectionGridThickness);
            DrawCircle(ProjectionBackgroundImage, projectionCenterPoint, ProjectionCoefficient * 105, projectionGridColor, ProjectionGridThickness);
            DrawCircle(ProjectionBackgroundImage, projectionCenterPoint, ProjectionCoefficient * 160, projectionGridColor, ProjectionGridThickness);
            DrawCircle(ProjectionBackgroundImage, projectionCenterPoint, ProjectionCoefficient * 170, projectionGridColor, ProjectionGridThickness);
            for (var i = 0; i <= 360; i += 9)
            {
                var segmentPoint1 = new PointF((float) (projectionCenterPoint.X + Math.Cos(MeasureService.SectorStepRad * i - MeasureService.SemiSectorStepRad) * ProjectionCoefficient * 170),
                                               (float) (projectionCenterPoint.Y + Math.Sin(MeasureService.SectorStepRad * i - MeasureService.SemiSectorStepRad) * ProjectionCoefficient * 170));
                var segmentPoint2 = new PointF((float) (projectionCenterPoint.X + Math.Cos(MeasureService.SectorStepRad * i - MeasureService.SemiSectorStepRad) * ProjectionCoefficient * 17),
                                               (float) (projectionCenterPoint.Y + Math.Sin(MeasureService.SectorStepRad * i - MeasureService.SemiSectorStepRad) * ProjectionCoefficient * 17));
                DrawLine(ProjectionBackgroundImage, segmentPoint1, segmentPoint2, projectionGridColor, ProjectionGridThickness);
            }

            // Draw digits
            var sectors = new List<int>()
                          {
                              11, 14, 9, 12, 5,
                              20, 1, 18, 4, 13,
                              6, 10, 15, 2, 17,
                              3, 19, 7, 16, 8
                          };
            var startRadSector = MeasureService.StartRadSector_11;
            var radSector = startRadSector;
            foreach (var sector in sectors)
            {
                DrawString(ProjectionBackgroundImage,
                           sector.ToString(),
                           (int) (projectionCenterPoint.X - 40 + Math.Cos(radSector) * ProjectionCoefficient * 190),
                           (int) (projectionCenterPoint.Y + 20 + Math.Sin(radSector) * ProjectionCoefficient * 190),
                           ProjectionDigitsScale,
                           projectionDigitsColor,
                           ProjectionDigitsThickness);
                radSector += MeasureService.SectorStepRad;
            }
        }

        public Image<Bgr, byte> ProjectionDrawThrow(DetectedThrow thrw)
        {
            var image = ProjectionBackgroundImage.Clone();
            DrawLine(image, thrw.FirstRay.RayPoint, thrw.FirstRay.CamPoint, projectionThrowRayColor, PoiThickness);
            DrawLine(image, thrw.SecondRay.RayPoint, thrw.SecondRay.CamPoint, projectionThrowRayColor, PoiThickness);
            DrawCircle(image, thrw.Poi, PoiRadius, poiColor, PoiThickness);
            return image;
        }

        public Image<Bgr, byte> ProjectionDrawLine(Ray ray)
        {
            var image = ProjectionBackgroundImage.Clone();
            DrawLine(image, ray.CamPoint, ray.RayPoint, projectionRayColor, PoiThickness);
            return image;
        }

        public Image<Bgr, byte> DrawSetupLines(Image<Bgr, byte> image,
                                               List<double> slidersData)
        {
            var surfaceSlider = slidersData.ElementAt(0);
            var surfaceCenterSlider = slidersData.ElementAt(1);
            var roiPosYSlider = slidersData.ElementAt(2);
            var roiHeightSlider = slidersData.ElementAt(3);
            var resolutionWidth = slidersData.ElementAt(4);

            var roiRectangle = new Rectangle(0,
                                             (int) roiPosYSlider,
                                             (int) resolutionWidth,
                                             (int) roiHeightSlider);

            DrawRectangle(image,
                          roiRectangle,
                          camRoiRectColor.MCvScalar,
                          camRoiRectThickness);

            var surfacePoint1 = new PointF(0, (float) surfaceSlider);
            var surfacePoint2 = new PointF((int) resolutionWidth,
                                           (float) surfaceSlider);
            DrawLine(image,
                     surfacePoint1,
                     surfacePoint2,
                     camSurfaceLineColor.MCvScalar,
                     camSurfaceLineThickness);

            var surfaceCenterPoint1 = new PointF((float) surfaceCenterSlider,
                                                 (float) surfaceSlider);

            var surfaceCenterPoint2 = new PointF(surfaceCenterPoint1.X,
                                                 surfaceCenterPoint1.Y - 50);
            DrawLine(image,
                     surfaceCenterPoint1,
                     surfaceCenterPoint2,
                     camSurfaceLineColor.MCvScalar,
                     camSurfaceLineThickness);

            return image;
        }

        private void DrawLine(Image<Bgr, byte> image,
                              PointF point1,
                              PointF point2,
                              MCvScalar color,
                              int thickness)
        {
            CvInvoke.Line(image,
                          new Point((int) point1.X, (int) point1.Y),
                          new Point((int) point2.X, (int) point2.Y),
                          color,
                          thickness);
        }

        private void DrawRectangle(Image<Bgr, byte> image,
                                   Rectangle rectangle,
                                   MCvScalar color,
                                   int thickness)
        {
            CvInvoke.Rectangle(image, rectangle, color, thickness);
        }

        private void DrawCircle(Image<Bgr, byte> image,
                                PointF centerpoint,
                                int radius,
                                MCvScalar color,
                                int thickness)
        {
            CvInvoke.Circle(image,
                            new Point((int) centerpoint.X, (int) centerpoint.Y),
                            radius,
                            color,
                            thickness);
        }

        private void DrawString(Image<Bgr, byte> image,
                                string text,
                                int pointX,
                                int pointY,
                                double scale,
                                Bgr color,
                                int thickness)
        {
            image.Draw(text,
                       new Point(pointX, pointY),
                       FontFace.HersheySimplex,
                       scale,
                       color,
                       thickness);
        }

        public void SaveToFile(BitmapSource image, string path = null)
        {
            var pathString = path ?? "image.png";
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (var fileStream = new FileStream(pathString, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }
    }
}