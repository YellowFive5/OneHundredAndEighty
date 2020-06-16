#region Usings

using NLog;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public class ConfigService : IConfigService
    {
        private readonly Logger logger;
        private readonly IDBService dbService;

        public ConfigService(Logger logger, IDBService dbService)
        {
            this.logger = logger;
            this.dbService = dbService;
        }

        public double DbVersion { get; set; }
        public double Cam1ThresholdSliderValue { get; set; }
        public double Cam2ThresholdSliderValue { get; set; }
        public double Cam3ThresholdSliderValue { get; set; }
        public double Cam4ThresholdSliderValue { get; set; }
        public double Cam1RoiPosYSliderValue { get; set; }
        public double Cam2RoiPosYSliderValue { get; set; }
        public double Cam3RoiPosYSliderValue { get; set; }
        public double Cam4RoiPosYSliderValue { get; set; }
        public double Cam1RoiHeightSliderValue { get; set; }
        public double Cam2RoiHeightSliderValue { get; set; }
        public double Cam3RoiHeightSliderValue { get; set; }
        public double Cam4RoiHeightSliderValue { get; set; }
        public double Cam1SurfaceSliderValue { get; set; }
        public double Cam2SurfaceSliderValue { get; set; }
        public double Cam3SurfaceSliderValue { get; set; }
        public double Cam4SurfaceSliderValue { get; set; }
        public double Cam1SurfaceCenterSliderValue { get; set; }
        public double Cam2SurfaceCenterSliderValue { get; set; }
        public double Cam3SurfaceCenterSliderValue { get; set; }
        public double Cam4SurfaceCenterSliderValue { get; set; }
        public int Cam1XValue { get; set; }
        public int Cam2XValue { get; set; }
        public int Cam3XValue { get; set; }
        public int Cam4XValue { get; set; }
        public int Cam1YValue { get; set; }
        public int Cam2YValue { get; set; }
        public int Cam3YValue { get; set; }
        public int Cam4YValue { get; set; }
        public int CamResolutionWidth { get; set; }
        public int CamResolutionHeight { get; set; }
        public int MovesExtractionValue { get; set; }
        public int MovesDartValue { get; set; }
        public int MovesNoiseValue { get; set; }
        public int SmoothGaussValue { get; set; }
        public int MinContourArcValue { get; set; }
        public double CamsFovAngle { get; set; }
        public string Cam1Id { get; set; }
        public string Cam2Id { get; set; }
        public string Cam3Id { get; set; }
        public string Cam4Id { get; set; }
        public double ExtractionSleepTimeValue { get; set; }
        public double ThresholdSleepTimeValue { get; set; }
        public double MoveDetectedSleepTimeValue { get; set; }
        public bool DetectionEnabled { get; set; }
        public double CamsDetectionWindowPositionLeft { get; set; }
        public double CamsDetectionWindowPositionTop { get; set; }
        public double CamsDetectionWindowHeight { get; set; }
        public double CamsDetectionWindowWidth { get; set; }
        public bool Cam1Enabled { get; set; }
        public bool Cam2Enabled { get; set; }
        public bool Cam3Enabled { get; set; }
        public bool Cam4Enabled { get; set; }
        public double ToCam1Distance { get; set; }
        public double ToCam2Distance { get; set; }
        public double ToCam3Distance { get; set; }
        public double ToCam4Distance { get; set; }
        public string Cam1SetupSector { get; set; }
        public string Cam2SetupSector { get; set; }
        public string Cam3SetupSector { get; set; }
        public string Cam4SetupSector { get; set; }
        public double MainWindowPositionLeft { get; set; }
        public double MainWindowPositionTop { get; set; }
        public double MainWindowHeight { get; set; }
        public double MainWindowWidth { get; set; }
        public double FreeThrowsSingleScoreWindowPositionLeft { get; set; }
        public double FreeThrowsSingleScoreWindowPositionTop { get; set; }
        public double FreeThrowsSingleScoreWindowHeight { get; set; }
        public double FreeThrowsSingleScoreWindowWidth { get; set; }
        public double FreeThrowsDoubleScoreWindowPositionLeft { get; set; }
        public double FreeThrowsDoubleScoreWindowPositionTop { get; set; }
        public double FreeThrowsDoubleScoreWindowHeight { get; set; }
        public double FreeThrowsDoubleScoreWindowWidth { get; set; }
        public double ClassicScoreWindowPositionLeft { get; set; }
        public double ClassicScoreWindowPositionTop { get; set; }
        public double ClassicScoreWindowHeight { get; set; }
        public double ClassicScoreWindowWidth { get; set; }

        public void LoadSettings()
        {
            DbVersion = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.DBVersion));
            MainWindowPositionLeft = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.MainWindowPositionLeft));
            MainWindowPositionTop = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.MainWindowPositionTop));
            MainWindowHeight = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.MainWindowHeight));
            MainWindowWidth = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.MainWindowWidth));
            Cam1ThresholdSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam1ThresholdSlider));
            Cam2ThresholdSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam2ThresholdSlider));
            Cam3ThresholdSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam3ThresholdSlider));
            Cam4ThresholdSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam4ThresholdSlider));
            Cam1RoiPosYSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam1RoiPosYSlider));
            Cam2RoiPosYSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam2RoiPosYSlider));
            Cam3RoiPosYSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam3RoiPosYSlider));
            Cam4RoiPosYSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam4RoiPosYSlider));
            Cam1RoiHeightSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam1RoiHeightSlider));
            Cam2RoiHeightSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam2RoiHeightSlider));
            Cam3RoiHeightSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam3RoiHeightSlider));
            Cam4RoiHeightSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam4RoiHeightSlider));
            Cam1SurfaceSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam1SurfaceSlider));
            Cam2SurfaceSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam2SurfaceSlider));
            Cam3SurfaceSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam3SurfaceSlider));
            Cam4SurfaceSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam4SurfaceSlider));
            Cam1SurfaceCenterSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam1SurfaceCenterSlider));
            Cam2SurfaceCenterSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam2SurfaceCenterSlider));
            Cam3SurfaceCenterSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam3SurfaceCenterSlider));
            Cam4SurfaceCenterSliderValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.Cam4SurfaceCenterSlider));
            Cam1XValue = Converter.ToInt(dbService.SettingsGetValue(SettingsType.Cam1X));
            Cam2XValue = Converter.ToInt(dbService.SettingsGetValue(SettingsType.Cam2X));
            Cam3XValue = Converter.ToInt(dbService.SettingsGetValue(SettingsType.Cam3X));
            Cam4XValue = Converter.ToInt(dbService.SettingsGetValue(SettingsType.Cam4X));
            Cam1YValue = Converter.ToInt(dbService.SettingsGetValue(SettingsType.Cam1Y));
            Cam2YValue = Converter.ToInt(dbService.SettingsGetValue(SettingsType.Cam2Y));
            Cam3YValue = Converter.ToInt(dbService.SettingsGetValue(SettingsType.Cam3Y));
            Cam4YValue = Converter.ToInt(dbService.SettingsGetValue(SettingsType.Cam4Y));
            CamResolutionWidth = Converter.ToInt(dbService.SettingsGetValue(SettingsType.ResolutionWidth));
            CamResolutionHeight = Converter.ToInt(dbService.SettingsGetValue(SettingsType.ResolutionHeight));
            MovesExtractionValue = Converter.ToInt(dbService.SettingsGetValue(SettingsType.MovesExtraction));
            MovesDartValue = Converter.ToInt(dbService.SettingsGetValue(SettingsType.MovesDart));
            MovesNoiseValue = Converter.ToInt(dbService.SettingsGetValue(SettingsType.MovesNoise));
            SmoothGaussValue = Converter.ToInt(dbService.SettingsGetValue(SettingsType.SmoothGauss));
            MinContourArcValue = Converter.ToInt(dbService.SettingsGetValue(SettingsType.MinContourArc));
            CamsFovAngle = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.CamFovAngle));
            Cam1Id = Converter.ToString(dbService.SettingsGetValue(SettingsType.Cam1Id));
            Cam2Id = Converter.ToString(dbService.SettingsGetValue(SettingsType.Cam2Id));
            Cam3Id = Converter.ToString(dbService.SettingsGetValue(SettingsType.Cam3Id));
            Cam4Id = Converter.ToString(dbService.SettingsGetValue(SettingsType.Cam4Id));
            ExtractionSleepTimeValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.ExtractionSleepTime));
            ThresholdSleepTimeValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.ThresholdSleepTime));
            MoveDetectedSleepTimeValue = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.MoveDetectedSleepTime));
            DetectionEnabled = Converter.ToBool(dbService.SettingsGetValue(SettingsType.WithDetectionCheckBox));
            CamsDetectionWindowPositionLeft = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.CamsDetectionWindowPositionLeft));
            CamsDetectionWindowPositionTop = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.CamsDetectionWindowPositionTop));
            CamsDetectionWindowHeight = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.CamsDetectionWindowHeight));
            CamsDetectionWindowWidth = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.CamsDetectionWindowWidth));
            Cam1Enabled = Converter.ToBool(dbService.SettingsGetValue(SettingsType.Cam1CheckBox));
            Cam2Enabled = Converter.ToBool(dbService.SettingsGetValue(SettingsType.Cam2CheckBox));
            Cam3Enabled = Converter.ToBool(dbService.SettingsGetValue(SettingsType.Cam3CheckBox));
            Cam4Enabled = Converter.ToBool(dbService.SettingsGetValue(SettingsType.Cam4CheckBox));
            ToCam1Distance = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.ToCam1Distance));
            ToCam2Distance = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.ToCam2Distance));
            ToCam3Distance = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.ToCam3Distance));
            ToCam4Distance = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.ToCam4Distance));
            Cam1SetupSector = Converter.ToString(dbService.SettingsGetValue(SettingsType.Cam1SetupSector));
            Cam2SetupSector = Converter.ToString(dbService.SettingsGetValue(SettingsType.Cam2SetupSector));
            Cam3SetupSector = Converter.ToString(dbService.SettingsGetValue(SettingsType.Cam3SetupSector));
            Cam4SetupSector = Converter.ToString(dbService.SettingsGetValue(SettingsType.Cam4SetupSector));
            FreeThrowsSingleScoreWindowPositionLeft = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.FreeThrowsSingleScoreWindowPositionLeft));
            FreeThrowsSingleScoreWindowPositionTop = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.FreeThrowsSingleScoreWindowPositionTop));
            FreeThrowsSingleScoreWindowHeight = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.FreeThrowsSingleScoreWindowHeight));
            FreeThrowsSingleScoreWindowWidth = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.FreeThrowsSingleScoreWindowWidth));
            FreeThrowsDoubleScoreWindowPositionLeft = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.FreeThrowsDoubleScoreWindowPositionLeft));
            FreeThrowsDoubleScoreWindowPositionTop = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.FreeThrowsDoubleScoreWindowPositionTop));
            FreeThrowsDoubleScoreWindowHeight = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.FreeThrowsDoubleScoreWindowHeight));
            FreeThrowsDoubleScoreWindowWidth = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.FreeThrowsDoubleScoreWindowWidth));
            ClassicScoreWindowPositionLeft = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.ClassicScoreWindowPositionLeft));
            ClassicScoreWindowPositionTop = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.ClassicScoreWindowPositionTop));
            ClassicScoreWindowHeight = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.ClassicScoreWindowHeight));
            ClassicScoreWindowWidth = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.ClassicScoreWindowWidth));
        }

        public void SaveSettings()
        {
            dbService.SettingsSetValue(SettingsType.MainWindowHeight, Converter.ToString(MainWindowHeight));
            dbService.SettingsSetValue(SettingsType.MainWindowWidth, Converter.ToString(MainWindowWidth));
            dbService.SettingsSetValue(SettingsType.MainWindowPositionLeft, Converter.ToString(MainWindowPositionLeft));
            dbService.SettingsSetValue(SettingsType.MainWindowPositionTop, Converter.ToString(MainWindowPositionTop));
            dbService.SettingsSetValue(SettingsType.Cam1ThresholdSlider, Converter.ToString(Cam1ThresholdSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam2ThresholdSlider, Converter.ToString(Cam2ThresholdSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam3ThresholdSlider, Converter.ToString(Cam3ThresholdSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam4ThresholdSlider, Converter.ToString(Cam4ThresholdSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam1RoiPosYSlider, Converter.ToString(Cam1RoiPosYSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam2RoiPosYSlider, Converter.ToString(Cam2RoiPosYSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam3RoiPosYSlider, Converter.ToString(Cam3RoiPosYSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam4RoiPosYSlider, Converter.ToString(Cam4RoiPosYSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam1RoiHeightSlider, Converter.ToString(Cam1RoiHeightSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam2RoiHeightSlider, Converter.ToString(Cam2RoiHeightSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam3RoiHeightSlider, Converter.ToString(Cam3RoiHeightSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam4RoiHeightSlider, Converter.ToString(Cam4RoiHeightSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam1SurfaceSlider, Converter.ToString(Cam1SurfaceSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam2SurfaceSlider, Converter.ToString(Cam2SurfaceSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam3SurfaceSlider, Converter.ToString(Cam3SurfaceSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam4SurfaceSlider, Converter.ToString(Cam4SurfaceSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam1SurfaceCenterSlider, Converter.ToString(Cam1SurfaceCenterSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam2SurfaceCenterSlider, Converter.ToString(Cam2SurfaceCenterSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam3SurfaceCenterSlider, Converter.ToString(Cam3SurfaceCenterSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam4SurfaceCenterSlider, Converter.ToString(Cam4SurfaceCenterSliderValue));
            dbService.SettingsSetValue(SettingsType.Cam1X, Converter.ToString(Cam1XValue));
            dbService.SettingsSetValue(SettingsType.Cam2X, Converter.ToString(Cam2XValue));
            dbService.SettingsSetValue(SettingsType.Cam3X, Converter.ToString(Cam3XValue));
            dbService.SettingsSetValue(SettingsType.Cam4X, Converter.ToString(Cam4XValue));
            dbService.SettingsSetValue(SettingsType.Cam1Y, Converter.ToString(Cam1YValue));
            dbService.SettingsSetValue(SettingsType.Cam2Y, Converter.ToString(Cam2YValue));
            dbService.SettingsSetValue(SettingsType.Cam3Y, Converter.ToString(Cam3YValue));
            dbService.SettingsSetValue(SettingsType.Cam4Y, Converter.ToString(Cam4YValue));
            dbService.SettingsSetValue(SettingsType.ResolutionWidth, Converter.ToString(CamResolutionWidth));
            dbService.SettingsSetValue(SettingsType.ResolutionHeight, Converter.ToString(CamResolutionHeight));
            dbService.SettingsSetValue(SettingsType.MovesExtraction, Converter.ToString(MovesExtractionValue));
            dbService.SettingsSetValue(SettingsType.MovesDart, Converter.ToString(MovesDartValue));
            dbService.SettingsSetValue(SettingsType.MovesNoise, Converter.ToString(MovesNoiseValue));
            dbService.SettingsSetValue(SettingsType.SmoothGauss, Converter.ToString(SmoothGaussValue));
            dbService.SettingsSetValue(SettingsType.MinContourArc, Converter.ToString(MinContourArcValue));
            dbService.SettingsSetValue(SettingsType.CamFovAngle, Converter.ToString(CamsFovAngle));
            dbService.SettingsSetValue(SettingsType.Cam1Id, Converter.ToString(Cam1Id));
            dbService.SettingsSetValue(SettingsType.Cam2Id, Converter.ToString(Cam2Id));
            dbService.SettingsSetValue(SettingsType.Cam3Id, Converter.ToString(Cam3Id));
            dbService.SettingsSetValue(SettingsType.Cam4Id, Converter.ToString(Cam4Id));
            dbService.SettingsSetValue(SettingsType.ExtractionSleepTime, Converter.ToString(ExtractionSleepTimeValue));
            dbService.SettingsSetValue(SettingsType.ThresholdSleepTime, Converter.ToString(ThresholdSleepTimeValue));
            dbService.SettingsSetValue(SettingsType.MoveDetectedSleepTime, Converter.ToString(MoveDetectedSleepTimeValue));
            dbService.SettingsSetValue(SettingsType.WithDetectionCheckBox, Converter.ToString(DetectionEnabled));
            dbService.SettingsSetValue(SettingsType.CamsDetectionWindowPositionLeft, Converter.ToString(CamsDetectionWindowPositionLeft));
            dbService.SettingsSetValue(SettingsType.CamsDetectionWindowPositionTop, Converter.ToString(CamsDetectionWindowPositionTop));
            dbService.SettingsSetValue(SettingsType.CamsDetectionWindowHeight, Converter.ToString(CamsDetectionWindowHeight));
            dbService.SettingsSetValue(SettingsType.CamsDetectionWindowWidth, Converter.ToString(CamsDetectionWindowWidth));
            dbService.SettingsSetValue(SettingsType.Cam1CheckBox, Converter.ToString(Cam1Enabled));
            dbService.SettingsSetValue(SettingsType.Cam2CheckBox, Converter.ToString(Cam2Enabled));
            dbService.SettingsSetValue(SettingsType.Cam3CheckBox, Converter.ToString(Cam3Enabled));
            dbService.SettingsSetValue(SettingsType.Cam4CheckBox, Converter.ToString(Cam4Enabled));
            dbService.SettingsSetValue(SettingsType.ToCam1Distance, Converter.ToString(ToCam1Distance));
            dbService.SettingsSetValue(SettingsType.ToCam2Distance, Converter.ToString(ToCam2Distance));
            dbService.SettingsSetValue(SettingsType.ToCam3Distance, Converter.ToString(ToCam3Distance));
            dbService.SettingsSetValue(SettingsType.ToCam4Distance, Converter.ToString(ToCam4Distance));
            dbService.SettingsSetValue(SettingsType.Cam1SetupSector, Converter.ToString(Cam1SetupSector));
            dbService.SettingsSetValue(SettingsType.Cam2SetupSector, Converter.ToString(Cam2SetupSector));
            dbService.SettingsSetValue(SettingsType.Cam3SetupSector, Converter.ToString(Cam3SetupSector));
            dbService.SettingsSetValue(SettingsType.Cam4SetupSector, Converter.ToString(Cam4SetupSector));
            dbService.SettingsSetValue(SettingsType.FreeThrowsSingleScoreWindowPositionLeft, Converter.ToString(FreeThrowsSingleScoreWindowPositionLeft));
            dbService.SettingsSetValue(SettingsType.FreeThrowsSingleScoreWindowPositionTop, Converter.ToString(FreeThrowsSingleScoreWindowPositionTop));
            dbService.SettingsSetValue(SettingsType.FreeThrowsSingleScoreWindowHeight, Converter.ToString(FreeThrowsSingleScoreWindowHeight));
            dbService.SettingsSetValue(SettingsType.FreeThrowsSingleScoreWindowWidth, Converter.ToString(FreeThrowsSingleScoreWindowWidth));
            dbService.SettingsSetValue(SettingsType.FreeThrowsDoubleScoreWindowPositionLeft, Converter.ToString(FreeThrowsDoubleScoreWindowPositionLeft));
            dbService.SettingsSetValue(SettingsType.FreeThrowsDoubleScoreWindowPositionTop, Converter.ToString(FreeThrowsDoubleScoreWindowPositionTop));
            dbService.SettingsSetValue(SettingsType.FreeThrowsDoubleScoreWindowHeight, Converter.ToString(FreeThrowsDoubleScoreWindowHeight));
            dbService.SettingsSetValue(SettingsType.FreeThrowsDoubleScoreWindowWidth, Converter.ToString(FreeThrowsDoubleScoreWindowWidth));
            dbService.SettingsSetValue(SettingsType.ClassicScoreWindowPositionLeft, Converter.ToString(ClassicScoreWindowPositionLeft));
            dbService.SettingsSetValue(SettingsType.ClassicScoreWindowPositionTop, Converter.ToString(ClassicScoreWindowPositionTop));
            dbService.SettingsSetValue(SettingsType.ClassicScoreWindowHeight, Converter.ToString(ClassicScoreWindowHeight));
            dbService.SettingsSetValue(SettingsType.ClassicScoreWindowWidth, Converter.ToString(ClassicScoreWindowWidth));
        }
    }
}