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

        public delegate void EndMatchDelegate();

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

            Game.Players.First().GameData.Order = PlayerOrder.First;
            if (!Game.IsSingle)
            {
                Game.Players.ElementAt(1).GameData.Order = PlayerOrder.Second;
            }

            foreach (var player in Game.Players)
            {
                player.GameData.SetsWon = 0;
                player.GameData.LegsWon = 0;
                player.GameData.LegPoints = Game.legPoints;
                player.GameData.HandPoints = 0;
                player.GameData.ThrowNumber = ThrowNumber.FirstThrow;
                player.GameData.HandThrows = new List<Throw>();
            }
        }

        private void TakeSnapshot()
        {
            GameSnapshots.Push(new GameSnapshot(Game));
        }

        protected void Check180()
        {
            if (Game.PlayerOnThrow.GameData.HandThrows.Sum(t => t.Points) == 180)
            {
                Game.Hands180.Add(new Hand180(Game.PlayerOnThrow));
            }
        }

        protected bool IsHandOver()
        {
            return Game.PlayerOnThrow.GameData.ThrowNumber == ThrowNumber.ThirdThrow;
        }

        protected void ClearPlayerOnThrowHand()
        {
            Game.PlayerOnThrow.GameData.HandThrows.Clear();
            Game.PlayerOnThrow.GameData.ThrowNumber = ThrowNumber.FirstThrow;
            Game.PlayerOnThrow.GameData.HandPoints = 0;
            scoreBoard.SetThrowNumber(Game.PlayerOnThrow.GameData.ThrowNumber);
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
            return IsSetOver(thrw) && Game.PlayerOnThrow.GameData.SetsWon + 1 == Game.sets;
        }

        protected bool IsSetOver(DetectedThrow thrw)
        {
            return IsLegOver(thrw) && Game.PlayerOnThrow.GameData.LegsWon + 1 == Game.legs;
        }

        protected bool IsLegOver(DetectedThrow thrw)
        {
            return Game.PlayerOnThrow.GameData.LegPoints - thrw.TotalPoints == 0 &&
                   (thrw.Type == ThrowType.Double || thrw.Type == ThrowType.Bull);
        }

        protected bool IsFault(DetectedThrow thrw)
        {
            return Game.PlayerOnThrow.GameData.LegPoints - thrw.TotalPoints == 1 ||
                   Game.PlayerOnThrow.GameData.LegPoints - thrw.TotalPoints < 0 ||
                   Game.PlayerOnThrow.GameData.LegPoints - thrw.TotalPoints == 0;
        }

        protected void OnFault(DetectedThrow thrw)
        {
            ConvertAndSaveThrow(thrw, ThrowResult.Fault);
            Game.PlayerOnThrow.GameData.LegPoints += Game.PlayerOnThrow.GameData.HandPoints;
            scoreBoard.SetPointsTo(Game.PlayerOnThrow, Game.PlayerOnThrow.GameData.LegPoints);
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
                Game.PlayerOnThrow.GameData.ThrowNumber += 1;
                scoreBoard.SetThrowNumber(Game.PlayerOnThrow.GameData.ThrowNumber);
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
                Game.PlayerOnThrow.GameData.ThrowNumber += 1;
                scoreBoard.SetThrowNumber(Game.PlayerOnThrow.GameData.ThrowNumber);
                scoreBoard.CheckPointsHintFor(Game.PlayerOnThrow);
            }
        }

        protected void OnMatchOver(DetectedThrow thrw)
        {
            ConvertAndSaveThrow(thrw, ThrowResult.MatchWon);
            Game.Winner = Game.PlayerOnThrow;

            OnMatchEnd?.Invoke();
        }

        protected void ConvertAndSaveThrow(DetectedThrow detectedThrow,
                                           ThrowResult throwResult = ThrowResult.Ordinary)
        {
            var convertedThrow = new Throw(Game.PlayerOnThrow,
                                           detectedThrow.Sector,
                                           detectedThrow.Type,
                                           throwResult,
                                           (int) Game.PlayerOnThrow.GameData.ThrowNumber,
                                           detectedThrow.TotalPoints,
                                           detectedThrow.Poi,
                                           detectedThrow.ProjectionResolution);

            Game.Throws.Push(convertedThrow);
            Game.PlayerOnThrow.GameData.HandThrows.Add(convertedThrow);
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

            foreach (var playerFromSnapshot in gameSnapshot.PlayersGameData)
            {
                foreach (var player in Game.Players.Where(player => playerFromSnapshot.PlayerId == player.Id))
                {
                    player.GameData.SetsWon = playerFromSnapshot.SetsWon;
                    player.GameData.LegsWon = playerFromSnapshot.LegsWon;
                    player.GameData.LegPoints = playerFromSnapshot.LegPoints;
                    player.GameData.HandPoints = playerFromSnapshot.HandPoints;
                    player.GameData.ThrowNumber = playerFromSnapshot.ThrowNumber;
                    player.GameData.HandThrows = playerFromSnapshot.HandThrows.ToList();
                    player.GameData.Order = playerFromSnapshot.Order;

                    if (gameSnapshot.PlayerOnThrowId == player.Id)
                    {
                        Game.PlayerOnThrow = player;
                    }

                    if (gameSnapshot.PlayerOnLegId == player.Id)
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