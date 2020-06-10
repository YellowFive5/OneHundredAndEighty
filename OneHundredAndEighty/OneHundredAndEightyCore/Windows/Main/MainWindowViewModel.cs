#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autofac;
using DirectShowLib;
using Microsoft.Win32;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Windows.CamsDetection;
using OneHundredAndEightyCore.Windows.DebugPanel;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Windows.Main
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IMainWindow mainWindow;
        private readonly Logger logger;
        private readonly MessageBoxService messageBoxService;
        private readonly DBService dbService;
        private readonly VersionChecker versionChecker;
        private readonly ConfigService configService;
        private readonly DrawService drawService;
        private readonly ThrowService throwService;
        private readonly DetectionService detectionService;
        private readonly ScoreBoardService scoreBoardService;
        private readonly GameService gameService;
        private readonly ManualThrowPanel manualThrowPanel;
        private readonly CamsDetectionBoard camsDetectionBoard;
        private CancellationTokenSource cts;
        private CancellationToken cancelToken;

        public bool IsSettingsDirty { get; set; }


        #region Bindable props

        private List<Player> players;

        #endregion

        public List<Player> Players
        {
            get => players;
            set
            {
                players = value;
                OnPropertyChanged(nameof(Players));
            }
        }

        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(IMainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            logger = mainWindow.ServiceContainer.Resolve<Logger>();
            messageBoxService = mainWindow.ServiceContainer.Resolve<MessageBoxService>();
            dbService = mainWindow.ServiceContainer.Resolve<DBService>();
            versionChecker = mainWindow.ServiceContainer.Resolve<VersionChecker>();
            configService = mainWindow.ServiceContainer.Resolve<ConfigService>();
            drawService = mainWindow.ServiceContainer.Resolve<DrawService>();
            throwService = mainWindow.ServiceContainer.Resolve<ThrowService>();
            gameService = mainWindow.ServiceContainer.Resolve<GameService>();
            scoreBoardService = mainWindow.ServiceContainer.Resolve<ScoreBoardService>();
            detectionService = mainWindow.ServiceContainer.Resolve<DetectionService>();
            manualThrowPanel = mainWindow.ServiceContainer.Resolve<ManualThrowPanel>();
            camsDetectionBoard = mainWindow.ServiceContainer.Resolve<CamsDetectionBoard>();
            detectionService.OnErrorOccurred += OnDetectionServiceErrorOccurred;

            versionChecker.CheckVersions();

            LoadSettings();
            LoadPlayers();
            FindConnectedCams();
        }

        #region Start\Stop game

        public void StartGame(string newGameType,
                              string newGamePoints,
                              Player player1,
                              Player player2)
        {
            if (!Validator.ValidateImplementedGameTypes(newGameType))
            {
                messageBoxService.ShowError(Resources.Resources.NotImplementedYetErrorText);
                return;
            }

            if (!Validator.ValidateStartNewGamePlayersSelected(newGameType,
                                                               player1,
                                                               player2))
            {
                messageBoxService.ShowError(Resources.Resources.NewGamePlayersNotSelectedErrorText);
                return;
            }

            if (!Validator.ValidateStartNewClassicGame(newGameType,
                                                       newGamePoints))
            {
                messageBoxService.ShowError(Resources.Resources.NewClassicGamePointsNotSelectedErrorText);
                return;
            }

            ToggleControlsWhenStarStopGame();

            try
            {
                camsDetectionBoard.Open();
                drawService.ProjectionPrepare();
                gameService.StartGame();
            }
            catch (Exception e)
            {
                StopGameByError();
                messageBoxService.ShowError($"{e.Message} \n {e.StackTrace}");
            }
        }

        public void StopGameByButton()
        {
            CloseCamsDetectionBoard();

            ToggleControlsWhenStarStopGame();

            gameService.StopGame(GameResultType.Aborted);
        }

        public void StopGameByError()
        {
            CloseScoreBoard();
            CloseCamsDetectionBoard();

            ToggleControlsWhenStarStopGame();

            gameService.StopGame(GameResultType.Error);
        }

        private void ToggleControlsWhenStarStopGame()
        {
            mainWindow.ToggleMainTabItemsEnabled();
            mainWindow.ToggleMatchControlsEnabled();
        }

        #endregion

        public void SaveNewPlayer(string newPlayerName,
                                  string newPlayerNickName,
                                  BitmapImage newPlayerAvatar)
        {
            if (!Validator.ValidateNewPlayerNameAndNickName(newPlayerName, newPlayerNickName))
            {
                messageBoxService.ShowError(Resources.Resources.NewPlayerEmptyDataErrorText);
                return;
            }

            var newPlayer = new Player(newPlayerName,
                                       newPlayerNickName,
                                       -1,
                                       newPlayerAvatar);
            try
            {
                dbService.PlayerSaveNew(newPlayer);
            }
            catch (Exception e)
            {
                messageBoxService.ShowError(e.Message); // todo need to explain error
                return;
            }

            messageBoxService.ShowInfo(Resources.Resources.NewPlayerSuccessfullySavedText, newPlayer);

            mainWindow.ClearNewPlayerControls();

            LoadPlayers();
        }

        private void LoadPlayers()
        {
            var playersTable = dbService.PlayersLoadAll();
            Players = Converter.PlayersFromTable(playersTable);
        }

        public void CalibrateCamsSetupPoints(string cam1SetupSector,
                                             string cam2SetupSector,
                                             string cam3SetupSector,
                                             string cam4SetupSector,
                                             string toCam1Distance,
                                             string toCam2Distance,
                                             string toCam3Distance,
                                             string toCam4Distance)
        {
            IsSettingsDirty = true;
            var calibratedCam1SetupPoint = MeasureService.CalculateCamSetupPoint(Converter.ToDouble(toCam1Distance),
                                                                                 cam1SetupSector);
            var calibratedCam2SetupPoint = MeasureService.CalculateCamSetupPoint(Converter.ToDouble(toCam2Distance),
                                                                                 cam2SetupSector);
            var calibratedCam3SetupPoint = MeasureService.CalculateCamSetupPoint(Converter.ToDouble(toCam3Distance),
                                                                                 cam3SetupSector);
            var calibratedCam4SetupPoint = MeasureService.CalculateCamSetupPoint(Converter.ToDouble(toCam4Distance),
                                                                                 cam4SetupSector);
            mainWindow.SetCalibratedCamsControls(calibratedCam1SetupPoint,
                                                 calibratedCam2SetupPoint,
                                                 calibratedCam3SetupPoint,
                                                 calibratedCam4SetupPoint);

            SaveSettingsIfDirty();
        }

        public void SelectAvatarImage()
        {
            var ofd = new OpenFileDialog
                      {
                          Title = $"{Resources.Resources.ChoosePlayerAvatarText}",
                          Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif"
                      };
            if (ofd.ShowDialog() == true)
            {
                var image = new BitmapImage(new Uri(ofd.FileName));
                if (Validator.ValidateNewPlayerAvatar(image.PixelHeight,
                                                      image.PixelWidth))
                {
                    mainWindow.SetSelectedAvatar(image);
                }
                else
                {
                    messageBoxService.ShowError(Resources.Resources.PlayerAvatarTooBigErrorText);
                }
            }
        }

        #region Error

        private void OnDetectionServiceErrorOccurred(Exception e)
        {
            messageBoxService.ShowError($"{e.Message} \n {e.StackTrace}");
            StopGameByError();
        }

        #endregion

        #region CamSetupCapturing

        public async void StartCamSetupCapturing(CamNumber camNumber)
        {
            ToggleControlsWhenCamSetupCapturing(camNumber);
            cts = new CancellationTokenSource();
            var cancelToken = cts.Token;

            try
            {
                await Task.Run(() =>
                               {
                                   var cam = new CamService(mainWindow, camNumber, CamServiceWorkingMode.Setup);
                                   while (!cancelToken.IsCancellationRequested)
                                   {
                                       cam.DoCapture(true);
                                       cam.RefreshImageBoxes();
                                   }

                                   cam.ClearImageBoxes();
                                   cam.Dispose();
                               });
            }
            catch (Exception e)
            {
                messageBoxService.ShowError($"{e.Message} \n {e.StackTrace}");
                StopCamSetupCapturing(camNumber);
            }
        }

        public void StopCamSetupCapturing(CamNumber camNumber)
        {
            cts?.Cancel();
            ToggleControlsWhenCamSetupCapturing(camNumber);
        }

        private void ToggleControlsWhenCamSetupCapturing(CamNumber camNumber)
        {
            mainWindow.ToggleCamSetupGridControlsEnabled(camNumber);
            mainWindow.ToggleMainTabItemsEnabled();
            mainWindow.ToggleSetupTabItemsEnabled();
        }

        #endregion

        #region RuntimeCrossing

        public void StartCrossing()
        {
            SaveSettingsIfDirty();
            ToggleControlsWhenRuntimeCrossing();

            try
            {
                camsDetectionBoard.Open();

                drawService.ProjectionPrepare();
                drawService.ProjectionClear();
                drawService.PointsHistoryBoxClear();

                detectionService.PrepareCamsAndTryCapture(CamServiceWorkingMode.Crossing);
                detectionService.RunDetection();
            }
            catch (Exception e)
            {
                messageBoxService.ShowError($"{e.Message} \n {e.StackTrace}");
                StopCrossing();
            }
        }

        public void StopCrossing()
        {
            detectionService.StopDetection();
            drawService.ProjectionClear();
            CloseCamsDetectionBoard();

            ToggleControlsWhenRuntimeCrossing();
        }

        private void ToggleControlsWhenRuntimeCrossing()
        {
            mainWindow.ToggleMainTabItemsEnabled();
            mainWindow.ToggleCrossingButtonsEnabled();
            mainWindow.ToggleSetupTabItemsEnabled();
        }

        #endregion

        #region Closing windows

        public void CloseScoreBoard()
        {
            scoreBoardService.CloseScoreBoard();
        }

        public void CloseManualThrowPanel()
        {
            manualThrowPanel.Close();
        }

        public void CloseCamsDetectionBoard()
        {
            camsDetectionBoard.Close();
        }

        #endregion

        #region Settings

        private void LoadSettings()
        {
            logger.Debug("Load settings start");

            mainWindow.LoadAllData(configService);

            IsSettingsDirty = false;

            logger.Debug("Load settings end");
        }

        public void SaveSettingsIfDirty()
        {
            logger.Debug("Save settings start");
            logger.Debug($"Is settings dirty: {IsSettingsDirty}");

            mainWindow.SavePosition(configService);

            if (IsSettingsDirty)
            {
                mainWindow.SaveAllData(configService);

                IsSettingsDirty = false;
            }

            logger.Debug("Save settings end");
        }

        #endregion

        #region PropertyChangingFire

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public void FindConnectedCams()
        {
            var allCams = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice).ToList();
            var str = new StringBuilder();
            for (var i = 0; i < allCams.Count; i++)
            {
                var cam = allCams[i];
                var camId = cam.DevicePath.Substring(44, 10);
                str.AppendLine($"[{cam.Name}]-[ID:'{camId}']");
            }

            var text = allCams.Count == 0
                           ? "No cameras found"
                           : str.ToString();

            mainWindow.SetConnectedCamsText(text);
        }

        public void CheckCamsSimultaneousWork()
        {
            SaveSettingsIfDirty();
            try
            {
                mainWindow.ToggleConnectedCamsControls();
                detectionService.PrepareCamsAndTryCapture(CamServiceWorkingMode.Check);
                mainWindow.SetConnectedCamsText("Checked cams simultaneous work: OK");
                mainWindow.ToggleConnectedCamsControls();
            }
            catch (Exception e)
            {
                mainWindow.ToggleConnectedCamsControls();
                mainWindow.SetConnectedCamsText("Checked cams simultaneous work: ERROR");
            }
        }
    }
}