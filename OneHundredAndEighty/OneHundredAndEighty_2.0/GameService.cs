#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using OneHundredAndEighty_2._0.Recognition;

#endregion

namespace OneHundredAndEighty_2._0
{
    public class GameService
    {
        private readonly MainWindow mainWindow;
        private readonly ThrowService throwService;
        private readonly ConfigService configService;
        private readonly DrawService drawService;
        private readonly Logger logger;
        private readonly DBService dbService;
        private List<CamService> cams;
        private double moveDetectedSleepTime;
        private double extractionSleepTime;
        private double thresholdSleepTime;
        private bool withDetection;
        private CancellationTokenSource cts;
        private CancellationToken cancelToken;
        private Game game;
        private List<Player> players;

        public GameService(MainWindow mainWindow,
                           ThrowService throwService,
                           ConfigService configService,
                           DrawService drawService,
                           Logger logger,
                           DBService dbService)
        {
            this.mainWindow = mainWindow;
            this.throwService = throwService;
            this.configService = configService;
            this.drawService = drawService;
            this.logger = logger;
            this.dbService = dbService;
        }

        public void StartMatch(GameType type, List<Player> players)
        {
            Prepare(type, players);

            StartMatchInternal();
        }

        public void StopMatch()
        {
            cts?.Cancel();
            drawService.ProjectionClear();
            dbService.EndGame(game);
        }

        private void Prepare(GameType type, List<Player> players)
        {
            game = new Game(type);
            this.players = players;
            dbService.StartNewGame(game, players);

            drawService.ProjectionClear();
            cams = new List<CamService>();
            var cam1Active = configService.Read<bool>(SettingsType.Cam1CheckBox);
            var cam2Active = configService.Read<bool>(SettingsType.Cam2CheckBox);
            var cam3Active = configService.Read<bool>(SettingsType.Cam3CheckBox);
            var cam4Active = configService.Read<bool>(SettingsType.Cam4CheckBox);
            if (cam1Active)
            {
                cams.Add(new CamService(mainWindow, mainWindow.Cam1Grid.Name));
            }

            if (cam2Active)
            {
                cams.Add(new CamService(mainWindow, mainWindow.Cam2Grid.Name));
            }

            if (cam3Active)
            {
                cams.Add(new CamService(mainWindow, mainWindow.Cam3Grid.Name));
            }

            if (cam4Active)
            {
                cams.Add(new CamService(mainWindow, mainWindow.Cam4Grid.Name));
            }

            cts = new CancellationTokenSource();
            cancelToken = cts.Token;

            extractionSleepTime = configService.Read<double>(SettingsType.ExtractionSleepTime);
            thresholdSleepTime = configService.Read<double>(SettingsType.ThresholdSleepTime);
            moveDetectedSleepTime = configService.Read<double>(SettingsType.MoveDetectedSleepTime);
            withDetection = configService.Read<bool>(SettingsType.WithDetectionCheckBox);
        }

        private void StartMatchInternal()
        {
            Task.Run(() =>
                     {
                         Thread.CurrentThread.Name = $"Recognition_workerThread";

                         ClearAllCamsImages();

                         while (!cancelToken.IsCancellationRequested)
                         {
                             foreach (var cam in cams)
                             {
                                 logger.Debug($"Cam_{cam.camNumber} detection start");

                                 var response = withDetection
                                                    ? cam.DetectMove()
                                                    : ResponseType.Nothing;

                                 if (response == ResponseType.Move)
                                 {
                                     Thread.Sleep(TimeSpan.FromSeconds(moveDetectedSleepTime));
                                     response = cam.DetectThrow();

                                     if (response == ResponseType.Trow)
                                     {
                                         cam.FindAndProcessDartContour();

                                         FindThrowOnRemainingCams(cam);

                                         logger.Debug($"Cam_{cam.camNumber} detection end with response type '{ResponseType.Trow}'. Cycle break");
                                         break;
                                     }

                                     if (response == ResponseType.Extraction)
                                     {
                                         Thread.Sleep(TimeSpan.FromSeconds(extractionSleepTime));

                                         drawService.ProjectionClear();
                                         ClearAllCamsImages();

                                         logger.Debug($"Cam_{cam.camNumber} detection end with response type '{ResponseType.Extraction}'. Cycle break");
                                         break;
                                     }
                                 }

                                 Thread.Sleep(TimeSpan.FromSeconds(thresholdSleepTime));

                                 logger.Debug($"Cam_{cam.camNumber} detection end with response type '{ResponseType.Nothing}'");
                             }
                         }

                         foreach (var cam in cams)
                         {
                             cam.Dispose();
                             cam.ClearImageBoxes();
                         }

                         logger.Info($"Detection for {cams.Count} cams end. Cancellation requested");
                     });
        }

        private void FindThrowOnRemainingCams(CamService succeededCam)
        {
            logger.Info($"Finding throws from remaining cams start. Succeeded cam: {succeededCam.camNumber}");

            foreach (var cam in cams.Where(cam => cam != succeededCam))
            {
                cam.FindThrow();
                cam.FindAndProcessDartContour();
            }

            var nextThrow = throwService.GetThrow();
            if (nextThrow != null)
            {
                SaveThrow(nextThrow);
            }

            logger.Info($"Finding throws from remaining cams end");
        }

        private void ClearAllCamsImages()
        {
            logger.Debug($"Clear all cams imageboxes start");

            foreach (var cam in cams)
            {
                cam.DoCapture(true);
            }

            logger.Debug($"Clear all cams imageboxes end");
        }

        private void SaveThrow(DetectedThrow nextThrow)
        {
            var newThrow = new Throw(players.First(),
                                     game,
                                     nextThrow.Sector,
                                     nextThrow.Type,
                                     ThrowResultativity.Ordinary, // todo
                                     1,
                                     nextThrow.TotalPoints,
                                     nextThrow.Poi,
                                     drawService.projectionFrameSide);
            dbService.SaveThrow(newThrow);
        }
    }
}