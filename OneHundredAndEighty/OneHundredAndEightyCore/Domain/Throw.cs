#region Usings

using System;
using System.Drawing;

#endregion

namespace OneHundredAndEightyCore.Domain
{
    public enum ThrowType
    {
        Single = 1,
        Double,
        Tremble,
        Zero,
        _25,
        Bulleye
    }

    public enum ThrowResult
    {
        Ordinary = 1,
        Fault,
        LegWon,
        SetWon,
        MatchWon
    }

    public class Throw
    {
        public int Id { get; private set; }
        public Player Player { get; }
        public Domain.Game Game { get; }
        public int Sector { get; }
        public ThrowType Type { get; }
        public ThrowResult Result { get; }
        public int Number { get; }
        public int Points { get; }
        public PointF Poi { get; }
        public int ProjectionResolution { get; }
        public DateTime TimeStamp { get; }

        public Throw(Player player, Domain.Game game, int sector, ThrowType type, ThrowResult result,
                     int number, int points, PointF poi, int projectionResolution, int id = -1)
        {
            Id = id;
            Player = player;
            Game = game;
            Sector = sector;
            Type = type;
            Result = result;
            Number = number;
            Points = points;
            Poi = poi;
            ProjectionResolution = projectionResolution;
            TimeStamp = DateTime.Now;
        }

        public void SetId(int id)
        {
            Id = id;
        }
    }
}