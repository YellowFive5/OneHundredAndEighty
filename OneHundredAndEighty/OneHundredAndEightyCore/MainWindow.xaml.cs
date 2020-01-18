#region Usings

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using NLog;
using NLog.Web;
using OneHundredAndEightyCore.Recognition;
using IContainer = Autofac.IContainer;

#endregion

namespace OneHundredAndEightyCore
{
    public partial class MainWindow
    {
        private const double AppVersion = 2.0;
        private readonly MainWindowViewModel viewModel;
        private readonly Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        public static IContainer ServiceContainer { get; private set; }

        public MainWindow()
        {
            logger.Info("\n");
            logger.Info("App start");

            InitializeComponent();
            RegisterContainer();
            viewModel = new MainWindowViewModel(this);
            DataContext = viewModel;
            viewModel.CheckVersion(AppVersion);
            viewModel.LoadSettings();
        }

        private void RegisterContainer()
        {
            logger.Debug("Services container register start");

            var cb = new ContainerBuilder();

            cb.Register(r => logger).AsSelf().SingleInstance();

            var dbService = new DBService();
            cb.Register(r => dbService).AsSelf().SingleInstance();

            var configService = new ConfigService(logger, dbService);
            cb.Register(r => configService).AsSelf().SingleInstance();

            var drawService = new DrawService(this, configService, logger);
            cb.Register(r => drawService).AsSelf().SingleInstance();

            var throwService = new ThrowService(drawService, logger);
            cb.Register(r => throwService).AsSelf().SingleInstance();

            var detectionService = new DetectionService(this, drawService, configService, throwService, logger);
            cb.Register(r => detectionService).AsSelf().SingleInstance();

            var gameService = new GameService(this, detectionService, configService, drawService, logger, dbService);
            cb.Register(r => gameService).AsSelf().SingleInstance();

            ServiceContainer = cb.Build();

            logger.Debug("Services container register end");
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            logger.Debug("MainWindow on closing");

            viewModel.SaveSettingsIfDirty();
        }

        private void OnTabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.SaveSettingsIfDirty();
        }

        private void CalibrateCamsSetupPointButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.IsSettingsDirty = true;
            viewModel.CalibrateCamsSetupPoint();
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

        private void StartFreeThrowsButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.StartFreeThrowsGame();
        }

        private void StopFreeThrowsButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.StopFreeThrowsGame();
        }
    }
}