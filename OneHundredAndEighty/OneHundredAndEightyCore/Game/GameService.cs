#region Usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game.Processors;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.ScoreBoard;

#endregion

namespace OneHundredAndEightyCore.Game
{
    public class GameService
    {
        private readonly MainWindow mainWindow;
        private readonly ScoreBoardService scoreBoardService;
        private readonly DrawService drawService;
        private readonly DetectionService detectionService;
        private readonly ConfigService configService;
        private readonly Logger logger;
        private readonly DBService dbService;

        private bool IsGameRun { get; set; }
        private GameType GameType { get; set; }
        private IGameProcessor GameProcessor { get; set; }
        private Game Game { get; set; }

        public GameService(MainWindow mainWindow,
                           ScoreBoardService scoreBoardService,
                           DetectionService detectionService,
                           ConfigService configService,
                           DrawService drawService,
                           Logger logger,
                           DBService dbService)
        {
            this.mainWindow = mainWindow;
            this.scoreBoardService = scoreBoardService;
            this.detectionService = detectionService;
            this.configService = configService;
            this.drawService = drawService;
            this.logger = logger;
            this.dbService = dbService;
        }

        #region Start/Stop

        public void StartGame()
        {
            drawService.ProjectionClear();
            drawService.PointsHistoryBoxClear();

            detectionService.PrepareAndTryCapture();
            detectionService.RunDetection();

            var selectedGameTypeUi = Converter.NewGameControlsToGameTypeUi(mainWindow.NewGameControls);
            var selectedGameType = Converter.NewGameControlsToGameTypeGameService(mainWindow.NewGameControls);

            GameType = selectedGameType;

            var selectedPlayer1 = mainWindow.NewGamePlayer1ComboBox.SelectedItem as Player;
            var selectedPlayer2 = mainWindow.NewGamePlayer2ComboBox.SelectedItem as Player;

            var players = new List<Player>();
            if (selectedPlayer1 != null)
            {
                players.Add(selectedPlayer1);
            }

            if (selectedPlayer2 != null)
            {
                players.Add(selectedPlayer2);
            }

            Game = new Game(selectedGameType);

            dbService.GameSaveNew(Game, players);

            switch (selectedGameType)
            {
                case GameType.FreeThrowsSingleFreePoints:
                    GameProcessor = new FreeThrowsSingleFreePointsProcessor(Game, players, dbService, scoreBoardService);
                    scoreBoardService.OpenScoreBoard(selectedGameTypeUi, players, "Free throws");
                    break;
                case GameType.FreeThrowsSingle301Points:
                    GameProcessor = new FreeThrowsSingleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 301);
                    scoreBoardService.OpenScoreBoard(selectedGameTypeUi, players, "Write off 301", 301);
                    break;
                case GameType.FreeThrowsSingle501Points:
                    GameProcessor = new FreeThrowsSingleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 501);
                    scoreBoardService.OpenScoreBoard(selectedGameTypeUi, players, "Write off 501", 501);
                    break;
                case GameType.FreeThrowsSingle701Points:
                    GameProcessor = new FreeThrowsSingleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 701);
                    scoreBoardService.OpenScoreBoard(selectedGameTypeUi, players, "Write off 701", 701);
                    break;
                case GameType.FreeThrowsSingle1001Points:
                    GameProcessor = new FreeThrowsSingleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 1001);
                    scoreBoardService.OpenScoreBoard(selectedGameTypeUi, players, "Write off 1001", 1001);
                    break;
                case GameType.FreeThrowsDoubleFreePoints:
                    GameProcessor = new FreeThrowsDoubleFreePointsProcessor(Game, players, dbService, scoreBoardService);
                    scoreBoardService.OpenScoreBoard(selectedGameTypeUi, players, "Free throws");
                    break;
                case GameType.FreeThrowsDouble301Points:
                    break;
                case GameType.FreeThrowsDouble501Points:
                    break;
                case GameType.FreeThrowsDouble701Points:
                    break;
                case GameType.FreeThrowsDouble1001Points:
                    break;
                case GameType.Classic301Points:
                    break;
                case GameType.Classic501Points:
                    break;
                case GameType.Classic701Points:
                    break;
                case GameType.Classic1001Points:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Task.Run(() =>
                     {
                         IsGameRun = true;
                         detectionService.OnThrowDetected += OnAnotherThrow;
                         while (IsGameRun)
                         {
                         }

                         detectionService.OnThrowDetected -= OnAnotherThrow;
                     });
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
            dbService.GameEnd(Game, type);
        }

        #endregion

        private void OnAnotherThrow(DetectedThrow thrw)
        {
            GameProcessor.OnThrow(thrw);
        }
    }
}