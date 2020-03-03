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
using Point = System.Drawing.Point;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public class DrawService
    {
        private readonly MainWindow mainWindow;
        private readonly ConfigService configService;
        private readonly Logger logger;
        public Bgr camRoiRectColor = new Bgr(Color.LawnGreen);
        public readonly int camRoiRectThickness = 5;
        public Bgr camSurfaceLineColor = new Bgr(Color.Red);
        public readonly int camSurfaceLineThickness = 5;
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
        private readonly int poiRadius = 6;
        private readonly int poiThickness = 6;
        private readonly int projectionGridThickness = 2;
        private readonly Bgr projectionDigitsColor = new Bgr(Color.White);
        private readonly double projectionDigitsScale = 2;
        private readonly int projectionDigitsThickness = 2;
        public readonly int projectionCoefficent = 3;
        public PointF projectionCenterPoint;
        public readonly int projectionFrameSide;
        private Image<Bgr, byte> DartboardProjectionFrameBackground { get; }
        private Image<Bgr, byte> DartboardProjectionWorkingFrame { get; set; }

        public DrawService(MainWindow mainWindow, ConfigService configService, Logger logger)
        {
            this.mainWindow = mainWindow;
            this.configService = configService;
            this.logger = logger;
            projectionFrameSide = 1300;
            DartboardProjectionFrameBackground = new Image<Bgr, byte>(projectionFrameSide,
                                                                      projectionFrameSide);
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
            mainWindow.Dispatcher.Invoke(() =>
                                         {
                                             mainWindow.PointsBox.Text = thrw.ToString();
                                             mainWindow.PointsHistoryBox.Text = $"{mainWindow.PointsHistoryBox.Text}\n{thrw}";
                                             mainWindow.PointsHistoryBox.ScrollToEnd();
                                         });
        }

        public void ProjectionDrawThrow(PointF poi, bool exclusiveDraw = true)
        {
            if (exclusiveDraw)
            {
                DartboardProjectionWorkingFrame = DartboardProjectionFrameBackground.Clone();
            }

            DrawCircle(DartboardProjectionWorkingFrame, poi, poiRadius, poiColor, poiThickness);

            mainWindow.Dispatcher.Invoke(() => { mainWindow.DartboardProjectionImageBox.Source = ToBitmap(DartboardProjectionWorkingFrame); });
        }

        public void ProjectionDrawLine(PointF point1, PointF point2, MCvScalar color, bool clearBeforeDraw = true)
        {
            if (clearBeforeDraw)
            {
                DartboardProjectionWorkingFrame = DartboardProjectionFrameBackground.Clone();
            }

            DrawLine(DartboardProjectionWorkingFrame, point1, point2, color, poiThickness);

            mainWindow.Dispatcher.Invoke(() => { mainWindow.DartboardProjectionImageBox.Source = ToBitmap(DartboardProjectionWorkingFrame); });
        }

        public void ProjectionPrepare()
        {
            // Draw dartboard projection
            DrawCircle(DartboardProjectionFrameBackground, projectionCenterPoint, projectionCoefficent * 7, projectionGridColor, projectionGridThickness);
            DrawCircle(DartboardProjectionFrameBackground, projectionCenterPoint, projectionCoefficent * 17, projectionGridColor, projectionGridThickness);
            DrawCircle(DartboardProjectionFrameBackground, projectionCenterPoint, projectionCoefficent * 95, projectionGridColor, projectionGridThickness);
            DrawCircle(DartboardProjectionFrameBackground, projectionCenterPoint, projectionCoefficent * 105, projectionGridColor, projectionGridThickness);
            DrawCircle(DartboardProjectionFrameBackground, projectionCenterPoint, projectionCoefficent * 160, projectionGridColor, projectionGridThickness);
            DrawCircle(DartboardProjectionFrameBackground, projectionCenterPoint, projectionCoefficent * 170, projectionGridColor, projectionGridThickness);
            for (var i = 0; i <= 360; i += 9)
            {
                var segmentPoint1 = new PointF((float) (projectionCenterPoint.X + Math.Cos(0.314159 * i - 0.15708) * projectionCoefficent * 170),
                                               (float) (projectionCenterPoint.Y + Math.Sin(0.314159 * i - 0.15708) * projectionCoefficent * 170));
                var segmentPoint2 = new PointF((float) (projectionCenterPoint.X + Math.Cos(0.314159 * i - 0.15708) * projectionCoefficent * 17),
                                               (float) (projectionCenterPoint.Y + Math.Sin(0.314159 * i - 0.15708) * projectionCoefficent * 17));
                DrawLine(DartboardProjectionFrameBackground, segmentPoint1, segmentPoint2, projectionGridColor, projectionGridThickness);
            }

            // Draw digits
            var sectors = new List<int>()
                          {
                              11, 14, 9, 12, 5,
                              20, 1, 18, 4, 13,
                              6, 10, 15, 2, 17,
                              3, 19, 7, 16, 8
                          };
            var startRadSector = -3.14159;
            var radSectorStep = 0.314159;
            var radSector = startRadSector;
            foreach (var sector in sectors)
            {
                DrawString(DartboardProjectionFrameBackground,
                           sector.ToString(),
                           (int) (projectionCenterPoint.X - 40 + Math.Cos(radSector) * projectionCoefficent * 190),
                           (int) (projectionCenterPoint.Y + 20 + Math.Sin(radSector) * projectionCoefficent * 190),
                           projectionDigitsScale,
                           projectionDigitsColor,
                           projectionDigitsThickness);
                radSector += radSectorStep;
            }

            DartboardProjectionWorkingFrame = DartboardProjectionFrameBackground.Clone();

            mainWindow.Dispatcher.Invoke(() => { mainWindow.DartboardProjectionImageBox.Source = ToBitmap(DartboardProjectionWorkingFrame); });
        }

        public void ProjectionClear()
        {
            DartboardProjectionWorkingFrame = DartboardProjectionFrameBackground.Clone();
            mainWindow.Dispatcher.Invoke(() =>
                                         {
                                             mainWindow.DartboardProjectionImageBox.Source = ToBitmap(DartboardProjectionFrameBackground);
                                             mainWindow.PointsBox.Text = string.Empty;
                                         });
        }

        public void PointsHistoryBoxClear()
        {
            mainWindow.Dispatcher.Invoke(() => { mainWindow.PointsHistoryBox.Text = string.Empty; });
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