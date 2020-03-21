#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
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
        private Game Game { get; set; }
        private List<Player> Players { get; set; }
        private Player PlayerOnThrow { get; set; }
        private Player PlayerOnSet { get; set; }
        private GameTypeGameService GameType { get; set; }
        private IGameProcessor GameProcessor { get; set; }

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

            var selectedGameTypeDb = Converter.NewGameControlsToGameTypeDb(mainWindow.NewGameControls);
            var selectedGameTypeUi = Converter.NewGameControlsToGameTypeUi(mainWindow.NewGameControls);
            var selectedGameTypeGameService = Converter.NewGameControlsToGameTypeGameService(mainWindow.NewGameControls);

            GameType = selectedGameTypeGameService;

            var selectedPlayer1 = mainWindow.NewGamePlayer1ComboBox.SelectedItem as Player;
            var selectedPlayer2 = mainWindow.NewGamePlayer2ComboBox.SelectedItem as Player;

            Players = new List<Player>();
            if (selectedPlayer1 != null)
            {
                Players.Add(selectedPlayer1);
            }

            if (selectedPlayer2 != null)
            {
                Players.Add(selectedPlayer2);
            }

            PlayerOnThrow = Players.First();
            PlayerOnSet = Players.First();

            Game = new Game(selectedGameTypeDb);

            dbService.SaveNewGame(Game, Players);

            switch (selectedGameTypeGameService)
            {
                case GameTypeGameService.FreeThrowsSingleFreePoints:
                    GameProcessor = new FreeThrowsSingleFreePointsProcessor();
                    scoreBoardService.OpenScoreBoard(selectedGameTypeUi, Players, "Free throws");
                    break;
                case GameTypeGameService.FreeThrowsSingle101Points:
                    break;
                case GameTypeGameService.FreeThrowsSingle301Points:
                    break;
                case GameTypeGameService.FreeThrowsSingle501Points:
                    break;
                case GameTypeGameService.FreeThrowsSingle701Points:
                    break;
                case GameTypeGameService.FreeThrowsSingle1001Points:
                    break;
                case GameTypeGameService.FreeThrowsDoubleFreePoints:
                    break;
                case GameTypeGameService.FreeThrowsDouble101Points:
                    break;
                case GameTypeGameService.FreeThrowsDouble301Points:
                    break;
                case GameTypeGameService.FreeThrowsDouble501Points:
                    break;
                case GameTypeGameService.FreeThrowsDouble701Points:
                    break;
                case GameTypeGameService.FreeThrowsDouble1001Points:
                    break;
                case GameTypeGameService.Classic101Points:
                    break;
                case GameTypeGameService.Classic301Points:
                    break;
                case GameTypeGameService.Classic501Points:
                    break;
                case GameTypeGameService.Classic701Points:
                    break;
                case GameTypeGameService.Classic1001Points:
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
            dbService.EndGame(Game, type);
        }

        #endregion

        private void OnAnotherThrow(DetectedThrow thrw)
        {
            var dbThrow = new Throw(PlayerOnThrow,
                                    Game,
                                    thrw.Sector,
                                    thrw.Type,
                                    ThrowResultativity.Ordinary, // todo
                                    PlayerOnThrow.ThrowNumber,
                                    thrw.TotalPoints,
                                    thrw.Poi,
                                    drawService.projectionFrameSide);
            dbService.SaveThrow(dbThrow);

            GameProcessor.OnThrow(thrw.TotalPoints, Players, PlayerOnThrow, scoreBoardService);
        }
    }
}