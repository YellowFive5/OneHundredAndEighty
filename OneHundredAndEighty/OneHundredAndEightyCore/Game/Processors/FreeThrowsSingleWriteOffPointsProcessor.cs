#region Usings

using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsSingleWriteOffPointsProcessor : ProcessorBase
    {
        public FreeThrowsSingleWriteOffPointsProcessor(Domain.Game game,
                                                       DBService dbService,
                                                       ScoreBoardService scoreBoard)
            : base(game, dbService, scoreBoard)
        {
            Game.Players.ForEach(p => p.LegPoints = Game.legPoints);
        }

        public override void OnThrow(DetectedThrow thrw)
        {
            if (IsLegOver(thrw))
            {
                Game.PlayerOnThrow.LegPoints = Game.legPoints;
                scoreBoard.SetPointsTo(Game.PlayerOnThrow, Game.legPoints);

                ConvertAndSaveThrow(thrw, ThrowResult.LegWon);

                dbService.StatisticUpdateAddLegsPlayedForPlayers(Game.Id);
                dbService.StatisticUpdateAddLegsWonForPlayer(Game.PlayerOnThrow, Game.Id);

                ClearPlayerOnThrowHand();
                scoreBoard.CheckPointsHintFor(Game.PlayerOnThrow);
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

            Game.PlayerOnThrow.HandThrows.Push(dbThrow);
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

            scoreBoard.CheckPointsHintFor(Game.PlayerOnThrow);
        }
    }
}