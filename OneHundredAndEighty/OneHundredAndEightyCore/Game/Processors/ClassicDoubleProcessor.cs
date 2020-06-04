#region Usings

using System.Collections.Generic;
using System.Linq;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class ClassicDoubleProcessor : ProcessorBase
    {
        private readonly int legPoints;

        public ClassicDoubleProcessor(Game game,
                                      List<Player> players,
                                      DBService dbService,
                                      ScoreBoardService scoreBoard,
                                      int legPoints,
                                      int legs,
                                      int sets)
            : base(game, players, dbService, scoreBoard, legs, sets)
        {
            this.legPoints = legPoints;
            players.ForEach(p => p.LegPoints = legPoints);
        }

        public override void OnThrow(DetectedThrow thrw)
        {
            if (IsGameOver(thrw))
            {
                ConvertAndSaveThrow(thrw, ThrowResult.MatchWon);

                dbService.StatisticUpdateAddLegsPlayedForPlayers(Game.Id);
                dbService.StatisticUpdateAddLegsWonForPlayer(PlayerOnThrow, Game.Id);

                dbService.StatisticUpdateAddSetsPlayedForPlayers(Game.Id);
                dbService.StatisticUpdateAddSetsWonForPlayer(PlayerOnThrow, Game.Id);

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

            PlayerOnThrow.HandPoints += thrw.TotalPoints;
            PlayerOnThrow.LegPoints -= thrw.TotalPoints;
            scoreBoard.AddPointsTo(thrw.TotalPoints * -1, PlayerOnThrow);

            var dbThrow = ConvertAndSaveThrow(thrw, ThrowResult.Ordinary);

            PlayerOnThrow.HandThrows.Push(dbThrow);

            if (IsHandOver())
            {
                Check180();
                ClearPlayerOnThrowHand();
                scoreBoard.CheckPointsHintFor(PlayerOnThrow);
                TogglePlayerOnThrow();
            }
            else
            {
                PlayerOnThrow.ThrowNumber += 1;
                scoreBoard.SetThrowNumber(PlayerOnThrow.ThrowNumber);
                scoreBoard.CheckPointsHintFor(PlayerOnThrow);
            }
        }

        private void OnLegOver()
        {
            dbService.StatisticUpdateAddLegsPlayedForPlayers(Game.Id);
            dbService.StatisticUpdateAddLegsWonForPlayer(PlayerOnThrow, Game.Id);

            PlayerOnThrow.LegsWon += 1;
            scoreBoard.AddLegsWonTo(PlayerOnThrow);

            ClearPlayerOnThrowHand();

            foreach (var player in Players)
            {
                player.LegPoints = legPoints;
                scoreBoard.SetPointsTo(legPoints, player);
                scoreBoard.CheckPointsHintFor(player);
            }

            TogglePlayerOnLegAndOnThrow();
        }

        private void OnSetOver()
        {
            dbService.StatisticUpdateAddLegsPlayedForPlayers(Game.Id);
            dbService.StatisticUpdateAddLegsWonForPlayer(PlayerOnThrow, Game.Id);

            dbService.StatisticUpdateAddSetsPlayedForPlayers(Game.Id);
            dbService.StatisticUpdateAddSetsWonForPlayer(PlayerOnThrow, Game.Id);

            PlayerOnThrow.SetsWon += 1;
            scoreBoard.AddSetsWonTo(PlayerOnThrow);

            ClearPlayerOnThrowHand();

            foreach (var player in Players)
            {
                player.LegPoints = legPoints;
                scoreBoard.SetPointsTo(legPoints, player);
                player.LegsWon = 0;
                scoreBoard.SetLegsWonTo(0, player);
                scoreBoard.CheckPointsHintFor(player);
            }

            TogglePlayerOnLegAndOnThrow();
        }

        private void TogglePlayerOnLegAndOnThrow()
        {
            PlayerOnLeg = Players.First(p => p != PlayerOnLeg);
            scoreBoard.LegPointSetOn(PlayerOnLeg);
            PlayerOnThrow = PlayerOnLeg;
            scoreBoard.OnThrowPointerSetOn(PlayerOnThrow);
        }
    }
}