#region Usings

using System.Collections.Generic;
using System.Linq;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.ScoreBoard;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsDoubleFreePointsProcessor : ProcessorBase, IGameProcessor
    {
        public void OnThrow(DetectedThrow thrw,
                            List<Player> players,
                            Player playerOnThrow,
                            Game game,
                            ScoreBoardService scoreBoard,
                            DBService dbService)
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