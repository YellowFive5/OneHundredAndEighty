#region Usings

using System.Linq;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class ClassicDoubleProcessor : ProcessorBase
    {
        public ClassicDoubleProcessor(Domain.Game game,
                                      ScoreBoardService scoreBoard)
            : base(game, scoreBoard)
        {
        }

        protected override void OnThrowInternal(DetectedThrow thrw)
        {
            if (IsGameOver(thrw))
            {
                ConvertAndSaveThrow(thrw, ThrowResult.MatchWon);
                InvokeEndMatch();
                return;
            }

            if (IsSetOver(thrw))
            {
                OnSetOver(thrw);
                return;
            }

            if (IsLegOver(thrw))
            {
                OnLegOver(thrw);
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

            OnHandOverDoublePlayersCheck(thrw);
        }

        private void OnLegOver(DetectedThrow thrw)
        {
            ConvertAndSaveThrow(thrw, ThrowResult.LegWon);
            Game.PlayerOnThrow.LegsWon += 1;
            scoreBoard.AddLegsWonTo(Game.PlayerOnThrow);
            ClearPlayerOnThrowHand();

            foreach (var player in Game.Players)
            {
                player.LegPoints = Game.legPoints;
                scoreBoard.SetPointsTo(player, Game.legPoints);
                scoreBoard.CheckPointsHintFor(player);
            }

            TogglePlayerOnLegAndOnThrow();
        }

        private void OnSetOver(DetectedThrow thrw)
        {
            ConvertAndSaveThrow(thrw, ThrowResult.SetWon);
            Game.PlayerOnThrow.SetsWon += 1;
            scoreBoard.AddSetsWonTo(Game.PlayerOnThrow);
            ClearPlayerOnThrowHand();

            foreach (var player in Game.Players)
            {
                player.LegPoints = Game.legPoints;
                scoreBoard.SetPointsTo(player, Game.legPoints);
                player.LegsWon = 0;
                scoreBoard.SetLegsWonTo(player, 0);
                scoreBoard.CheckPointsHintFor(player);
            }

            TogglePlayerOnLegAndOnThrow();
        }

        private void TogglePlayerOnLegAndOnThrow()
        {
            Game.PlayerOnLeg = Game.Players.First(p => p != Game.PlayerOnLeg);
            scoreBoard.OnLegPointSetOn(Game.PlayerOnLeg);
            Game.PlayerOnThrow = Game.PlayerOnLeg;
            scoreBoard.OnThrowPointerSetOn(Game.PlayerOnThrow);
        }

        protected override void ThrowUndoInternal(GameSnapshot gameSnapshot)
        {
            scoreBoard.OnThrowPointerSetOn(Game.PlayerOnThrow);

            scoreBoard.OnLegPointSetOn(Game.PlayerOnLeg);
            foreach (var player in Game.Players)
            {
                scoreBoard.SetPointsTo(player, player.LegPoints);
                scoreBoard.SetLegsWonTo(player, player.LegsWon);
                scoreBoard.SetSetsWonTo(player, player.SetsWon);
                scoreBoard.CheckPointsHintFor(player);
            }

            scoreBoard.SetThrowNumber(Game.PlayerOnThrow.ThrowNumber);
        }
    }
}