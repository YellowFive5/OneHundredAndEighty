#region Usings

using System;
using System.Collections.Generic;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.ScoreBoard;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsSingleWriteOffPointsProcessor : ProcessorBase
    {
        private readonly int writeOffPoints;

        public FreeThrowsSingleWriteOffPointsProcessor(Game game,
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
            ThrowResult throwResult;

            if (IsFault(thrw))
            {
                throwResult = ThrowResult.Fault;
                PlayerOnThrow.LegPoints += PlayerOnThrow.HandPoints;
                scoreBoard.AddPointsToSinglePlayer(PlayerOnThrow.HandPoints);
            }
            else if (IsOut(thrw))
            {
                throwResult = ThrowResult.LegWon;
            }
            else
            {
                PlayerOnThrow.HandPoints += thrw.TotalPoints;
                PlayerOnThrow.LegPoints -= thrw.TotalPoints;
                throwResult = ThrowResult.Ordinary;
                scoreBoard.AddPointsToSinglePlayer(thrw.TotalPoints * -1);
            }

            var dbThrow = new Throw(PlayerOnThrow,
                                    Game,
                                    thrw.Sector,
                                    thrw.Type,
                                    throwResult,
                                    PlayerOnThrow.ThrowNumber,
                                    thrw.TotalPoints,
                                    thrw.Poi,
                                    thrw.ProjectionResolution);

            dbService.ThrowSaveNew(dbThrow);

            switch (throwResult)
            {
                case ThrowResult.Fault:

                    ClearThrows();

                    break;
                case ThrowResult.Ordinary:

                    PlayerOnThrow.HandThrows.Push(dbThrow);

                    if (IsHandOver())
                    {
                        Check180();

                        ClearThrows();
                    }
                    else
                    {
                        PlayerOnThrow.ThrowNumber += 1;
                    }

                    break;
                case ThrowResult.LegWon:
                    break;
                case ThrowResult.SetWon:
                    break;
                case ThrowResult.MatchWon:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}