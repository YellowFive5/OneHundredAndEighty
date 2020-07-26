#region Usings

using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsDoubleFreePointsProcessor : ProcessorBase
    {
        public FreeThrowsDoubleFreePointsProcessor(Domain.Game game,
                                                   DBService dbService,
                                                   ScoreBoardService scoreBoard)
            : base(game, dbService, scoreBoard)
        {
        }

        public override void OnThrow(DetectedThrow thrw)
        {
            Game.PlayerOnThrow.HandPoints += thrw.TotalPoints;

            scoreBoard.AddPointsTo(Game.PlayerOnThrow, thrw.TotalPoints);

            var dbThrow = ConvertAndSaveThrow(thrw, ThrowResult.Ordinary);

            Game.PlayerOnThrow.HandThrows.Push(dbThrow);

            if (IsHandOver())
            {
                Check180();

                ClearPlayerOnThrowHand();

                TogglePlayerOnThrow();
            }
            else
            {
                Game.PlayerOnThrow.ThrowNumber += 1;
                scoreBoard.SetThrowNumber(Game.PlayerOnThrow.ThrowNumber);
            }
        }
    }
}