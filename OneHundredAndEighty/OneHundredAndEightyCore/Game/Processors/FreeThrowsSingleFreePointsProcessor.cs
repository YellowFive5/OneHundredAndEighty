#region Usings

using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsSingleFreePointsProcessor : ProcessorBase
    {
        public FreeThrowsSingleFreePointsProcessor(Domain.Game game,
                                                   ScoreBoardService scoreBoard)
            : base(game, scoreBoard)
        {
        }

        protected override void OnThrowInternal(DetectedThrow thrw)
        {
            Game.PlayerOnThrow.GameData.HandPoints += thrw.TotalPoints;
            Game.PlayerOnThrow.GameData.LegPoints += thrw.TotalPoints;

            scoreBoard.AddPointsTo(Game.PlayerOnThrow, thrw.TotalPoints);

            OnHandOverSinglePlayerCheck(thrw);
        }

        protected override void ThrowUndoInternal(GameSnapshot gameSnapshot)
        {
            scoreBoard.SetPointsTo(Game.PlayerOnThrow, Game.PlayerOnThrow.GameData.LegPoints);

            scoreBoard.SetThrowNumber(Game.PlayerOnThrow.GameData.ThrowNumber);
        }
    }
}