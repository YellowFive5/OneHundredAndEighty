#region Usings

using System.Collections.Generic;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.ScoreBoard;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsSingleFreePointsProcessor : IGameProcessor
    {
        public void OnThrow(DetectedThrow thrw, List<Player> players, Player playerOnThrow, Game game, ScoreBoardService scoreBoard, DBService dbService)
        {
            playerOnThrow.Points += thrw.TotalPoints;

            scoreBoard.AddPointsToSinglePlayer(thrw.TotalPoints);

            var dbThrow = new Throw(playerOnThrow,
                                    game,
                                    thrw.Sector,
                                    thrw.Type,
                                    ThrowResult.Ordinary,
                                    playerOnThrow.ThrowNumber,
                                    thrw.TotalPoints,
                                    thrw.Poi,
                                    thrw.ProjectionResolution);

            dbService.ThrowSaveNew(dbThrow);

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