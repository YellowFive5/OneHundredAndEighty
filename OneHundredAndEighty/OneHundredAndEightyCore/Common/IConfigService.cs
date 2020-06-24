namespace OneHundredAndEightyCore.Common
{
    public interface IConfigService
    {
        void SaveSettings();
        void LoadSettings();
        double DbVersion { get; }
        double Cam1ThresholdSliderValue { get; set; }
        double Cam2ThresholdSliderValue { get; set; }
        double Cam3ThresholdSliderValue { get; set; }
        double Cam4ThresholdSliderValue { get; set; }
        double Cam1RoiPosYSliderValue { get; set; }
        double Cam2RoiPosYSliderValue { get; set; }
        double Cam3RoiPosYSliderValue { get; set; }
        double Cam4RoiPosYSliderValue { get; set; }
        double Cam1RoiHeightSliderValue { get; set; }
        double Cam2RoiHeightSliderValue { get; set; }
        double Cam3RoiHeightSliderValue { get; set; }
        double Cam4RoiHeightSliderValue { get; set; }
        double Cam1SurfaceSliderValue { get; set; }
        double Cam2SurfaceSliderValue { get; set; }
        double Cam3SurfaceSliderValue { get; set; }
        double Cam4SurfaceSliderValue { get; set; }
        double Cam1SurfaceCenterSliderValue { get; set; }
        double Cam2SurfaceCenterSliderValue { get; set; }
        double Cam3SurfaceCenterSliderValue { get; set; }
        double Cam4SurfaceCenterSliderValue { get; set; }
        int Cam1XSetupValue { get; set; }
        int Cam2XSetupValue { get; set; }
        int Cam3XSetupValue { get; set; }
        int Cam4XSetupValue { get; set; }
        int Cam1YSetupValue { get; set; }
        int Cam2YSetupValue { get; set; }
        int Cam3YSetupValue { get; set; }
        int Cam4YSetupValue { get; set; }
        int CamsResolutionWidth { get; set; }
        int CamsResolutionHeight { get; set; }
        int SmoothGaussValue { get; set; }
        int MinContourArcValue { get; set; }
        double CamsFovAngle { get; set; }
        string Cam1Id { get; set; }
        string Cam2Id { get; set; }
        string Cam3Id { get; set; }
        string Cam4Id { get; set; }
        double ExtractionSleepTimeValue { get; set; }
        double ThresholdSleepTimeValue { get; set; }
        double MovesDetectedSleepTimeValue { get; set; }
        bool DetectionEnabled { get; set; }
        double CamsDetectionWindowPositionLeft { get; set; }
        double CamsDetectionWindowPositionTop { get; set; }
        double CamsDetectionWindowHeight { get; set; }
        double CamsDetectionWindowWidth { get; set; }
        bool Cam1Enabled { get; set; }
        bool Cam2Enabled { get; set; }
        bool Cam3Enabled { get; set; }
        bool Cam4Enabled { get; set; }
        double ToCam1Distance { get; set; }
        double ToCam2Distance { get; set; }
        double ToCam3Distance { get; set; }
        double ToCam4Distance { get; set; }
        string Cam1SetupSector { get; set; }
        string Cam2SetupSector { get; set; }
        string Cam3SetupSector { get; set; }
        string Cam4SetupSector { get; set; }
        double MainWindowPositionLeft { get; set; }
        double MainWindowPositionTop { get; set; }
        double MainWindowHeight { get; set; }
        double MainWindowWidth { get; set; }
        double FreeThrowsSingleScoreWindowPositionLeft { get; set; }
        double FreeThrowsSingleScoreWindowPositionTop { get; set; }
        double FreeThrowsSingleScoreWindowHeight { get; set; }
        double FreeThrowsSingleScoreWindowWidth { get; set; }
        double FreeThrowsDoubleScoreWindowPositionLeft { get; set; }
        double FreeThrowsDoubleScoreWindowPositionTop { get; set; }
        double FreeThrowsDoubleScoreWindowHeight { get; set; }
        double FreeThrowsDoubleScoreWindowWidth { get; set; }
        double ClassicScoreWindowPositionLeft { get; set; }
        double ClassicScoreWindowPositionTop { get; set; }
        double ClassicScoreWindowHeight { get; set; }
        double ClassicScoreWindowWidth { get; set; }
        int MaxContourArcValue { get; set; }
        int MinContourAreaValue { get; set; }
        int MaxContourAreaValue { get; set; }
        int MinContourWidthValue { get; set; }
        int MaxContourWidthValue { get; set; }
    }
}