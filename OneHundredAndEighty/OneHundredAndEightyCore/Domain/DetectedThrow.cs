#region Usings

using System;
using System.Drawing;

#endregion

namespace OneHundredAndEightyCore.Domain
{
    public class DetectedThrow
    {
        public Ray FirstRay { get; }
        public Ray SecondRay { get; }
        public PointF Poi { get; }
        public ThrowType Type { get; }
        public int TotalPoints { get; }
        public int Sector { get; }
        public int Multiplier { get; }
        public int ProjectionResolution { get; }

        public DetectedThrow(PointF poi, Ray firstRay, Ray secondRay, int sector, ThrowType type, int projectionSide)
        {
            FirstRay = firstRay;
            SecondRay = secondRay;
            ProjectionResolution = projectionSide;
            Poi = poi;
            Sector = sector;
            Type = type;
            switch (type)
            {
                case ThrowType.Zero:
                    Multiplier = 0;
                    TotalPoints = 0;
                    break;
                case ThrowType.Single:
                    Multiplier = 1;
                    TotalPoints = sector * Multiplier;
                    break;
                case ThrowType.Double:
                    Multiplier = 2;
                    TotalPoints = sector * Multiplier;
                    break;
                case ThrowType.Tremble:
                    Multiplier = 3;
                    TotalPoints = sector * Multiplier;
                    break;
                case ThrowType._25:
                    Multiplier = 2;
                    TotalPoints = 25;
                    break;
                case ThrowType.Bulleye:
                    Multiplier = 3;
                    TotalPoints = 50;
                    break;
                default:
                    throw new Exception("Unknown ThrowType");
            }
        }

        public override string ToString()
        {
            string str;
            switch (Type)
            {
                case ThrowType.Bulleye:
                    str = "Bull";
                    break;
                case ThrowType._25:
                    str = "25";
                    break;
                case ThrowType.Zero:
                    str = "0";
                    break;
                default:
                    str = $"{Type} {Sector} = {TotalPoints}";
                    break;
            }

            return str;
        }
    }
}