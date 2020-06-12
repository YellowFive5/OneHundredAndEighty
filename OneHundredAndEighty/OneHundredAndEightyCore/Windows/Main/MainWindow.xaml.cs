#region Usings

using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using NLog;
using NLog.Web;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Telemetry;
using OneHundredAndEightyCore.Windows.CamsDetection;
using OneHundredAndEightyCore.Windows.DebugPanel;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Windows.Main
{
    public partial class MainWindow
    {
        private readonly Logger logger;
        private readonly TelemetryWriter telemetryWriter;
        private readonly MainWindowViewModel viewModel;
        private readonly MessageBoxService messageBoxService;
        private readonly DBService dbService;
        private readonly ConfigService configService;
        private readonly VersionChecker versionChecker;
        private readonly ScoreBoardService scoreBoardService;
        private readonly CamsDetectionBoard camsDetectionBoard;
        private readonly DrawService drawService;
        private readonly ThrowService throwService;
        private readonly DetectionService detectionService;
        private readonly ManualThrowPanel manualThrowPanel;
        private readonly GameService gameService;

        public MainWindow()
        {
            logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            telemetryWriter = new TelemetryWriter();
            logger.Info($"\n\nApp start");
            telemetryWriter.WriteAppStart();
            messageBoxService = new MessageBoxService();
            dbService = new DBService();
            configService = new ConfigService(logger, dbService);
            versionChecker = new VersionChecker(dbService, configService, messageBoxService);
            scoreBoardService = new ScoreBoardService(logger, configService);
            camsDetectionBoard = new CamsDetectionBoard(configService, logger);
            drawService = new DrawService(camsDetectionBoard, configService, logger);
            throwService = new ThrowService(drawService, logger);
            detectionService = new DetectionService(drawService, configService, throwService, logger, camsDetectionBoard);
            manualThrowPanel = new ManualThrowPanel(logger, detectionService);
            gameService = new GameService(scoreBoardService, camsDetectionBoard, detectionService, configService, drawService, logger, dbService);

            InitializeComponent();
            viewModel = new MainWindowViewModel(logger,
                                                messageBoxService,
                                                dbService,
                                                versionChecker,
                                                scoreBoardService,
                                                camsDetectionBoard,
                                                drawService,
                                                detectionService,
                                                manualThrowPanel,
                                                gameService,
                                                configService,
                                                throwService);
            DataContext = viewModel;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel.OnMainWindowLoaded();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            viewModel.OnMainWindowClosing();
        }

        private void CalibrateCamsSetupPointButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.CalibrateCamsSetupPoints();
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
            viewModel.StopCamSetupCapturing();
        }

        private void StartGameButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.StartGame();
        }

        private void StopGameButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.StopGameByButton();
        }

        private void OnSaveNewPlayerButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.SaveNewPlayer();
        }

        private void SelectAvatarImageButton_OnClick(object sender, RoutedEventArgs e)
        {
            viewModel.SelectAvatarImage();
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

        private void DoubleValidation(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void IntValidation(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}