#region Usings

using System.Collections.Generic;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.ScoreBoard;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsDoubleFreePointsProcessor : ProcessorBase, IGameProcessor
    {
        public FreeThrowsDoubleFreePointsProcessor(Game game, List<Player> players) : base(game, players)
        {
        }

        public void OnThrow(DetectedThrow thrw,
                            ScoreBoardService scoreBoard,
                            DBService dbService)
        {
            PlayerOnThrow.HandPoints += thrw.TotalPoints;

            scoreBoard.AddPointsToClassic(thrw.TotalPoints, PlayerOnThrow);

            var dbThrow = new Throw(PlayerOnThrow,
                                    Game,
                                    thrw.Sector,
                                    thrw.Type,
                                    ThrowResult.Ordinary,
                                    PlayerOnThrow.ThrowNumber,
                                    thrw.TotalPoints,
                                    thrw.Poi,
                                    thrw.ProjectionResolution);

            dbService.ThrowSaveNew(dbThrow);

            ProceedThrow(dbThrow, dbService);
        }

        private void ProceedThrow(Throw dbThrow, DBService dbService)
        {
            PlayerOnThrow.HandThrows.Push(dbThrow);

            if (IsHandOver())
            {
                Check180(dbService);

                ClearThrows();

                TogglePlayerOnThrow();
            }
            else
            {
                PlayerOnThrow.ThrowNumber += 1;
            }
        }
    }
}