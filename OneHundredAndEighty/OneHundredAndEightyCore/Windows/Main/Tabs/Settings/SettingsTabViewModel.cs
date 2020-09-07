﻿#region Usings

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Enums;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Windows.CamsDetection;
using OneHundredAndEightyCore.Windows.Main.Tabs.Shared;
using OneHundredAndEightyCore.Windows.MessageBox;

#endregion

namespace OneHundredAndEightyCore.Windows.Main.Tabs.Settings
{
    public class SettingsTabViewModel : TabViewModelBase
    {
        public SettingsTabViewModel()
        {
        }

        public SettingsTabViewModel(DataContext dataContext,
                                    IDbService dbService,
                                    ILogger logger,
                                    IConfigService configService,
                                    DrawService drawService,
                                    IMessageBoxService messageBoxService,
                                    CamsDetectionBoard camsDetectionBoard,
                                    IDetectionService detectionService)
            : base(dataContext, dbService, logger, configService, drawService, messageBoxService, camsDetectionBoard, detectionService)
        {
            CalibrateCamsSetupPointsCommand = new CalibrateCamsSetupPointsCommand(CalibrateCamsSetupPoints);
            CheckCamsCommand = new CheckCamsCommand(CheckCamsSimultaneousWork);
            FindCamsCommand = new FindCamsCommand(FindConnectedCams);
            StartRuntimeCrossingCommand = new StartRuntimeCrossingCommand(StartCrossing);
            StopRuntimeCrossingCommand = new StopRuntimeCrossingCommand(StopCrossing);
            StartCamSetupCapturingCommand = new StartCamSetupCapturingCommand(StartCamSetupCapturing);
            StopCamSetupCapturingCommand = new StopCamSetupCapturingCommand(StopCamSetupCapturing);
        }

        public CalibrateCamsSetupPointsCommand CalibrateCamsSetupPointsCommand { get; }
        public CheckCamsCommand CheckCamsCommand { get; }
        public FindCamsCommand FindCamsCommand { get; }
        public StartRuntimeCrossingCommand StartRuntimeCrossingCommand { get; }
        public StopRuntimeCrossingCommand StopRuntimeCrossingCommand { get; }
        public StartCamSetupCapturingCommand StartCamSetupCapturingCommand { get; }
        public StopCamSetupCapturingCommand StopCamSetupCapturingCommand { get; }

        private CancellationTokenSource cts;

        #region Bindable props

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
                configService.CamsResolutionHeight = value;
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
                configService.CamsResolutionWidth = value;
                OnPropertyChanged(nameof(CamsResolutionWidth));
            }
        }

        private int smoothGaussValue;

        private double movesDetectedSleepTimeValue;

        public double MovesDetectedSleepTimeValue
        {
            get => movesDetectedSleepTimeValue;
            set
            {
                movesDetectedSleepTimeValue = value;
                configService.MovesDetectedSleepTimeValue = value;
                OnPropertyChanged(nameof(MovesDetectedSleepTimeValue));
            }
        }

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

        private int maxContourArcValue;

        public int MaxContourArcValue
        {
            get => maxContourArcValue;
            set
            {
                maxContourArcValue = value;
                configService.MaxContourArcValue = value;
                OnPropertyChanged(nameof(MaxContourArcValue));
            }
        }

        private int minContourAreaValue;

        public int MinContourAreaValue
        {
            get => minContourAreaValue;
            set
            {
                minContourAreaValue = value;
                configService.MinContourAreaValue = value;
                OnPropertyChanged(nameof(MinContourAreaValue));
            }
        }

        private int maxContourAreaValue;

        public int MaxContourAreaValue
        {
            get => maxContourAreaValue;
            set
            {
                maxContourAreaValue = value;
                configService.MaxContourAreaValue = value;
                OnPropertyChanged(nameof(MaxContourAreaValue));
            }
        }

        private int minContourWidthValue;

        public int MinContourWidthValue
        {
            get => minContourWidthValue;
            set
            {
                minContourWidthValue = value;
                configService.MinContourWidthValue = value;
                OnPropertyChanged(nameof(MinContourWidthValue));
            }
        }

        private int maxContourWidthValue;

        public int MaxContourWidthValue
        {
            get => maxContourWidthValue;
            set
            {
                maxContourWidthValue = value;
                configService.MaxContourWidthValue = value;
                OnPropertyChanged(nameof(MaxContourWidthValue));
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

        private int cam1XSetupValue;

        public int Cam1XSetupValue
        {
            get => cam1XSetupValue;
            set
            {
                cam1XSetupValue = value;
                configService.Cam1XSetupValue = value;
                OnPropertyChanged(nameof(Cam1XSetupValue));
            }
        }

        private int cam1YSetupValue;

        public int Cam1YSetupValue
        {
            get => cam1YSetupValue;
            set
            {
                cam1YSetupValue = value;
                configService.Cam1YSetupValue = value;
                OnPropertyChanged(nameof(Cam1YSetupValue));
            }
        }

        private int cam2XSetupValue;

        public int Cam2XSetupValue
        {
            get => cam2XSetupValue;
            set
            {
                cam2XSetupValue = value;
                configService.Cam2XSetupValue = value;
                OnPropertyChanged(nameof(Cam2XSetupValue));
            }
        }

        private int cam2YSetupValue;

        public int Cam2YSetupValue
        {
            get => cam2YSetupValue;
            set
            {
                cam2YSetupValue = value;
                configService.Cam2YSetupValue = value;
                OnPropertyChanged(nameof(Cam2YSetupValue));
            }
        }

        private int cam3XSetupValue;

        public int Cam3XSetupValue
        {
            get => cam3XSetupValue;
            set
            {
                cam3XSetupValue = value;
                configService.Cam3XSetupValue = value;
                OnPropertyChanged(nameof(Cam3XSetupValue));
            }
        }

        private int cam3YSetupValue;

        public int Cam3YSetupValue
        {
            get => cam3YSetupValue;
            set
            {
                cam3YSetupValue = value;
                configService.Cam3YSetupValue = value;
                OnPropertyChanged(nameof(Cam3YSetupValue));
            }
        }

        private int cam4XSetupValue;

        public int Cam4XSetupValue
        {
            get => cam4XSetupValue;
            set
            {
                cam4XSetupValue = value;
                configService.Cam4XSetupValue = value;
                OnPropertyChanged(nameof(Cam4XSetupValue));
            }
        }

        private int cam4YSetupValue;

        public int Cam4YSetupValue
        {
            get => cam4YSetupValue;
            set
            {
                cam4YSetupValue = value;
                configService.Cam4YSetupValue = value;
                OnPropertyChanged(nameof(Cam4YSetupValue));
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

        private string contoursBoxText;

        public string ContoursBoxText
        {
            get => contoursBoxText;
            set
            {
                contoursBoxText = value;
                OnPropertyChanged(nameof(ContoursBoxText));
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

        public void LoadSettings()
        {
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
            CamsResolutionWidth = configService.CamsResolutionWidth;
            CamsResolutionHeight = configService.CamsResolutionHeight;
            MovesDetectedSleepTimeValue = configService.MovesDetectedSleepTimeValue;
            SmoothGaussValue = configService.SmoothGaussValue;
            ThresholdSleepTimeValue = configService.ThresholdSleepTimeValue;
            ExtractionSleepTimeValue = configService.ExtractionSleepTimeValue;
            MinContourArcValue = configService.MinContourArcValue;
            MaxContourArcValue = configService.MaxContourArcValue;
            MinContourAreaValue = configService.MinContourAreaValue;
            MaxContourAreaValue = configService.MaxContourAreaValue;
            MinContourWidthValue = configService.MinContourWidthValue;
            MaxContourWidthValue = configService.MaxContourWidthValue;
            ToCam1Distance = configService.ToCam1Distance;
            ToCam2Distance = configService.ToCam2Distance;
            ToCam3Distance = configService.ToCam3Distance;
            ToCam4Distance = configService.ToCam4Distance;
            Cam1XSetupValue = configService.Cam1XSetupValue;
            Cam1YSetupValue = configService.Cam1YSetupValue;
            Cam2XSetupValue = configService.Cam2XSetupValue;
            Cam2YSetupValue = configService.Cam2YSetupValue;
            Cam3XSetupValue = configService.Cam3XSetupValue;
            Cam3YSetupValue = configService.Cam3YSetupValue;
            Cam4XSetupValue = configService.Cam4XSetupValue;
            Cam4YSetupValue = configService.Cam4YSetupValue;

            Cam1SetupSector = configService.Cam1SetupSector;
            Cam2SetupSector = configService.Cam2SetupSector;
            Cam3SetupSector = configService.Cam3SetupSector;
            Cam4SetupSector = configService.Cam4SetupSector;
        }

        private void CalibrateCamsSetupPoints()
        {
            var calibratedCam1SetupPoint = MeasureService.CalculateCamSetupPoint(ToCam1Distance,
                                                                                 Cam1SetupSector);
            var calibratedCam2SetupPoint = MeasureService.CalculateCamSetupPoint(ToCam2Distance,
                                                                                 Cam2SetupSector);
            var calibratedCam3SetupPoint = MeasureService.CalculateCamSetupPoint(ToCam3Distance,
                                                                                 Cam3SetupSector);
            var calibratedCam4SetupPoint = MeasureService.CalculateCamSetupPoint(ToCam4Distance,
                                                                                 Cam4SetupSector);
            Cam1XSetupValue = (int) calibratedCam1SetupPoint.X;
            Cam1YSetupValue = (int) calibratedCam1SetupPoint.Y;
            Cam2XSetupValue = (int) calibratedCam2SetupPoint.X;
            Cam2YSetupValue = (int) calibratedCam2SetupPoint.Y;
            Cam3XSetupValue = (int) calibratedCam3SetupPoint.X;
            Cam3YSetupValue = (int) calibratedCam3SetupPoint.Y;
            Cam4XSetupValue = (int) calibratedCam4SetupPoint.X;
            Cam4YSetupValue = (int) calibratedCam4SetupPoint.Y;
        }

        private void CheckCamsSimultaneousWork()
        {
            try
            {
                var cams = CreateCamsServices();
                detectionService.CheckCamsAndTryCapture(cams);
                CheckCamsBoxText = "Cams simultaneous work: OK";
            }
            catch (Exception)
            {
                CheckCamsBoxText = "Cams simultaneous work: ERROR";
            }
        }

        public void FindConnectedCams()
        {
            CheckCamsBoxText = detectionService.FindConnectedCams();
        }

        private void StartCrossing()
        {
            IsRuntimeCrossingRunning = true;
            try
            {
                var cams = CreateCamsServices();
                detectionService.CheckCamsAndTryCapture(cams);

                camsDetectionBoard.Open();

                detectionService.RunDetection(cams, DetectionServiceWorkingMode.Crossing);
            }
            catch (Exception e)
            {
                messageBoxService.ShowError($"{e.Message} \n {e.StackTrace}");
                StopCrossing();
            }
        }

        private void StopCrossing()
        {
            detectionService.StopDetection();
            camsDetectionBoard.Close();

            IsRuntimeCrossingRunning = false;
        }

        private async void StartCamSetupCapturing(CamNumber camNumber)
        {
            IsCamsSetupRunning = true;

            cts = new CancellationTokenSource();
            var cancelToken = cts.Token;

            try
            {
                await Task.Factory.StartNew(() =>
                                            {
                                                var cam = new CamService(camNumber,
                                                                         logger,
                                                                         drawService,
                                                                         configService);
                                                while (!cancelToken.IsCancellationRequested)
                                                {
                                                    cam.DoSetupCaptures();
                                                    Application.Current.Dispatcher.InvokeAsync(() =>
                                                                                               {
                                                                                                   CamImage = cam.GetImage();
                                                                                                   CamRoiImage = cam.GetRoiImage();
                                                                                                   ContoursBoxText = detectionService.FindContourOnRoiFrame(cam);
                                                                                               });
                                                }

                                                Application.Current.Dispatcher.InvokeAsync(() =>
                                                                                           {
                                                                                               CamImage = new BitmapImage();
                                                                                               CamRoiImage = new BitmapImage();
                                                                                               ContoursBoxText = string.Empty;
                                                                                           });
                                                cam.Dispose();
                                            },
                                            cancelToken);
            }
            catch (Exception e)
            {
                messageBoxService.ShowError($"{e.Message} \n {e.StackTrace}");
                StopCamSetupCapturing();
            }
        }

        private void StopCamSetupCapturing()
        {
            cts?.Cancel();
            IsCamsSetupRunning = false;
        }
    }
}