#region Usings

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using NLog;
using NLog.Web;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Telemetry;
using OneHundredAndEightyCore.Windows.CamsDetection;
using OneHundredAndEightyCore.Windows.Debug;
using OneHundredAndEightyCore.Windows.MessageBox;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Windows.Main
{
    public partial class MainWindow
    {
        private readonly Version appVersion = new Version(2, 4);

        private readonly Logger logger;
        private readonly TelemetryWriter telemetryWriter;
        private readonly MainWindowViewModel viewModel;
        private readonly MessageBoxService messageBoxService;
        private readonly DBService dbService;
        private readonly ConfigService configService;
        private readonly FileSystemService fileSystemService;
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
            logger.Info("\n\nApp start");
            telemetryWriter.WriteAppStart();
            messageBoxService = new MessageBoxService();
            dbService = new DBService();
            drawService = new DrawService(logger);
            throwService = new ThrowService(logger);
            configService = new ConfigService(logger, dbService);
            scoreBoardService = new ScoreBoardService(logger, configService, drawService);
            camsDetectionBoard = new CamsDetectionBoard(configService, logger, drawService);
            fileSystemService = new FileSystemService();
            versionChecker = new VersionChecker(appVersion, fileSystemService, dbService, configService, messageBoxService);
            detectionService = new DetectionService(drawService, configService, throwService, logger, camsDetectionBoard);
            manualThrowPanel = new ManualThrowPanel(logger, detectionService);
            gameService = new GameService(scoreBoardService, camsDetectionBoard, detectionService, logger, dbService, manualThrowPanel);

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
                                                configService);
            DataContext = viewModel;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel.OnMainWindowLoaded();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            viewModel.OnMainWindowClosing(e);
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void OnCloseButtonClick(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void OnMaximizeButtonClick(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState == WindowState.Normal
                              ? WindowState.Maximized
                              : WindowState.Normal;
        }

        private void OnMinimizeButtonClick(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}