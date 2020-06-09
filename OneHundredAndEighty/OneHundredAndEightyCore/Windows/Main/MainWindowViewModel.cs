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
        private readonly MainWindow mainWindow;
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

        public MainWindowViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            logger = MainWindow.ServiceContainer.Resolve<Logger>();
            messageBoxService = MainWindow.ServiceContainer.Resolve<MessageBoxService>();
            dbService = MainWindow.ServiceContainer.Resolve<DBService>();
            versionChecker = MainWindow.ServiceContainer.Resolve<VersionChecker>();
            configService = MainWindow.ServiceContainer.Resolve<ConfigService>();
            drawService = MainWindow.ServiceContainer.Resolve<DrawService>();
            throwService = MainWindow.ServiceContainer.Resolve<ThrowService>();
            gameService = MainWindow.ServiceContainer.Resolve<GameService>();
            scoreBoardService = MainWindow.ServiceContainer.Resolve<ScoreBoardService>();
            detectionService = MainWindow.ServiceContainer.Resolve<DetectionService>();
            manualThrowPanel = MainWindow.ServiceContainer.Resolve<ManualThrowPanel>();
            camsDetectionBoard = MainWindow.ServiceContainer.Resolve<CamsDetectionBoard>();
            detectionService.OnErrorOccurred += OnDetectionServiceErrorOccurred;

            versionChecker.CheckVersions();

            LoadSettings();
            LoadPlayers();
            FindConnectedCams();
        }

        #region Start\Stop game

        public void StartGame(string newGameType, string newGamePoints, Player player1, Player player2)
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

        public async void StartCamSetupCapturing(string gridName)
        {
            ToggleControlsWhenCamSetupCapturing(gridName);
            cts = new CancellationTokenSource();
            var cancelToken = cts.Token;

            try
            {
                await Task.Run(() =>
                               {
                                   var cam = new CamService(mainWindow, gridName, CamServiceWorkingMode.Setup);
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
                StopCamSetupCapturing(gridName);
            }
        }

        public void StopCamSetupCapturing(string gridName)
        {
            cts?.Cancel();
            ToggleControlsWhenCamSetupCapturing(gridName);
        }

        private void ToggleControlsWhenCamSetupCapturing(string gridName)
        {
            mainWindow.ToggleCamSetupGridControlsEnabled(gridName);
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

            mainWindow.Left = configService.Read<double>(SettingsType.MainWindowPositionLeft);
            mainWindow.Top = configService.Read<double>(SettingsType.MainWindowPositionTop);
            mainWindow.Height = configService.Read<double>(SettingsType.MainWindowHeight);
            mainWindow.Width = configService.Read<double>(SettingsType.MainWindowWidth);
            mainWindow.CamFovTextBox.Text = Converter.ToString(configService.Read<double>(SettingsType.CamFovAngle));
            mainWindow.CamResolutionHeightTextBox.Text = configService.Read<int>(SettingsType.ResolutionHeight).ToString();
            mainWindow.CamResolutionWidthTextBox.Text = configService.Read<int>(SettingsType.ResolutionWidth).ToString();
            mainWindow.MovesExtractionTextBox.Text = configService.Read<int>(SettingsType.MovesExtraction).ToString();
            mainWindow.MoveDetectedSleepTimeTextBox.Text = Converter.ToString(configService.Read<double>(SettingsType.MoveDetectedSleepTime));
            mainWindow.MovesNoiseTextBox.Text = configService.Read<int>(SettingsType.MovesNoise).ToString();
            mainWindow.SmoothGaussTextBox.Text = configService.Read<int>(SettingsType.SmoothGauss).ToString();
            mainWindow.ThresholdSleepTimeTimeTextBox.Text = Converter.ToString(configService.Read<double>(SettingsType.ThresholdSleepTime));
            mainWindow.ExtractionSleepTimeTimeTextBox.Text = Converter.ToString(configService.Read<double>(SettingsType.ExtractionSleepTime));
            mainWindow.MinContourArcTextBox.Text = configService.Read<int>(SettingsType.MinContourArc).ToString();
            mainWindow.MovesDartTextBox.Text = configService.Read<int>(SettingsType.MovesDart).ToString();
            mainWindow.Cam1IdTextBox.Text = configService.Read<string>(SettingsType.Cam1Id);
            mainWindow.Cam2IdTextBox.Text = configService.Read<string>(SettingsType.Cam2Id);
            mainWindow.Cam3IdTextBox.Text = configService.Read<string>(SettingsType.Cam3Id);
            mainWindow.Cam4IdTextBox.Text = configService.Read<string>(SettingsType.Cam4Id);
            mainWindow.ToCam1Distance.Text = Converter.ToString(configService.Read<double>(SettingsType.ToCam1Distance));
            mainWindow.ToCam2Distance.Text = Converter.ToString(configService.Read<double>(SettingsType.ToCam2Distance));
            mainWindow.ToCam3Distance.Text = Converter.ToString(configService.Read<double>(SettingsType.ToCam3Distance));
            mainWindow.ToCam4Distance.Text = Converter.ToString(configService.Read<double>(SettingsType.ToCam4Distance));
            mainWindow.Cam1XTextBox.Text = configService.Read<int>(SettingsType.Cam1X).ToString();
            mainWindow.Cam2XTextBox.Text = configService.Read<int>(SettingsType.Cam2X).ToString();
            mainWindow.Cam3XTextBox.Text = configService.Read<int>(SettingsType.Cam3X).ToString();
            mainWindow.Cam4XTextBox.Text = configService.Read<int>(SettingsType.Cam4X).ToString();
            mainWindow.Cam1YTextBox.Text = configService.Read<int>(SettingsType.Cam1Y).ToString();
            mainWindow.Cam2YTextBox.Text = configService.Read<int>(SettingsType.Cam2Y).ToString();
            mainWindow.Cam3YTextBox.Text = configService.Read<int>(SettingsType.Cam3Y).ToString();
            mainWindow.Cam4YTextBox.Text = configService.Read<int>(SettingsType.Cam4Y).ToString();
            mainWindow.Cam1CheckBox.IsChecked = configService.Read<bool>(SettingsType.Cam1CheckBox);
            mainWindow.Cam2CheckBox.IsChecked = configService.Read<bool>(SettingsType.Cam2CheckBox);
            mainWindow.Cam3CheckBox.IsChecked = configService.Read<bool>(SettingsType.Cam3CheckBox);
            mainWindow.Cam4CheckBox.IsChecked = configService.Read<bool>(SettingsType.Cam4CheckBox);
            mainWindow.WithDetectionCheckBox.IsChecked = configService.Read<bool>(SettingsType.WithDetectionCheckBox);
            mainWindow.Cam1ThresholdSlider.Value = configService.Read<double>(SettingsType.Cam1ThresholdSlider);
            mainWindow.Cam1SurfaceSlider.Value = configService.Read<double>(SettingsType.Cam1SurfaceSlider);
            mainWindow.Cam1SurfaceCenterSlider.Value = configService.Read<double>(SettingsType.Cam1SurfaceCenterSlider);
            mainWindow.Cam1RoiPosYSlider.Value = configService.Read<double>(SettingsType.Cam1RoiPosYSlider);
            mainWindow.Cam1RoiHeightSlider.Value = configService.Read<double>(SettingsType.Cam1RoiHeightSlider);
            mainWindow.Cam2ThresholdSlider.Value = configService.Read<double>(SettingsType.Cam2ThresholdSlider);
            mainWindow.Cam2SurfaceSlider.Value = configService.Read<double>(SettingsType.Cam2SurfaceSlider);
            mainWindow.Cam2SurfaceCenterSlider.Value = configService.Read<double>(SettingsType.Cam2SurfaceCenterSlider);
            mainWindow.Cam2RoiPosYSlider.Value = configService.Read<double>(SettingsType.Cam2RoiPosYSlider);
            mainWindow.Cam2RoiHeightSlider.Value = configService.Read<double>(SettingsType.Cam2RoiHeightSlider);
            mainWindow.Cam3ThresholdSlider.Value = configService.Read<double>(SettingsType.Cam3ThresholdSlider);
            mainWindow.Cam3SurfaceSlider.Value = configService.Read<double>(SettingsType.Cam3SurfaceSlider);
            mainWindow.Cam3SurfaceCenterSlider.Value = configService.Read<double>(SettingsType.Cam3SurfaceCenterSlider);
            mainWindow.Cam3RoiPosYSlider.Value = configService.Read<double>(SettingsType.Cam3RoiPosYSlider);
            mainWindow.Cam3RoiHeightSlider.Value = configService.Read<double>(SettingsType.Cam3RoiHeightSlider);
            mainWindow.Cam4ThresholdSlider.Value = configService.Read<double>(SettingsType.Cam4ThresholdSlider);
            mainWindow.Cam4SurfaceSlider.Value = configService.Read<double>(SettingsType.Cam4SurfaceSlider);
            mainWindow.Cam4SurfaceCenterSlider.Value = configService.Read<double>(SettingsType.Cam4SurfaceCenterSlider);
            mainWindow.Cam4RoiPosYSlider.Value = configService.Read<double>(SettingsType.Cam4RoiPosYSlider);
            mainWindow.Cam4RoiHeightSlider.Value = configService.Read<double>(SettingsType.Cam4RoiHeightSlider);
            mainWindow.Cam1SetupSector.SelectedIndex = Converter.CamSetupSectorSettingValueToComboboxSelectedIndex(configService.Read<string>(SettingsType.Cam1SetupSector));
            mainWindow.Cam2SetupSector.SelectedIndex = Converter.CamSetupSectorSettingValueToComboboxSelectedIndex(configService.Read<string>(SettingsType.Cam2SetupSector));
            mainWindow.Cam3SetupSector.SelectedIndex = Converter.CamSetupSectorSettingValueToComboboxSelectedIndex(configService.Read<string>(SettingsType.Cam3SetupSector));
            mainWindow.Cam4SetupSector.SelectedIndex = Converter.CamSetupSectorSettingValueToComboboxSelectedIndex(configService.Read<string>(SettingsType.Cam4SetupSector));

            IsSettingsDirty = false;

            logger.Debug("Load settings end");
        }

        public void SaveSettingsIfDirty()
        {
            logger.Debug("Save settings start");
            logger.Debug($"Is settings dirty: {IsSettingsDirty}");

            configService.Write(SettingsType.MainWindowPositionLeft, mainWindow.Left);
            configService.Write(SettingsType.MainWindowPositionTop, mainWindow.Top);
            configService.Write(SettingsType.MainWindowHeight, mainWindow.Height);
            configService.Write(SettingsType.MainWindowWidth, mainWindow.Width);

            if (IsSettingsDirty)
            {
                configService.Write(SettingsType.CamFovAngle, mainWindow.CamFovTextBox.Text);
                configService.Write(SettingsType.ResolutionHeight, mainWindow.CamResolutionHeightTextBox.Text);
                configService.Write(SettingsType.ResolutionWidth, mainWindow.CamResolutionWidthTextBox.Text);
                configService.Write(SettingsType.MovesExtraction, mainWindow.MovesExtractionTextBox.Text);
                configService.Write(SettingsType.MoveDetectedSleepTime, mainWindow.MoveDetectedSleepTimeTextBox.Text);
                configService.Write(SettingsType.MovesNoise, mainWindow.MovesNoiseTextBox.Text);
                configService.Write(SettingsType.SmoothGauss, mainWindow.SmoothGaussTextBox.Text);
                configService.Write(SettingsType.ThresholdSleepTime, mainWindow.ThresholdSleepTimeTimeTextBox.Text);
                configService.Write(SettingsType.ExtractionSleepTime, mainWindow.ExtractionSleepTimeTimeTextBox.Text);
                configService.Write(SettingsType.MinContourArc, mainWindow.MinContourArcTextBox.Text);
                configService.Write(SettingsType.MovesDart, mainWindow.MovesDartTextBox.Text);
                configService.Write(SettingsType.Cam1Id, mainWindow.Cam1IdTextBox.Text);
                configService.Write(SettingsType.Cam2Id, mainWindow.Cam2IdTextBox.Text);
                configService.Write(SettingsType.Cam3Id, mainWindow.Cam3IdTextBox.Text);
                configService.Write(SettingsType.Cam4Id, mainWindow.Cam4IdTextBox.Text);
                configService.Write(SettingsType.ToCam1Distance, mainWindow.ToCam1Distance.Text);
                configService.Write(SettingsType.ToCam2Distance, mainWindow.ToCam2Distance.Text);
                configService.Write(SettingsType.ToCam3Distance, mainWindow.ToCam3Distance.Text);
                configService.Write(SettingsType.ToCam4Distance, mainWindow.ToCam4Distance.Text);
                configService.Write(SettingsType.Cam1X, mainWindow.Cam1XTextBox.Text);
                configService.Write(SettingsType.Cam2X, mainWindow.Cam2XTextBox.Text);
                configService.Write(SettingsType.Cam3X, mainWindow.Cam3XTextBox.Text);
                configService.Write(SettingsType.Cam4X, mainWindow.Cam4XTextBox.Text);
                configService.Write(SettingsType.Cam1Y, mainWindow.Cam1YTextBox.Text);
                configService.Write(SettingsType.Cam2Y, mainWindow.Cam2YTextBox.Text);
                configService.Write(SettingsType.Cam3Y, mainWindow.Cam3YTextBox.Text);
                configService.Write(SettingsType.Cam4Y, mainWindow.Cam4YTextBox.Text);
                configService.Write(SettingsType.Cam1CheckBox, mainWindow.Cam1CheckBox.IsChecked);
                configService.Write(SettingsType.Cam2CheckBox, mainWindow.Cam2CheckBox.IsChecked);
                configService.Write(SettingsType.Cam3CheckBox, mainWindow.Cam3CheckBox.IsChecked);
                configService.Write(SettingsType.Cam4CheckBox, mainWindow.Cam4CheckBox.IsChecked);
                configService.Write(SettingsType.WithDetectionCheckBox, mainWindow.WithDetectionCheckBox.IsChecked);
                configService.Write(SettingsType.Cam1ThresholdSlider, mainWindow.Cam1ThresholdSlider.Value);
                configService.Write(SettingsType.Cam1SurfaceSlider, mainWindow.Cam1SurfaceSlider.Value);
                configService.Write(SettingsType.Cam1SurfaceCenterSlider, mainWindow.Cam1SurfaceCenterSlider.Value);
                configService.Write(SettingsType.Cam1RoiPosYSlider, mainWindow.Cam1RoiPosYSlider.Value);
                configService.Write(SettingsType.Cam1RoiHeightSlider, mainWindow.Cam1RoiHeightSlider.Value);
                configService.Write(SettingsType.Cam2ThresholdSlider, mainWindow.Cam2ThresholdSlider.Value);
                configService.Write(SettingsType.Cam2SurfaceSlider, mainWindow.Cam2SurfaceSlider.Value);
                configService.Write(SettingsType.Cam2SurfaceCenterSlider, mainWindow.Cam2SurfaceCenterSlider.Value);
                configService.Write(SettingsType.Cam2RoiPosYSlider, mainWindow.Cam2RoiPosYSlider.Value);
                configService.Write(SettingsType.Cam2RoiHeightSlider, mainWindow.Cam2RoiHeightSlider.Value);
                configService.Write(SettingsType.Cam3ThresholdSlider, mainWindow.Cam3ThresholdSlider.Value);
                configService.Write(SettingsType.Cam3SurfaceSlider, mainWindow.Cam3SurfaceSlider.Value);
                configService.Write(SettingsType.Cam3SurfaceCenterSlider, mainWindow.Cam3SurfaceCenterSlider.Value);
                configService.Write(SettingsType.Cam3RoiPosYSlider, mainWindow.Cam3RoiPosYSlider.Value);
                configService.Write(SettingsType.Cam3RoiHeightSlider, mainWindow.Cam3RoiHeightSlider.Value);
                configService.Write(SettingsType.Cam4ThresholdSlider, mainWindow.Cam4ThresholdSlider.Value);
                configService.Write(SettingsType.Cam4SurfaceSlider, mainWindow.Cam4SurfaceSlider.Value);
                configService.Write(SettingsType.Cam4SurfaceCenterSlider, mainWindow.Cam4SurfaceCenterSlider.Value);
                configService.Write(SettingsType.Cam4RoiPosYSlider, mainWindow.Cam4RoiPosYSlider.Value);
                configService.Write(SettingsType.Cam4RoiHeightSlider, mainWindow.Cam4RoiHeightSlider.Value);
                configService.Write(SettingsType.Cam1SetupSector, mainWindow.Cam1SetupSector.Text);
                configService.Write(SettingsType.Cam2SetupSector, mainWindow.Cam2SetupSector.Text);
                configService.Write(SettingsType.Cam3SetupSector, mainWindow.Cam3SetupSector.Text);
                configService.Write(SettingsType.Cam4SetupSector, mainWindow.Cam4SetupSector.Text);
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

            mainWindow.CamsTextBox.Text = allCams.Count == 0
                                              ? "No cameras found"
                                              : str.ToString();
        }

        public void CheckCamsSimultaneousWork()
        {
            SaveSettingsIfDirty();
            try
            {
                mainWindow.CheckCams.IsEnabled = false;
                detectionService.PrepareCamsAndTryCapture(CamServiceWorkingMode.Check);
                mainWindow.CamsTextBox.Text = "Checked cams simultaneous work: OK";
                mainWindow.CheckCams.IsEnabled = true;
            }
            catch (Exception e)
            {
                mainWindow.CheckCams.IsEnabled = true;
                mainWindow.CamsTextBox.Text = "Checked cams simultaneous work: ERROR";
            }
        }
    }
}