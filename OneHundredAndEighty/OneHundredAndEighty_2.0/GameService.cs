#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using OneHundredAndEighty_2._0.Recognition;

#endregion

namespace OneHundredAndEighty_2._0
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
        private bool GameRun { get; set; }

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
            GameRun = true;
            this.players = players;
            game = new Game(type);
            dbService.SaveNewGame(game, players);
            drawService.ProjectionClear();
            drawService.PointsHistoryBoxClear();

            detectionService.RunDetection();

            Task.Run(() =>
                     {
                         while (GameRun)
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
            GameRun = false;
            detectionService.StopDetection();

            drawService.ProjectionClear();
            dbService.EndGame(game);
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