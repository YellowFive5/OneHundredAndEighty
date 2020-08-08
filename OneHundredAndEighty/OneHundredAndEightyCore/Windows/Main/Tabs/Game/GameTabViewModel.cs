#region Usings

using System;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Enums;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Windows.CamsDetection;
using OneHundredAndEightyCore.Windows.Main.Tabs.Shared;
using OneHundredAndEightyCore.Windows.MessageBox;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Windows.Main.Tabs.Game
{
    public class GameTabViewModel : TabViewModelBase
    {
        public GameTabViewModel()
        {
        }

        private readonly GameService gameService;
        private readonly ScoreBoardService scoreBoardService;
        public StartNewGameCommand StartNewGameCommand { get; }
        public StopGameCommand StopGameCommand { get; }

        public const int DefaultNewGameSetsValue = 5;
        public const int DefaultNewGameLegsValue = 3;
        public const GameType DefaultNewGameType = GameType.Classic;
        public const GamePoints DefaultNewGamePoints = GamePoints._501;

        public GameTabViewModel(DataContext dataContext,
                                IDBService dbService,
                                ILogger logger,
                                IConfigService configService,
                                DrawService drawService,
                                IMessageBoxService messageBoxService,
                                CamsDetectionBoard camsDetectionBoard,
                                IDetectionService detectionService,
                                GameService gameService,
                                ScoreBoardService scoreBoardService)
            : base(dataContext, dbService, logger, configService, drawService, messageBoxService, camsDetectionBoard, detectionService)
        {
            this.gameService = gameService;
            this.scoreBoardService = scoreBoardService;

            StartNewGameCommand = new StartNewGameCommand(StartGame);
            StopGameCommand = new StopGameCommand(StopGameByButton);
        }

        #region Bindable props

        public Domain.Player NewGamePlayer1 { get; set; }
        public Domain.Player NewGamePlayer2 { get; set; }

        private GameType newGameType;

        public GameType NewGameType
        {
            get => newGameType;
            set
            {
                if (newGameType != value)
                {
                    newGameType = value;
                    OnPropertyChanged(nameof(NewGameType));
                    IsNewGameForSingle = NewGameType == GameType.FreeThrowsSingle;
                    OnPropertyChanged(nameof(IsNewGameForSingle));
                    IsNewGameForPair = NewGameType != GameType.FreeThrowsSingle;
                    OnPropertyChanged(nameof(IsNewGameForPair));
                    IsNewGameIsClassic = NewGameType == GameType.Classic;
                    OnPropertyChanged(nameof(IsNewGameIsClassic));
                }
            }
        }

        public bool IsNewGameForSingle { get; set; }

        public bool IsNewGameForPair { get; set; }

        public bool IsNewGameIsClassic { get; set; }

        private GamePoints newGamePoints;

        public GamePoints NewGamePoints
        {
            get => newGamePoints;
            set
            {
                if (newGamePoints != value)
                {
                    newGamePoints = value;
                    OnPropertyChanged(nameof(NewGamePoints));
                }
            }
        }

        private int newGameSets;

        public int NewGameSets
        {
            get => newGameSets;
            set
            {
                if (newGameSets != value)
                {
                    newGameSets = value;
                    OnPropertyChanged(nameof(NewGameSets));
                }
            }
        }

        private int newGameLegs;

        public int NewGameLegs
        {
            get => newGameLegs;
            set
            {
                if (newGameLegs != value)
                {
                    newGameLegs = value;
                    OnPropertyChanged(nameof(NewGameLegs));
                }
            }
        }

        private bool isGameRunning;

        public bool IsGameRunning
        {
            get => isGameRunning;
            set
            {
                isGameRunning = value;
                OnPropertyChanged(nameof(IsGameRunning));
            }
        }

        #endregion

        public void LoadSettings()
        {
            NewGameSets = DefaultNewGameSetsValue;
            NewGameLegs = DefaultNewGameLegsValue;
            NewGameType = DefaultNewGameType;
            NewGamePoints = DefaultNewGamePoints;
        }

        private void StartGame()
        {
            if (!ValidateGameStart())
            {
                return;
            }

            //
            // IsMainTabsEnabled = false;
            IsGameRunning = true;

            try
            {
                camsDetectionBoard.Open();

                var cams = CreateCamsServices();
                detectionService.CheckCamsAndTryCapture(cams);
                detectionService.RunDetection(cams, DetectionServiceWorkingMode.Detection);
                gameService.OnGameEnd += StopGameInternal;
                gameService.StartGame(NewGamePlayer1, NewGamePlayer2, NewGameType, NewGamePoints, NewGameSets, NewGameLegs);
            }
            catch (Exception e)
            {
                StopGameByError();
                messageBoxService.ShowError($"{e.Message} \n {e.StackTrace}");
            }
        }

        private bool ValidateGameStart()
        {
            if (!Validator.ValidateImplementedGameTypes(NewGameType))
            {
                messageBoxService.ShowWarning(Resources.Resources.NotImplementedYetErrorText);
                return false;
            }

            if (!Validator.ValidateStartNewGamePlayersSelected(NewGameType,
                                                               NewGamePlayer1,
                                                               NewGamePlayer2))
            {
                messageBoxService.ShowWarning(Resources.Resources.NewGamePlayersNotSelectedErrorText);
                return false;
            }

            if (!Validator.ValidateStartNewClassicGame(NewGameType,
                                                       NewGamePoints))
            {
                messageBoxService.ShowWarning(Resources.Resources.NewClassicGamePointsNotSelectedErrorText);
                return false;
            }

            return true;
        }

        public void StopGameByButton()
        {
            gameService.StopGame(GameResultType.Aborted);
            StopGameInternal();
        }

        public void StopGameByError()
        {
            gameService.StopGame(GameResultType.Error);
            StopGameInternal();
        }

        private void StopGameInternal()
        {
            gameService.OnGameEnd -= StopGameInternal;
            scoreBoardService.CloseScoreBoard();
            camsDetectionBoard.Close();
            detectionService.StopDetection();
            // IsMainTabsEnabled = true;
            IsGameRunning = false;
        }
    }
}