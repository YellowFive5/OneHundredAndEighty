#region Usings

using System.Collections.Generic;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.ScoreBoard;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsSingleFreePointsProcessor : ProcessorBase, IGameProcessor
    {
        public void OnThrow(DetectedThrow thrw,
                            List<Player> players,
                            Player playerOnThrow,
                            Game game,
                            ScoreBoardService scoreBoard,
                            DBService dbService)
        {
            playerOnThrow.HandPoints += thrw.TotalPoints;

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

            ProceedThrow(playerOnThrow, dbThrow, game, dbService);
        }

        private void ProceedThrow(Player playerOnThrow, Throw dbThrow, Game game, DBService dbService)
        {
            playerOnThrow.HandThrows.Push(dbThrow);

            if (IsHandOver(playerOnThrow))
            {
                Check180(game, playerOnThrow, dbService);

                ClearThrows(playerOnThrow);
            }
            else
            {
                playerOnThrow.ThrowNumber += 1;
            }
        }
    }
}