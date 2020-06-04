#region Usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game.Processors;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Windows.Main;
using OneHundredAndEightyCore.Windows.Score;

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

            detectionService.PrepareCamsAndTryCapture();
            detectionService.RunDetection();

            var selectedGameType = Converter.NewGameControlsToGameType(mainWindow.NewGameTypeComboBox);
            var legs = Converter.ComboBoxSelectedContentToInt(mainWindow.NewGameLegsComboBox);
            var sets = Converter.ComboBoxSelectedContentToInt(mainWindow.NewGameSetsComboBox);
            var legPoints = Converter.ComboBoxSelectedContentToString(mainWindow.NewGamePointsComboBox);

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
                case GameType.FreeThrowsSingle:
                    switch (legPoints)
                    {
                        case "Free":
                            GameProcessor = new FreeThrowsSingleFreePointsProcessor(Game, players, dbService, scoreBoardService);
                            scoreBoardService.OpenScoreBoard(selectedGameType, players, "Free throws");
                            break;
                        case "301":
                            GameProcessor = new FreeThrowsSingleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 301);
                            scoreBoardService.OpenScoreBoard(selectedGameType, players, "Write off 301", 301);
                            break;
                        case "501":
                            GameProcessor = new FreeThrowsSingleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 501);
                            scoreBoardService.OpenScoreBoard(selectedGameType, players, "Write off 501", 501);
                            break;
                        case "701":
                            GameProcessor = new FreeThrowsSingleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 701);
                            scoreBoardService.OpenScoreBoard(selectedGameType, players, "Write off 701", 701);
                            break;
                        case "1001":
                            GameProcessor = new FreeThrowsSingleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 1001);
                            scoreBoardService.OpenScoreBoard(selectedGameType, players, "Write off 1001", 1001);
                            break;
                    }

                    break;

                case GameType.FreeThrowsDouble:
                    switch (legPoints)
                    {
                        case "Free":
                            GameProcessor = new FreeThrowsDoubleFreePointsProcessor(Game, players, dbService, scoreBoardService);
                            scoreBoardService.OpenScoreBoard(selectedGameType, players, "Free throws");
                            break;
                        case "301":
                            GameProcessor = new FreeThrowsDoubleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 301);
                            scoreBoardService.OpenScoreBoard(selectedGameType, players, "Write off 301", 301);
                            break;
                        case "501":
                            GameProcessor = new FreeThrowsDoubleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 501);
                            scoreBoardService.OpenScoreBoard(selectedGameType, players, "Write off 501", 501);
                            break;
                        case "701":
                            GameProcessor = new FreeThrowsDoubleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 701);
                            scoreBoardService.OpenScoreBoard(selectedGameType, players, "Write off 701", 701);
                            break;
                        case "1001":
                            GameProcessor = new FreeThrowsDoubleWriteOffPointsProcessor(Game, players, dbService, scoreBoardService, 1001);
                            scoreBoardService.OpenScoreBoard(selectedGameType, players, "Write off 1001", 1001);
                            break;
                    }

                    break;

                case GameType.Classic:
                    switch (legPoints)
                    {
                        case "301":
                            GameProcessor = new ClassicDoubleProcessor(Game, players, dbService, scoreBoardService, 301, legs, sets);
                            scoreBoardService.OpenScoreBoard(selectedGameType, players, $"First to {sets}", 301);
                            break;
                        case "501":
                            GameProcessor = new ClassicDoubleProcessor(Game, players, dbService, scoreBoardService, 501, legs, sets);
                            scoreBoardService.OpenScoreBoard(selectedGameType, players, $"First to {sets}", 501);
                            break;
                        case "701":
                            GameProcessor = new ClassicDoubleProcessor(Game, players, dbService, scoreBoardService, 701, legs, sets);
                            scoreBoardService.OpenScoreBoard(selectedGameType, players, $"First to {sets}", 701);
                            break;
                        case "1001":
                            GameProcessor = new ClassicDoubleProcessor(Game, players, dbService, scoreBoardService, 1001, legs, sets);
                            scoreBoardService.OpenScoreBoard(selectedGameType, players, $"First to {sets}", 1001);
                            break;
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Task.Run(() =>
                     {
                         IsGameRun = true;
                         detectionService.OnThrowDetected += OnAnotherThrow;
                         detectionService.OnStatusChanged += OnDetectionServiceStatusChanged;
                         GameProcessor.OnMatchEnd += OnMatchEnd;
                         while (IsGameRun)
                         {
                         }

                         detectionService.OnThrowDetected -= OnAnotherThrow;
                         detectionService.OnStatusChanged -= OnDetectionServiceStatusChanged;
                         GameProcessor.OnMatchEnd -= OnMatchEnd;
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
            dbService.GameEnd(Game, gameResultType: type);
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
            dbService.GameEnd(Game, winner);

            // todo
            mainWindow.Dispatcher.Invoke(() =>
                                         {
                                             foreach (TabItem tabItem in mainWindow.MainTabControl.Items)
                                             {
                                                 tabItem.IsEnabled = !tabItem.IsEnabled;
                                             }

                                             mainWindow.StartGameButton.IsEnabled = !mainWindow.StartGameButton.IsEnabled;
                                             mainWindow.StopGameButton.IsEnabled = !mainWindow.StopGameButton.IsEnabled;
                                             mainWindow.NewGameTypeComboBox.IsEnabled = !mainWindow.NewGameTypeComboBox.IsEnabled;
                                             mainWindow.NewGamePlayer1ComboBox.IsEnabled = !mainWindow.NewGamePlayer1ComboBox.IsEnabled;
                                             mainWindow.NewGamePlayer2ComboBox.IsEnabled = !mainWindow.NewGamePlayer2ComboBox.IsEnabled;
                                             mainWindow.NewGameSetsComboBox.IsEnabled = !mainWindow.NewGameSetsComboBox.IsEnabled;
                                             mainWindow.NewGameLegsComboBox.IsEnabled = !mainWindow.NewGameLegsComboBox.IsEnabled;
                                         });
            //
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
    }
}