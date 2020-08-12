#region Usings

using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsSingleFreePointsProcessor : ProcessorBase
    {
        public FreeThrowsSingleFreePointsProcessor(Domain.Game game,
                                                   ScoreBoardService scoreBoard)
            : base(game, scoreBoard)
        {
        }

        protected override void OnThrowInternal(DetectedThrow thrw)
        {
            Game.PlayerOnThrow.HandPoints += thrw.TotalPoints;
            Game.PlayerOnThrow.LegPoints += thrw.TotalPoints;

            scoreBoard.AddPointsTo(Game.PlayerOnThrow, thrw.TotalPoints);

            var dbThrow = ConvertAndSaveThrow(thrw);
            Game.PlayerOnThrow.HandThrows.Add(dbThrow);

            if (IsHandOver())
            {
                Check180();
                ClearPlayerOnThrowHand();
            }
            else
            {
                Game.PlayerOnThrow.ThrowNumber += 1;
                scoreBoard.SetThrowNumber(Game.PlayerOnThrow.ThrowNumber);
            }
        }

        protected override void ThrowUndoInternal(GameSnapshot gameSnapshot)
        {
            scoreBoard.SetPointsTo(Game.PlayerOnThrow, Game.PlayerOnThrow.LegPoints);

            scoreBoard.SetThrowNumber(Game.PlayerOnThrow.ThrowNumber);
        }
    }
}