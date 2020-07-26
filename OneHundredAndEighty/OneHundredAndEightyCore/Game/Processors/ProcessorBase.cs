#region Usings

using System.Collections.Generic;
using System.Linq;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Enums;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public abstract class ProcessorBase : IGameProcessor
    {
        protected readonly DBService dbService;
        protected readonly ScoreBoardService scoreBoard;
        protected Stack<GameSnapshot> GameSnapshots;

        public delegate void EndMatchDelegate(Player winner);

        public event EndMatchDelegate OnMatchEnd;

        protected ProcessorBase(Domain.Game game,
                                DBService dbService,
                                ScoreBoardService scoreBoard)
        {
            this.dbService = dbService;
            this.scoreBoard = scoreBoard;

            GameSnapshots = new Stack<GameSnapshot>();

            Game = game;
        }

        protected Domain.Game Game { get; }

        protected void Check180()
        {
            if (Game.PlayerOnThrow.HandThrows.Sum(t => t.Points) == 180)
            {
                dbService._180SaveNew(Game, Game.PlayerOnThrow);
            }
        }

        protected bool IsHandOver()
        {
            return Game.PlayerOnThrow.ThrowNumber == ThrowNumber.ThirdThrow;
        }

        protected void ClearPlayerOnThrowHand()
        {
            Game.PlayerOnThrow.HandThrows.Clear();
            Game.PlayerOnThrow.ThrowNumber = ThrowNumber.FirstThrow;
            Game.PlayerOnThrow.HandPoints = 0;
            scoreBoard.SetThrowNumber(Game.PlayerOnThrow.ThrowNumber);
        }

        protected void TogglePlayerOnThrow()
        {
            Game.PlayerOnThrow = Game.Players.Count > 1
                                     ? Game.Players.First(p => p != Game.PlayerOnThrow)
                                     : Game.PlayerOnThrow;
            scoreBoard.OnThrowPointerSetOn(Game.PlayerOnThrow);
        }

        protected bool IsGameOver(DetectedThrow thrw)
        {
            return IsSetOver(thrw) && Game.PlayerOnThrow.SetsWon + 1 == Game.sets;
        }

        protected bool IsSetOver(DetectedThrow thrw)
        {
            return IsLegOver(thrw) && Game.PlayerOnThrow.LegsWon + 1 == Game.legs;
        }

        protected bool IsLegOver(DetectedThrow thrw)
        {
            return Game.PlayerOnThrow.LegPoints - thrw.TotalPoints == 0 &&
                   (thrw.Type == ThrowType.Double || thrw.Type == ThrowType.Bulleye);
        }

        protected bool IsFault(DetectedThrow thrw)
        {
            return Game.PlayerOnThrow.LegPoints - thrw.TotalPoints == 1 ||
                   Game.PlayerOnThrow.LegPoints - thrw.TotalPoints < 0 ||
                   Game.PlayerOnThrow.LegPoints - thrw.TotalPoints == 0;
        }

        protected void OnFault()
        {
            Game.PlayerOnThrow.LegPoints += Game.PlayerOnThrow.HandPoints;
            scoreBoard.SetPointsTo(Game.PlayerOnThrow, Game.PlayerOnThrow.LegPoints);

            ClearPlayerOnThrowHand();

            scoreBoard.CheckPointsHintFor(Game.PlayerOnThrow);

            TogglePlayerOnThrow();
        }

        protected Throw ConvertAndSaveThrow(DetectedThrow thrw, ThrowResult throwResult)
        {
            var dbThrow = new Throw(Game.PlayerOnThrow,
                                    Game,
                                    thrw.Sector,
                                    thrw.Type,
                                    throwResult,
                                    (int) Game.PlayerOnThrow.ThrowNumber,
                                    thrw.TotalPoints,
                                    thrw.Poi,
                                    thrw.ProjectionResolution);
            dbService.ThrowSaveNew(dbThrow);
            return dbThrow;
        }

        protected void InvokeEndMatch()
        {
            OnMatchEnd?.Invoke(Game.PlayerOnThrow);
        }

        public abstract void OnThrow(DetectedThrow thrw);
    }
}