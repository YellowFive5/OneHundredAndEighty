#region Usings

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Windows.CamsDetection;
using OneHundredAndEightyCore.Windows.Debug;
using OneHundredAndEightyCore.Windows.Main.Tabs.About;
using OneHundredAndEightyCore.Windows.Main.Tabs.Game;
using OneHundredAndEightyCore.Windows.Main.Tabs.Player;
using OneHundredAndEightyCore.Windows.Main.Tabs.Settings;
using OneHundredAndEightyCore.Windows.MessageBox;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Windows.Main
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ILogger logger;
        private readonly IMessageBoxService messageBoxService;
        private readonly IDbService dbService;
        private readonly IVersionChecker versionChecker;
        private readonly IDetectionService detectionService;
        private readonly ManualThrowPanel manualThrowPanel;
        private readonly IConfigService configService;

        #region Bindable props

        private double mainWindowPositionLeft;

        public double MainWindowPositionLeft
        {
            get => mainWindowPositionLeft;
            set
            {
                mainWindowPositionLeft = value;
                OnPropertyChanged(nameof(MainWindowPositionLeft));
            }
        }

        private double mainWindowPositionTop;

        public double MainWindowPositionTop
        {
            get => mainWindowPositionTop;
            set
            {
                mainWindowPositionTop = value;
                OnPropertyChanged(nameof(MainWindowPositionTop));
            }
        }

        private double mainWindowHeight;

        public double MainWindowHeight
        {
            get => mainWindowHeight;
            set
            {
                mainWindowHeight = value;
                OnPropertyChanged(nameof(MainWindowHeight));
            }
        }

        private double mainWindowWidth;

        public double MainWindowWidth
        {
            get => mainWindowWidth;
            set
            {
                mainWindowWidth = value;
                OnPropertyChanged(nameof(MainWindowWidth));
            }
        }

        #endregion

        public MainWindowViewModel()
        {
        }

        public DataContext DataContext { get; }
        public GameTabViewModel GameTabViewModel { get; }
        public PlayerTabViewModel PlayerTabViewModel { get; }
        public SettingsTabViewModel SettingsTabViewModel { get; }
        public AboutTabViewModel AboutTabViewModel { get; }

        public MainWindowViewModel(ILogger logger,
                                   IMessageBoxService messageBoxService,
                                   IDbService dbService,
                                   IVersionChecker versionChecker,
                                   ScoreBoardService scoreBoardService,
                                   CamsDetectionBoard camsDetectionBoard,
                                   DrawService drawService,
                                   IDetectionService detectionService,
                                   ManualThrowPanel manualThrowPanel,
                                   GameService gameService,
                                   IConfigService configService)
        {
            this.logger = logger;
            this.messageBoxService = messageBoxService;
            this.dbService = dbService;
            this.versionChecker = versionChecker;
            this.detectionService = detectionService;
            this.manualThrowPanel = manualThrowPanel;
            this.configService = configService;

            DataContext = new DataContext();
            GameTabViewModel = new GameTabViewModel(DataContext, dbService, logger, configService, drawService, messageBoxService, camsDetectionBoard, detectionService, gameService, scoreBoardService, manualThrowPanel);
            PlayerTabViewModel = new PlayerTabViewModel(DataContext, logger, configService, drawService, dbService, messageBoxService, camsDetectionBoard, detectionService);
            SettingsTabViewModel = new SettingsTabViewModel(DataContext, dbService, logger, configService, drawService, messageBoxService, camsDetectionBoard, detectionService);
            AboutTabViewModel = new AboutTabViewModel(DataContext, dbService, logger, configService, drawService, messageBoxService, camsDetectionBoard, detectionService);
        }

        public void OnMainWindowLoaded()
        {
            versionChecker.CheckAndUpdate();
            LoadSettings();
            detectionService.OnErrorOccurred += OnDetectionServiceErrorOccurred;
        }

        public void OnMainWindowClosing(CancelEventArgs cancelEventArgs)
        {
            if (!messageBoxService.AskInfoQuestion(Resources.Resources.ExitMessageBoxText))
            {
                cancelEventArgs.Cancel = true;
                return;
            }

            GameTabViewModel.StopGameByButton();
            manualThrowPanel.ClosePanel();
            configService.MainWindowWidth = MainWindowWidth;
            configService.MainWindowHeight = MainWindowHeight;
            configService.MainWindowPositionLeft = MainWindowPositionLeft;
            configService.MainWindowPositionTop = MainWindowPositionTop;

            configService.SaveSettings();
        }

        private void LoadSettings()
        {
            configService.LoadSettings();

            DataContext.Players = Converter.PlayersFromTable(dbService.PlayersAllLoad());

            MainWindowHeight = configService.MainWindowHeight;
            MainWindowWidth = configService.MainWindowWidth;
            MainWindowPositionLeft = configService.MainWindowPositionLeft;
            MainWindowPositionTop = configService.MainWindowPositionTop;

            GameTabViewModel.LoadSettings();
            PlayerTabViewModel.LoadSettings();
            SettingsTabViewModel.LoadSettings();
            SettingsTabViewModel.FindConnectedCams();
        }

        private void OnDetectionServiceErrorOccurred(Exception e)
        {
            messageBoxService.ShowError($"{e.Message} \n {e.StackTrace}");
            GameTabViewModel.StopGameByError();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}