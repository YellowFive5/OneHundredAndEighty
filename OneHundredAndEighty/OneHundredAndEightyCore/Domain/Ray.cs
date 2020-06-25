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
        public double ContourArea { get; }

        public Ray(CamNumber camNumber, PointF camPoint, PointF rayPoint, double contourArea)
        {
            CamNumber = camNumber;
            CamPoint = camPoint;
            RayPoint = rayPoint;
            ContourArea = contourArea;
        }

        public override string ToString()
        {
            return $"Cam_{CamNumber}. From {CamPoint} to {RayPoint}, area {ContourArea}";
        }
    }
}