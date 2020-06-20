#region Usings

using System.Collections.Generic;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsSingleWriteOffPointsProcessor : ProcessorBase
    {
        private readonly int writeOffPoints;

        public FreeThrowsSingleWriteOffPointsProcessor(Domain.Game game,
                                                       List<Player> players,
                                                       DBService dbService,
                                                       ScoreBoardService scoreBoard,
                                                       int writeOffPoints)
            : base(game, players, dbService, scoreBoard)
        {
            this.writeOffPoints = writeOffPoints;
            players.ForEach(p => p.LegPoints = writeOffPoints);
        }

        public override void OnThrow(DetectedThrow thrw)
        {
            if (IsLegOver(thrw))
            {
                PlayerOnThrow.LegPoints = writeOffPoints;
                scoreBoard.SetPointsTo(PlayerOnThrow, writeOffPoints);

                ConvertAndSaveThrow(thrw, ThrowResult.LegWon);

                dbService.StatisticUpdateAddLegsPlayedForPlayers(Game.Id);
                dbService.StatisticUpdateAddLegsWonForPlayer(PlayerOnThrow, Game.Id);

                ClearPlayerOnThrowHand();
                scoreBoard.CheckPointsHintFor(PlayerOnThrow);
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
            scoreBoard.AddPointsTo(PlayerOnThrow, thrw.TotalPoints * -1);

            var dbThrow = ConvertAndSaveThrow(thrw, ThrowResult.Ordinary);

            PlayerOnThrow.HandThrows.Push(dbThrow);
            if (IsHandOver())
            {
                Check180();
                ClearPlayerOnThrowHand();
            }
            else
            {
                PlayerOnThrow.ThrowNumber += 1;
                scoreBoard.SetThrowNumber(PlayerOnThrow.ThrowNumber);
            }

            scoreBoard.CheckPointsHintFor(PlayerOnThrow);
        }
    }
}