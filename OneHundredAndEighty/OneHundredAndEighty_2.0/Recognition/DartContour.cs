#region Usings

using Emgu.CV.Util;

#endregion

namespace OneHundredAndEighty_2._0.Recognition
{
    public class DartContour
    {
        public VectorOfPoint ContourPoints { get; }
        public double Arc { get; }

        public DartContour(VectorOfPoint points, double arc)
        {
            ContourPoints = new VectorOfPoint();
            ContourPoints.Push(points);
            Arc = arc;
        }
    }
}