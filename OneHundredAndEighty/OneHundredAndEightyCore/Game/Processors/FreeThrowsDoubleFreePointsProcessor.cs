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
            Game.PlayerOnThrow.GameData.HandPoints += thrw.TotalPoints;
            Game.PlayerOnThrow.GameData.LegPoints += thrw.TotalPoints;

            scoreBoard.AddPointsTo(Game.PlayerOnThrow, thrw.TotalPoints);

            ConvertAndSaveThrow(thrw);

            if (IsHandOver())
            {
                Check180();
                ClearPlayerOnThrowHand();
                TogglePlayerOnThrow();
            }
            else
            {
                Game.PlayerOnThrow.GameData.ThrowNumber += 1;
                scoreBoard.SetThrowNumber(Game.PlayerOnThrow.GameData.ThrowNumber);
            }
        }

        protected override void ThrowUndoInternal(GameSnapshot gameSnapshot)
        {
            scoreBoard.OnThrowPointerSetOn(Game.PlayerOnThrow);

            foreach (var player in Game.Players)
            {
                scoreBoard.SetPointsTo(player, player.GameData.LegPoints);
            }

            scoreBoard.SetThrowNumber(Game.PlayerOnThrow.GameData.ThrowNumber);
        }
    }
}