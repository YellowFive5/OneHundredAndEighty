#region Usings

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using NLog;
using OneHundredAndEighty_2._0.Recognition;
using IContainer = Autofac.IContainer;

#endregion

namespace OneHundredAndEighty_2._0
{
    public partial class MainWindow
    {
        private readonly double AppVersion = 2.0;
        private readonly MainWindowViewModel viewModel;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
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


            ServiceContainer = cb.Build();

            logger.Debug("Services container register end");
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            logger.Debug("MainWindow on closing");

            viewModel.SaveSettings();
        }

        private void OnTabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void StopButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void CalibrateCamsSetupPointButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.CalibrateCamsSetupPoint();
        }

        private void CamSetupStartButtonClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void CamSetupStopButtonClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}