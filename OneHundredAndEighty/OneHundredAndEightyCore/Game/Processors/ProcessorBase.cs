#region Usings

using System.Linq;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public abstract class ProcessorBase
    {
        protected void Check180(Game game, Player player, DBService dbService)
        {
            if (player.HandThrows.Sum(t => t.Points) == 180)
            {
                dbService._180SaveNew(game, player);
            }
        }

        protected static bool IsHandOver(Player player)
        {
            return player.ThrowNumber == 3;
        }

        protected static void ClearThrows(Player player)
        {
            player.HandThrows.Clear();
            player.ThrowNumber = 1;
        }
    }
}