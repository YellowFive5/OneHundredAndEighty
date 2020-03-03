#region Usings

using System;
using System.Drawing;

#endregion

namespace OneHundredAndEightyCore.Game
{
    public enum ThrowType
    {
        Zero = 1,
        Single,
        Double,
        Tremble,
        _25,
        Bulleye
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
        public int Id { get; private set; }
        public Player Player { get; }
        public OneHundredAndEightyCore.Game.Game Game { get; }
        public int Sector { get; }
        public ThrowType Type { get; }
        public ThrowResultativity Resultativity { get; }
        public int Number { get; }
        public int Points { get; }
        public PointF Poi { get; }
        public int ProjectionResolution { get; }
        public DateTime TimeStamp { get; }

        public Throw(Player player, OneHundredAndEightyCore.Game.Game game, int sector, ThrowType type, ThrowResultativity resultativity,
                     int number, int points, PointF poi, int projectionResolution, int id = -1)
        {
            Id = id;
            Player = player;
            Game = game;
            Sector = sector;
            Type = type;
            Resultativity = resultativity;
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