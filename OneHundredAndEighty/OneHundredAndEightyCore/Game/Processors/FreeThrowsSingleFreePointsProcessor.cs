#region Usings

using System.Collections.Generic;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.ScoreBoard;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsSingleFreePointsProcessor : ProcessorBase
    {
        public FreeThrowsSingleFreePointsProcessor(Game game,
                                                   List<Player> players,
                                                   DBService dbService,
                                                   ScoreBoardService scoreBoard)
            : base(game, players, dbService, scoreBoard)
        {
        }

        public override void OnThrow(DetectedThrow thrw)
        {
            PlayerOnThrow.HandPoints += thrw.TotalPoints;

            scoreBoard.AddPointsToSinglePlayer(thrw.TotalPoints);

            var dbThrow = ConvertAndSaveThrow(thrw, ThrowResult.Ordinary);

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