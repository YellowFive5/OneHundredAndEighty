#region Usings

using System.Collections.Generic;
using OneHundredAndEightyCore.ScoreBoard;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsSingleFreePointsProcessor : IGameProcessor
    {
        public void OnThrow(int thrwTotalPoints, List<Player> players, Player playerOnThrow, ScoreBoardService scoreBoard)
        {
            playerOnThrow.Points += thrwTotalPoints;
            scoreBoard.AddPoints(thrwTotalPoints);

            AddThrowNumber(playerOnThrow);
        }

        private void AddThrowNumber(Player playerOnThrow)
        {
            if (playerOnThrow.ThrowNumber == 3)
            {
                playerOnThrow.ThrowNumber = 1;
            }
            else
            {
                playerOnThrow.ThrowNumber += 1;
            }
        }
    }
}