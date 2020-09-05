#region Usings

using System;
using System.Collections.Generic;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Domain
{
    public class Game
    {
        public readonly int legs;
        public readonly int sets;
        public readonly int legPoints;
        public int Id { get; set; }
        public GameType Type { get; }
        public GamePoints Points { get; }
        public List<Player> Players { get; }
        public DateTime StartTimeStamp { get; }
        public DateTime EndTimeStamp { get; set; }
        public Player PlayerOnThrow { get; set; }
        public Player PlayerOnLeg { get; set; }
        public Stack<Throw> Throws { get; }
        public List<Hand180> Hands180 { get; set; }
        public GameResultType Result { get; set; }
        public Player Winner { get; set; }
        public bool IsSingle => Players.Count == 1;

        public Game(GameType type,
                    List<Player> players,
                    int legs,
                    int sets,
                    GamePoints points)
        {
            Type = type;
            Players = players;
            this.legs = legs;
            this.sets = sets;
            Points = points;
            legPoints = Converter.GamePointsToInt(points);
            StartTimeStamp = DateTime.Now;
            Throws = new Stack<Throw>();
            Hands180 = new List<Hand180>();
            Result = GameResultType.NotDefined;
        }
    }
}