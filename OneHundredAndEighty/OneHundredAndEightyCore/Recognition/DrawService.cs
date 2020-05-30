#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Windows.CamsDetection;
using Point = System.Drawing.Point;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public class DrawService
    {
        private readonly CamsDetectionBoard camsDetectionBoard;
        private readonly ConfigService configService;
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

        public const int ProjectionFrameSide = 1300;
        private const int PoiRadius = 6;
        private const int PoiThickness = 6;
        private const int ProjectionGridThickness = 2;
        private const double ProjectionDigitsScale = 2;
        private const int ProjectionDigitsThickness = 2;
        public const int ProjectionCoefficient = 3;
        public static PointF projectionCenterPoint;

        private Image<Bgr, byte> DartboardProjectionFrameBackground { get; }
        private Image<Bgr, byte> DartboardProjectionWorkingFrame { get; set; }

        public DrawService(CamsDetectionBoard camsDetectionBoard, ConfigService configService, Logger logger)
        {
            this.camsDetectionBoard = camsDetectionBoard;
            this.configService = configService;
            this.logger = logger;
            DartboardProjectionFrameBackground = new Image<Bgr, byte>(ProjectionFrameSide,
                                                                      ProjectionFrameSide);
            projectionCenterPoint = new PointF((float) DartboardProjectionFrameBackground.Width / 2,
                                               (float) DartboardProjectionFrameBackground.Height / 2);
        }

        public void DrawLine(Image<Bgr, byte> image,
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

        public void DrawRectangle(Image<Bgr, byte> image,
                                  Rectangle rectangle,
                                  MCvScalar color,
                                  int thickness)
        {
            CvInvoke.Rectangle(image, rectangle, color, thickness);
        }

        public void DrawCircle(Image<Bgr, byte> image,
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

        public void DrawString(Image<Bgr, byte> image,
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

        public void PrintThrow(DetectedThrow thrw)
        {
            camsDetectionBoard.dispatcher.Invoke(() => { camsDetectionBoard.PrintThrow(thrw); });
        }

        public void ProjectionDrawThrow(PointF poi, bool exclusiveDraw = true)
        {
            if (exclusiveDraw)
            {
                DartboardProjectionWorkingFrame = DartboardProjectionFrameBackground.Clone();
            }

            DrawCircle(DartboardProjectionWorkingFrame, poi, PoiRadius, poiColor, PoiThickness);

            camsDetectionBoard.dispatcher.Invoke(() => { camsDetectionBoard.SetProjectionImage(ToBitmap(DartboardProjectionWorkingFrame)); });
        }

        public void ProjectionDrawLine(PointF point1, PointF point2, MCvScalar color, bool clearBeforeDraw = true)
        {
            if (clearBeforeDraw)
            {
                DartboardProjectionWorkingFrame = DartboardProjectionFrameBackground.Clone();
            }

            DrawLine(DartboardProjectionWorkingFrame, point1, point2, color, PoiThickness);

            camsDetectionBoard.dispatcher.Invoke(() => { camsDetectionBoard.SetProjectionImage(ToBitmap(DartboardProjectionWorkingFrame)); });
        }

        public void ProjectionPrepare()
        {
            // Draw dartboard projection
            DrawCircle(DartboardProjectionFrameBackground, projectionCenterPoint, ProjectionCoefficient * 7, projectionGridColor, ProjectionGridThickness);
            DrawCircle(DartboardProjectionFrameBackground, projectionCenterPoint, ProjectionCoefficient * 17, projectionGridColor, ProjectionGridThickness);
            DrawCircle(DartboardProjectionFrameBackground, projectionCenterPoint, ProjectionCoefficient * 95, projectionGridColor, ProjectionGridThickness);
            DrawCircle(DartboardProjectionFrameBackground, projectionCenterPoint, ProjectionCoefficient * 105, projectionGridColor, ProjectionGridThickness);
            DrawCircle(DartboardProjectionFrameBackground, projectionCenterPoint, ProjectionCoefficient * 160, projectionGridColor, ProjectionGridThickness);
            DrawCircle(DartboardProjectionFrameBackground, projectionCenterPoint, ProjectionCoefficient * 170, projectionGridColor, ProjectionGridThickness);
            for (var i = 0; i <= 360; i += 9)
            {
                var segmentPoint1 = new PointF((float) (projectionCenterPoint.X + Math.Cos(MeasureService.SectorStepRad * i - MeasureService.SemiSectorStepRad) * ProjectionCoefficient * 170),
                                               (float) (projectionCenterPoint.Y + Math.Sin(MeasureService.SectorStepRad * i - MeasureService.SemiSectorStepRad) * ProjectionCoefficient * 170));
                var segmentPoint2 = new PointF((float) (projectionCenterPoint.X + Math.Cos(MeasureService.SectorStepRad * i - MeasureService.SemiSectorStepRad) * ProjectionCoefficient * 17),
                                               (float) (projectionCenterPoint.Y + Math.Sin(MeasureService.SectorStepRad * i - MeasureService.SemiSectorStepRad) * ProjectionCoefficient * 17));
                DrawLine(DartboardProjectionFrameBackground, segmentPoint1, segmentPoint2, projectionGridColor, ProjectionGridThickness);
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
                DrawString(DartboardProjectionFrameBackground,
                           sector.ToString(),
                           (int) (projectionCenterPoint.X - 40 + Math.Cos(radSector) * ProjectionCoefficient * 190),
                           (int) (projectionCenterPoint.Y + 20 + Math.Sin(radSector) * ProjectionCoefficient * 190),
                           ProjectionDigitsScale,
                           projectionDigitsColor,
                           ProjectionDigitsThickness);
                radSector += MeasureService.SectorStepRad;
            }

            DartboardProjectionWorkingFrame = DartboardProjectionFrameBackground.Clone();

            camsDetectionBoard.dispatcher.Invoke(() => { camsDetectionBoard.SetProjectionImage(ToBitmap(DartboardProjectionWorkingFrame)); });
        }

        public void ProjectionClear()
        {
            DartboardProjectionWorkingFrame = DartboardProjectionFrameBackground.Clone();
            camsDetectionBoard.dispatcher.Invoke(() =>
                                                 {
                                                     camsDetectionBoard.SetProjectionImage(ToBitmap(DartboardProjectionFrameBackground));
                                                     camsDetectionBoard.ClearPointsBox();
                                                 });
        }

        public void PointsHistoryBoxClear()
        {
            camsDetectionBoard.dispatcher.Invoke(() => { camsDetectionBoard.ClearHistoryPointsBox(); });
        }

        public BitmapImage ToBitmap(Image<Bgr, byte> image)
        {
            var imageToSave = new BitmapImage();

            using (var stream = new MemoryStream())
            {
                image.Bitmap.Save(stream, ImageFormat.Bmp);
                imageToSave.BeginInit();
                imageToSave.StreamSource = new MemoryStream(stream.ToArray());
                imageToSave.EndInit();
            }

            return imageToSave;
        } // todo something happened with IImage interface

        public BitmapImage ToBitmap(Image<Gray, byte> image)
        {
            var imageToSave = new BitmapImage();

            using (var stream = new MemoryStream())
            {
                image.Bitmap.Save(stream, ImageFormat.Bmp);
                imageToSave.BeginInit();
                imageToSave.StreamSource = new MemoryStream(stream.ToArray());
                imageToSave.EndInit();
            }

            return imageToSave;
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