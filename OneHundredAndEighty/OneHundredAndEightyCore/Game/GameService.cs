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

        public void StartGame(GameType type, List<Player> players)
        {
            IsGameRun = true;
            this.players = players;
            game = new Game(type);

            switch (type)
            {
                case GameType.FreeThrows:
                    StartFreeThrows();
                    break;
                case GameType.Classic1001:
                case GameType.Classic701:
                case GameType.Classic501:
                case GameType.Classic301:
                case GameType.Classic101:
                    // todo
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            drawService.ProjectionClear();
            drawService.PointsHistoryBoxClear();
            dbService.SaveNewGame(game, players);

            detectionService.RunDetection();

            Task.Run(() =>
                     {
                         while (IsGameRun)
                         {
                             var thrw = detectionService.TryPopThrow();
                             if (thrw != null)
                             {
                                 SaveThrow(thrw);
                             }
                         }
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

        }

        private void SaveThrow(DetectedThrow thrw)
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