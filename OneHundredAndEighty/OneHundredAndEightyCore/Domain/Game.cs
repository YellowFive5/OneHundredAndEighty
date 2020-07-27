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
        public GameType Type { get; }
        public readonly int sets;
        public readonly int legs;
        public readonly int legPoints;
        public DateTime StartTimeStamp { get; }
        public DateTime EndTimeStamp { get; private set; }
        public List<Player> Players { get; }
        public Player PlayerOnThrow { get; set; }
        public Player PlayerOnLeg { get; set; }
        public Stack<Throw> Throws { get; set; }
        public List<Hand180> Hands180 { get; set; }

        public Game(GameType type,
                    List<Player> players,
                    int legs,
                    int sets,
                    GamePoints points)
        {
            Type = type;
            this.legs = legs;
            this.sets = sets;
            legPoints = Converter.GamePointsToInt(points);
            StartTimeStamp = DateTime.Now;
            Players = players;

            Throws = new Stack<Throw>();
            Hands180 = new List<Hand180>();
        }
    }
}