#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            mainWindow.NewPlayerAvatar.Source = Converter.BitmapToBitmapImage(Resources.Resources.EmptyUserIcon);
            LoadPlayers();
            FindConnectedCams();
        }

        private void LoadPlayers()
        {
            var playersTable = dbService.PlayersLoadAll();
            Players = Converter.PlayersFromTable(playersTable);
        }

        public void StartGame()
        {
            if (!Validator.ValidateImplementedGameTypes(mainWindow.NewGameTypeComboBox))
            {
                messageBoxService.ShowError(Resources.Resources.NotImplementedYetErrorText);
                return;
            }

            if (!Validator.ValidateStartNewGamePlayersSelected(mainWindow.NewGameTypeComboBox,
                                                               mainWindow.NewGamePlayer1ComboBox,
                                                               mainWindow.NewGamePlayer2ComboBox))
            {
                messageBoxService.ShowError(Resources.Resources.NewGamePlayersNotSelectedErrorText);
                return;
            }

            if (!Validator.ValidateStartNewClassicGamePoints(mainWindow.NewGameTypeComboBox, mainWindow.NewGamePointsComboBox))
            {
                messageBoxService.ShowError(Resources.Resources.NewClassicGamePointsNotSelectedErrorText);
                return;
            }

            ToggleMainTabItemsEnabled();
            ToggleMatchControlsEnabled();

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

            ToggleMainTabItemsEnabled();
            ToggleMatchControlsEnabled();

            gameService.StopGame(GameResultType.Aborted);
        }

        public void StopGameByError()
        {
            CloseScoreBoard();
            CloseCamsDetectionBoard();

            ToggleMainTabItemsEnabled();
            ToggleMatchControlsEnabled();

            gameService.StopGame(GameResultType.Error);
        }

        public void SaveNewPlayer()
        {
            var newPlayerName = mainWindow.NewPlayerNameTextBox.Text;
            var newPlayerNickName = mainWindow.NewPlayerNickNameTextBox.Text;
            if (!Validator.ValidateNewPlayerNameAndNickName(newPlayerName, newPlayerNickName))
            {
                messageBoxService.ShowError(Resources.Resources.NewPlayerEmptyDataErrorText);
                return;
            }

            var newPlayer = new Player(newPlayerName,
                                       newPlayerNickName,
                                       -1,
                                       mainWindow.NewPlayerAvatar.Source as BitmapImage);
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

            mainWindow.NewPlayerNameTextBox.Text = string.Empty;
            mainWindow.NewPlayerNickNameTextBox.Text = string.Empty;
            mainWindow.NewPlayerAvatar.Source = Converter.BitmapToBitmapImage(Resources.Resources.EmptyUserIcon);

            LoadPlayers();
        }

        public void CalibrateCamsSetupPoints()
        {
            var cam1SetupSector = mainWindow.Cam1SetupSector.Text;
            var cam2SetupSector = mainWindow.Cam2SetupSector.Text;
            var cam3SetupSector = mainWindow.Cam3SetupSector.Text;
            var cam4SetupSector = mainWindow.Cam4SetupSector.Text;

            var toCam1CmDistance = Converter.ToDouble(mainWindow.ToCam1Distance.Text);
            var toCam2CmDistance = Converter.ToDouble(mainWindow.ToCam2Distance.Text);
            var toCam3CmDistance = Converter.ToDouble(mainWindow.ToCam3Distance.Text);
            var toCam4CmDistance = Converter.ToDouble(mainWindow.ToCam4Distance.Text);

            var calibratedCam1SetupPoint = MeasureService.CalculateCamSetupPoint(toCam1CmDistance, cam1SetupSector);
            var calibratedCam2SetupPoint = MeasureService.CalculateCamSetupPoint(toCam2CmDistance, cam2SetupSector);
            var calibratedCam3SetupPoint = MeasureService.CalculateCamSetupPoint(toCam3CmDistance, cam3SetupSector);
            var calibratedCam4SetupPoint = MeasureService.CalculateCamSetupPoint(toCam4CmDistance, cam4SetupSector);

            mainWindow.Cam1XTextBox.Text = Converter.ToString(calibratedCam1SetupPoint.X);
            mainWindow.Cam1YTextBox.Text = Converter.ToString(calibratedCam1SetupPoint.Y);
            mainWindow.Cam2XTextBox.Text = Converter.ToString(calibratedCam2SetupPoint.X);
            mainWindow.Cam2YTextBox.Text = Converter.ToString(calibratedCam2SetupPoint.Y);
            mainWindow.Cam3XTextBox.Text = Converter.ToString(calibratedCam3SetupPoint.X);
            mainWindow.Cam3YTextBox.Text = Converter.ToString(calibratedCam3SetupPoint.Y);
            mainWindow.Cam4XTextBox.Text = Converter.ToString(calibratedCam4SetupPoint.X);
            mainWindow.Cam4YTextBox.Text = Converter.ToString(calibratedCam4SetupPoint.Y);

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
                if (Validator.ValidateNewPlayerAvatar(image))
                {
                    mainWindow.NewPlayerAvatar.Source = image;
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
            ToggleCamSetupGridControlsEnabled(gridName);
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
            ToggleCamSetupGridControlsEnabled(gridName);
        }

        #endregion

        #region RuntimeCrossing

        public void StartCrossing()
        {
            SaveSettingsIfDirty();
            ToggleMainTabItemsEnabled();
            mainWindow.CrossingStopButton.IsEnabled = !mainWindow.CrossingStopButton.IsEnabled;

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

            ToggleMainTabItemsEnabled();
            mainWindow.CrossingStopButton.IsEnabled = !mainWindow.CrossingStopButton.IsEnabled;
        }

        #endregion

        #region Controls toggles

        private void ToggleCamSetupGridControlsEnabled(string gridName)
        {
            ToggleMainTabItemsEnabled();
            ToggleSetupTabItemsEnabled();

            switch (gridName)
            {
                case "Cam1Grid":
                    mainWindow.Cam1StartButton.IsEnabled = !mainWindow.Cam1StartButton.IsEnabled;
                    mainWindow.Cam1StopButton.IsEnabled = !mainWindow.Cam1StopButton.IsEnabled;
                    mainWindow.Cam1ThresholdSlider.IsEnabled = !mainWindow.Cam1ThresholdSlider.IsEnabled;
                    mainWindow.Cam1SurfaceSlider.IsEnabled = !mainWindow.Cam1SurfaceSlider.IsEnabled;
                    mainWindow.Cam1SurfaceCenterSlider.IsEnabled = !mainWindow.Cam1SurfaceCenterSlider.IsEnabled;
                    mainWindow.Cam1RoiPosYSlider.IsEnabled = !mainWindow.Cam1RoiPosYSlider.IsEnabled;
                    mainWindow.Cam1RoiHeightSlider.IsEnabled = !mainWindow.Cam1RoiHeightSlider.IsEnabled;
                    break;
                case "Cam2Grid":
                    mainWindow.Cam2StartButton.IsEnabled = !mainWindow.Cam2StartButton.IsEnabled;
                    mainWindow.Cam2StopButton.IsEnabled = !mainWindow.Cam2StopButton.IsEnabled;
                    mainWindow.Cam2ThresholdSlider.IsEnabled = !mainWindow.Cam2ThresholdSlider.IsEnabled;
                    mainWindow.Cam2SurfaceSlider.IsEnabled = !mainWindow.Cam2SurfaceSlider.IsEnabled;
                    mainWindow.Cam2SurfaceCenterSlider.IsEnabled = !mainWindow.Cam2SurfaceCenterSlider.IsEnabled;
                    mainWindow.Cam2RoiPosYSlider.IsEnabled = !mainWindow.Cam2RoiPosYSlider.IsEnabled;
                    mainWindow.Cam2RoiHeightSlider.IsEnabled = !mainWindow.Cam2RoiHeightSlider.IsEnabled;
                    break;
                case "Cam3Grid":
                    mainWindow.Cam3StartButton.IsEnabled = !mainWindow.Cam3StartButton.IsEnabled;
                    mainWindow.Cam3StopButton.IsEnabled = !mainWindow.Cam3StopButton.IsEnabled;
                    mainWindow.Cam3ThresholdSlider.IsEnabled = !mainWindow.Cam3ThresholdSlider.IsEnabled;
                    mainWindow.Cam3SurfaceSlider.IsEnabled = !mainWindow.Cam3SurfaceSlider.IsEnabled;
                    mainWindow.Cam3SurfaceCenterSlider.IsEnabled = !mainWindow.Cam3SurfaceCenterSlider.IsEnabled;
                    mainWindow.Cam3RoiPosYSlider.IsEnabled = !mainWindow.Cam3RoiPosYSlider.IsEnabled;
                    mainWindow.Cam3RoiHeightSlider.IsEnabled = !mainWindow.Cam3RoiHeightSlider.IsEnabled;
                    break;
                case "Cam4Grid":
                    mainWindow.Cam4StartButton.IsEnabled = !mainWindow.Cam4StartButton.IsEnabled;
                    mainWindow.Cam4StopButton.IsEnabled = !mainWindow.Cam4StopButton.IsEnabled;
                    mainWindow.Cam4ThresholdSlider.IsEnabled = !mainWindow.Cam4ThresholdSlider.IsEnabled;
                    mainWindow.Cam4SurfaceSlider.IsEnabled = !mainWindow.Cam4SurfaceSlider.IsEnabled;
                    mainWindow.Cam4SurfaceCenterSlider.IsEnabled = !mainWindow.Cam4SurfaceCenterSlider.IsEnabled;
                    mainWindow.Cam4RoiPosYSlider.IsEnabled = !mainWindow.Cam4RoiPosYSlider.IsEnabled;
                    mainWindow.Cam4RoiHeightSlider.IsEnabled = !mainWindow.Cam4RoiHeightSlider.IsEnabled;
                    break;
            }
        }

        private void ToggleSetupTabItemsEnabled()
        {
            foreach (TabItem tabItem in mainWindow.SetupTabControl.Items)
            {
                tabItem.IsEnabled = !tabItem.IsEnabled;
            }
        }

        private void ToggleMainTabItemsEnabled()
        {
            foreach (TabItem tabItem in mainWindow.MainTabControl.Items)
            {
                tabItem.IsEnabled = !tabItem.IsEnabled;
            }

            mainWindow.CrossingStartButton.IsEnabled = !mainWindow.CrossingStartButton.IsEnabled;
        }

        private void ToggleMatchControlsEnabled()
        {
            mainWindow.StartGameButton.IsEnabled = !mainWindow.StartGameButton.IsEnabled;
            mainWindow.StopGameButton.IsEnabled = !mainWindow.StopGameButton.IsEnabled;
            mainWindow.NewGameTypeComboBox.IsEnabled = !mainWindow.NewGameTypeComboBox.IsEnabled;
            mainWindow.NewGamePlayer1ComboBox.IsEnabled = !mainWindow.NewGamePlayer1ComboBox.IsEnabled;
            mainWindow.NewGamePlayer2ComboBox.IsEnabled = !mainWindow.NewGamePlayer2ComboBox.IsEnabled;
            mainWindow.NewGameSetsComboBox.IsEnabled = !mainWindow.NewGameSetsComboBox.IsEnabled;
            mainWindow.NewGameLegsComboBox.IsEnabled = !mainWindow.NewGameLegsComboBox.IsEnabled;
            mainWindow.NewGamePointsComboBox.IsEnabled = !mainWindow.NewGamePointsComboBox.IsEnabled;
        }

        public void ToggleNewGameControlsVisibility()
        {
            var selectedGameType = Converter.NewGameControlsToGameType(mainWindow.NewGameTypeComboBox);

            switch (selectedGameType)
            {
                case GameType.FreeThrowsSingle:
                    mainWindow.NewGamePlayer2ComboBox.Visibility = Visibility.Hidden;
                    mainWindow.NewGamePlayer2Label.Visibility = Visibility.Hidden;
                    mainWindow.NewGameSetsComboBox.Visibility = Visibility.Hidden;
                    mainWindow.NewGameSetsLabel.Visibility = Visibility.Hidden;
                    mainWindow.NewGameLegsComboBox.Visibility = Visibility.Hidden;
                    mainWindow.NewGameLegsLabel.Visibility = Visibility.Hidden;
                    break;
                case GameType.FreeThrowsDouble:
                    mainWindow.NewGamePlayer2ComboBox.Visibility = Visibility.Visible;
                    mainWindow.NewGamePlayer2Label.Visibility = Visibility.Visible;
                    mainWindow.NewGameSetsComboBox.Visibility = Visibility.Hidden;
                    mainWindow.NewGameSetsLabel.Visibility = Visibility.Hidden;
                    mainWindow.NewGameLegsComboBox.Visibility = Visibility.Hidden;
                    mainWindow.NewGameLegsLabel.Visibility = Visibility.Hidden;
                    break;
                case GameType.Classic:
                    mainWindow.NewGamePlayer2ComboBox.Visibility = Visibility.Visible;
                    mainWindow.NewGamePlayer2Label.Visibility = Visibility.Visible;
                    mainWindow.NewGameSetsComboBox.Visibility = Visibility.Visible;
                    mainWindow.NewGameSetsLabel.Visibility = Visibility.Visible;
                    mainWindow.NewGameLegsComboBox.Visibility = Visibility.Visible;
                    mainWindow.NewGameLegsLabel.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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