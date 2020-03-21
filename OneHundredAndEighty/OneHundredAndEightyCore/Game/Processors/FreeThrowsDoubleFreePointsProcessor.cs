#region Usings

using System.Collections.Generic;
using System.Linq;
using OneHundredAndEightyCore.ScoreBoard;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsDoubleFreePointsProcessor : IGameProcessor
    {
        public void OnThrow(int thrwTotalPoints, List<Player> players, Player playerOnThrow, ScoreBoardService scoreBoard)
        {
            AddThrowNumberAndTogglePlayerOnThrow(players, playerOnThrow);
        }

        private void AddThrowNumberAndTogglePlayerOnThrow(List<Player> players, Player playerOnThrow)
        {
            if (playerOnThrow.ThrowNumber == 3)
            {
                playerOnThrow.ThrowNumber = 1;
                playerOnThrow = players.First(p => p != playerOnThrow);
            }
            else
            {
                playerOnThrow.ThrowNumber += 1;
            }
        }
    }
}