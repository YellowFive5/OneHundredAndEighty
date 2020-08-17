#region Usings

using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsDoubleWriteOffPointsProcessor : ProcessorBase
    {
        public FreeThrowsDoubleWriteOffPointsProcessor(Domain.Game game,
                                                       ScoreBoardService scoreBoard)
            : base(game, scoreBoard)
        {
        }

        protected override void OnThrowInternal(DetectedThrow thrw)
        {
            if (IsLegOver(thrw))
            {
                ConvertAndSaveThrow(thrw, ThrowResult.LegWon);

                foreach (var player in Game.Players)
                {
                    player.LegPoints = Game.legPoints;
                    scoreBoard.SetPointsTo(player, Game.legPoints);
                    scoreBoard.CheckPointsHintFor(player);
                }

                ClearPlayerOnThrowHand();
                TogglePlayerOnThrow();

                return;
            }

            if (IsFault(thrw))
            {
                ConvertAndSaveThrow(thrw, ThrowResult.Fault);
                OnFault();

                return;
            }

            Game.PlayerOnThrow.HandPoints += thrw.TotalPoints;
            Game.PlayerOnThrow.LegPoints -= thrw.TotalPoints;
            scoreBoard.AddPointsTo(Game.PlayerOnThrow, thrw.TotalPoints * -1);

            OnHandOverDoublePlayersCheck(thrw);
        }

        protected override void ThrowUndoInternal(GameSnapshot gameSnapshot)
        {
            scoreBoard.OnThrowPointerSetOn(Game.PlayerOnThrow);

            foreach (var player in Game.Players)
            {
                scoreBoard.SetPointsTo(player, player.LegPoints);
                scoreBoard.CheckPointsHintFor(player);
            }

            scoreBoard.SetThrowNumber(Game.PlayerOnThrow.ThrowNumber);
        }
    }
}