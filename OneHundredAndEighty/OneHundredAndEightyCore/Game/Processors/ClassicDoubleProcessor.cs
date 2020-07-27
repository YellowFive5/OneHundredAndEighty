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

                // dbService.StatisticUpdateAddLegsPlayedForPlayers(Game.Id);
                // dbService.StatisticUpdateAddLegsWonForPlayer(Game.PlayerOnThrow, Game.Id);
                //
                // dbService.StatisticUpdateAddSetsPlayedForPlayers(Game.Id);
                // dbService.StatisticUpdateAddSetsWonForPlayer(Game.PlayerOnThrow, Game.Id);

                InvokeEndMatch();
                return;
            }

            if (IsSetOver(thrw))
            {
                ConvertAndSaveThrow(thrw, ThrowResult.SetWon);

                OnSetOver();
                return;
            }

            if (IsLegOver(thrw))
            {
                ConvertAndSaveThrow(thrw, ThrowResult.LegWon);

                OnLegOver();
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

            var dbThrow = ConvertAndSaveThrow(thrw, ThrowResult.Ordinary);

            Game.PlayerOnThrow.HandThrows.Add(dbThrow);

            if (IsHandOver())
            {
                Check180();
                ClearPlayerOnThrowHand();
                scoreBoard.CheckPointsHintFor(Game.PlayerOnThrow);
                TogglePlayerOnThrow();
            }
            else
            {
                Game.PlayerOnThrow.ThrowNumber += 1;
                scoreBoard.SetThrowNumber(Game.PlayerOnThrow.ThrowNumber);
                scoreBoard.CheckPointsHintFor(Game.PlayerOnThrow);
            }
        }

        private void OnLegOver()
        {
            // dbService.StatisticUpdateAddLegsPlayedForPlayers(Game.Id);
            // dbService.StatisticUpdateAddLegsWonForPlayer(Game.PlayerOnThrow, Game.Id);

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

        private void OnSetOver()
        {
            // dbService.StatisticUpdateAddLegsPlayedForPlayers(Game.Id);
            // dbService.StatisticUpdateAddLegsWonForPlayer(Game.PlayerOnThrow, Game.Id);
            //
            // dbService.StatisticUpdateAddSetsPlayedForPlayers(Game.Id);
            // dbService.StatisticUpdateAddSetsWonForPlayer(Game.PlayerOnThrow, Game.Id);
            //
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
    }
}