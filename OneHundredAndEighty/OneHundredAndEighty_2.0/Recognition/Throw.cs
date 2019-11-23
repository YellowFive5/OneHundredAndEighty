#region Usings

using System;
using System.Drawing;

#endregion

namespace OneHundredAndEighty_2._0.Recognition
{
    public enum ThrowType
    {
        Zero,
        Single,
        Double,
        Tremble,
        _25,
        Bull
    }

    public class Throw
    {
        public PointF Poi { get; }
        public ThrowType Type { get; }
        public int TotalPoints { get; }
        public int Sector { get; }
        public int Multiplier { get; }
        public int ProjectionResolution { get; }

        public Throw(PointF poi, int sector, ThrowType type, int projectionSide)
        {
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
                case ThrowType.Bull:
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
                case ThrowType.Bull:
                    str = "Bull";
                    break;
                case ThrowType._25:
                    str = "25";
                    break;
                case ThrowType.Zero:
                    str = "0";
                    break;
                default:
                    str = $"{Multiplier} x {Sector} = {TotalPoints}";
                    break;
            }

            return str;
        }
    }
}