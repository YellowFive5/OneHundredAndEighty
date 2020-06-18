#region Usings

using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

#endregion

namespace OneHundredAndEightyCore.Tests.MeasureService
{
    public class WhenMeasuringAndCalculating : MeasureServiceTestBase
    {
        [TestCase(1, 1, 6, 1, 1, 3, 6, 3, float.NegativeInfinity, float.NaN)]
        [TestCase(1, 1, 5, 3, 1, 3, 5, 1, 3, 2)]
        [TestCase(1, 1, 1, 3, 6, 1, 6, 3, 1, float.NaN)]
        [TestCase(1, 1, 2, 3, 6, 1, 6, 3, 6, 11)]
        public void LinesIntersectionCalculatesCorrectly(float line1Point1X,
                                                         float line1Point1Y,
                                                         float line1Point2X,
                                                         float line1Point2Y,
                                                         float line2Point1X,
                                                         float line2Point1Y,
                                                         float line2Point2X,
                                                         float line2Point2Y,
                                                         float intersectionLinePoint1X,
                                                         float intersectionLinePoint2Y)
        {
            var line1Point1 = new PointF(line1Point1X, line1Point1Y);
            var line1Point2 = new PointF(line1Point2X, line1Point2Y);
            var line2Point1 = new PointF(line2Point1X, line2Point1Y);
            var line2Point2 = new PointF(line2Point2X, line2Point2Y);

            var intersectionLinePoint = Recognition.MeasureService.FindLinesIntersection(line1Point1,
                                                                                         line1Point2,
                                                                                         line2Point1,
                                                                                         line2Point2);
            intersectionLinePoint.X.Should().Be(intersectionLinePoint1X);
            intersectionLinePoint.Y.Should().Be(intersectionLinePoint2Y);
        }

        [Test]
        public void MiddleFindsCorrectly()
        {
            var linePoint1 = new PointF(1, 1);
            var linePoint2 = new PointF(5, 1);

            var centerPoint = Recognition.MeasureService.FindMiddle(linePoint1, linePoint2);

            centerPoint.X.Should().Be(3);
            centerPoint.Y.Should().Be(1);
        }

        [Test]
        public void DistanceFindsCorrectly()
        {
            var linePoint1 = new PointF(1, 1);
            var linePoint2 = new PointF(5, 1);

            var distance = Recognition.MeasureService.FindDistance(linePoint1, linePoint2);

            distance.Should().Be(4);
        }

        [Test]
        public void AngleFindsCorrectly()
        {
            var linePoint1 = new PointF(1, 1);
            var linePoint2 = new PointF(5, 3);

            var angle = Recognition.MeasureService.FindAngle(linePoint1, linePoint2);

            angle.Should().Be(0.4636476f);
        }

        [Test]
        public void CamSetupPointCalculatesCorrectly()
        {
            var setupPoint = Recognition.MeasureService.CalculateCamSetupPoint(35.5, "20/1");

            setupPoint.X.Should().Be(816);
            setupPoint.Y.Should().Be(-401);
        }
    }
}