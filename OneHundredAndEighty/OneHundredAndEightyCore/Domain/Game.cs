#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Domain
{
    public class Game
    {
        public int Id { get; private set; }
        public GameType Type { get; }
        public readonly int sets;
        public readonly int legs;
        public readonly int legPoints;
        public DateTime StartTimeStamp { get; }
        public DateTime EndTimeStamp { get; private set; }
        public List<Player> Players { get; }
        public Player PlayerOnThrow { get; set; }
        public Player PlayerOnLeg { get; set; }

        public Game(GameType type,
                    List<Player> players,
                    int legs,
                    int sets,
                    GamePoints points,
                    int id = -1)
        {
            Id = id;
            Type = type;
            this.legs = legs;
            this.sets = sets;
            legPoints = Converter.GamePointsToInt(points);
            Players = players;
            StartTimeStamp = DateTime.Now;

            if (players.Count == 1) //  todo maybe do another way cuz ugly
            {
                players[0].Order = PlayerOrder.First;
            }
            else
            {
                players[0].Order = PlayerOrder.First;
                players[1].Order = PlayerOrder.Second;
            }

            Players = players;
            PlayerOnThrow = Players.First();
            PlayerOnLeg = Players.First();
        }
    }
}