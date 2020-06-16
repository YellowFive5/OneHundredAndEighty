#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
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
        private readonly Logger logger;
        private readonly MessageBoxService messageBoxService;
        private readonly IDBService dbService;
        private readonly IVersionChecker versionChecker;
        private readonly ScoreBoardService scoreBoardService;
        private readonly CamsDetectionBoard camsDetectionBoard;
        private readonly DrawService drawService;
        private readonly IDetectionService detectionService;
        private readonly ManualThrowPanel manualThrowPanel;
        private readonly GameService gameService;
        private readonly IConfigService configService;
        private readonly ThrowService throwService;
        private CancellationTokenSource cts;
        private CancellationToken cancelToken;

        #region Bindable props

        #region Window position

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

        #region Images

        private BitmapImage camImage;

        public BitmapImage CamImage
        {
            get => camImage;
            set
            {
                camImage = value;
                OnPropertyChanged(nameof(CamImage));
            }
        }

        private BitmapImage camRoiImage;

        public BitmapImage CamRoiImage
        {
            get => camRoiImage;
            set
            {
                camRoiImage = value;
                OnPropertyChanged(nameof(CamRoiImage));
            }
        }

        #endregion

        #region CamsSetupSliders

        private double cam1ThresholdSliderValue;

        public double Cam1ThresholdSliderValue
        {
            get => cam1ThresholdSliderValue;
            set
            {
                cam1ThresholdSliderValue = value;
                configService.Cam1ThresholdSliderValue = value;
                OnPropertyChanged(nameof(Cam1ThresholdSliderValue));
            }
        }

        private double cam2ThresholdSliderValue;

        public double Cam2ThresholdSliderValue
        {
            get => cam2ThresholdSliderValue;
            set
            {
                cam2ThresholdSliderValue = value;
                configService.Cam2ThresholdSliderValue = value;
                OnPropertyChanged(nameof(Cam2ThresholdSliderValue));
            }
        }

        private double cam3ThresholdSliderValue;

        public double Cam3ThresholdSliderValue
        {
            get => cam3ThresholdSliderValue;
            set
            {
                cam3ThresholdSliderValue = value;
                configService.Cam3ThresholdSliderValue = value;
                OnPropertyChanged(nameof(Cam3ThresholdSliderValue));
            }
        }

        private double cam4ThresholdSliderValue;

        public double Cam4ThresholdSliderValue
        {
            get => cam4ThresholdSliderValue;
            set
            {
                cam4ThresholdSliderValue = value;
                configService.Cam4ThresholdSliderValue = value;
                OnPropertyChanged(nameof(Cam4ThresholdSliderValue));
            }
        }

        private double cam1SurfaceSliderValue;

        public double Cam1SurfaceSliderValue
        {
            get => cam1SurfaceSliderValue;
            set
            {
                cam1SurfaceSliderValue = value;
                configService.Cam1SurfaceSliderValue = value;
                OnPropertyChanged(nameof(Cam1SurfaceSliderValue));
            }
        }

        private double cam2SurfaceSliderValue;

        public double Cam2SurfaceSliderValue
        {
            get => cam2SurfaceSliderValue;
            set
            {
                cam2SurfaceSliderValue = value;
                configService.Cam2SurfaceSliderValue = value;
                OnPropertyChanged(nameof(Cam2SurfaceSliderValue));
            }
        }

        private double cam3SurfaceSliderValue;

        public double Cam3SurfaceSliderValue
        {
            get => cam3SurfaceSliderValue;
            set
            {
                cam3SurfaceSliderValue = value;
                configService.Cam3SurfaceSliderValue = value;
                OnPropertyChanged(nameof(Cam3SurfaceSliderValue));
            }
        }

        private double cam4SurfaceSliderValue;

        public double Cam4SurfaceSliderValue
        {
            get => cam4SurfaceSliderValue;
            set
            {
                cam4SurfaceSliderValue = value;
                configService.Cam4SurfaceSliderValue = value;
                OnPropertyChanged(nameof(Cam4SurfaceSliderValue));
            }
        }

        private double cam1SurfaceCenterSliderValue;

        public double Cam1SurfaceCenterSliderValue
        {
            get => cam1SurfaceCenterSliderValue;
            set
            {
                cam1SurfaceCenterSliderValue = value;
                configService.Cam1SurfaceCenterSliderValue = value;
                OnPropertyChanged(nameof(Cam1SurfaceCenterSliderValue));
            }
        }

        private double cam2SurfaceCenterSliderValue;

        public double Cam2SurfaceCenterSliderValue
        {
            get => cam2SurfaceCenterSliderValue;
            set
            {
                cam2SurfaceCenterSliderValue = value;
                configService.Cam2SurfaceCenterSliderValue = value;
                OnPropertyChanged(nameof(Cam2SurfaceCenterSliderValue));
            }
        }

        private double cam3SurfaceCenterSliderValue;

        public double Cam3SurfaceCenterSliderValue
        {
            get => cam3SurfaceCenterSliderValue;
            set
            {
                cam3SurfaceCenterSliderValue = value;
                configService.Cam3SurfaceCenterSliderValue = value;
                OnPropertyChanged(nameof(Cam3SurfaceCenterSliderValue));
            }
        }

        private double cam4SurfaceCenterSliderValue;

        public double Cam4SurfaceCenterSliderValue
        {
            get => cam4SurfaceCenterSliderValue;
            set
            {
                cam4SurfaceCenterSliderValue = value;
                configService.Cam4SurfaceCenterSliderValue = value;
                OnPropertyChanged(nameof(Cam4SurfaceCenterSliderValue));
            }
        }

        private double cam1RoiPosYSliderValue;

        public double Cam1RoiPosYSliderValue
        {
            get => cam1RoiPosYSliderValue;
            set
            {
                cam1RoiPosYSliderValue = value;
                configService.Cam1RoiPosYSliderValue = value;
                OnPropertyChanged(nameof(Cam1RoiPosYSliderValue));
            }
        }

        private double cam2RoiPosYSliderValue;

        public double Cam2RoiPosYSliderValue
        {
            get => cam2RoiPosYSliderValue;
            set
            {
                cam2RoiPosYSliderValue = value;
                configService.Cam2RoiPosYSliderValue = value;
                OnPropertyChanged(nameof(Cam2RoiPosYSliderValue));
            }
        }

        private double cam3RoiPosYSliderValue;

        public double Cam3RoiPosYSliderValue
        {
            get => cam3RoiPosYSliderValue;
            set
            {
                cam3RoiPosYSliderValue = value;
                configService.Cam3RoiPosYSliderValue = value;
                OnPropertyChanged(nameof(Cam3RoiPosYSliderValue));
            }
        }

        private double cam4RoiPosYSliderValue;

        public double Cam4RoiPosYSliderValue
        {
            get => cam4RoiPosYSliderValue;
            set
            {
                cam4RoiPosYSliderValue = value;
                configService.Cam4RoiPosYSliderValue = value;
                OnPropertyChanged(nameof(Cam4RoiPosYSliderValue));
            }
        }

        private double cam1RoiHeightSliderValue;

        public double Cam1RoiHeightSliderValue
        {
            get => cam1RoiHeightSliderValue;
            set
            {
                cam1RoiHeightSliderValue = value;
                configService.Cam1RoiHeightSliderValue = value;
                OnPropertyChanged(nameof(Cam1RoiHeightSliderValue));
            }
        }

        private double cam2RoiHeightSliderValue;

        public double Cam2RoiHeightSliderValue
        {
            get => cam2RoiHeightSliderValue;
            set
            {
                cam2RoiHeightSliderValue = value;
                configService.Cam2RoiHeightSliderValue = value;
                OnPropertyChanged(nameof(Cam2RoiHeightSliderValue));
            }
        }

        private double cam3RoiHeightSliderValue;

        public double Cam3RoiHeightSliderValue
        {
            get => cam3RoiHeightSliderValue;
            set
            {
                cam3RoiHeightSliderValue = value;
                configService.Cam3RoiHeightSliderValue = value;
                OnPropertyChanged(nameof(Cam3RoiHeightSliderValue));
            }
        }

        private double cam4RoiHeightSliderValue;

        public double Cam4RoiHeightSliderValue
        {
            get => cam4RoiHeightSliderValue;
            set
            {
                cam4RoiHeightSliderValue = value;
                configService.Cam4RoiHeightSliderValue = value;
                OnPropertyChanged(nameof(Cam4RoiHeightSliderValue));
            }
        }

        #endregion

        #region Main setup tab

        private bool cam1Enabled;

        public bool Cam1Enabled
        {
            get => cam1Enabled;
            set
            {
                cam1Enabled = value;
                configService.Cam1Enabled = value;
                OnPropertyChanged(nameof(Cam1Enabled));
            }
        }

        private bool cam2Enabled;

        public bool Cam2Enabled
        {
            get => cam2Enabled;
            set
            {
                cam2Enabled = value;
                configService.Cam2Enabled = value;
                OnPropertyChanged(nameof(Cam2Enabled));
            }
        }

        private bool cam3Enabled;

        public bool Cam3Enabled
        {
            get => cam3Enabled;
            set
            {
                cam3Enabled = value;
                configService.Cam3Enabled = value;
                OnPropertyChanged(nameof(Cam3Enabled));
            }
        }

        private bool cam4Enabled;

        public bool Cam4Enabled
        {
            get => cam4Enabled;
            set
            {
                cam4Enabled = value;
                configService.Cam4Enabled = value;
                OnPropertyChanged(nameof(Cam4Enabled));
            }
        }

        private bool detectionEnabled;

        public bool DetectionEnabled
        {
            get => detectionEnabled;
            set
            {
                detectionEnabled = value;
                configService.DetectionEnabled = value;
                OnPropertyChanged(nameof(DetectionEnabled));
            }
        }

        private string cam1Id;

        public string Cam1Id
        {
            get => cam1Id;
            set
            {
                configService.Cam1Id = value;
                cam1Id = value;
                OnPropertyChanged(nameof(Cam1Id));
            }
        }

        private string cam2Id;

        public string Cam2Id
        {
            get => cam2Id;
            set
            {
                configService.Cam2Id = value;
                cam2Id = value;
                OnPropertyChanged(nameof(Cam2Id));
            }
        }

        private string cam3Id;

        public string Cam3Id
        {
            get => cam3Id;
            set
            {
                configService.Cam3Id = value;
                cam3Id = value;
                OnPropertyChanged(nameof(Cam3Id));
            }
        }

        private string cam4Id;

        public string Cam4Id
        {
            get => cam4Id;
            set
            {
                configService.Cam4Id = value;
                cam4Id = value;
                OnPropertyChanged(nameof(Cam4Id));
            }
        }

        private double camsFovAngle;

        public double CamsFovAngle
        {
            get => camsFovAngle;
            set
            {
                camsFovAngle = value;
                configService.CamsFovAngle = value;
                OnPropertyChanged(nameof(CamsFovAngle));
            }
        }

        private int camsResolutionHeight;

        public int CamsResolutionHeight
        {
            get => camsResolutionHeight;
            set
            {
                camsResolutionHeight = value;
                configService.CamResolutionHeight = value;
                OnPropertyChanged(nameof(CamsResolutionHeight));
            }
        }

        private int camsResolutionWidth;

        public int CamsResolutionWidth
        {
            get => camsResolutionWidth;
            set
            {
                camsResolutionWidth = value;
                configService.CamResolutionWidth = value;
                OnPropertyChanged(nameof(CamsResolutionWidth));
            }
        }

        private int movesExtractionValue;

        public int MovesExtractionValue
        {
            get => movesExtractionValue;
            set
            {
                movesExtractionValue = value;
                configService.MovesExtractionValue = value;
                OnPropertyChanged(nameof(MovesExtractionValue));
            }
        }

        private double movesDetectedSleepTimeValue;

        public double MovesDetectedSleepTimeValue
        {
            get => movesDetectedSleepTimeValue;
            set
            {
                movesDetectedSleepTimeValue = value;
                configService.MoveDetectedSleepTimeValue = value;
                OnPropertyChanged(nameof(MovesDetectedSleepTimeValue));
            }
        }

        private int movesNoiseValue;

        public int MovesNoiseValue
        {
            get => movesNoiseValue;
            set
            {
                movesNoiseValue = value;
                configService.MovesNoiseValue = value;
                OnPropertyChanged(nameof(MovesNoiseValue));
            }
        }

        private int smoothGaussValue;

        public int SmoothGaussValue
        {
            get => smoothGaussValue;
            set
            {
                smoothGaussValue = value;
                configService.SmoothGaussValue = value;
                OnPropertyChanged(nameof(SmoothGaussValue));
            }
        }

        private double thresholdSleepTimeValue;

        public double ThresholdSleepTimeValue
        {
            get => thresholdSleepTimeValue;
            set
            {
                thresholdSleepTimeValue = value;
                configService.ThresholdSleepTimeValue = value;
                OnPropertyChanged(nameof(ThresholdSleepTimeValue));
            }
        }

        private double extractionSleepTimeValue;

        public double ExtractionSleepTimeValue
        {
            get => extractionSleepTimeValue;
            set
            {
                extractionSleepTimeValue = value;
                configService.ExtractionSleepTimeValue = value;
                OnPropertyChanged(nameof(ExtractionSleepTimeValue));
            }
        }

        private int minContourArcValue;

        public int MinContourArcValue
        {
            get => minContourArcValue;
            set
            {
                minContourArcValue = value;
                configService.MinContourArcValue = value;
                OnPropertyChanged(nameof(MinContourArcValue));
            }
        }

        private int movesDartValue;

        public int MovesDartValue
        {
            get => movesDartValue;
            set
            {
                movesDartValue = value;
                configService.MovesDartValue = value;
                OnPropertyChanged(nameof(MovesDartValue));
            }
        }

        private double toCam1Distance;

        public double ToCam1Distance
        {
            get => toCam1Distance;
            set
            {
                toCam1Distance = value;
                configService.ToCam1Distance = value;
                OnPropertyChanged(nameof(ToCam1Distance));
            }
        }

        private double toCam2Distance;

        public double ToCam2Distance
        {
            get => toCam2Distance;
            set
            {
                toCam2Distance = value;
                configService.ToCam2Distance = value;
                OnPropertyChanged(nameof(ToCam2Distance));
            }
        }

        private double toCam3Distance;

        public double ToCam3Distance
        {
            get => toCam3Distance;
            set
            {
                toCam3Distance = value;
                configService.ToCam3Distance = value;
                OnPropertyChanged(nameof(ToCam3Distance));
            }
        }

        private double toCam4Distance;

        public double ToCam4Distance
        {
            get => toCam4Distance;
            set
            {
                toCam4Distance = value;
                configService.ToCam4Distance = value;
                OnPropertyChanged(nameof(ToCam4Distance));
            }
        }

        private int cam1SetupXValue;

        public int Cam1SetupXValue
        {
            get => cam1SetupXValue;
            set
            {
                cam1SetupXValue = value;
                configService.Cam1XValue = value;
                OnPropertyChanged(nameof(Cam1SetupXValue));
            }
        }

        private int cam1SetupYValue;

        public int Cam1SetupYValue
        {
            get => cam1SetupYValue;
            set
            {
                cam1SetupYValue = value;
                configService.Cam1YValue = value;
                OnPropertyChanged(nameof(Cam1SetupYValue));
            }
        }

        private int cam2SetupXValue;

        public int Cam2SetupXValue
        {
            get => cam2SetupXValue;
            set
            {
                cam2SetupXValue = value;
                configService.Cam2XValue = value;
                OnPropertyChanged(nameof(Cam2SetupXValue));
            }
        }

        private int cam2SetupYValue;

        public int Cam2SetupYValue
        {
            get => cam2SetupYValue;
            set
            {
                cam2SetupYValue = value;
                configService.Cam2YValue = value;
                OnPropertyChanged(nameof(Cam2SetupYValue));
            }
        }

        private int cam3SetupXValue;

        public int Cam3SetupXValue
        {
            get => cam3SetupXValue;
            set
            {
                cam3SetupXValue = value;
                configService.Cam3XValue = value;
                OnPropertyChanged(nameof(Cam3SetupXValue));
            }
        }

        private int cam3SetupYValue;

        public int Cam3SetupYValue
        {
            get => cam3SetupYValue;
            set
            {
                cam3SetupYValue = value;
                configService.Cam3YValue = value;
                OnPropertyChanged(nameof(Cam3SetupYValue));
            }
        }

        private int cam4SetupXValue;

        public int Cam4SetupXValue
        {
            get => cam4SetupXValue;
            set
            {
                cam4SetupXValue = value;
                configService.Cam4XValue = value;
                OnPropertyChanged(nameof(Cam4SetupXValue));
            }
        }

        private int cam4SetupYValue;

        public int Cam4SetupYValue
        {
            get => cam4SetupYValue;
            set
            {
                cam4SetupYValue = value;
                configService.Cam4YValue = value;
                OnPropertyChanged(nameof(Cam4SetupYValue));
            }
        }

        private string cam1SetupSector;

        public string Cam1SetupSector
        {
            get => cam1SetupSector;
            set
            {
                cam1SetupSector = value;
                configService.Cam1SetupSector = value;
                OnPropertyChanged(nameof(Cam1SetupSector));
            }
        }

        private string cam2SetupSector;

        public string Cam2SetupSector
        {
            get => cam2SetupSector;
            set
            {
                cam2SetupSector = value;
                configService.Cam2SetupSector = value;
                OnPropertyChanged(nameof(Cam2SetupSector));
            }
        }

        private string cam3SetupSector;

        public string Cam3SetupSector
        {
            get => cam3SetupSector;
            set
            {
                cam3SetupSector = value;
                configService.Cam3SetupSector = value;
                OnPropertyChanged(nameof(Cam3SetupSector));
            }
        }

        private string cam4SetupSector;

        public string Cam4SetupSector
        {
            get => cam4SetupSector;
            set
            {
                cam4SetupSector = value;
                configService.Cam4SetupSector = value;
                OnPropertyChanged(nameof(Cam4SetupSector));
            }
        }

        private string checkCamsBoxText;

        public string CheckCamsBoxText
        {
            get => checkCamsBoxText;
            set
            {
                checkCamsBoxText = value;
                OnPropertyChanged(nameof(CheckCamsBoxText));
            }
        }

        #endregion

        #region Game tab

        private List<Player> players;

        public List<Player> Players
        {
            get => players;
            set
            {
                players = value;
                OnPropertyChanged(nameof(Players));
            }
        }

        public Player NewGamePlayer1 { get; set; }
        public Player NewGamePlayer2 { get; set; }
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

        #endregion

        #region Player tab

        private string newPlayerNameText;

        public string NewPlayerNameText
        {
            get => newPlayerNameText;
            set
            {
                newPlayerNameText = value;
                OnPropertyChanged(nameof(NewPlayerNameText));
            }
        }

        private string newPlayerNickNameText;

        public string NewPlayerNickNameText
        {
            get => newPlayerNickNameText;
            set
            {
                newPlayerNickNameText = value;
                OnPropertyChanged(nameof(NewPlayerNickNameText));
            }
        }

        private BitmapImage newPlayerAvatar;

        public BitmapImage NewPlayerAvatar
        {
            get => newPlayerAvatar;
            set
            {
                newPlayerAvatar = value;
                OnPropertyChanged(nameof(NewPlayerAvatar));
            }
        }

        #endregion

        #region Processes

        private bool isMainTabsEnabled;

        public bool IsMainTabsEnabled
        {
            get => isMainTabsEnabled;
            set
            {
                isMainTabsEnabled = value;
                OnPropertyChanged(nameof(IsMainTabsEnabled));
            }
        }

        private bool isSetupTabsEnabled;

        public bool IsSetupTabsEnabled
        {
            get => isSetupTabsEnabled;
            set
            {
                isSetupTabsEnabled = value;
                OnPropertyChanged(nameof(IsSetupTabsEnabled));
            }
        }

        private bool isCamsSetupRunning;

        public bool IsCamsSetupRunning
        {
            get => isCamsSetupRunning;
            set
            {
                isCamsSetupRunning = value;
                OnPropertyChanged(nameof(IsCamsSetupRunning));
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

        private bool isRuntimeCrossingRunning;

        public bool IsRuntimeCrossingRunning
        {
            get => isRuntimeCrossingRunning;
            set
            {
                isRuntimeCrossingRunning = value;
                OnPropertyChanged(nameof(IsRuntimeCrossingRunning));
            }
        }

        #endregion

        #endregion

        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(Logger logger,
                                   MessageBoxService messageBoxService,
                                   IDBService dbService,
                                   IVersionChecker versionChecker,
                                   ScoreBoardService scoreBoardService,
                                   CamsDetectionBoard camsDetectionBoard,
                                   DrawService drawService,
                                   IDetectionService detectionService,
                                   ManualThrowPanel manualThrowPanel,
                                   GameService gameService,
                                   IConfigService configService,
                                   ThrowService throwService)
        {
            this.logger = logger;
            this.messageBoxService = messageBoxService;
            this.dbService = dbService;
            this.versionChecker = versionChecker;
            this.scoreBoardService = scoreBoardService;
            this.camsDetectionBoard = camsDetectionBoard;
            this.drawService = drawService;
            this.detectionService = detectionService;
            this.manualThrowPanel = manualThrowPanel;
            this.gameService = gameService;
            this.configService = configService;
            this.throwService = throwService;
        }

        public const int DefaultNewGameSetsValue = 5;
        public const int DefaultNewGameLegsValue = 3;
        public const GameType DefaultNewGameType = GameType.Classic;
        public const GamePoints DefaultNewGamePoints = GamePoints._501;

        public void OnMainWindowLoaded()
        {
            versionChecker.CheckVersions();
            LoadSettings();
            LoadPlayers();
            FindConnectedCams();
            detectionService.OnErrorOccurred += OnDetectionServiceErrorOccurred;
        }

        public void OnMainWindowClosing()
        {
            StopGameByButton();
            scoreBoardService.CloseScoreBoard();
            manualThrowPanel.Close();
            camsDetectionBoard.Close();

            configService.MainWindowWidth = MainWindowWidth;
            configService.MainWindowHeight = MainWindowHeight;
            configService.MainWindowPositionLeft = MainWindowPositionLeft;
            configService.MainWindowPositionTop = MainWindowPositionTop;

            configService.SaveSettings();
        }

        #region Settings

        private void LoadSettings()
        {
            NewPlayerAvatar = Converter.BitmapToBitmapImage(Resources.Resources.EmptyUserIcon);
            IsMainTabsEnabled = true;
            IsSetupTabsEnabled = true;

            NewGameSets = DefaultNewGameSetsValue;
            NewGameLegs = DefaultNewGameLegsValue;
            NewGameType = DefaultNewGameType;
            NewGamePoints = DefaultNewGamePoints;

            MainWindowHeight = configService.MainWindowHeight;
            MainWindowWidth = configService.MainWindowWidth;
            MainWindowPositionLeft = configService.MainWindowPositionLeft;
            MainWindowPositionTop = configService.MainWindowPositionTop;

            Cam1ThresholdSliderValue = configService.Cam1ThresholdSliderValue;
            Cam2ThresholdSliderValue = configService.Cam2ThresholdSliderValue;
            Cam3ThresholdSliderValue = configService.Cam3ThresholdSliderValue;
            Cam4ThresholdSliderValue = configService.Cam4ThresholdSliderValue;
            Cam1SurfaceSliderValue = configService.Cam1SurfaceSliderValue;
            Cam2SurfaceSliderValue = configService.Cam2SurfaceSliderValue;
            Cam3SurfaceSliderValue = configService.Cam3SurfaceSliderValue;
            Cam4SurfaceSliderValue = configService.Cam4SurfaceSliderValue;
            Cam1SurfaceCenterSliderValue = configService.Cam1SurfaceCenterSliderValue;
            Cam2SurfaceCenterSliderValue = configService.Cam2SurfaceCenterSliderValue;
            Cam3SurfaceCenterSliderValue = configService.Cam3SurfaceCenterSliderValue;
            Cam4SurfaceCenterSliderValue = configService.Cam4SurfaceCenterSliderValue;
            Cam1RoiPosYSliderValue = configService.Cam1RoiPosYSliderValue;
            Cam2RoiPosYSliderValue = configService.Cam2RoiPosYSliderValue;
            Cam3RoiPosYSliderValue = configService.Cam3RoiPosYSliderValue;
            Cam4RoiPosYSliderValue = configService.Cam4RoiPosYSliderValue;
            Cam1RoiHeightSliderValue = configService.Cam1RoiHeightSliderValue;
            Cam2RoiHeightSliderValue = configService.Cam2RoiHeightSliderValue;
            Cam3RoiHeightSliderValue = configService.Cam3RoiHeightSliderValue;
            Cam4RoiHeightSliderValue = configService.Cam4RoiHeightSliderValue;

            Cam1Enabled = configService.Cam1Enabled;
            Cam2Enabled = configService.Cam2Enabled;
            Cam3Enabled = configService.Cam3Enabled;
            Cam4Enabled = configService.Cam4Enabled;
            DetectionEnabled = configService.DetectionEnabled;
            Cam1Id = configService.Cam1Id;
            Cam2Id = configService.Cam2Id;
            Cam3Id = configService.Cam3Id;
            Cam4Id = configService.Cam4Id;
            CamsFovAngle = configService.CamsFovAngle;
            CamsResolutionWidth = configService.CamResolutionWidth;
            CamsResolutionHeight = configService.CamResolutionHeight;
            MovesExtractionValue = configService.MovesExtractionValue;
            MovesDetectedSleepTimeValue = configService.MoveDetectedSleepTimeValue;
            MovesNoiseValue = configService.MovesNoiseValue;
            SmoothGaussValue = configService.SmoothGaussValue;
            ThresholdSleepTimeValue = configService.ThresholdSleepTimeValue;
            ExtractionSleepTimeValue = configService.ExtractionSleepTimeValue;
            MinContourArcValue = configService.MinContourArcValue;
            MovesDartValue = configService.MovesDartValue;
            ToCam1Distance = configService.ToCam1Distance;
            ToCam2Distance = configService.ToCam2Distance;
            ToCam3Distance = configService.ToCam3Distance;
            ToCam4Distance = configService.ToCam4Distance;
            Cam1SetupXValue = configService.Cam1XValue;
            Cam1SetupYValue = configService.Cam1YValue;
            Cam2SetupXValue = configService.Cam2XValue;
            Cam2SetupYValue = configService.Cam2YValue;
            Cam3SetupXValue = configService.Cam3XValue;
            Cam3SetupYValue = configService.Cam3YValue;
            Cam4SetupXValue = configService.Cam4XValue;
            Cam4SetupYValue = configService.Cam4YValue;

            Cam1SetupSector = configService.Cam1SetupSector;
            Cam2SetupSector = configService.Cam2SetupSector;
            Cam3SetupSector = configService.Cam3SetupSector;
            Cam4SetupSector = configService.Cam4SetupSector;
        }

        public void CalibrateCamsSetupPoints()
        {
            var calibratedCam1SetupPoint = MeasureService.CalculateCamSetupPoint(ToCam1Distance,
                                                                                 Cam1SetupSector);
            var calibratedCam2SetupPoint = MeasureService.CalculateCamSetupPoint(ToCam2Distance,
                                                                                 Cam2SetupSector);
            var calibratedCam3SetupPoint = MeasureService.CalculateCamSetupPoint(ToCam3Distance,
                                                                                 Cam3SetupSector);
            var calibratedCam4SetupPoint = MeasureService.CalculateCamSetupPoint(ToCam4Distance,
                                                                                 Cam4SetupSector);
            Cam1SetupXValue = (int) calibratedCam1SetupPoint.X;
            Cam1SetupYValue = (int) calibratedCam1SetupPoint.Y;
            Cam2SetupXValue = (int) calibratedCam2SetupPoint.X;
            Cam2SetupYValue = (int) calibratedCam2SetupPoint.Y;
            Cam3SetupXValue = (int) calibratedCam3SetupPoint.X;
            Cam3SetupYValue = (int) calibratedCam3SetupPoint.Y;
            Cam4SetupXValue = (int) calibratedCam4SetupPoint.X;
            Cam4SetupYValue = (int) calibratedCam4SetupPoint.Y;
        }

        #endregion

        #region Start\Stop game

        public void StartGame()
        {
            if (!ValidateGameStart())
            {
                return;
            }

            IsMainTabsEnabled = false;
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
                messageBoxService.ShowError(Resources.Resources.NotImplementedYetErrorText);
                return false;
            }

            if (!Validator.ValidateStartNewGamePlayersSelected(NewGameType,
                                                               NewGamePlayer1,
                                                               NewGamePlayer2))
            {
                messageBoxService.ShowError(Resources.Resources.NewGamePlayersNotSelectedErrorText);
                return false;
            }

            if (!Validator.ValidateStartNewClassicGame(NewGameType,
                                                       NewGamePoints))
            {
                messageBoxService.ShowError(Resources.Resources.NewClassicGamePointsNotSelectedErrorText);
                return false;
            }

            return true;
        }

        public void StopGameByButton()
        {
            gameService.StopGame(GameResultType.Aborted);
            StopGameInternal();
        }

        private void StopGameByError()
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
            IsMainTabsEnabled = true;
            IsGameRunning = false;
        }

        #endregion

        #region New player

        public void SaveNewPlayer()
        {
            if (!Validator.ValidateNewPlayerNameAndNickName(NewPlayerNameText, NewPlayerNickNameText))
            {
                messageBoxService.ShowError(Resources.Resources.NewPlayerEmptyDataErrorText);
                return;
            }

            var newPlayer = new Player(NewPlayerNameText,
                                       NewPlayerNickNameText,
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

            NewPlayerNameText = string.Empty;
            NewPlayerNickNameText = string.Empty;
            NewPlayerAvatar = Converter.BitmapToBitmapImage(Resources.Resources.EmptyUserIcon);

            LoadPlayers();
        }

        private void LoadPlayers()
        {
            var playersTable = dbService.PlayersLoadAll();
            Players = Converter.PlayersFromTable(playersTable);
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
                    NewPlayerAvatar = image;
                }
                else
                {
                    messageBoxService.ShowError(Resources.Resources.PlayerAvatarTooBigErrorText);
                }
            }
        }

        #endregion

        #region CamSetupCapturing

        public async void StartCamSetupCapturing(CamNumber camNumber)
        {
            IsMainTabsEnabled = false;
            IsSetupTabsEnabled = false;
            IsCamsSetupRunning = true;

            cts = new CancellationTokenSource();
            var cancelToken = cts.Token;

            try
            {
                await Task.Run(() =>
                               {
                                   var cam = new CamService(camNumber,
                                                            logger,
                                                            drawService,
                                                            configService,
                                                            throwService);
                                   while (!cancelToken.IsCancellationRequested)
                                   {
                                       var setupSlidersData = GetCamsSetupSlidersData(camNumber);
                                       cam.DoSetupCaptures(setupSlidersData);
                                       Application.Current.Dispatcher.Invoke(() =>
                                                                             {
                                                                                 CamImage = cam.GetImage();
                                                                                 CamRoiImage = cam.GetRoiImage();
                                                                             });
                                   }

                                   Application.Current.Dispatcher.Invoke(() =>
                                                                         {
                                                                             CamImage = new BitmapImage();
                                                                             CamRoiImage = new BitmapImage();
                                                                         });
                                   cam.Dispose();
                               });
            }
            catch (Exception e)
            {
                messageBoxService.ShowError($"{e.Message} \n {e.StackTrace}");
                StopCamSetupCapturing();
            }
        }

        private List<double> GetCamsSetupSlidersData(CamNumber camNumber)
        {
            List<double> slidersData;
            switch (camNumber)
            {
                case CamNumber._1:
                    slidersData = new List<double>
                                  {
                                      Cam1ThresholdSliderValue,
                                      Cam1SurfaceSliderValue,
                                      Cam1SurfaceCenterSliderValue,
                                      Cam1RoiPosYSliderValue,
                                      Cam1RoiHeightSliderValue
                                  };
                    break;
                case CamNumber._2:
                    slidersData = new List<double>
                                  {
                                      Cam2ThresholdSliderValue,
                                      Cam2SurfaceSliderValue,
                                      Cam2SurfaceCenterSliderValue,
                                      Cam2RoiPosYSliderValue,
                                      Cam2RoiHeightSliderValue
                                  };
                    break;
                case CamNumber._3:
                    slidersData = new List<double>
                                  {
                                      Cam3ThresholdSliderValue,
                                      Cam3SurfaceSliderValue,
                                      Cam3SurfaceCenterSliderValue,
                                      Cam3RoiPosYSliderValue,
                                      Cam3RoiHeightSliderValue
                                  };
                    break;
                case CamNumber._4:
                    slidersData = new List<double>
                                  {
                                      Cam4ThresholdSliderValue,
                                      Cam4SurfaceSliderValue,
                                      Cam4SurfaceCenterSliderValue,
                                      Cam4RoiPosYSliderValue,
                                      Cam4RoiHeightSliderValue
                                  };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(camNumber), camNumber, null);
            }

            slidersData.Add(CamsResolutionWidth);
            return slidersData;
        }

        public void StopCamSetupCapturing()
        {
            cts?.Cancel();
            IsMainTabsEnabled = true;
            IsSetupTabsEnabled = true;
            IsCamsSetupRunning = false;
        }

        #endregion

        #region RuntimeCrossing

        public void StartCrossing()
        {
            IsRuntimeCrossingRunning = true;
            try
            {
                camsDetectionBoard.Open();

                var cams = CreateCamsServices();
                detectionService.CheckCamsAndTryCapture(cams);

                detectionService.RunDetection(cams, DetectionServiceWorkingMode.Crossing);
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
            // drawService.ProjectionClear();
            camsDetectionBoard.Close();

            IsRuntimeCrossingRunning = false;
        }

        private List<CamService> CreateCamsServices()
        {
            var cams = new List<CamService>();
            var cam1Active = Cam1Enabled && !App.NoCams;
            var cam2Active = Cam2Enabled && !App.NoCams;
            var cam3Active = Cam3Enabled && !App.NoCams;
            var cam4Active = Cam4Enabled && !App.NoCams;

            if (cam1Active)
            {
                cams.Add(new CamService(CamNumber._1,
                                        logger,
                                        drawService,
                                        configService,
                                        throwService));
            }

            if (cam2Active)
            {
                cams.Add(new CamService(CamNumber._2,
                                        logger,
                                        drawService,
                                        configService,
                                        throwService));
            }

            if (cam3Active)
            {
                cams.Add(new CamService(CamNumber._3,
                                        logger,
                                        drawService,
                                        configService,
                                        throwService));
            }

            if (cam4Active)
            {
                cams.Add(new CamService(CamNumber._4,
                                        logger,
                                        drawService,
                                        configService,
                                        throwService));
            }

            return cams;
        }

        #endregion

        #region CamsChecker

        public void FindConnectedCams()
        {
            CheckCamsBoxText = detectionService.FindConnectedCams();
        }

        public void CheckCamsSimultaneousWork()
        {
            try
            {
                var cams = CreateCamsServices();
                detectionService.CheckCamsAndTryCapture(cams);
                CheckCamsBoxText = "Checked cams simultaneous work: OK";
            }
            catch (Exception e)
            {
                CheckCamsBoxText = "Checked cams simultaneous work: ERROR";
            }
        }

        #endregion

        #region Error

        private void OnDetectionServiceErrorOccurred(Exception e)
        {
            messageBoxService.ShowError($"{e.Message} \n {e.StackTrace}");
            StopGameByError();
        }

        #endregion

        #region PropertyChangingFire

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}