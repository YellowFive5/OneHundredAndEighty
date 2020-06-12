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
using System.Windows.Media.Imaging;
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
        private readonly Logger logger;
        private readonly MessageBoxService messageBoxService;
        private readonly DBService dbService;
        private readonly ScoreBoardService scoreBoardService;
        private readonly CamsDetectionBoard camsDetectionBoard;
        private readonly DrawService drawService;
        private readonly DetectionService detectionService;
        private readonly ManualThrowPanel manualThrowPanel;
        private readonly GameService gameService;
        private readonly ConfigService configService;
        private readonly ThrowService throwService;
        private CancellationTokenSource cts;
        private CancellationToken cancelToken;

        #region Bindable props

        #region Window position

        public double MainWindowPositionLeft { get; set; }

        public double MainWindowPositionTop { get; set; }

        public double MainWindowHeight { get; set; }

        public double MainWindowWidth { get; set; }

        #endregion

        #region Images

        private BitmapImage cam1Image;

        public BitmapImage Cam1Image
        {
            get => cam1Image;
            set
            {
                cam1Image = value;
                OnPropertyChanged(nameof(Cam1Image));
            }
        }

        private BitmapImage cam2Image;

        public BitmapImage Cam2Image
        {
            get => cam2Image;
            private set
            {
                cam2Image = value;
                OnPropertyChanged(nameof(Cam2Image));
            }
        }

        private BitmapImage cam3Image;

        public BitmapImage Cam3Image
        {
            get => cam3Image;
            private set
            {
                cam3Image = value;
                OnPropertyChanged(nameof(Cam3Image));
            }
        }

        private BitmapImage cam4Image;

        public BitmapImage Cam4Image
        {
            get => cam4Image;
            private set
            {
                cam4Image = value;
                OnPropertyChanged(nameof(Cam4Image));
            }
        }

        private BitmapImage cam1RoiImage;

        public BitmapImage Cam1RoiImage
        {
            get => cam1RoiImage;
            private set
            {
                cam1RoiImage = value;
                OnPropertyChanged(nameof(Cam1RoiImage));
            }
        }

        private BitmapImage cam2RoiImage;

        public BitmapImage Cam2RoiImage
        {
            get => cam2RoiImage;
            private set
            {
                cam2RoiImage = value;
                OnPropertyChanged(nameof(Cam2RoiImage));
            }
        }

        private BitmapImage cam3RoiImage;

        public BitmapImage Cam3RoiImage
        {
            get => cam3RoiImage;
            private set
            {
                cam3RoiImage = value;
                OnPropertyChanged(nameof(Cam3RoiImage));
            }
        }

        private BitmapImage cam4RoiImage;

        public BitmapImage Cam4RoiImage
        {
            get => cam4RoiImage;
            private set
            {
                cam4RoiImage = value;
                OnPropertyChanged(nameof(Cam4RoiImage));
            }
        }

        #endregion

        #region CamsSetupSliders

        public double Cam1ThresholdSliderValue { get; set; }

        public double Cam2ThresholdSliderValue { get; set; }

        public double Cam3ThresholdSliderValue { get; set; }

        public double Cam4ThresholdSliderValue { get; set; }

        public double Cam1SurfaceSliderValue { get; set; }

        public double Cam2SurfaceSliderValue { get; set; }

        public double Cam3SurfaceSliderValue { get; set; }

        public double Cam4SurfaceSliderValue { get; set; }

        public double Cam1SurfaceCenterSliderValue { get; set; }

        public double Cam2SurfaceCenterSliderValue { get; set; }

        public double Cam3SurfaceCenterSliderValue { get; set; }

        public double Cam4SurfaceCenterSliderValue { get; set; }

        public double Cam1RoiPosYSliderValue { get; set; }

        public double Cam2RoiPosYSliderValue { get; set; }

        public double Cam3RoiPosYSliderValue { get; set; }

        public double Cam4RoiPosYSliderValue { get; set; }

        public double Cam1RoiHeightSliderValue { get; set; }

        public double Cam2RoiHeightSliderValue { get; set; }

        public double Cam3RoiHeightSliderValue { get; set; }

        public double Cam4RoiHeightSliderValue { get; set; }

        #endregion

        #region Main setup tab

        private bool cam1Enabled;

        public bool Cam1Enabled
        {
            get => cam1Enabled;
            set
            {
                if (cam1Enabled != value)
                {
                    cam1Enabled = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.Cam1CheckBox, Cam1Enabled);
                    }
                }
            }
        }

        private bool cam2Enabled;

        public bool Cam2Enabled
        {
            get => cam2Enabled;
            set
            {
                if (cam2Enabled != value)
                {
                    cam2Enabled = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.Cam2CheckBox, Cam2Enabled);
                    }
                }
            }
        }

        private bool cam3Enabled;

        public bool Cam3Enabled
        {
            get => cam3Enabled;
            set
            {
                if (cam3Enabled != value)
                {
                    cam3Enabled = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.Cam3CheckBox, Cam3Enabled);
                    }
                }
            }
        }

        private bool cam4Enabled;

        public bool Cam4Enabled
        {
            get => cam4Enabled;
            set
            {
                if (cam4Enabled != value)
                {
                    cam4Enabled = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.Cam4CheckBox, Cam4Enabled);
                    }
                }
            }
        }

        private bool detectionEnabled;

        public bool DetectionEnabled
        {
            get => detectionEnabled;
            set
            {
                if (detectionEnabled != value)
                {
                    detectionEnabled = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.WithDetectionCheckBox, DetectionEnabled);
                    }
                }
            }
        }

        private string cam1Id;

        public string Cam1Id
        {
            get => cam1Id;
            set
            {
                if (cam1Id != value)
                {
                    cam1Id = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.Cam1Id, Cam1Id);
                    }
                }
            }
        }

        private string cam2Id;

        public string Cam2Id
        {
            get => cam2Id;
            set
            {
                if (cam2Id != value)
                {
                    cam2Id = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.Cam2Id, Cam2Id);
                    }
                }
            }
        }

        private string cam3Id;

        public string Cam3Id
        {
            get => cam3Id;
            set
            {
                if (cam3Id != value)
                {
                    cam3Id = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.Cam3Id, Cam3Id);
                    }
                }
            }
        }

        private string cam4Id;

        public string Cam4Id
        {
            get => cam4Id;
            set
            {
                if (cam4Id != value)
                {
                    cam4Id = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.Cam4Id, Cam4Id);
                    }
                }
            }
        }

        private double camsFovAngle;

        public double CamsFovAngle
        {
            get => camsFovAngle;
            set
            {
                if (camsFovAngle != value)
                {
                    camsFovAngle = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.CamFovAngle, CamsFovAngle);
                    }
                }
            }
        }

        private int camsResolutionHeight;

        public int CamsResolutionHeight
        {
            get => camsResolutionHeight;
            set
            {
                if (camsResolutionHeight != value)
                {
                    camsResolutionHeight = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.ResolutionHeight, CamsResolutionHeight);
                    }
                }
            }
        }

        private int camsResolutionWidth;

        public int CamsResolutionWidth
        {
            get => camsResolutionWidth;
            set
            {
                if (camsResolutionWidth != value)
                {
                    camsResolutionWidth = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.ResolutionWidth, CamsResolutionWidth);
                    }
                }
            }
        }

        private int movesExtractionValue;

        public int MovesExtractionValue
        {
            get => movesExtractionValue;
            set
            {
                if (movesExtractionValue != value)
                {
                    movesExtractionValue = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.MovesExtraction, MovesExtractionValue);
                    }
                }
            }
        }

        private double movesDetectedSleepTimeValue;

        public double MovesDetectedSleepTimeValue
        {
            get => movesDetectedSleepTimeValue;
            set
            {
                if (movesDetectedSleepTimeValue != value)
                {
                    movesDetectedSleepTimeValue = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.MoveDetectedSleepTime, MovesDetectedSleepTimeValue);
                    }
                }
            }
        }

        private int movesNoiseValue;

        public int MovesNoiseValue
        {
            get => movesNoiseValue;
            set
            {
                if (movesNoiseValue != value)
                {
                    movesNoiseValue = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.MovesNoise, MovesNoiseValue);
                    }
                }
            }
        }

        private int smoothGaussValue;

        public int SmoothGaussValue
        {
            get => smoothGaussValue;
            set
            {
                if (smoothGaussValue != value)
                {
                    smoothGaussValue = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.SmoothGauss, SmoothGaussValue);
                    }
                }
            }
        }

        private double thresholdSleepTimeValue;

        public double ThresholdSleepTimeValue
        {
            get => thresholdSleepTimeValue;
            set
            {
                if (thresholdSleepTimeValue != value)
                {
                    thresholdSleepTimeValue = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.ThresholdSleepTime, ThresholdSleepTimeValue);
                    }
                }
            }
        }

        private double extractionSleepTimeValue;

        public double ExtractionSleepTimeValue
        {
            get => extractionSleepTimeValue;
            set
            {
                if (extractionSleepTimeValue != value)
                {
                    extractionSleepTimeValue = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.ExtractionSleepTime, ExtractionSleepTimeValue);
                    }
                }
            }
        }

        private int minContourArcValue;

        public int MinContourArcValue
        {
            get => minContourArcValue;
            set
            {
                if (minContourArcValue != value)
                {
                    minContourArcValue = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.MinContourArc, MinContourArcValue);
                    }
                }
            }
        }

        private int movesDartValue;

        public int MovesDartValue
        {
            get => movesDartValue;
            set
            {
                if (movesDartValue != value)
                {
                    movesDartValue = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.MovesDart, MovesDartValue);
                    }
                }
            }
        }

        private double toCam1Distance;

        public double ToCam1Distance
        {
            get => toCam1Distance;
            set
            {
                if (toCam1Distance != value)
                {
                    toCam1Distance = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.ToCam1Distance, ToCam1Distance);
                    }
                }
            }
        }

        private double toCam2Distance;

        public double ToCam2Distance
        {
            get => toCam2Distance;
            set
            {
                if (toCam2Distance != value)
                {
                    toCam2Distance = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.ToCam2Distance, ToCam2Distance);
                    }
                }
            }
        }

        private double toCam3Distance;

        public double ToCam3Distance
        {
            get => toCam3Distance;
            set
            {
                if (toCam3Distance != value)
                {
                    toCam3Distance = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.ToCam3Distance, ToCam3Distance);
                    }
                }
            }
        }

        private double toCam4Distance;

        public double ToCam4Distance
        {
            get => toCam4Distance;
            set
            {
                if (toCam4Distance != value)
                {
                    toCam4Distance = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.ToCam4Distance, ToCam4Distance);
                    }
                }
            }
        }

        private int cam1SetupXValue;

        public int Cam1SetupXValue
        {
            get => cam1SetupXValue;
            set
            {
                cam1SetupXValue = value;
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
                OnPropertyChanged(nameof(Cam4SetupYValue));
            }
        }

        private string cam1SetupSector;

        public string Cam1SetupSector
        {
            get => cam1SetupSector;
            set
            {
                if (cam1SetupSector != value)
                {
                    cam1SetupSector = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.Cam1SetupSector, cam1SetupSector);
                    }
                }
            }
        }

        private string cam2SetupSector;

        public string Cam2SetupSector
        {
            get => cam2SetupSector;
            set
            {
                if (cam2SetupSector != value)
                {
                    cam2SetupSector = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.Cam2SetupSector, cam2SetupSector);
                    }
                }
            }
        }

        private string cam3SetupSector;

        public string Cam3SetupSector
        {
            get => cam3SetupSector;
            set
            {
                if (cam3SetupSector != value)
                {
                    cam3SetupSector = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.Cam3SetupSector, cam3SetupSector);
                    }
                }
            }
        }

        private string cam4SetupSector;

        public string Cam4SetupSector
        {
            get => cam4SetupSector;
            set
            {
                if (cam4SetupSector != value)
                {
                    cam4SetupSector = value;
                    if (IsSettingsLoaded)
                    {
                        configService.Write(SettingsType.Cam4SetupSector, cam4SetupSector);
                    }
                }
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
                newGameType = value;

                IsNewGameForSingle = NewGameType == GameType.FreeThrowsSingle;
                OnPropertyChanged(nameof(IsNewGameForSingle));
                IsNewGameForPair = NewGameType != GameType.FreeThrowsSingle;
                OnPropertyChanged(nameof(IsNewGameForPair));
                IsNewGameIsClassic = NewGameType == GameType.Classic;
                OnPropertyChanged(nameof(IsNewGameIsClassic));
            }
        }

        public bool IsNewGameForSingle { get; set; }

        public bool IsNewGameForPair { get; set; }

        public bool IsNewGameIsClassic { get; set; }

        public GamePoints NewGamePoints { get; set; }
        public int NewGameSets { get; set; }
        public int NewGameLegs { get; set; }

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
                                   DBService dbService,
                                   VersionChecker versionChecker,
                                   ScoreBoardService scoreBoardService,
                                   CamsDetectionBoard camsDetectionBoard,
                                   DrawService drawService,
                                   DetectionService detectionService,
                                   ManualThrowPanel manualThrowPanel,
                                   GameService gameService,
                                   ConfigService configService,
                                   ThrowService throwService)
        {
            this.logger = logger;
            this.messageBoxService = messageBoxService;
            this.dbService = dbService;
            this.scoreBoardService = scoreBoardService;
            this.camsDetectionBoard = camsDetectionBoard;
            this.drawService = drawService;
            this.detectionService = detectionService;
            this.manualThrowPanel = manualThrowPanel;
            this.gameService = gameService;
            this.configService = configService;
            this.throwService = throwService;

            detectionService.OnErrorOccurred += OnDetectionServiceErrorOccurred;
            versionChecker.CheckVersions();

            LoadSettings();
            LoadPlayers();
            FindConnectedCams();
        }

        #region Settings

        private bool IsSettingsLoaded { get; set; }

        private void LoadSettings()
        {
            NewPlayerAvatar = Converter.BitmapToBitmapImage(Resources.Resources.EmptyUserIcon);
            IsMainTabsEnabled = true;
            IsSetupTabsEnabled = true;
            NewGameSets = 3;
            NewGameLegs = 3;
            NewGameType = GameType.Classic;
            NewGamePoints = GamePoints._501;

            MainWindowHeight = configService.Read<double>(SettingsType.MainWindowHeight);
            MainWindowWidth = configService.Read<double>(SettingsType.MainWindowWidth);
            MainWindowPositionLeft = configService.Read<double>(SettingsType.MainWindowPositionLeft);
            MainWindowPositionTop = configService.Read<double>(SettingsType.MainWindowPositionTop);

            Cam1ThresholdSliderValue = configService.Read<double>(SettingsType.Cam1ThresholdSlider);
            Cam2ThresholdSliderValue = configService.Read<double>(SettingsType.Cam2ThresholdSlider);
            Cam3ThresholdSliderValue = configService.Read<double>(SettingsType.Cam3ThresholdSlider);
            Cam4ThresholdSliderValue = configService.Read<double>(SettingsType.Cam4ThresholdSlider);
            Cam1SurfaceSliderValue = configService.Read<double>(SettingsType.Cam1SurfaceSlider);
            Cam2SurfaceSliderValue = configService.Read<double>(SettingsType.Cam2SurfaceSlider);
            Cam3SurfaceSliderValue = configService.Read<double>(SettingsType.Cam3SurfaceSlider);
            Cam4SurfaceSliderValue = configService.Read<double>(SettingsType.Cam4SurfaceSlider);
            Cam1SurfaceCenterSliderValue = configService.Read<double>(SettingsType.Cam1SurfaceCenterSlider);
            Cam2SurfaceCenterSliderValue = configService.Read<double>(SettingsType.Cam2SurfaceCenterSlider);
            Cam3SurfaceCenterSliderValue = configService.Read<double>(SettingsType.Cam3SurfaceCenterSlider);
            Cam4SurfaceCenterSliderValue = configService.Read<double>(SettingsType.Cam4SurfaceCenterSlider);
            Cam1RoiPosYSliderValue = configService.Read<double>(SettingsType.Cam1RoiPosYSlider);
            Cam2RoiPosYSliderValue = configService.Read<double>(SettingsType.Cam2RoiPosYSlider);
            Cam3RoiPosYSliderValue = configService.Read<double>(SettingsType.Cam3RoiPosYSlider);
            Cam4RoiPosYSliderValue = configService.Read<double>(SettingsType.Cam4RoiPosYSlider);
            Cam1RoiHeightSliderValue = configService.Read<double>(SettingsType.Cam1RoiHeightSlider);
            Cam2RoiHeightSliderValue = configService.Read<double>(SettingsType.Cam2RoiHeightSlider);
            Cam3RoiHeightSliderValue = configService.Read<double>(SettingsType.Cam3RoiHeightSlider);
            Cam4RoiHeightSliderValue = configService.Read<double>(SettingsType.Cam4RoiHeightSlider);

            Cam1Enabled = configService.Read<bool>(SettingsType.Cam1CheckBox);
            Cam2Enabled = configService.Read<bool>(SettingsType.Cam2CheckBox);
            Cam3Enabled = configService.Read<bool>(SettingsType.Cam3CheckBox);
            Cam4Enabled = configService.Read<bool>(SettingsType.Cam4CheckBox);
            DetectionEnabled = configService.Read<bool>(SettingsType.WithDetectionCheckBox);
            Cam1Id = configService.Read<string>(SettingsType.Cam1Id);
            Cam2Id = configService.Read<string>(SettingsType.Cam2Id);
            Cam3Id = configService.Read<string>(SettingsType.Cam3Id);
            Cam4Id = configService.Read<string>(SettingsType.Cam4Id);
            CamsFovAngle = configService.Read<double>(SettingsType.CamFovAngle);
            CamsResolutionWidth = configService.Read<int>(SettingsType.ResolutionWidth);
            CamsResolutionHeight = configService.Read<int>(SettingsType.ResolutionHeight);
            MovesExtractionValue = configService.Read<int>(SettingsType.MovesExtraction);
            MovesDetectedSleepTimeValue = configService.Read<double>(SettingsType.MoveDetectedSleepTime);
            MovesNoiseValue = configService.Read<int>(SettingsType.MovesNoise);
            SmoothGaussValue = configService.Read<int>(SettingsType.SmoothGauss);
            ThresholdSleepTimeValue = configService.Read<double>(SettingsType.ThresholdSleepTime);
            ExtractionSleepTimeValue = configService.Read<double>(SettingsType.ExtractionSleepTime);
            MinContourArcValue = configService.Read<int>(SettingsType.MinContourArc);
            MovesDartValue = configService.Read<int>(SettingsType.MovesDart);
            ToCam1Distance = configService.Read<double>(SettingsType.ToCam1Distance);
            ToCam2Distance = configService.Read<double>(SettingsType.ToCam2Distance);
            ToCam3Distance = configService.Read<double>(SettingsType.ToCam3Distance);
            ToCam4Distance = configService.Read<double>(SettingsType.ToCam4Distance);
            Cam1SetupXValue = configService.Read<int>(SettingsType.Cam1X);
            Cam1SetupYValue = configService.Read<int>(SettingsType.Cam1Y);
            Cam2SetupXValue = configService.Read<int>(SettingsType.Cam2X);
            Cam2SetupYValue = configService.Read<int>(SettingsType.Cam2Y);
            Cam3SetupXValue = configService.Read<int>(SettingsType.Cam3X);
            Cam3SetupYValue = configService.Read<int>(SettingsType.Cam3Y);
            Cam4SetupXValue = configService.Read<int>(SettingsType.Cam4X);
            Cam4SetupYValue = configService.Read<int>(SettingsType.Cam4Y);

            Cam1SetupSector = configService.Read<string>(SettingsType.Cam1SetupSector);
            Cam2SetupSector = configService.Read<string>(SettingsType.Cam2SetupSector);
            Cam3SetupSector = configService.Read<string>(SettingsType.Cam3SetupSector);
            Cam4SetupSector = configService.Read<string>(SettingsType.Cam4SetupSector);

            IsSettingsLoaded = true;
        }

        private void SaveWindowPositionSettings()
        {
            configService.Write(SettingsType.MainWindowHeight, MainWindowHeight);
            configService.Write(SettingsType.MainWindowWidth, MainWindowWidth);
            configService.Write(SettingsType.MainWindowPositionLeft, MainWindowPositionLeft);
            configService.Write(SettingsType.MainWindowPositionTop, MainWindowPositionTop);
        }

        private void SaveCamsSetupSlidersSettings(CamNumber camNumber)
        {
            switch (camNumber)
            {
                case CamNumber._1:
                    configService.Write(SettingsType.Cam1ThresholdSlider, Cam1ThresholdSliderValue);
                    configService.Write(SettingsType.Cam1SurfaceSlider, Cam1SurfaceSliderValue);
                    configService.Write(SettingsType.Cam1SurfaceCenterSlider, Cam1SurfaceCenterSliderValue);
                    configService.Write(SettingsType.Cam1RoiPosYSlider, Cam1RoiPosYSliderValue);
                    configService.Write(SettingsType.Cam1RoiHeightSlider, Cam1RoiHeightSliderValue);
                    break;
                case CamNumber._2:
                    configService.Write(SettingsType.Cam2ThresholdSlider, Cam2ThresholdSliderValue);
                    configService.Write(SettingsType.Cam2SurfaceSlider, Cam2SurfaceSliderValue);
                    configService.Write(SettingsType.Cam2SurfaceCenterSlider, Cam2SurfaceCenterSliderValue);
                    configService.Write(SettingsType.Cam2RoiPosYSlider, Cam2RoiPosYSliderValue);
                    configService.Write(SettingsType.Cam2RoiHeightSlider, Cam2RoiHeightSliderValue);
                    break;
                case CamNumber._3:
                    configService.Write(SettingsType.Cam3ThresholdSlider, Cam3ThresholdSliderValue);
                    configService.Write(SettingsType.Cam3SurfaceSlider, Cam3SurfaceSliderValue);
                    configService.Write(SettingsType.Cam3SurfaceCenterSlider, Cam3SurfaceCenterSliderValue);
                    configService.Write(SettingsType.Cam3RoiPosYSlider, Cam3RoiPosYSliderValue);
                    configService.Write(SettingsType.Cam3RoiHeightSlider, Cam3RoiHeightSliderValue);
                    break;
                case CamNumber._4:
                    configService.Write(SettingsType.Cam4ThresholdSlider, Cam4ThresholdSliderValue);
                    configService.Write(SettingsType.Cam4SurfaceSlider, Cam4SurfaceSliderValue);
                    configService.Write(SettingsType.Cam4SurfaceCenterSlider, Cam4SurfaceCenterSliderValue);
                    configService.Write(SettingsType.Cam4RoiPosYSlider, Cam4RoiPosYSliderValue);
                    configService.Write(SettingsType.Cam4RoiHeightSlider, Cam4RoiHeightSliderValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(camNumber), camNumber, null);
            }
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

            SaveCalibratedCamsPositions();
        }

        private void SaveCalibratedCamsPositions()
        {
            configService.Write(SettingsType.Cam1X, Cam1SetupXValue);
            configService.Write(SettingsType.Cam1Y, Cam1SetupYValue);
            configService.Write(SettingsType.Cam2X, Cam2SetupXValue);
            configService.Write(SettingsType.Cam2Y, Cam2SetupYValue);
            configService.Write(SettingsType.Cam3X, Cam3SetupXValue);
            configService.Write(SettingsType.Cam3Y, Cam3SetupYValue);
            configService.Write(SettingsType.Cam4X, Cam4SetupXValue);
            configService.Write(SettingsType.Cam4Y, Cam4SetupYValue);
        }

        #endregion

        #region Start\Stop game

        public void StartGame()
        {
            if (!Validator.ValidateImplementedGameTypes(NewGameType))
            {
                messageBoxService.ShowError(Resources.Resources.NotImplementedYetErrorText);
                return;
            }

            if (!Validator.ValidateStartNewGamePlayersSelected(NewGameType,
                                                               NewGamePlayer1,
                                                               NewGamePlayer2))
            {
                messageBoxService.ShowError(Resources.Resources.NewGamePlayersNotSelectedErrorText);
                return;
            }

            if (!Validator.ValidateStartNewClassicGame(NewGameType,
                                                       NewGamePoints))
            {
                messageBoxService.ShowError(Resources.Resources.NewClassicGamePointsNotSelectedErrorText);
                return;
            }

            IsMainTabsEnabled = false;
            IsGameRunning = true;

            try
            {
                camsDetectionBoard.Open();
                drawService.ProjectionPrepare();

                var cams = PrepareCamsServices(CamServiceWorkingMode.Crossing);
                gameService.StartGame(cams, NewGamePlayer1, NewGamePlayer2, NewGameType, NewGamePoints, NewGameSets, NewGameLegs);
            }
            catch (Exception e)
            {
                StopGameByError();
                messageBoxService.ShowError($"{e.Message} \n {e.StackTrace}");
            }
        }

        public void StopGameByButton()
        {
            scoreBoardService.CloseScoreBoard();
            camsDetectionBoard.Close();

            IsMainTabsEnabled = true;
            IsGameRunning = false;

            gameService.StopGame(GameResultType.Aborted);
        }

        private void StopGameByError()
        {
            scoreBoardService.CloseScoreBoard();
            camsDetectionBoard.Close();

            IsMainTabsEnabled = true;
            IsGameRunning = false;

            gameService.StopGame(GameResultType.Error);
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
                                                            CamServiceWorkingMode.Setup,
                                                            logger,
                                                            drawService,
                                                            configService,
                                                            camsDetectionBoard,
                                                            throwService);
                                   while (!cancelToken.IsCancellationRequested)
                                   {
                                       cam.DoSetupCapture(GetCamsSetupSlidersData(camNumber));
                                       Application.Current.Dispatcher.Invoke(() => { RefreshImages(camNumber, cam.GetImage(), cam.GetRoiImage()); });
                                   }

                                   Application.Current.Dispatcher.Invoke(() => { RefreshImages(camNumber, new BitmapImage(), new BitmapImage()); });
                                   cam.Dispose();
                                   SaveCamsSetupSlidersSettings(camNumber);
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
            switch (camNumber)
            {
                case CamNumber._1:
                    return new List<double>
                           {
                               Cam1ThresholdSliderValue,
                               Cam1SurfaceSliderValue,
                               Cam1SurfaceCenterSliderValue,
                               Cam1RoiPosYSliderValue,
                               Cam1RoiHeightSliderValue
                           };
                case CamNumber._2:
                    return new List<double>
                           {
                               Cam2ThresholdSliderValue,
                               Cam2SurfaceSliderValue,
                               Cam2SurfaceCenterSliderValue,
                               Cam2RoiPosYSliderValue,
                               Cam2RoiHeightSliderValue
                           };
                case CamNumber._3:
                    return new List<double>
                           {
                               Cam3ThresholdSliderValue,
                               Cam3SurfaceSliderValue,
                               Cam3SurfaceCenterSliderValue,
                               Cam3RoiPosYSliderValue,
                               Cam3RoiHeightSliderValue
                           };
                case CamNumber._4:
                    return new List<double>
                           {
                               Cam4ThresholdSliderValue,
                               Cam4SurfaceSliderValue,
                               Cam4SurfaceCenterSliderValue,
                               Cam4RoiPosYSliderValue,
                               Cam4RoiHeightSliderValue
                           };
                default:
                    throw new ArgumentOutOfRangeException(nameof(camNumber), camNumber, null);
            }
        }

        private void RefreshImages(CamNumber camNumber, BitmapImage image, BitmapImage roiImage)
        {
            switch (camNumber)
            {
                case CamNumber._1:
                    Cam1Image = image;
                    Cam1RoiImage = roiImage;
                    break;
                case CamNumber._2:
                    Cam2Image = image;
                    Cam2RoiImage = roiImage;
                    break;
                case CamNumber._3:
                    Cam3Image = image;
                    Cam3RoiImage = roiImage;
                    break;
                case CamNumber._4:
                    Cam4Image = image;
                    Cam4RoiImage = roiImage;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(camNumber), camNumber, null);
            }
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

                drawService.ProjectionPrepare();
                drawService.ProjectionClear();
                drawService.PointsHistoryBoxClear();

                var cams = PrepareCamsServices(CamServiceWorkingMode.Crossing);
                detectionService.PrepareCamsAndTryCapture(cams, CamServiceWorkingMode.Crossing);

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
            camsDetectionBoard.Close();

            IsRuntimeCrossingRunning = false;
        }

        private List<CamService> PrepareCamsServices(CamServiceWorkingMode mode)
        {
            var cams = new List<CamService>();
            var cam1Active = Cam1Enabled && !App.NoCams;
            var cam2Active = Cam2Enabled && !App.NoCams;
            var cam3Active = Cam3Enabled && !App.NoCams;
            var cam4Active = Cam4Enabled && !App.NoCams;

            if (cam1Active)
            {
                cams.Add(new CamService(CamNumber._1,
                                        mode,
                                        logger,
                                        drawService,
                                        configService,
                                        camsDetectionBoard,
                                        throwService));
            }

            if (cam2Active)
            {
                cams.Add(new CamService(CamNumber._2,
                                        mode,
                                        logger,
                                        drawService,
                                        configService,
                                        camsDetectionBoard,
                                        throwService));
            }

            if (cam3Active)
            {
                cams.Add(new CamService(CamNumber._3,
                                        mode,
                                        logger,
                                        drawService,
                                        configService,
                                        camsDetectionBoard,
                                        throwService));
            }

            if (cam4Active)
            {
                cams.Add(new CamService(CamNumber._4,
                                        mode,
                                        logger,
                                        drawService,
                                        configService,
                                        camsDetectionBoard,
                                        throwService));
            }

            return cams;
        }

        #endregion

        #region CamsChecker

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

            var text = allCams.Count == 0
                           ? "No cameras found"
                           : str.ToString();

            CheckCamsBoxText = text;
        }

        public void CheckCamsSimultaneousWork()
        {
            try
            {
                var cams = PrepareCamsServices(CamServiceWorkingMode.Check);
                detectionService.PrepareCamsAndTryCapture(cams, CamServiceWorkingMode.Check);
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

        public void OnClosing()
        {
            SaveWindowPositionSettings();
            StopGameByButton();
            scoreBoardService.CloseScoreBoard();
            manualThrowPanel.Close();
            camsDetectionBoard.Close();
        }
    }
}