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
        public FreeThrowsSingleFreePointsProcessor(Game game,
                                                   List<Player> players,
                                                   DBService dbService,
                                                   ScoreBoardService scoreBoard)
            : base(game, players, dbService, scoreBoard)
        {
        }

        public void OnThrow(DetectedThrow thrw)
        {
            PlayerOnThrow.HandPoints += thrw.TotalPoints;

            scoreBoard.AddPointsToSinglePlayer(thrw.TotalPoints);

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

            ProceedThrow(dbThrow);
        }

        private void ProceedThrow(Throw dbThrow)
        {
            PlayerOnThrow.HandThrows.Push(dbThrow);

            if (IsHandOver())
            {
                Check180();

                ClearThrows();
            }
            else
            {
                PlayerOnThrow.ThrowNumber += 1;
            }
        }
    }
}