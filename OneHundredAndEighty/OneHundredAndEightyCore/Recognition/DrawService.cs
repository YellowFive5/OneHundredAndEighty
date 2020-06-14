#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using NLog;
using Point = System.Drawing.Point;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public class DrawService
    {
        private readonly Logger logger;

        public Bgr camRoiRectColor = new Bgr(Color.LawnGreen);
        public const int CamRoiRectThickness = 5;
        public Bgr camSurfaceLineColor = new Bgr(Color.Red);
        public const int CamSurfaceLineThickness = 5;
        public MCvScalar camContourRectColor = new Bgr(Color.Blue).MCvScalar;
        public int camContourRectThickness = 5;
        public MCvScalar camSpikeLineColor = new Bgr(Color.White).MCvScalar;
        public int camSpikeLineThickness = 4;
        public MCvScalar projectionPoiColor = new Bgr(Color.Yellow).MCvScalar;
        public int projectionPoiRadius = 6;
        public int projectionPoiThickness = 6;
        public MCvScalar camContourColor = new Bgr(Color.Violet).MCvScalar;
        public int camContourThickness = 2;
        private readonly MCvScalar projectionGridColor = new Bgr(Color.DarkGray).MCvScalar;
        public MCvScalar projectionSurfaceLineColor = new Bgr(Color.Red).MCvScalar;
        public int projectionSurfaceLineThickness = 2;
        private readonly MCvScalar projectionRayColor = new Bgr(Color.DeepSkyBlue).MCvScalar;
        public int projectionRayThickness = 2;
        private readonly MCvScalar poiColor = new Bgr(Color.MediumVioletRed).MCvScalar;
        private readonly Bgr projectionDigitsColor = new Bgr(Color.White);

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

        public Image<Bgr, byte> ProjectionDrawThrow(Image<Bgr, byte> projectionImage,
                                                    PointF poi)
        {
            DrawCircle(projectionImage, poi, PoiRadius, poiColor, PoiThickness);

            return projectionImage;
        }

        public Image<Bgr, byte> ProjectionDrawLine(Image<Bgr, byte> projectionImage,
                                                   PointF point1,
                                                   PointF point2,
                                                   MCvScalar color)
        {
            DrawLine(projectionImage, point1, point2, color, PoiThickness);

            return projectionImage;
        }

        public Image<Bgr, byte> DrawSetupLines(Image<Bgr, byte> image,
                                               List<double> sliderData)
        {
            var surfaceSlider = sliderData.ElementAt(1);
            var surfaceCenterSlider = sliderData.ElementAt(2);
            var roiPosYSlider = sliderData.ElementAt(3);
            var roiHeightSlider = sliderData.ElementAt(4);
            var resolutionWidth = sliderData.ElementAt(5);

            var roiRectangle = new Rectangle(0,
                                             (int) roiPosYSlider,
                                             (int) resolutionWidth,
                                             (int) roiHeightSlider);

            DrawRectangle(image,
                          roiRectangle,
                          camRoiRectColor.MCvScalar,
                          CamRoiRectThickness);

            var surfacePoint1 = new PointF(0, (float) surfaceSlider);
            var surfacePoint2 = new PointF((int) resolutionWidth,
                                           (float) surfaceSlider);
            DrawLine(image,
                     surfacePoint1,
                     surfacePoint2,
                     camSurfaceLineColor.MCvScalar,
                     CamSurfaceLineThickness);

            var surfaceCenterPoint1 = new PointF((float) surfaceCenterSlider,
                                                 (float) surfaceSlider);

            var surfaceCenterPoint2 = new PointF(surfaceCenterPoint1.X,
                                                 surfaceCenterPoint1.Y - 50);
            DrawLine(image,
                     surfaceCenterPoint1,
                     surfaceCenterPoint2,
                     camSurfaceLineColor.MCvScalar,
                     CamSurfaceLineThickness);

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