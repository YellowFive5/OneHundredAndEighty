#region Usings

using Moq;
using NUnit.Framework;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Tests.ConfigService
{
    public class WhenSavingSettings : ConfigServiceTestBase
    {
        [Test]
        public void _78_ConfigItemsLoaded()
        {
            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(It.IsAny<SettingsType>(), It.IsAny<string>()),
                             Times.Exactly(78));
        }

        [Test]
        public void DbServiceMethodCallsWithMainWindowHeight()
        {
            var settingsType = SettingsType.MainWindowHeight;
            var value = 567;
            configService.MainWindowHeight = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithMainWindowWidth()
        {
            var settingsType = SettingsType.MainWindowWidth;
            var value = 567;
            configService.MainWindowWidth = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithMainWindowPositionLeft()
        {
            var settingsType = SettingsType.MainWindowPositionLeft;
            var value = 567;
            configService.MainWindowPositionLeft = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithMainWindowPositionTop()
        {
            var settingsType = SettingsType.MainWindowPositionTop;
            var value = 567;
            configService.MainWindowPositionTop = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam1ThresholdSliderValue()
        {
            var settingsType = SettingsType.Cam1ThresholdSlider;
            var value = 155.25;
            configService.Cam1ThresholdSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam2ThresholdSliderValue()
        {
            var settingsType = SettingsType.Cam2ThresholdSlider;
            var value = 155.25;
            configService.Cam2ThresholdSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam3ThresholdSliderValue()
        {
            var settingsType = SettingsType.Cam3ThresholdSlider;
            var value = 155.25;
            configService.Cam3ThresholdSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam4ThresholdSliderValue()
        {
            var settingsType = SettingsType.Cam4ThresholdSlider;
            var value = 155.25;
            configService.Cam4ThresholdSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam1RoiPosYSliderValue()
        {
            var settingsType = SettingsType.Cam1RoiPosYSlider;
            var value = 155.25;
            configService.Cam1RoiPosYSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam2RoiPosYSliderValue()
        {
            var settingsType = SettingsType.Cam2RoiPosYSlider;
            var value = 155.25;
            configService.Cam2RoiPosYSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam3RoiPosYSliderValue()
        {
            var settingsType = SettingsType.Cam3RoiPosYSlider;
            var value = 155.25;
            configService.Cam3RoiPosYSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam4RoiPosYSliderValue()
        {
            var settingsType = SettingsType.Cam4RoiPosYSlider;
            var value = 155.25;
            configService.Cam4RoiPosYSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam1RoiHeightSliderValue()
        {
            var settingsType = SettingsType.Cam1RoiHeightSlider;
            var value = 155.25;
            configService.Cam1RoiHeightSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam2RoiHeightSliderValue()
        {
            var settingsType = SettingsType.Cam2RoiHeightSlider;
            var value = 155.25;
            configService.Cam2RoiHeightSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam3RoiHeightSliderValue()
        {
            var settingsType = SettingsType.Cam3RoiHeightSlider;
            var value = 155.25;
            configService.Cam3RoiHeightSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam4RoiHeightSliderValue()
        {
            var settingsType = SettingsType.Cam4RoiHeightSlider;
            var value = 155.25;
            configService.Cam4RoiHeightSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam1SurfaceSliderValue()
        {
            var settingsType = SettingsType.Cam1SurfaceSlider;
            var value = 155.25;
            configService.Cam1SurfaceSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam2SurfaceSliderValue()
        {
            var settingsType = SettingsType.Cam2SurfaceSlider;
            var value = 155.25;
            configService.Cam2SurfaceSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam3SurfaceSliderValue()
        {
            var settingsType = SettingsType.Cam3SurfaceSlider;
            var value = 155.25;
            configService.Cam3SurfaceSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam4SurfaceSliderValue()
        {
            var settingsType = SettingsType.Cam4SurfaceSlider;
            var value = 155.25;
            configService.Cam4SurfaceSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam1SurfaceCenterSliderValue()
        {
            var settingsType = SettingsType.Cam1SurfaceCenterSlider;
            var value = 155.25;
            configService.Cam1SurfaceCenterSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam2SurfaceCenterSliderValue()
        {
            var settingsType = SettingsType.Cam2SurfaceCenterSlider;
            var value = 155.25;
            configService.Cam2SurfaceCenterSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam3SurfaceCenterSliderValue()
        {
            var settingsType = SettingsType.Cam3SurfaceCenterSlider;
            var value = 155.25;
            configService.Cam3SurfaceCenterSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam4SurfaceCenterSliderValue()
        {
            var settingsType = SettingsType.Cam4SurfaceCenterSlider;
            var value = 155.25;
            configService.Cam4SurfaceCenterSliderValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam1XSetupValue()
        {
            var settingsType = SettingsType.Cam1X;
            var value = 55;
            configService.Cam1XSetupValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam2XSetupValue()
        {
            var settingsType = SettingsType.Cam2X;
            var value = 55;
            configService.Cam2XSetupValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam3XSetupValue()
        {
            var settingsType = SettingsType.Cam3X;
            var value = 55;
            configService.Cam3XSetupValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam4XSetupValue()
        {
            var settingsType = SettingsType.Cam4X;
            var value = 55;
            configService.Cam4XSetupValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam1YSetupValue()
        {
            var settingsType = SettingsType.Cam1Y;
            var value = 55;
            configService.Cam1YSetupValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam2YSetupValue()
        {
            var settingsType = SettingsType.Cam2Y;
            var value = 55;
            configService.Cam2YSetupValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam3YSetupValue()
        {
            var settingsType = SettingsType.Cam3Y;
            var value = 55;
            configService.Cam3YSetupValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam4YSetupValue()
        {
            var settingsType = SettingsType.Cam4Y;
            var value = 55;
            configService.Cam4YSetupValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithResolutionWidth()
        {
            var settingsType = SettingsType.ResolutionWidth;
            var value = 1980;
            configService.CamsResolutionWidth = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithResolutionHeight()
        {
            var settingsType = SettingsType.ResolutionHeight;
            var value = 1980;
            configService.CamsResolutionHeight = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithSmoothGaussValue()
        {
            var settingsType = SettingsType.SmoothGauss;
            var value = 200;
            configService.SmoothGaussValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithMinContourArcValue()
        {
            var settingsType = SettingsType.MinContourArc;
            var value = 200;
            configService.MinContourArcValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithMaxContourArcValue()
        {
            var settingsType = SettingsType.MaxContourArc;
            var value = 200;
            configService.MaxContourArcValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithMaxContourAreaValue()
        {
            var settingsType = SettingsType.MaxContourArea;
            var value = 200;
            configService.MaxContourAreaValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithMinContourAreaValue()
        {
            var settingsType = SettingsType.MinContourArea;
            var value = 200;
            configService.MinContourAreaValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithMaxContourWidthValue()
        {
            var settingsType = SettingsType.MaxContourWidth;
            var value = 200;
            configService.MaxContourWidthValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithMinContourWidthValue()
        {
            var settingsType = SettingsType.MinContourWidth;
            var value = 200;
            configService.MinContourWidthValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCamsFovAngle()
        {
            var settingsType = SettingsType.CamFovAngle;
            var value = 75.5;
            configService.CamsFovAngle = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam1Id()
        {
            var settingsType = SettingsType.Cam1Id;
            var value = "2323re";
            configService.Cam1Id = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam2Id()
        {
            var settingsType = SettingsType.Cam2Id;
            var value = "2323re";
            configService.Cam2Id = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam3Id()
        {
            var settingsType = SettingsType.Cam3Id;
            var value = "2323re";
            configService.Cam3Id = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam4Id()
        {
            var settingsType = SettingsType.Cam4Id;
            var value = "2323re";
            configService.Cam4Id = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithExtractionSleepTimeValue()
        {
            var settingsType = SettingsType.ExtractionSleepTime;
            var value = 0.25;
            configService.ExtractionSleepTimeValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithThresholdSleepTimeValue()
        {
            var settingsType = SettingsType.ThresholdSleepTime;
            var value = 0.25;
            configService.ThresholdSleepTimeValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithMovesDetectedSleepTimeValue()
        {
            var settingsType = SettingsType.MoveDetectedSleepTime;
            var value = 0.25;
            configService.MovesDetectedSleepTimeValue = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithDetectionEnabled()
        {
            var settingsType = SettingsType.WithDetectionCheckBox;
            var value = true;
            configService.DetectionEnabled = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCamsDetectionWindowPositionLeft()
        {
            var settingsType = SettingsType.CamsDetectionWindowPositionLeft;
            var value = 526.25;
            configService.CamsDetectionWindowPositionLeft = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCamsDetectionWindowPositionTop()
        {
            var settingsType = SettingsType.CamsDetectionWindowPositionTop;
            var value = 526.25;
            configService.CamsDetectionWindowPositionTop = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCamsDetectionWindowHeight()
        {
            var settingsType = SettingsType.CamsDetectionWindowHeight;
            var value = 526.25;
            configService.CamsDetectionWindowHeight = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCamsDetectionWindowWidth()
        {
            var settingsType = SettingsType.CamsDetectionWindowWidth;
            var value = 526.25;
            configService.CamsDetectionWindowWidth = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam1Enabled()
        {
            var settingsType = SettingsType.Cam1CheckBox;
            var value = true;
            configService.Cam1Enabled = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam2Enabled()
        {
            var settingsType = SettingsType.Cam2CheckBox;
            var value = true;
            configService.Cam2Enabled = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam3Enabled()
        {
            var settingsType = SettingsType.Cam3CheckBox;
            var value = true;
            configService.Cam3Enabled = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam4Enabled()
        {
            var settingsType = SettingsType.Cam4CheckBox;
            var value = true;
            configService.Cam4Enabled = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithToCam1Distance()
        {
            var settingsType = SettingsType.ToCam1Distance;
            var value = 25.6;
            configService.ToCam1Distance = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithToCam2Distance()
        {
            var settingsType = SettingsType.ToCam2Distance;
            var value = 25.6;
            configService.ToCam2Distance = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithToCam3Distance()
        {
            var settingsType = SettingsType.ToCam3Distance;
            var value = 25.6;
            configService.ToCam3Distance = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithToCam4Distance()
        {
            var settingsType = SettingsType.ToCam4Distance;
            var value = 25.6;
            configService.ToCam4Distance = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam1SetupSector()
        {
            var settingsType = SettingsType.Cam1SetupSector;
            var value = "20/1";
            configService.Cam1SetupSector = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam2SetupSector()
        {
            var settingsType = SettingsType.Cam2SetupSector;
            var value = "20/1";
            configService.Cam2SetupSector = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam3SetupSector()
        {
            var settingsType = SettingsType.Cam3SetupSector;
            var value = "20/1";
            configService.Cam3SetupSector = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithCam4SetupSector()
        {
            var settingsType = SettingsType.Cam4SetupSector;
            var value = "20/1";
            configService.Cam4SetupSector = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithFreeThrowsSingleScoreWindowPositionLeft()
        {
            var settingsType = SettingsType.FreeThrowsSingleScoreWindowPositionLeft;
            var value = 158.26;
            configService.FreeThrowsSingleScoreWindowPositionLeft = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithFreeThrowsSingleScoreWindowPositionTop()
        {
            var settingsType = SettingsType.FreeThrowsSingleScoreWindowPositionTop;
            var value = 158.26;
            configService.FreeThrowsSingleScoreWindowPositionTop = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithFreeThrowsSingleScoreWindowHeight()
        {
            var settingsType = SettingsType.FreeThrowsSingleScoreWindowHeight;
            var value = 158.26;
            configService.FreeThrowsSingleScoreWindowHeight = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithFreeThrowsSingleScoreWindowWidth()
        {
            var settingsType = SettingsType.FreeThrowsSingleScoreWindowWidth;
            var value = 158.26;
            configService.FreeThrowsSingleScoreWindowWidth = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithFreeThrowsDoubleScoreWindowPositionLeft()
        {
            var settingsType = SettingsType.FreeThrowsDoubleScoreWindowPositionLeft;
            var value = 158.26;
            configService.FreeThrowsDoubleScoreWindowPositionLeft = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithFreeThrowsDoubleScoreWindowPositionTop()
        {
            var settingsType = SettingsType.FreeThrowsDoubleScoreWindowPositionTop;
            var value = 158.26;
            configService.FreeThrowsDoubleScoreWindowPositionTop = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithFreeThrowsDoubleScoreWindowHeight()
        {
            var settingsType = SettingsType.FreeThrowsDoubleScoreWindowHeight;
            var value = 158.26;
            configService.FreeThrowsDoubleScoreWindowHeight = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithFreeThrowsDoubleScoreWindowWidth()
        {
            var settingsType = SettingsType.FreeThrowsDoubleScoreWindowWidth;
            var value = 158.26;
            configService.FreeThrowsDoubleScoreWindowWidth = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithClassicScoreWindowPositionLeft()
        {
            var settingsType = SettingsType.ClassicScoreWindowPositionLeft;
            var value = 158.26;
            configService.ClassicScoreWindowPositionLeft = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithClassicScoreWindowPositionTop()
        {
            var settingsType = SettingsType.ClassicScoreWindowPositionTop;
            var value = 158.26;
            configService.ClassicScoreWindowPositionTop = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithClassicScoreWindowHeight()
        {
            var settingsType = SettingsType.ClassicScoreWindowHeight;
            var value = 158.26;
            configService.ClassicScoreWindowHeight = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }

        [Test]
        public void DbServiceMethodCallsWithClassicScoreWindowWidth()
        {
            var settingsType = SettingsType.ClassicScoreWindowWidth;
            var value = 158.26;
            configService.ClassicScoreWindowWidth = value;

            configService.SaveSettings();

            dbService.Verify(m => m.SettingsSetValue(settingsType,
                                                     Common.Converter.ToString(value)));
        }
    }
}