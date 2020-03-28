#region Usings

using System.Collections.Generic;
using System.Linq;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.ScoreBoard;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public abstract class ProcessorBase : IGameProcessor
    {
        protected readonly DBService dbService;
        protected readonly ScoreBoardService scoreBoard;

        protected ProcessorBase(Game game, List<Player> players, DBService dbService, ScoreBoardService scoreBoard)
        {
            this.dbService = dbService;
            this.scoreBoard = scoreBoard;

            Game = game;
            Players = players;
            PlayerOnThrow = Players.First();
            PlayerOnSet = Players.First();
        }

        protected List<Player> Players { get; }
        protected Player PlayerOnThrow { get; set; }
        protected Player PlayerOnSet { get; set; }
        protected Game Game { get; }

        protected void Check180()
        {
            if (PlayerOnThrow.HandThrows.Sum(t => t.Points) == 180)
            {
                dbService._180SaveNew(Game, PlayerOnThrow);
            }
        }

        protected bool IsHandOver()
        {
            return PlayerOnThrow.ThrowNumber == 3;
        }

        protected void ClearThrows()
        {
            PlayerOnThrow.HandThrows.Clear();
            PlayerOnThrow.ThrowNumber = 1;
            PlayerOnThrow.HandPoints = 0;
            scoreBoard.SetThrowNumber(PlayerOnThrow.ThrowNumber);
        }

        protected void TogglePlayerOnThrow()
        {
            PlayerOnThrow = Players.First(p => p != PlayerOnThrow);
            scoreBoard.WhoThrowsPointerSetOn(PlayerOnThrow);
        }

        protected bool IsFault(DetectedThrow thrw)
        {
            return PlayerOnThrow.LegPoints - thrw.TotalPoints == 1 ||
                   PlayerOnThrow.LegPoints - thrw.TotalPoints < 0 ||
                   PlayerOnThrow.LegPoints - thrw.TotalPoints == 0;
        }

        protected bool IsOut(DetectedThrow thrw)
        {
            return PlayerOnThrow.LegPoints - thrw.TotalPoints == 0 &&
                   (thrw.Type == ThrowType.Double || thrw.Type == ThrowType.Bulleye);
        }

        protected Throw ConvertAndSaveThrow(DetectedThrow thrw, ThrowResult throwResult)
        {
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
            return dbThrow;
        }

        public virtual void OnThrow(DetectedThrow thrw)
        {
        }
    }
}