#region Usings

using System;
using System.Collections.Generic;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game.Processors;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Windows.CamsDetection;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Game
{
    public class GameService
    {
        private readonly ScoreBoardService scoreBoardService;
        private readonly CamsDetectionBoard camsDetectionBoard;
        private readonly DrawService drawService;
        private readonly DetectionService detectionService;
        private readonly ConfigService configService;
        private readonly Logger logger;
        private readonly DBService dbService;
        private bool IsGameRun { get; set; }
        private GameType GameType { get; set; }
        private IGameProcessor GameProcessor { get; set; }
        private Game Game { get; set; }

        public GameService(ScoreBoardService scoreBoardService,
                           CamsDetectionBoard camsDetectionBoard,
                           DetectionService detectionService,
                           ConfigService configService,
                           DrawService drawService,
                           Logger logger,
                           DBService dbService)
        {
            this.scoreBoardService = scoreBoardService;
            this.camsDetectionBoard = camsDetectionBoard;
            this.detectionService = detectionService;
            this.configService = configService;
            this.drawService = drawService;
            this.logger = logger;
            this.dbService = dbService;
        }

        #region Start/Stop

        public void StartGame(List<CamService> cams,
                              Player player1,
                              Player player2,
                              GameType gameType,
                              GamePoints gamePoints,
                              int gameSets,
                              int gameLegs)
        {
            drawService.ProjectionClear();
            drawService.PointsHistoryBoxClear();

            detectionService.PrepareCamsAndTryCapture(cams, CamServiceWorkingMode.Detection);
            detectionService.RunDetection();

            GameType = gameType;

            var players = new List<Player>();
            players.AddIfNotNull(player1);
            players.AddIfNotNull(player2);

            Game = new Game(gameType);

            dbService.GameSaveNew(Game, players);

            switch (gameType)
            {
                case GameType.FreeThrowsSingle:
                    switch (gamePoints)
                    {
                        case GamePoints.Free:
                            GameProcessor = new FreeThrowsSingleFreePointsProcessor(Game, players, dbService, scoreBoardService);
                            scoreBoardService.OpenScoreBoard(gameType, players, "Free throws");
                            break;
                        case GamePoints._301:
                            GameProcessor = new FreeThrowsSingleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 301);
                            scoreBoardService.OpenScoreBoard(gameType, players, "Write off 301", 301);
                            break;
                        case GamePoints._501:
                            GameProcessor = new FreeThrowsSingleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 501);
                            scoreBoardService.OpenScoreBoard(gameType, players, "Write off 501", 501);
                            break;
                        case GamePoints._701:
                            GameProcessor = new FreeThrowsSingleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 701);
                            scoreBoardService.OpenScoreBoard(gameType, players, "Write off 701", 701);
                            break;
                        case GamePoints._1001:
                            GameProcessor = new FreeThrowsSingleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 1001);
                            scoreBoardService.OpenScoreBoard(gameType, players, "Write off 1001", 1001);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(gamePoints), gamePoints, null);
                    }

                    break;

                case GameType.FreeThrowsDouble:
                    switch (gamePoints)
                    {
                        case GamePoints.Free:
                            GameProcessor = new FreeThrowsDoubleFreePointsProcessor(Game, players, dbService, scoreBoardService);
                            scoreBoardService.OpenScoreBoard(gameType, players, "Free throws");
                            break;
                        case GamePoints._301:
                            GameProcessor = new FreeThrowsDoubleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 301);
                            scoreBoardService.OpenScoreBoard(gameType, players, "Write off 301", 301);
                            break;
                        case GamePoints._501:
                            GameProcessor = new FreeThrowsDoubleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 501);
                            scoreBoardService.OpenScoreBoard(gameType, players, "Write off 501", 501);
                            break;
                        case GamePoints._701:
                            GameProcessor = new FreeThrowsDoubleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 701);
                            scoreBoardService.OpenScoreBoard(gameType, players, "Write off 701", 701);
                            break;
                        case GamePoints._1001:
                            GameProcessor = new FreeThrowsDoubleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 1001);
                            scoreBoardService.OpenScoreBoard(gameType, players, "Write off 1001", 1001);
                            break;
                    }

                    break;

                case GameType.Classic:
                    switch (gamePoints)
                    {
                        case GamePoints._301:
                            GameProcessor = new ClassicDoubleProcessor(Game, players, dbService, scoreBoardService, 301, gameLegs, gameSets);
                            scoreBoardService.OpenScoreBoard(gameType, players, $"First to {gameSets}", 301);
                            break;
                        case GamePoints._501:
                            GameProcessor = new ClassicDoubleProcessor(Game, players, dbService, scoreBoardService, 501, gameLegs, gameSets);
                            scoreBoardService.OpenScoreBoard(gameType, players, $"First to {gameSets}", 501);
                            break;
                        case GamePoints._701:
                            GameProcessor = new ClassicDoubleProcessor(Game, players, dbService, scoreBoardService, 701, gameLegs, gameSets);
                            scoreBoardService.OpenScoreBoard(gameType, players, $"First to {gameSets}", 701);
                            break;
                        case GamePoints._1001:
                            GameProcessor = new ClassicDoubleProcessor(Game, players, dbService, scoreBoardService, 1001, gameLegs, gameSets);
                            scoreBoardService.OpenScoreBoard(gameType, players, $"First to {gameSets}", 1001);
                            break;
                    }

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
            if (!IsGameRun)
            {
                return;
            }

            IsGameRun = false;

            scoreBoardService.CloseScoreBoard();
            detectionService.StopDetection();
            drawService.ProjectionClear();
            dbService.GameEnd(Game, gameResultType: type);

            detectionService.OnThrowDetected -= OnAnotherThrow;
            detectionService.OnStatusChanged -= OnDetectionServiceStatusChanged;
            GameProcessor.OnMatchEnd -= OnMatchEnd;
            camsDetectionBoard.OnUndoThrowButtonPressed -= OnThrowUndo;
            camsDetectionBoard.OnCorrectThrowButtonPressed -= OnThrowCorrect;
        }

        private void OnMatchEnd(Game game, Player winner)
        {
            if (!IsGameRun)
            {
                return;
            }

            IsGameRun = false;

            scoreBoardService.CloseScoreBoard();
            detectionService.StopDetection();
            drawService.ProjectionClear();
            camsDetectionBoard.Close();
            dbService.GameEnd(Game, winner);

            // mainWindow.ToggleMainTabItemsEnabled();
            // mainWindow.ToggleMatchControlsEnabled();
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
            // todo
        }

        private void OnThrowCorrect()
        {
            // todo
        }
    }
}