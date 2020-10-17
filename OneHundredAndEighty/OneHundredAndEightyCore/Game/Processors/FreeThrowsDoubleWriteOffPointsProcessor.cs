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
                Game.PlayerOnThrow.GameData.LegsWon += 1;
                OnMatchOver(thrw);
                return;
            }

            if (IsFault(thrw))
            {
                OnFault(thrw);
                return;
            }

            Game.PlayerOnThrow.GameData.HandPoints += thrw.TotalPoints;
            Game.PlayerOnThrow.GameData.LegPoints -= thrw.TotalPoints;
            scoreBoard.AddPointsTo(Game.PlayerOnThrow, thrw.TotalPoints * -1);

            OnHandOverDoublePlayersCheck(thrw);
        }

        protected override void ThrowUndoInternal(GameSnapshot gameSnapshot)
        {
            scoreBoard.OnThrowPointerSetOn(Game.PlayerOnThrow);

            foreach (var player in Game.Players)
            {
                scoreBoard.SetPointsTo(player, player.GameData.LegPoints);
                scoreBoard.CheckPointsHintFor(player);
            }

            scoreBoard.SetThrowNumber(Game.PlayerOnThrow.GameData.ThrowNumber);
        }
    }
}