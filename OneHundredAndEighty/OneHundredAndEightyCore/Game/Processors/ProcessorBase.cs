#region Usings

using System.Collections.Generic;
using System.Linq;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Enums;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public abstract class ProcessorBase : IGameProcessor
    {
        protected readonly ScoreBoardService scoreBoard;
        protected Domain.Game Game { get; }
        protected Stack<GameSnapshot> GameSnapshots;

        public delegate void EndMatchDelegate(Player winner);

        public event EndMatchDelegate OnMatchEnd;

        protected ProcessorBase(Domain.Game game,
                                ScoreBoardService scoreBoard)
        {
            this.scoreBoard = scoreBoard;
            Game = game;
            GameSnapshots = new Stack<GameSnapshot>();

            PrepareGame();
        }

        private void PrepareGame()
        {
            Game.PlayerOnThrow = Game.Players.First();
            Game.PlayerOnLeg = Game.Players.First();
            if (Game.Players.Count == 1) //  todo maybe do another way cuz ugly
            {
                Game.Players.ElementAt(0).Order = PlayerOrder.First;
            }
            else
            {
                Game.Players.ElementAt(0).Order = PlayerOrder.First;
                Game.Players.ElementAt(1).Order = PlayerOrder.Second;
            }

            foreach (var player in Game.Players)
            {
                player.SetsWon = 0;
                player.LegsWon = 0;
                player.LegPoints = Game.legPoints;
                player.HandPoints = 0;
                player.ThrowNumber = ThrowNumber.FirstThrow;
                player.HandThrows = new List<Throw>();
            }
        }

        private void TakeSnapshot()
        {
            GameSnapshots.Push(new GameSnapshot(Game));
        }

        protected void Check180()
        {
            if (Game.PlayerOnThrow.HandThrows.Sum(t => t.Points) == 180)
            {
                Game.Hands180.Add(new Hand180(Game.PlayerOnThrow));
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
                                    thrw.Sector,
                                    thrw.Type,
                                    throwResult,
                                    (int) Game.PlayerOnThrow.ThrowNumber,
                                    thrw.TotalPoints,
                                    thrw.Poi,
                                    thrw.ProjectionResolution);
            Game.Throws.Push(dbThrow);
            return dbThrow;
        }

        protected void InvokeEndMatch()
        {
            OnMatchEnd?.Invoke(Game.PlayerOnThrow);
        }

        public void OnThrow(DetectedThrow thrw)
        {
            TakeSnapshot();
            OnThrowInternal(thrw);
        }

        protected abstract void OnThrowInternal(DetectedThrow thrw);
    }
}