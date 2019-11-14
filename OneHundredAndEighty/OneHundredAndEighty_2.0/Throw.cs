#region Usings

using System;
using System.Drawing;

#endregion

namespace OneHundredAndEighty_2._0
{
    public enum ThrowType
    {
        Zero = 1,
        Single,
        Double,
        Tremble,
        _25,
        Bull
    }

    public enum ThrowResultativity
    {
        LegWon = 1,
        SetWon,
        MatchWon,
        Ordinary,
        Fault
    }

    public class Throw
    {
        public Throw(int id, Player player, int gameId, int sector, ThrowType type, ThrowResultativity resultativity,
                     int number, int points, PointF poi, int projectionResolution)
        {
            Id = id;
            Player = player;
            GameId = gameId;
            Sector = sector;
            Type = type;
            Resultativity = resultativity;
            Number = number;
            Points = points;
            Poi = poi;
            ProjectionResolution = projectionResolution;
            TimeStamp = DateTime.Now;
        }

        public int Id { get; set; }
        public Player Player { get; set; }
        public int GameId { get; set; }
        public int Sector { get; set; }
        public ThrowType Type { get; set; }
        public ThrowResultativity Resultativity { get; set; }
        public int Number { get; set; }
        public int Points { get; }
        public PointF Poi { get; }
        public int ProjectionResolution { get; }
        public DateTime TimeStamp { get; set; }
    }
}