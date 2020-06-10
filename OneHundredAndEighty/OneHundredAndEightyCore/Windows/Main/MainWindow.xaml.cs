#region Usings

using System;
using System.Collections.Generic;
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
    public partial class MainWindow : IMainWindow
    {
        private readonly MainWindowViewModel viewModel;
        private readonly Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        private readonly TelemetryWriter telemetryWriter = new TelemetryWriter();
        private bool IsInitialized;

        public IContainer ServiceContainer { get; private set; }

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
                viewModel.StartCamSetupCapturing(Converter.GridNameToCamNumber(grid?.Name));
            }
        }

        private void CamSetupStopButtonClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button button)
            {
                var grid = button.Parent as Grid;
                viewModel.StopCamSetupCapturing(Converter.GridNameToCamNumber(grid?.Name));
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

        public void SetCamImages(CamNumber number, BitmapImage image, BitmapImage roiImage)
        {
            switch (number)
            {
                case CamNumber._1:
                    Cam1ImageBox.Source = image;
                    Cam1ImageBoxRoi.Source = roiImage;
                    break;
                case CamNumber._2:
                    Cam2ImageBox.Source = image;
                    Cam2ImageBoxRoi.Source = roiImage;
                    break;
                case CamNumber._3:
                    Cam3ImageBox.Source = image;
                    Cam3ImageBoxRoi.Source = roiImage;
                    break;
                case CamNumber._4:
                    Cam4ImageBox.Source = image;
                    Cam4ImageBoxRoi.Source = roiImage;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(number), number, null);
            }
        }

        public void SavePosition(ConfigService configService)
        {
            configService.Write(SettingsType.MainWindowPositionLeft, Left);
            configService.Write(SettingsType.MainWindowPositionTop, Top);
            configService.Write(SettingsType.MainWindowHeight, Height);
            configService.Write(SettingsType.MainWindowWidth, Width);
        }

        public void SaveAllData(ConfigService configService)
        {
            configService.Write(SettingsType.CamFovAngle, CamFovTextBox.Text);
            configService.Write(SettingsType.ResolutionHeight, CamResolutionHeightTextBox.Text);
            configService.Write(SettingsType.ResolutionWidth, CamResolutionWidthTextBox.Text);
            configService.Write(SettingsType.MovesExtraction, MovesExtractionTextBox.Text);
            configService.Write(SettingsType.MoveDetectedSleepTime, MoveDetectedSleepTimeTextBox.Text);
            configService.Write(SettingsType.MovesNoise, MovesNoiseTextBox.Text);
            configService.Write(SettingsType.SmoothGauss, SmoothGaussTextBox.Text);
            configService.Write(SettingsType.ThresholdSleepTime, ThresholdSleepTimeTimeTextBox.Text);
            configService.Write(SettingsType.ExtractionSleepTime, ExtractionSleepTimeTimeTextBox.Text);
            configService.Write(SettingsType.MinContourArc, MinContourArcTextBox.Text);
            configService.Write(SettingsType.MovesDart, MovesDartTextBox.Text);
            configService.Write(SettingsType.Cam1Id, Cam1IdTextBox.Text);
            configService.Write(SettingsType.Cam2Id, Cam2IdTextBox.Text);
            configService.Write(SettingsType.Cam3Id, Cam3IdTextBox.Text);
            configService.Write(SettingsType.Cam4Id, Cam4IdTextBox.Text);
            configService.Write(SettingsType.ToCam1Distance, ToCam1Distance.Text);
            configService.Write(SettingsType.ToCam2Distance, ToCam2Distance.Text);
            configService.Write(SettingsType.ToCam3Distance, ToCam3Distance.Text);
            configService.Write(SettingsType.ToCam4Distance, ToCam4Distance.Text);
            configService.Write(SettingsType.Cam1X, Cam1XTextBox.Text);
            configService.Write(SettingsType.Cam2X, Cam2XTextBox.Text);
            configService.Write(SettingsType.Cam3X, Cam3XTextBox.Text);
            configService.Write(SettingsType.Cam4X, Cam4XTextBox.Text);
            configService.Write(SettingsType.Cam1Y, Cam1YTextBox.Text);
            configService.Write(SettingsType.Cam2Y, Cam2YTextBox.Text);
            configService.Write(SettingsType.Cam3Y, Cam3YTextBox.Text);
            configService.Write(SettingsType.Cam4Y, Cam4YTextBox.Text);
            configService.Write(SettingsType.Cam1CheckBox, Cam1CheckBox.IsChecked);
            configService.Write(SettingsType.Cam2CheckBox, Cam2CheckBox.IsChecked);
            configService.Write(SettingsType.Cam3CheckBox, Cam3CheckBox.IsChecked);
            configService.Write(SettingsType.Cam4CheckBox, Cam4CheckBox.IsChecked);
            configService.Write(SettingsType.WithDetectionCheckBox, WithDetectionCheckBox.IsChecked);
            configService.Write(SettingsType.Cam1ThresholdSlider, Cam1ThresholdSlider.Value);
            configService.Write(SettingsType.Cam1SurfaceSlider, Cam1SurfaceSlider.Value);
            configService.Write(SettingsType.Cam1SurfaceCenterSlider, Cam1SurfaceCenterSlider.Value);
            configService.Write(SettingsType.Cam1RoiPosYSlider, Cam1RoiPosYSlider.Value);
            configService.Write(SettingsType.Cam1RoiHeightSlider, Cam1RoiHeightSlider.Value);
            configService.Write(SettingsType.Cam2ThresholdSlider, Cam2ThresholdSlider.Value);
            configService.Write(SettingsType.Cam2SurfaceSlider, Cam2SurfaceSlider.Value);
            configService.Write(SettingsType.Cam2SurfaceCenterSlider, Cam2SurfaceCenterSlider.Value);
            configService.Write(SettingsType.Cam2RoiPosYSlider, Cam2RoiPosYSlider.Value);
            configService.Write(SettingsType.Cam2RoiHeightSlider, Cam2RoiHeightSlider.Value);
            configService.Write(SettingsType.Cam3ThresholdSlider, Cam3ThresholdSlider.Value);
            configService.Write(SettingsType.Cam3SurfaceSlider, Cam3SurfaceSlider.Value);
            configService.Write(SettingsType.Cam3SurfaceCenterSlider, Cam3SurfaceCenterSlider.Value);
            configService.Write(SettingsType.Cam3RoiPosYSlider, Cam3RoiPosYSlider.Value);
            configService.Write(SettingsType.Cam3RoiHeightSlider, Cam3RoiHeightSlider.Value);
            configService.Write(SettingsType.Cam4ThresholdSlider, Cam4ThresholdSlider.Value);
            configService.Write(SettingsType.Cam4SurfaceSlider, Cam4SurfaceSlider.Value);
            configService.Write(SettingsType.Cam4SurfaceCenterSlider, Cam4SurfaceCenterSlider.Value);
            configService.Write(SettingsType.Cam4RoiPosYSlider, Cam4RoiPosYSlider.Value);
            configService.Write(SettingsType.Cam4RoiHeightSlider, Cam4RoiHeightSlider.Value);
            configService.Write(SettingsType.Cam1SetupSector, Cam1SetupSector.Text);
            configService.Write(SettingsType.Cam2SetupSector, Cam2SetupSector.Text);
            configService.Write(SettingsType.Cam3SetupSector, Cam3SetupSector.Text);
            configService.Write(SettingsType.Cam4SetupSector, Cam4SetupSector.Text);
        }

        public void LoadAllData(ConfigService configService)
        {
            Left = configService.Read<double>(SettingsType.MainWindowPositionLeft);
            Top = configService.Read<double>(SettingsType.MainWindowPositionTop);
            Height = configService.Read<double>(SettingsType.MainWindowHeight);
            Width = configService.Read<double>(SettingsType.MainWindowWidth);

            CamFovTextBox.Text = Converter.ToString(configService.Read<double>(SettingsType.CamFovAngle));
            CamResolutionHeightTextBox.Text = configService.Read<int>(SettingsType.ResolutionHeight).ToString();
            CamResolutionWidthTextBox.Text = configService.Read<int>(SettingsType.ResolutionWidth).ToString();
            MovesExtractionTextBox.Text = configService.Read<int>(SettingsType.MovesExtraction).ToString();
            MoveDetectedSleepTimeTextBox.Text = Converter.ToString(configService.Read<double>(SettingsType.MoveDetectedSleepTime));
            MovesNoiseTextBox.Text = configService.Read<int>(SettingsType.MovesNoise).ToString();
            SmoothGaussTextBox.Text = configService.Read<int>(SettingsType.SmoothGauss).ToString();
            ThresholdSleepTimeTimeTextBox.Text = Converter.ToString(configService.Read<double>(SettingsType.ThresholdSleepTime));
            ExtractionSleepTimeTimeTextBox.Text = Converter.ToString(configService.Read<double>(SettingsType.ExtractionSleepTime));
            MinContourArcTextBox.Text = configService.Read<int>(SettingsType.MinContourArc).ToString();
            MovesDartTextBox.Text = configService.Read<int>(SettingsType.MovesDart).ToString();
            Cam1IdTextBox.Text = configService.Read<string>(SettingsType.Cam1Id);
            Cam2IdTextBox.Text = configService.Read<string>(SettingsType.Cam2Id);
            Cam3IdTextBox.Text = configService.Read<string>(SettingsType.Cam3Id);
            Cam4IdTextBox.Text = configService.Read<string>(SettingsType.Cam4Id);
            ToCam1Distance.Text = Converter.ToString(configService.Read<double>(SettingsType.ToCam1Distance));
            ToCam2Distance.Text = Converter.ToString(configService.Read<double>(SettingsType.ToCam2Distance));
            ToCam3Distance.Text = Converter.ToString(configService.Read<double>(SettingsType.ToCam3Distance));
            ToCam4Distance.Text = Converter.ToString(configService.Read<double>(SettingsType.ToCam4Distance));
            Cam1XTextBox.Text = configService.Read<int>(SettingsType.Cam1X).ToString();
            Cam2XTextBox.Text = configService.Read<int>(SettingsType.Cam2X).ToString();
            Cam3XTextBox.Text = configService.Read<int>(SettingsType.Cam3X).ToString();
            Cam4XTextBox.Text = configService.Read<int>(SettingsType.Cam4X).ToString();
            Cam1YTextBox.Text = configService.Read<int>(SettingsType.Cam1Y).ToString();
            Cam2YTextBox.Text = configService.Read<int>(SettingsType.Cam2Y).ToString();
            Cam3YTextBox.Text = configService.Read<int>(SettingsType.Cam3Y).ToString();
            Cam4YTextBox.Text = configService.Read<int>(SettingsType.Cam4Y).ToString();
            Cam1CheckBox.IsChecked = configService.Read<bool>(SettingsType.Cam1CheckBox);
            Cam2CheckBox.IsChecked = configService.Read<bool>(SettingsType.Cam2CheckBox);
            Cam3CheckBox.IsChecked = configService.Read<bool>(SettingsType.Cam3CheckBox);
            Cam4CheckBox.IsChecked = configService.Read<bool>(SettingsType.Cam4CheckBox);
            WithDetectionCheckBox.IsChecked = configService.Read<bool>(SettingsType.WithDetectionCheckBox);
            Cam1ThresholdSlider.Value = configService.Read<double>(SettingsType.Cam1ThresholdSlider);
            Cam1SurfaceSlider.Value = configService.Read<double>(SettingsType.Cam1SurfaceSlider);
            Cam1SurfaceCenterSlider.Value = configService.Read<double>(SettingsType.Cam1SurfaceCenterSlider);
            Cam1RoiPosYSlider.Value = configService.Read<double>(SettingsType.Cam1RoiPosYSlider);
            Cam1RoiHeightSlider.Value = configService.Read<double>(SettingsType.Cam1RoiHeightSlider);
            Cam2ThresholdSlider.Value = configService.Read<double>(SettingsType.Cam2ThresholdSlider);
            Cam2SurfaceSlider.Value = configService.Read<double>(SettingsType.Cam2SurfaceSlider);
            Cam2SurfaceCenterSlider.Value = configService.Read<double>(SettingsType.Cam2SurfaceCenterSlider);
            Cam2RoiPosYSlider.Value = configService.Read<double>(SettingsType.Cam2RoiPosYSlider);
            Cam2RoiHeightSlider.Value = configService.Read<double>(SettingsType.Cam2RoiHeightSlider);
            Cam3ThresholdSlider.Value = configService.Read<double>(SettingsType.Cam3ThresholdSlider);
            Cam3SurfaceSlider.Value = configService.Read<double>(SettingsType.Cam3SurfaceSlider);
            Cam3SurfaceCenterSlider.Value = configService.Read<double>(SettingsType.Cam3SurfaceCenterSlider);
            Cam3RoiPosYSlider.Value = configService.Read<double>(SettingsType.Cam3RoiPosYSlider);
            Cam3RoiHeightSlider.Value = configService.Read<double>(SettingsType.Cam3RoiHeightSlider);
            Cam4ThresholdSlider.Value = configService.Read<double>(SettingsType.Cam4ThresholdSlider);
            Cam4SurfaceSlider.Value = configService.Read<double>(SettingsType.Cam4SurfaceSlider);
            Cam4SurfaceCenterSlider.Value = configService.Read<double>(SettingsType.Cam4SurfaceCenterSlider);
            Cam4RoiPosYSlider.Value = configService.Read<double>(SettingsType.Cam4RoiPosYSlider);
            Cam4RoiHeightSlider.Value = configService.Read<double>(SettingsType.Cam4RoiHeightSlider);
            Cam1SetupSector.SelectedIndex = Converter.CamSetupSectorSettingValueToComboboxSelectedIndex(configService.Read<string>(SettingsType.Cam1SetupSector));
            Cam2SetupSector.SelectedIndex = Converter.CamSetupSectorSettingValueToComboboxSelectedIndex(configService.Read<string>(SettingsType.Cam2SetupSector));
            Cam3SetupSector.SelectedIndex = Converter.CamSetupSectorSettingValueToComboboxSelectedIndex(configService.Read<string>(SettingsType.Cam3SetupSector));
            Cam4SetupSector.SelectedIndex = Converter.CamSetupSectorSettingValueToComboboxSelectedIndex(configService.Read<string>(SettingsType.Cam4SetupSector));
        }

        public List<double> GetCamsSetupSlidersData(CamNumber camNumber)
        {
            return Dispatcher.Invoke(() =>
                                     {
                                         switch (camNumber)
                                         {
                                             case CamNumber._1:
                                                 return new List<double>
                                                        {
                                                            Cam1ThresholdSlider.Value,
                                                            Cam1RoiPosYSlider.Value,
                                                            Cam1RoiHeightSlider.Value,
                                                            Cam1SurfaceSlider.Value,
                                                            Cam1SurfaceCenterSlider.Value
                                                        };
                                             case CamNumber._2:
                                                 return new List<double>
                                                        {
                                                            Cam2ThresholdSlider.Value,
                                                            Cam2RoiPosYSlider.Value,
                                                            Cam2RoiHeightSlider.Value,
                                                            Cam2SurfaceSlider.Value,
                                                            Cam2SurfaceCenterSlider.Value
                                                        };
                                             case CamNumber._3:
                                                 return new List<double>
                                                        {
                                                            Cam3ThresholdSlider.Value,
                                                            Cam3RoiPosYSlider.Value,
                                                            Cam3RoiHeightSlider.Value,
                                                            Cam3SurfaceSlider.Value,
                                                            Cam3SurfaceCenterSlider.Value
                                                        };
                                             case CamNumber._4:
                                                 return new List<double>
                                                        {
                                                            Cam4ThresholdSlider.Value,
                                                            Cam4RoiPosYSlider.Value,
                                                            Cam4RoiHeightSlider.Value,
                                                            Cam4SurfaceSlider.Value,
                                                            Cam4SurfaceCenterSlider.Value
                                                        };
                                             default:
                                                 throw new ArgumentOutOfRangeException(nameof(camNumber), camNumber, null);
                                         }
                                     });
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

        public void ToggleCamSetupGridControlsEnabled(CamNumber camNumber)
        {
            switch (camNumber)
            {
                case CamNumber._1:
                    Cam1StartButton.IsEnabled = !Cam1StartButton.IsEnabled;
                    Cam1StopButton.IsEnabled = !Cam1StopButton.IsEnabled;
                    Cam1ThresholdSlider.IsEnabled = !Cam1ThresholdSlider.IsEnabled;
                    Cam1SurfaceSlider.IsEnabled = !Cam1SurfaceSlider.IsEnabled;
                    Cam1SurfaceCenterSlider.IsEnabled = !Cam1SurfaceCenterSlider.IsEnabled;
                    Cam1RoiPosYSlider.IsEnabled = !Cam1RoiPosYSlider.IsEnabled;
                    Cam1RoiHeightSlider.IsEnabled = !Cam1RoiHeightSlider.IsEnabled;
                    break;
                case CamNumber._2:
                    Cam2StartButton.IsEnabled = !Cam2StartButton.IsEnabled;
                    Cam2StopButton.IsEnabled = !Cam2StopButton.IsEnabled;
                    Cam2ThresholdSlider.IsEnabled = !Cam2ThresholdSlider.IsEnabled;
                    Cam2SurfaceSlider.IsEnabled = !Cam2SurfaceSlider.IsEnabled;
                    Cam2SurfaceCenterSlider.IsEnabled = !Cam2SurfaceCenterSlider.IsEnabled;
                    Cam2RoiPosYSlider.IsEnabled = !Cam2RoiPosYSlider.IsEnabled;
                    Cam2RoiHeightSlider.IsEnabled = !Cam2RoiHeightSlider.IsEnabled;
                    break;
                case CamNumber._3:
                    Cam3StartButton.IsEnabled = !Cam3StartButton.IsEnabled;
                    Cam3StopButton.IsEnabled = !Cam3StopButton.IsEnabled;
                    Cam3ThresholdSlider.IsEnabled = !Cam3ThresholdSlider.IsEnabled;
                    Cam3SurfaceSlider.IsEnabled = !Cam3SurfaceSlider.IsEnabled;
                    Cam3SurfaceCenterSlider.IsEnabled = !Cam3SurfaceCenterSlider.IsEnabled;
                    Cam3RoiPosYSlider.IsEnabled = !Cam3RoiPosYSlider.IsEnabled;
                    Cam3RoiHeightSlider.IsEnabled = !Cam3RoiHeightSlider.IsEnabled;
                    break;
                case CamNumber._4:
                    Cam4StartButton.IsEnabled = !Cam4StartButton.IsEnabled;
                    Cam4StopButton.IsEnabled = !Cam4StopButton.IsEnabled;
                    Cam4ThresholdSlider.IsEnabled = !Cam4ThresholdSlider.IsEnabled;
                    Cam4SurfaceSlider.IsEnabled = !Cam4SurfaceSlider.IsEnabled;
                    Cam4SurfaceCenterSlider.IsEnabled = !Cam4SurfaceCenterSlider.IsEnabled;
                    Cam4RoiPosYSlider.IsEnabled = !Cam4RoiPosYSlider.IsEnabled;
                    Cam4RoiHeightSlider.IsEnabled = !Cam4RoiHeightSlider.IsEnabled;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(camNumber), camNumber, null);
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

        public void SetConnectedCamsText(string text)
        {
            CamsTextBox.Text = text;
        }

        public void ToggleConnectedCamsControls()
        {
            CheckCams.IsEnabled = !CheckCams.IsEnabled;
        }
    }
}