#region Usings

using System;
using System.Collections.Generic;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Enums;
using OneHundredAndEightyCore.Game.Processors;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Windows.CamsDetection;
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
        private IGameProcessor GameProcessor { get; set; }
        private Domain.Game Game { get; set; }

        public delegate void EndMatchDelegate();

        public event EndMatchDelegate OnGameEnd;

        public GameService(ScoreBoardService scoreBoardService,
                           CamsDetectionBoard camsDetectionBoard,
                           DetectionService detectionService,
                           Logger logger,
                           DBService dbService)
        {
            this.logger = logger;
            this.scoreBoardService = scoreBoardService;
            this.camsDetectionBoard = camsDetectionBoard;
            this.detectionService = detectionService;
            this.dbService = dbService;
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
            camsDetectionBoard.OnUndoThrowButtonPressed += OnThrowUndo;
            camsDetectionBoard.OnCorrectThrowButtonPressed += OnThrowCorrect;
        }

        public void StopGame(GameResultType type)
        {
            if (Game != null)
            {
                // dbService.GameEnd(Game, gameResultType: type);
                StopGameInternal();
            }
        }

        private void OnMatchEnd(Player winner)
        {
            // dbService.GameEnd(Game, winner);
            OnGameEnd?.Invoke();

            StopGameInternal();
        }

        private void StopGameInternal()
        {
            SaveGameData();
            Game = null;
            detectionService.OnThrowDetected -= OnAnotherThrow;
            detectionService.OnStatusChanged -= OnDetectionServiceStatusChanged;
            GameProcessor.OnMatchEnd -= OnMatchEnd;
            camsDetectionBoard.OnUndoThrowButtonPressed -= OnThrowUndo;
            camsDetectionBoard.OnCorrectThrowButtonPressed -= OnThrowCorrect;
        }

        private void SaveGameData()
        {
            //todo
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
            GameProcessor.UndoThrow();
        }

        private void OnThrowCorrect()
        {
            // todo
        }
    }
}