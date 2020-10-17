﻿#region Usings

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

            OnHandOverSinglePlayerCheck(thrw);

            scoreBoard.CheckPointsHintFor(Game.PlayerOnThrow);
        }

        protected override void ThrowUndoInternal(GameSnapshot gameSnapshot)
        {
            scoreBoard.SetPointsTo(Game.PlayerOnThrow, Game.PlayerOnThrow.GameData.LegPoints);

            scoreBoard.SetThrowNumber(Game.PlayerOnThrow.GameData.ThrowNumber);
            scoreBoard.CheckPointsHintFor(Game.PlayerOnThrow);
        }
    }
}