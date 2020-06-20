#region Usings

using System.Collections.Generic;
using System.Linq;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Enums;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public abstract class ProcessorBase : IGameProcessor
    {
        protected readonly DBService dbService;
        protected readonly ScoreBoardService scoreBoard;
        private readonly int legs;
        private readonly int sets;

        public delegate void EndMatchDelegate(Player winner);

        public event EndMatchDelegate OnMatchEnd;

        protected ProcessorBase(Domain.Game game,
                                List<Player> players,
                                DBService dbService,
                                ScoreBoardService scoreBoard,
                                int legs = 0,
                                int sets = 0)
        {
            this.dbService = dbService;
            this.scoreBoard = scoreBoard;
            this.legs = legs;
            this.sets = sets;

            if (players.Count == 1) //  todo maybe do another way cuz ugly
            {
                players[0].Order = PlayerOrder.First;
            }
            else
            {
                players[1].Order = PlayerOrder.Second;
            }

            Game = game;
            Players = players;
            PlayerOnThrow = Players.First();
            PlayerOnLeg = Players.First();
        }

        protected List<Player> Players { get; }
        protected Player PlayerOnThrow { get; set; }
        protected Player PlayerOnLeg { get; set; }
        protected Domain.Game Game { get; }

        protected void Check180()
        {
            if (PlayerOnThrow.HandThrows.Sum(t => t.Points) == 180)
            {
                dbService._180SaveNew(Game, PlayerOnThrow);
            }
        }

        protected bool IsHandOver()
        {
            return PlayerOnThrow.ThrowNumber == ThrowNumber.ThirdThrow;
        }

        protected void ClearPlayerOnThrowHand()
        {
            PlayerOnThrow.HandThrows.Clear();
            PlayerOnThrow.ThrowNumber = ThrowNumber.FirstThrow;
            PlayerOnThrow.HandPoints = 0;
            scoreBoard.SetThrowNumber(PlayerOnThrow.ThrowNumber);
        }

        protected void TogglePlayerOnThrow()
        {
            PlayerOnThrow = Players.Count > 1
                                ? Players.First(p => p != PlayerOnThrow)
                                : PlayerOnThrow;
            scoreBoard.OnThrowPointerSetOn(PlayerOnThrow);
        }

        protected bool IsGameOver(DetectedThrow thrw)
        {
            return IsSetOver(thrw) && PlayerOnThrow.SetsWon + 1 == sets;
        }

        protected bool IsSetOver(DetectedThrow thrw)
        {
            return IsLegOver(thrw) && PlayerOnThrow.LegsWon + 1 == legs;
        }

        protected bool IsLegOver(DetectedThrow thrw)
        {
            return PlayerOnThrow.LegPoints - thrw.TotalPoints == 0 &&
                   (thrw.Type == ThrowType.Double || thrw.Type == ThrowType.Bulleye);
        }

        protected bool IsFault(DetectedThrow thrw)
        {
            return PlayerOnThrow.LegPoints - thrw.TotalPoints == 1 ||
                   PlayerOnThrow.LegPoints - thrw.TotalPoints < 0 ||
                   PlayerOnThrow.LegPoints - thrw.TotalPoints == 0;
        }

        protected void OnFault()
        {
            PlayerOnThrow.LegPoints += PlayerOnThrow.HandPoints;
            scoreBoard.SetPointsTo(PlayerOnThrow, PlayerOnThrow.LegPoints);

            ClearPlayerOnThrowHand();

            scoreBoard.CheckPointsHintFor(PlayerOnThrow);

            TogglePlayerOnThrow();
        }

        protected Throw ConvertAndSaveThrow(DetectedThrow thrw, ThrowResult throwResult)
        {
            var dbThrow = new Throw(PlayerOnThrow,
                                    Game,
                                    thrw.Sector,
                                    thrw.Type,
                                    throwResult,
                                    (int) PlayerOnThrow.ThrowNumber,
                                    thrw.TotalPoints,
                                    thrw.Poi,
                                    thrw.ProjectionResolution);

            dbService.ThrowSaveNew(dbThrow);
            return dbThrow;
        }

        protected void InvokeEndMatch()
        {
            OnMatchEnd?.Invoke(PlayerOnThrow);
        }

        public abstract void OnThrow(DetectedThrow thrw);
    }
}