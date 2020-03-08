#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.Game
{
    public class GameService
    {
        private readonly MainWindow mainWindow;
        private readonly DrawService drawService;
        private readonly DetectionService detectionService;
        private readonly ConfigService configService;
        private readonly Logger logger;
        private readonly DBService dbService;
        private Game game;
        private List<Player> players;
        private Player playerOnThrow;
        private bool IsGameRun { get; set; }

        public GameService(MainWindow mainWindow,
                           DetectionService detectionService,
                           ConfigService configService,
                           DrawService drawService,
                           Logger logger,
                           DBService dbService)
        {
            this.mainWindow = mainWindow;
            this.detectionService = detectionService;
            this.configService = configService;
            this.drawService = drawService;
            this.logger = logger;
            this.dbService = dbService;
        }

        public void StartGame()
        {
            IsGameRun = true;
            var selectedGameType = Enum.Parse<GameType>(mainWindow.NewGameTypeComboBox
                                                                  .SelectionBoxItem
                                                                  .ToString());
            var selectedPlayer1 = mainWindow.NewGamePlayer1ComboBox.SelectedItem as Player;
            var selectedPlayer2 = mainWindow.NewGamePlayer2ComboBox.SelectedItem as Player;
            game = new Game(selectedGameType);
            drawService.ProjectionClear();
            drawService.PointsHistoryBoxClear();

            switch (selectedGameType)
            {
                case GameType.FreeThrows:
                    players = new List<Player> {selectedPlayer1};
                    StartFreeThrows();
                    break;
                case GameType.Classic1001:
                case GameType.Classic701:
                case GameType.Classic501:
                case GameType.Classic301:
                case GameType.Classic101:
                    players = new List<Player> {selectedPlayer1, selectedPlayer2};
                    // todo
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectedGameType), selectedGameType, null);
            }

            dbService.SaveNewGame(game, players);

            detectionService.RunDetection();

            Task.Run(() =>
                     {
                         detectionService.OnThrowDetected += OnAnotherThrow;
                         while (IsGameRun)
                         {
                         }
                         detectionService.OnThrowDetected -= OnAnotherThrow;
                     });
        }

        public void StopGame()
        {
            IsGameRun = false;
            detectionService.StopDetection();

            drawService.ProjectionClear();
            dbService.EndGame(game);
        }

        private void StartFreeThrows()
        {
            // todo
        }

        private void OnAnotherThrow(DetectedThrow thrw)
        {
            var dbThrow = new Throw(players.First(),
                                    game,
                                    thrw.Sector,
                                    thrw.Type,
                                    ThrowResultativity.Ordinary, // todo
                                    1,
                                    thrw.TotalPoints,
                                    thrw.Poi,
                                    drawService.projectionFrameSide);
            dbService.SaveThrow(dbThrow);
        }
    }
}