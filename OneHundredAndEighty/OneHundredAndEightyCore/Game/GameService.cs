#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using OneHundredAndEightyCore.Common;
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

        public void StartGame()
        {
            drawService.ProjectionClear();
            drawService.PointsHistoryBoxClear();

            detectionService.PrepareAndTryCapture();
            detectionService.RunDetection();

            var selectedGameType = Enum.Parse<GameType>(mainWindow.NewGameTypeComboBox
                                                                  .SelectionBoxItem
                                                                  .ToString());
            var selectedPlayer1 = mainWindow.NewGamePlayer1ComboBox.SelectedItem as Player;
            var selectedPlayer2 = mainWindow.NewGamePlayer2ComboBox.SelectedItem as Player;
            Game = new Game(selectedGameType);

            Players = new List<Player>();
            if (selectedPlayer1 != null)
            {
                Players.Add(selectedPlayer1);
            }

            if (selectedPlayer2 != null)
            {
                Players.Add(selectedPlayer2);
            }

            switch (selectedGameType)
            {
                case GameType.FreeThrowsSingle:
                    StartFreeThrowsSingle();
                    break;
                case GameType.FreeThrowsDouble:
                    StartFreeThrowsDouble();
                    break;
                case GameType.Classic1001:
                case GameType.Classic701:
                case GameType.Classic501:
                case GameType.Classic301:
                case GameType.Classic101:
                    StartClassic();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectedGameType), selectedGameType, null);
            }

            dbService.SaveNewGame(Game, Players);

            PlayerOnThrow = Players.First();
            PlayerOnSet = Players.First();

            scoreBoardService.OpenScoreBoard(Game.Type);

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

        private void StartClassic()
        {
            throw new NotImplementedException();
        }

        private void StartFreeThrowsSingle()
        {
            // todo
        }

        private void StartFreeThrowsDouble()
        {
            throw new NotImplementedException();
        }

        private void OnAnotherThrow(DetectedThrow thrw)
        {
            CalculatePoints(thrw.TotalPoints);
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
            CheckAndTogglePlayerOnThrow();
        }

        private void CalculatePoints(int thrwTotalPoints)
        {
            switch (Game.Type)
            {
                case GameType.FreeThrowsSingle:
                    PlayerOnThrow.Points += thrwTotalPoints;
                    PlayerOnThrow.ThrowNumber += 1;
                    scoreBoardService.AddPoints(thrwTotalPoints);
                    break;
                case GameType.FreeThrowsDouble:
                    break;
                case GameType.Classic1001:
                case GameType.Classic701:
                case GameType.Classic501:
                case GameType.Classic301:
                case GameType.Classic101:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }       
        }

        private void CheckAndTogglePlayerOnThrow()
        {
            if (PlayerOnThrow.ThrowNumber == 3)
            {
                PlayerOnThrow.ThrowNumber = 1;
                PlayerOnThrow = Players.Count == 1
                                    ? Players.First()
                                    : Players.First(p => p != PlayerOnThrow);
            }
        }
    }
}