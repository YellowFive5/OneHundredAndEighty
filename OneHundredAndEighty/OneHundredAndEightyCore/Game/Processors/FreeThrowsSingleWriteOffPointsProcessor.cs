#region Usings

using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsSingleWriteOffPointsProcessor : ProcessorBase
    {
        public FreeThrowsSingleWriteOffPointsProcessor(Domain.Game game,
                                                       ScoreBoardService scoreBoard)
            : base(game, scoreBoard)
        {
        }

        protected override void OnThrowInternal(DetectedThrow thrw)
        {
            if (IsLegOver(thrw))
            {
                Game.PlayerOnThrow.LegPoints = Game.legPoints;
                scoreBoard.SetPointsTo(Game.PlayerOnThrow, Game.legPoints);

                ConvertAndSaveThrow(thrw, ThrowResult.LegWon);

                ClearPlayerOnThrowHand();
                scoreBoard.CheckPointsHintFor(Game.PlayerOnThrow);

                return;
            }

            if (IsFault(thrw))
            {
                OnFault(thrw);
                return;
            }

            Game.PlayerOnThrow.HandPoints += thrw.TotalPoints;
            Game.PlayerOnThrow.LegPoints -= thrw.TotalPoints;
            scoreBoard.AddPointsTo(Game.PlayerOnThrow, thrw.TotalPoints * -1);

            OnHandOverSinglePlayerCheck(thrw);

            scoreBoard.CheckPointsHintFor(Game.PlayerOnThrow);
        }

        protected override void ThrowUndoInternal(GameSnapshot gameSnapshot)
        {
            scoreBoard.SetPointsTo(Game.PlayerOnThrow, Game.PlayerOnThrow.LegPoints);

            scoreBoard.SetThrowNumber(Game.PlayerOnThrow.ThrowNumber);
            scoreBoard.CheckPointsHintFor(Game.PlayerOnThrow);
        }
    }
}