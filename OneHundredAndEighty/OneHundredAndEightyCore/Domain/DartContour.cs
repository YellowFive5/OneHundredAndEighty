#region Usings

using Emgu.CV.Util;

#endregion

namespace OneHundredAndEightyCore.Domain
{
    public class DartContour
    {
        public VectorOfPoint ContourPoints { get; }
        public double Area { get; }

        public DartContour(VectorOfPoint points, double area)
        {
            ContourPoints = new VectorOfPoint();
            ContourPoints.Push(points);
            Area = area;
        }
    }
}