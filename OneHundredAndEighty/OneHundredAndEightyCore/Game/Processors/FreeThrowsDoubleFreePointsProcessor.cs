#region Usings

using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsDoubleFreePointsProcessor : ProcessorBase
    {
        public FreeThrowsDoubleFreePointsProcessor(Domain.Game game,
                                                   ScoreBoardService scoreBoard)
            : base(game, scoreBoard)
        {
        }

        protected override void OnThrowInternal(DetectedThrow thrw)
        {
            Game.PlayerOnThrow.HandPoints += thrw.TotalPoints;

            scoreBoard.AddPointsTo(Game.PlayerOnThrow, thrw.TotalPoints);

            var dbThrow = ConvertAndSaveThrow(thrw, ThrowResult.Ordinary);

            Game.PlayerOnThrow.HandThrows.Add(dbThrow);

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

        protected override void ThrowUndoInternal(GameSnapshot gameSnapshot)
        {
            throw new System.NotImplementedException();
        }
    }
}