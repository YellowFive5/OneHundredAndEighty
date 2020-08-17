#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Enums;
using OneHundredAndEightyCore.Game.Processors;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Windows.CamsDetection;
using OneHundredAndEightyCore.Windows.Debug;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Game
{
    public class GameService
    {
        private readonly Logger logger;
        private readonly ScoreBoardService scoreBoardService;
        private readonly CamsDetectionBoard camsDetectionBoard;
        private readonly DetectionService detectionService;
        private readonly DBService dbService;
        private readonly ManualThrowPanel manualThrowPanel;
        private IGameProcessor GameProcessor { get; set; }
        private Domain.Game Game { get; set; }

        public delegate void EndMatchDelegate();

        public event EndMatchDelegate OnGameEnd;

        public GameService(ScoreBoardService scoreBoardService,
                           CamsDetectionBoard camsDetectionBoard,
                           DetectionService detectionService,
                           Logger logger,
                           DBService dbService,
                           ManualThrowPanel manualThrowPanel)
        {
            this.logger = logger;
            this.scoreBoardService = scoreBoardService;
            this.camsDetectionBoard = camsDetectionBoard;
            this.detectionService = detectionService;
            this.dbService = dbService;
            this.manualThrowPanel = manualThrowPanel;
        }

        #region Start/Stop

        public void StartGame(Player player1,
                              Player player2,
                              GameType gameType,
                              GamePoints gamePoints,
                              int gameSets,
                              int gameLegs)
        {
            var players = new List<Player>();
            players.AddIfNotNull(player1);
            players.AddIfNotNull(player2);

            Game = new Domain.Game(gameType, players, gameLegs, gameSets, gamePoints);

            scoreBoardService.OpenScoreBoard(Game);

            switch (gameType)
            {
                case GameType.FreeThrowsSingle:
                    switch (gamePoints)
                    {
                        case GamePoints.Free:
                            GameProcessor = new FreeThrowsSingleFreePointsProcessor(Game, scoreBoardService);
                            break;
                        case GamePoints._301:
                        case GamePoints._501:
                        case GamePoints._701:
                        case GamePoints._1001:
                            GameProcessor = new FreeThrowsSingleWriteOffPointsProcessor(Game, scoreBoardService);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(gamePoints), gamePoints, null);
                    }

                    break;

                case GameType.FreeThrowsDouble:
                    switch (gamePoints)
                    {
                        case GamePoints.Free:
                            GameProcessor = new FreeThrowsDoubleFreePointsProcessor(Game, scoreBoardService);
                            break;
                        case GamePoints._301:
                        case GamePoints._501:
                        case GamePoints._701:
                        case GamePoints._1001:
                            GameProcessor = new FreeThrowsDoubleWriteOffPointsProcessor(Game, scoreBoardService);
                            break;
                    }

                    break;

                case GameType.Classic:
                    GameProcessor = new ClassicDoubleProcessor(Game, scoreBoardService);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            detectionService.OnThrowDetected += OnAnotherThrow;
            detectionService.OnStatusChanged += OnDetectionServiceStatusChanged;
            GameProcessor.OnMatchEnd += OnMatchEnd;
            scoreBoardService.OnUndoThrowButtonPressed += OnThrowUndo;
            scoreBoardService.OnManualThrowButtonPressed += OnManualThrow;
        }

        public void StopGame(GameResultType type)
        {
            if (Game != null)
            {
                if (Game.legPoints == 0 && type == GameResultType.Aborted) // todo do another way
                {
                    Game.Result = GameResultType.NotDefined;
                }
                else
                {
                    Game.Result = type;
                }

                StopGameInternal();
            }
        }

        private void OnMatchEnd()
        {
            OnGameEnd?.Invoke();
            StopGameInternal();
        }

        private void StopGameInternal()
        {
            Game.EndTimeStamp = DateTime.Now;
            SaveGameData();
            Game = null;
            detectionService.OnThrowDetected -= OnAnotherThrow;
            detectionService.OnStatusChanged -= OnDetectionServiceStatusChanged;
            GameProcessor.OnMatchEnd -= OnMatchEnd;
            scoreBoardService.OnUndoThrowButtonPressed -= OnThrowUndo;
            scoreBoardService.OnManualThrowButtonPressed -= OnManualThrow;
        }

        private void SaveGameData()
        {
            Game.Id = dbService.GameSaveNew(Game);

            foreach (var thrw in Game.Throws.Reverse())
            {
                dbService.ThrowSaveNew(thrw,Game);
            }

            foreach (var player in Game.Players)
            {
                dbService.StatisticUpdateSetLegsPlayedForPlayer(Game);
                dbService.StatisticUpdateSetLegsWonForPlayer(player, Game);

                dbService.StatisticUpdateSetSetsPlayedForPlayer(Game);
                dbService.StatisticUpdateSetSetsWonForPlayer(player, Game);
            }

            foreach (var hand180 in Game.Hands180)
            {
                dbService._180SaveNew(hand180, Game); // todo 180 save problem - no throw id
            }

            switch (Game.Result)
            {
                case GameResultType.NotDefined:
                    dbService.StatisticUpdateAllPlayersSetGameResultForWinnersAndLosers(Game); // todo no winner problem
                    break;
                case GameResultType.Aborted:
                case GameResultType.Error:
                    dbService.StatisticUpdateAllPlayersSetGameResultAbortedOrError(Game);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        private void OnAnotherThrow(DetectedThrow thrw)
        {
            GameProcessor.OnThrow(thrw);
        }

        private void OnDetectionServiceStatusChanged(DetectionServiceStatus status)
        {
            scoreBoardService.SetDetectionStatus(status);
        }

        private void OnThrowUndo()
        {
            if (GameProcessor.CanUndoThrow)
            {
                GameProcessor.UndoThrow();
            }
        }

        private void OnManualThrow()
        {
            manualThrowPanel.ShowPanel();
        }
    }
}