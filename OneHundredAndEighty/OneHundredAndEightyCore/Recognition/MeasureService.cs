#region Usings

using System;
using System.Drawing;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public static class MeasureService
    {
        public const double StartRadSector_11 = -3.14159;
        public const double StartRadSector_1114 = -2.9845105;
        public const double SectorStepRad = 0.314159;
        public const double SemiSectorStepRad = SectorStepRad / 2;
        private const int DartboardDiameterInPixels = 1020;
        private const int DartboardDiameterInCm = 34;

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

        public static PointF FindMiddle(PointF point1,
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
            var multiplier = Converter.CamSetupSectorSettingValueToComboboxSelectedIndex(camSetupSector); // todo weird but useful

            var projectionCenterPoint = new PointF((float) DrawService.ProjectionFrameSide / 2,
                                                   (float) DrawService.ProjectionFrameSide / 2);

            var calibratedCamSetupPoint = new PointF((int) (projectionCenterPoint.X + Math.Cos(StartRadSector_11 + multiplier * SemiSectorStepRad) * toCamPixels),
                                                     (int) (projectionCenterPoint.Y + Math.Sin(StartRadSector_11 + multiplier * SemiSectorStepRad) * toCamPixels));
            return calibratedCamSetupPoint;
        }
    }
}