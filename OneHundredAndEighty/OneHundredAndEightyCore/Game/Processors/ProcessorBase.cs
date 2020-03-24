#region Usings

using System.Collections.Generic;
using System.Linq;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public abstract class ProcessorBase
    {
        protected ProcessorBase(Game game, List<Player> players)
        {
            Game = game;
            Players = players;
            PlayerOnThrow = Players.First();
            PlayerOnSet = Players.First();
        }

        protected List<Player> Players { get; set; }
        protected Player PlayerOnThrow { get; set; }
        protected Player PlayerOnSet { get; set; }
        protected Game Game { get; set; }

        protected void Check180(DBService dbService)
        {
            if (PlayerOnThrow.HandThrows.Sum(t => t.Points) == 180)
            {
                dbService._180SaveNew(Game, PlayerOnThrow);
            }
        }

        protected bool IsHandOver()
        {
            return PlayerOnThrow.ThrowNumber == 3;
        }

        protected void ClearThrows()
        {
            PlayerOnThrow.HandThrows.Clear();
            PlayerOnThrow.ThrowNumber = 1;
        }

        protected void TogglePlayerOnThrow()
        {
            PlayerOnThrow = Players.First(p => p != PlayerOnThrow);
        }
    }
}