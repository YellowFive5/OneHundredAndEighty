#region Usings

using System.Drawing;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Domain
{
    public class Ray
    {
        public CamNumber CamNumber { get; }
        public PointF CamPoint { get; }
        public PointF RayPoint { get; }
        public double ContourArc { get; }

        public Ray(CamNumber camNumber, PointF camPoint, PointF rayPoint, double contourArc)
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