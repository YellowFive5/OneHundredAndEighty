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
        private Stack<GameSnapshot> GameSnapshots { get; }

        public delegate void EndMatchDelegate(Player winner);

        public event EndMatchDelegate OnMatchEnd;

        public bool CanUndoThrow => GameSnapshots.Count != 0 &&
                                    Game.Throws.Count != 0;

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

        protected void OnFault(DetectedThrow thrw)
        {
            ConvertAndSaveThrow(thrw, ThrowResult.Fault);
            Game.PlayerOnThrow.LegPoints += Game.PlayerOnThrow.HandPoints;
            scoreBoard.SetPointsTo(Game.PlayerOnThrow, Game.PlayerOnThrow.LegPoints);
            ClearPlayerOnThrowHand();
            scoreBoard.CheckPointsHintFor(Game.PlayerOnThrow);
            TogglePlayerOnThrow();
        }

        protected void OnHandOverSinglePlayerCheck(DetectedThrow thrw)
        {
            ConvertAndSaveThrow(thrw);

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
        }

        protected void OnHandOverDoublePlayersCheck(DetectedThrow thrw)
        {
            ConvertAndSaveThrow(thrw);

            if (IsHandOver())
            {
                Check180();
                ClearPlayerOnThrowHand();

                scoreBoard.CheckPointsHintFor(Game.PlayerOnThrow);
                TogglePlayerOnThrow();
            }
            else
            {
                Game.PlayerOnThrow.ThrowNumber += 1;
                scoreBoard.SetThrowNumber(Game.PlayerOnThrow.ThrowNumber);
                scoreBoard.CheckPointsHintFor(Game.PlayerOnThrow);
            }
        }

        protected void ConvertAndSaveThrow(DetectedThrow thrw,
                                           ThrowResult throwResult = ThrowResult.Ordinary)
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
            Game.PlayerOnThrow.HandThrows.Add(dbThrow);
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

        public void UndoThrow()
        {
            Game.Throws.Pop();
            var gameSnapshot = GameSnapshots.Pop();

            foreach (var playerFromSnapshot in gameSnapshot.Players)
            {
                foreach (var player in Game.Players.Where(player => playerFromSnapshot.Id == player.Id))
                {
                    player.SetsWon = playerFromSnapshot.SetsWon;
                    player.LegsWon = playerFromSnapshot.LegsWon;
                    player.LegPoints = playerFromSnapshot.LegPoints;
                    player.HandPoints = playerFromSnapshot.HandPoints;
                    player.ThrowNumber = playerFromSnapshot.ThrowNumber;
                    player.HandThrows = playerFromSnapshot.HandThrows.ToList();
                    player.Order = playerFromSnapshot.Order;

                    if (gameSnapshot.PlayerOnThrow.Id == player.Id)
                    {
                        Game.PlayerOnThrow = player;
                    }

                    if (gameSnapshot.PlayerOnLeg.Id == player.Id)
                    {
                        Game.PlayerOnLeg = player;
                    }
                }

                Game.Hands180 = gameSnapshot.Hands180.ToList();
            }

            ThrowUndoInternal(gameSnapshot);
        }

        protected abstract void ThrowUndoInternal(GameSnapshot gameSnapshot);
    }
}