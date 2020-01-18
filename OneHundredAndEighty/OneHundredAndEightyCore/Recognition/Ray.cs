#region Usings

using System.Drawing;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public class Ray
    {
        public int CamNumber { get; }
        public PointF CamPoint { get; }
        public PointF RayPoint { get; }
        public double ContourArc { get; }

        public Ray(int camNumber, PointF camPoint, PointF rayPoint, double contourArc)
        {
            CamNumber = camNumber;
            CamPoint = camPoint;
            RayPoint = rayPoint;
            ContourArc = contourArc;
        }

        public override string ToString()
        {
            return $"Cam_{CamNumber}. From {CamPoint} to {RayPoint}, arc {ContourArc}";
        }
    }
}