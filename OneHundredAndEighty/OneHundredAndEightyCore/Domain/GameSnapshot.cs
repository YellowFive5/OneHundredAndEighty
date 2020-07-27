#region Usings

using System.Collections.Generic;
using System.Linq;

#endregion

namespace OneHundredAndEightyCore.Domain
{
    public class GameSnapshot
    {
        public List<Player> Players { get; }
        public Player PlayerOnThrow { get; }
        public Player PlayerOnLeg { get; }
        public List<Hand180> Hands180 { get; }

        public GameSnapshot(Domain.Game game)
        {
            Players = new List<Player>();
            foreach (var player in game.Players)
            {
                Players.Add(player.Copy());
            }

            PlayerOnThrow = game.PlayerOnThrow.Copy();
            PlayerOnLeg = game.PlayerOnLeg.Copy();

            Hands180 = game.Hands180.ToList();
        }
    }
}