#region Usings

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Autofac;
using NLog;
using NLog.Web;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Telemetry;
using OneHundredAndEightyCore.Windows.CamsDetection;
using OneHundredAndEightyCore.Windows.DebugPanel;
using OneHundredAndEightyCore.Windows.Score;
using IContainer = Autofac.IContainer;

#endregion

namespace OneHundredAndEightyCore.Windows.Main
{
    public partial class MainWindow
    {
        private readonly MainWindowViewModel viewModel;
        private readonly Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        private readonly TelemetryWriter telemetryWriter = new TelemetryWriter();
        public static IContainer ServiceContainer { get; private set; }
        private bool IsInitialized;

        public MainWindow()
        {
            logger.Info($"\n\nApp start");
            telemetryWriter.WriteAppStart();
            InitializeComponent();
            RegisterContainer();
            viewModel = new MainWindowViewModel(this);
            DataContext = viewModel;
            NewPlayerAvatar.Source = Converter.BitmapToBitmapImage(OneHundredAndEightyCore.Resources.Resources.EmptyUserIcon);
            IsInitialized = true;
        }

        private void RegisterContainer()
        {
            logger.Debug("Services container register start");

            var cb = new ContainerBuilder();

            cb.Register(r => logger).AsSelf().SingleInstance();

            cb.Register(r => telemetryWriter).AsSelf().SingleInstance();

            var messageBoxService = new MessageBoxService();
            cb.Register(r => messageBoxService).AsSelf().SingleInstance();

            var dbService = new DBService();
            cb.Register(r => dbService).AsSelf().SingleInstance();

            var configService = new ConfigService(logger, dbService);
            cb.Register(r => configService).AsSelf().SingleInstance();

            var versionChecker = new VersionChecker(dbService, configService, messageBoxService);
            cb.Register(r => versionChecker).AsSelf().SingleInstance();

            var scoreBoardService = new ScoreBoardService(logger, configService);
            cb.Register(r => scoreBoardService).AsSelf().SingleInstance();

            var camsDetectionBoard = new CamsDetectionBoard(configService, logger);
            cb.Register(r => camsDetectionBoard).AsSelf().SingleInstance();

            var drawService = new DrawService(camsDetectionBoard, configService, logger);
            cb.Register(r => drawService).AsSelf().SingleInstance();

            var throwService = new ThrowService(drawService, logger);
            cb.Register(r => throwService).AsSelf().SingleInstance();

            var detectionService = new DetectionService(this, drawService, configService, throwService, logger);
            cb.Register(r => detectionService).AsSelf().SingleInstance();

            var manualThrowPanel = new ManualThrowPanel(logger, detectionService);
            cb.Register(r => manualThrowPanel).AsSelf().SingleInstance();

            var gameService = new GameService(this, scoreBoardService, camsDetectionBoard, detectionService, configService, drawService, logger, dbService);
            cb.Register(r => gameService).AsSelf().SingleInstance();

            ServiceContainer = cb.Build();

            logger.Debug("Services container register end");
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            logger.Debug("MainWindow on closing");

            viewModel.CloseScoreBoard();
            viewModel.StopGameByButton();
            viewModel.SaveSettingsIfDirty();
            viewModel.CloseManualThrowPanel();
            viewModel.CloseCamsDetectionBoard();
        }

        private void OnTabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel?.SaveSettingsIfDirty();
        }

        private void CalibrateCamsSetupPointButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.CalibrateCamsSetupPoints(Cam1SetupSector.Text,
                                               Cam2SetupSector.Text,
                                               Cam3SetupSector.Text,
                                               Cam4SetupSector.Text,
                                               ToCam1Distance.Text,
                                               ToCam2Distance.Text,
                                               ToCam3Distance.Text,
                                               ToCam4Distance.Text);
        }

        private void CamSetupStartButtonClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button button)
            {
                var grid = button.Parent as Grid;
                viewModel.StartCamSetupCapturing(grid.Name);
            }
        }

        private void CamSetupStopButtonClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button button)
            {
                var grid = button.Parent as Grid;
                viewModel.StopCamSetupCapturing(grid.Name);
            }
        }

        private void SetSettingDirty(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.IsSettingsDirty = true;
            }
        }

        private void StartGameButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.StartGame(NewGameTypeComboBox.Text,
                                NewGamePointsComboBox.Text,
                                NewGamePlayer1ComboBox.SelectedItem as Player,
                                NewGamePlayer2ComboBox.SelectedItem as Player);
        }

        private void StopGameButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.StopGameByButton();
        }

        private void OnSaveNewPlayerButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.SaveNewPlayer(NewPlayerNameTextBox.Text,
                                    NewPlayerNickNameTextBox.Text,
                                    NewPlayerAvatar.Source as BitmapImage);
        }

        private void SelectAvatarImageButton_OnClick(object sender, RoutedEventArgs e)
        {
            viewModel.SelectAvatarImage();
        }

        private void NewGameTypeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsInitialized)
            {
                ToggleNewGameControlsVisibility(((sender as ComboBox)
                                                 ?.SelectedItem as ComboBoxItem)
                                                ?.Content.ToString());
            }
        }

        private void OnHyperlinkNavigate(object sender, RequestNavigateEventArgs e)
        {
            // so ugly because of https://github.com/dotnet/runtime/issues/28005

            var psi = new ProcessStartInfo
                      {
                          FileName = "cmd",
                          WindowStyle = ProcessWindowStyle.Hidden,
                          UseShellExecute = false,
                          CreateNoWindow = true,
                          Arguments = $"/c start {e.Uri.AbsoluteUri}"
                      };

            Process.Start(psi);
        }

        private void OnCrossingStartButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.StartCrossing();
        }

        private void OnCrossingStopButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.StopCrossing();
        }

        private void OnFindCamsButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.FindConnectedCams();
        }

        private void OnCheckCamsButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.CheckCamsSimultaneousWork();
        }

        public void ClearNewPlayerControls()
        {
            NewPlayerNameTextBox.Text = string.Empty;
            NewPlayerNickNameTextBox.Text = string.Empty;
            NewPlayerAvatar.Source = Converter.BitmapToBitmapImage(OneHundredAndEightyCore.Resources.Resources.EmptyUserIcon);
        }

        public void SetCalibratedCamsControls(PointF calibratedCam1SetupPoint,
                                              PointF calibratedCam2SetupPoint,
                                              PointF calibratedCam3SetupPoint,
                                              PointF calibratedCam4SetupPoint)
        {
            Cam1XTextBox.Text = Converter.ToString(calibratedCam1SetupPoint.X);
            Cam1YTextBox.Text = Converter.ToString(calibratedCam1SetupPoint.Y);
            Cam2XTextBox.Text = Converter.ToString(calibratedCam2SetupPoint.X);
            Cam2YTextBox.Text = Converter.ToString(calibratedCam2SetupPoint.Y);
            Cam3XTextBox.Text = Converter.ToString(calibratedCam3SetupPoint.X);
            Cam3YTextBox.Text = Converter.ToString(calibratedCam3SetupPoint.Y);
            Cam4XTextBox.Text = Converter.ToString(calibratedCam4SetupPoint.X);
            Cam4YTextBox.Text = Converter.ToString(calibratedCam4SetupPoint.Y);
        }

        public void SetSelectedAvatar(BitmapImage image)
        {
            NewPlayerAvatar.Source = image;
        }

        public void ToggleMainTabItemsEnabled()
        {
            foreach (TabItem tabItem in MainTabControl.Items)
            {
                tabItem.IsEnabled = !tabItem.IsEnabled;
            }
        }

        public void ToggleMatchControlsEnabled()
        {
            StartGameButton.IsEnabled = !StartGameButton.IsEnabled;
            StopGameButton.IsEnabled = !StopGameButton.IsEnabled;
            NewGameTypeComboBox.IsEnabled = !NewGameTypeComboBox.IsEnabled;
            NewGamePlayer1ComboBox.IsEnabled = !NewGamePlayer1ComboBox.IsEnabled;
            NewGamePlayer2ComboBox.IsEnabled = !NewGamePlayer2ComboBox.IsEnabled;
            NewGameSetsComboBox.IsEnabled = !NewGameSetsComboBox.IsEnabled;
            NewGameLegsComboBox.IsEnabled = !NewGameLegsComboBox.IsEnabled;
            NewGamePointsComboBox.IsEnabled = !NewGamePointsComboBox.IsEnabled;
        }

        public void ToggleSetupTabItemsEnabled()
        {
            foreach (TabItem tabItem in SetupTabControl.Items)
            {
                tabItem.IsEnabled = !tabItem.IsEnabled;
            }
        }

        public void ToggleCamSetupGridControlsEnabled(string gridName)
        {
            switch (gridName)
            {
                case "Cam1Grid":
                    Cam1StartButton.IsEnabled = !Cam1StartButton.IsEnabled;
                    Cam1StopButton.IsEnabled = !Cam1StopButton.IsEnabled;
                    Cam1ThresholdSlider.IsEnabled = !Cam1ThresholdSlider.IsEnabled;
                    Cam1SurfaceSlider.IsEnabled = !Cam1SurfaceSlider.IsEnabled;
                    Cam1SurfaceCenterSlider.IsEnabled = !Cam1SurfaceCenterSlider.IsEnabled;
                    Cam1RoiPosYSlider.IsEnabled = !Cam1RoiPosYSlider.IsEnabled;
                    Cam1RoiHeightSlider.IsEnabled = !Cam1RoiHeightSlider.IsEnabled;
                    break;
                case "Cam2Grid":
                    Cam2StartButton.IsEnabled = !Cam2StartButton.IsEnabled;
                    Cam2StopButton.IsEnabled = !Cam2StopButton.IsEnabled;
                    Cam2ThresholdSlider.IsEnabled = !Cam2ThresholdSlider.IsEnabled;
                    Cam2SurfaceSlider.IsEnabled = !Cam2SurfaceSlider.IsEnabled;
                    Cam2SurfaceCenterSlider.IsEnabled = !Cam2SurfaceCenterSlider.IsEnabled;
                    Cam2RoiPosYSlider.IsEnabled = !Cam2RoiPosYSlider.IsEnabled;
                    Cam2RoiHeightSlider.IsEnabled = !Cam2RoiHeightSlider.IsEnabled;
                    break;
                case "Cam3Grid":
                    Cam3StartButton.IsEnabled = !Cam3StartButton.IsEnabled;
                    Cam3StopButton.IsEnabled = !Cam3StopButton.IsEnabled;
                    Cam3ThresholdSlider.IsEnabled = !Cam3ThresholdSlider.IsEnabled;
                    Cam3SurfaceSlider.IsEnabled = !Cam3SurfaceSlider.IsEnabled;
                    Cam3SurfaceCenterSlider.IsEnabled = !Cam3SurfaceCenterSlider.IsEnabled;
                    Cam3RoiPosYSlider.IsEnabled = !Cam3RoiPosYSlider.IsEnabled;
                    Cam3RoiHeightSlider.IsEnabled = !Cam3RoiHeightSlider.IsEnabled;
                    break;
                case "Cam4Grid":
                    Cam4StartButton.IsEnabled = !Cam4StartButton.IsEnabled;
                    Cam4StopButton.IsEnabled = !Cam4StopButton.IsEnabled;
                    Cam4ThresholdSlider.IsEnabled = !Cam4ThresholdSlider.IsEnabled;
                    Cam4SurfaceSlider.IsEnabled = !Cam4SurfaceSlider.IsEnabled;
                    Cam4SurfaceCenterSlider.IsEnabled = !Cam4SurfaceCenterSlider.IsEnabled;
                    Cam4RoiPosYSlider.IsEnabled = !Cam4RoiPosYSlider.IsEnabled;
                    Cam4RoiHeightSlider.IsEnabled = !Cam4RoiHeightSlider.IsEnabled;
                    break;
            }
        }

        private void ToggleNewGameControlsVisibility(string selection)
        {
            var selectedGameType = Enum.Parse<GameType>(selection);

            switch (selectedGameType)
            {
                case GameType.FreeThrowsSingle:
                    NewGamePlayer2ComboBox.Visibility = Visibility.Hidden;
                    NewGamePlayer2Label.Visibility = Visibility.Hidden;
                    NewGameSetsComboBox.Visibility = Visibility.Hidden;
                    NewGameSetsLabel.Visibility = Visibility.Hidden;
                    NewGameLegsComboBox.Visibility = Visibility.Hidden;
                    NewGameLegsLabel.Visibility = Visibility.Hidden;
                    break;
                case GameType.FreeThrowsDouble:
                    NewGamePlayer2ComboBox.Visibility = Visibility.Visible;
                    NewGamePlayer2Label.Visibility = Visibility.Visible;
                    NewGameSetsComboBox.Visibility = Visibility.Hidden;
                    NewGameSetsLabel.Visibility = Visibility.Hidden;
                    NewGameLegsComboBox.Visibility = Visibility.Hidden;
                    NewGameLegsLabel.Visibility = Visibility.Hidden;
                    break;
                case GameType.Classic:
                    NewGamePlayer2ComboBox.Visibility = Visibility.Visible;
                    NewGamePlayer2Label.Visibility = Visibility.Visible;
                    NewGameSetsComboBox.Visibility = Visibility.Visible;
                    NewGameSetsLabel.Visibility = Visibility.Visible;
                    NewGameLegsComboBox.Visibility = Visibility.Visible;
                    NewGameLegsLabel.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ToggleCrossingButtonsEnabled()
        {
            CrossingStopButton.IsEnabled = !CrossingStopButton.IsEnabled;
            CrossingStartButton.IsEnabled = !CrossingStartButton.IsEnabled;
        }
    }
}