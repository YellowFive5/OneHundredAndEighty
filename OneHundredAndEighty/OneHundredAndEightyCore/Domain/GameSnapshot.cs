#region Usings

using System.Collections.Generic;
using System.Linq;

#endregion

namespace OneHundredAndEightyCore.Domain
{
    public class GameSnapshot
    {
        public List<PlayerGameData> PlayersGameData { get; }
        public int PlayerOnThrowId { get; }
        public int PlayerOnLegId { get; }
        public List<Hand180> Hands180 { get; }

        public GameSnapshot(Game game)
        {
            PlayersGameData = new List<PlayerGameData>();
            foreach (var player in game.Players)
            {
                PlayersGameData.Add(player.GameData.Copy());
            }

            PlayerOnThrowId = game.PlayerOnThrow.Id;
            PlayerOnLegId = game.PlayerOnLeg.Id;

            Hands180 = game.Hands180.ToList();
        }
    }
}