#region Usings

using FluentAssertions;
using Moq;
using NUnit.Framework;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Tests.ConfigService
{
    public class WhenLoadingSettings : ConfigServiceTestBase
    {
        [Test]
        public void All_77_ConfigItemsLoaded()
        {
            configService.LoadSettings();

            dbService.Verify(m => m.SettingsGetValue(It.IsAny<SettingsType>()),
                             Times.Exactly(77));
        }

        [Test]
        public void DbVersionLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.DBVersion))).Returns("2.2");

            configService.LoadSettings();

            configService.DbVersion.Should().Be(2.2);
        }

        [Test]
        public void MainWindowPositionLeftLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.MainWindowPositionLeft))).Returns("356.56");
            configService.MainWindowPositionLeft = 1.0;

            configService.LoadSettings();

            configService.MainWindowPositionLeft.Should().Be(356.56);
        }

        [Test]
        public void MainWindowPositionTopLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.MainWindowPositionTop))).Returns("356.56");
            configService.MainWindowPositionTop = 1.0;

            configService.LoadSettings();

            configService.MainWindowPositionTop.Should().Be(356.56);
        }

        [Test]
        public void MainWindowHeightLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.MainWindowHeight))).Returns("356.56");
            configService.MainWindowHeight = 1.0;

            configService.LoadSettings();

            configService.MainWindowHeight.Should().Be(356.56);
        }

        [Test]
        public void MainWindowWidthLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.MainWindowWidth))).Returns("356.56");
            configService.MainWindowWidth = 1.0;

            configService.LoadSettings();

            configService.MainWindowWidth.Should().Be(356.56);
        }

        [Test]
        public void Cam1ThresholdSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam1ThresholdSlider))).Returns("474.56");
            configService.Cam1ThresholdSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam1ThresholdSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam2ThresholdSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam2ThresholdSlider))).Returns("474.56");
            configService.Cam2ThresholdSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam2ThresholdSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam3ThresholdSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam3ThresholdSlider))).Returns("474.56");
            configService.Cam3ThresholdSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam3ThresholdSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam4ThresholdSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam4ThresholdSlider))).Returns("474.56");
            configService.Cam4ThresholdSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam4ThresholdSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam1RoiPosYSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam1RoiPosYSlider))).Returns("474.56");
            configService.Cam1RoiPosYSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam1RoiPosYSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam2RoiPosYSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam2RoiPosYSlider))).Returns("474.56");
            configService.Cam2RoiPosYSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam2RoiPosYSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam3RoiPosYSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam3RoiPosYSlider))).Returns("474.56");
            configService.Cam3RoiPosYSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam3RoiPosYSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam4RoiPosYSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam4RoiPosYSlider))).Returns("474.56");
            configService.Cam4RoiPosYSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam4RoiPosYSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam1RoiHeightSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam1RoiHeightSlider))).Returns("474.56");
            configService.Cam1RoiHeightSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam1RoiHeightSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam2RoiHeightSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam2RoiHeightSlider))).Returns("474.56");
            configService.Cam2RoiHeightSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam2RoiHeightSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam3RoiHeightSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam3RoiHeightSlider))).Returns("474.56");
            configService.Cam3RoiHeightSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam3RoiHeightSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam4RoiHeightSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam4RoiHeightSlider))).Returns("474.56");
            configService.Cam4RoiHeightSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam4RoiHeightSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam1SurfaceSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam1SurfaceSlider))).Returns("474.56");
            configService.Cam1SurfaceSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam1SurfaceSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam2SurfaceSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam2SurfaceSlider))).Returns("474.56");
            configService.Cam2SurfaceSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam2SurfaceSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam3SurfaceSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam3SurfaceSlider))).Returns("474.56");
            configService.Cam3SurfaceSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam3SurfaceSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam4SurfaceSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam4SurfaceSlider))).Returns("474.56");
            configService.Cam4SurfaceSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam4SurfaceSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam1SurfaceCenterSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam1SurfaceCenterSlider))).Returns("474.56");
            configService.Cam1SurfaceCenterSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam1SurfaceCenterSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam2SurfaceCenterSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam2SurfaceCenterSlider))).Returns("474.56");
            configService.Cam2SurfaceCenterSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam2SurfaceCenterSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam3SurfaceCenterSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam3SurfaceCenterSlider))).Returns("474.56");
            configService.Cam3SurfaceCenterSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam3SurfaceCenterSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam4SurfaceCenterSliderValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam4SurfaceCenterSlider))).Returns("474.56");
            configService.Cam4SurfaceCenterSliderValue = 1.0;

            configService.LoadSettings();

            configService.Cam4SurfaceCenterSliderValue.Should().Be(474.56);
        }

        [Test]
        public void Cam1XSetupValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam1X))).Returns("575");
            configService.Cam1XSetupValue = 1;

            configService.LoadSettings();

            configService.Cam1XSetupValue.Should().Be(575);
        }

        [Test]
        public void Cam2XSetupValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam2X))).Returns("575");
            configService.Cam2XSetupValue = 1;

            configService.LoadSettings();

            configService.Cam2XSetupValue.Should().Be(575);
        }

        [Test]
        public void Cam3XSetupValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam3X))).Returns("575");
            configService.Cam3XSetupValue = 1;

            configService.LoadSettings();

            configService.Cam3XSetupValue.Should().Be(575);
        }

        [Test]
        public void Cam4XSetupValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam4X))).Returns("575");
            configService.Cam4XSetupValue = 1;

            configService.LoadSettings();

            configService.Cam4XSetupValue.Should().Be(575);
        }

        [Test]
        public void Cam1YSetupValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam1Y))).Returns("575");
            configService.Cam1YSetupValue = 1;

            configService.LoadSettings();

            configService.Cam1YSetupValue.Should().Be(575);
        }

        [Test]
        public void Cam2YSetupValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam2Y))).Returns("575");
            configService.Cam2YSetupValue = 1;

            configService.LoadSettings();

            configService.Cam2YSetupValue.Should().Be(575);
        }

        [Test]
        public void Cam3YSetupValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam3Y))).Returns("575");
            configService.Cam3YSetupValue = 1;

            configService.LoadSettings();

            configService.Cam3YSetupValue.Should().Be(575);
        }

        [Test]
        public void Cam4YSetupValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam4Y))).Returns("575");
            configService.Cam4YSetupValue = 1;

            configService.LoadSettings();

            configService.Cam4YSetupValue.Should().Be(575);
        }

        [Test]
        public void CamsResolutionWidthLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.ResolutionWidth))).Returns("1920");
            configService.CamsResolutionWidth = 1;

            configService.LoadSettings();

            configService.CamsResolutionWidth.Should().Be(1920);
        }

        [Test]
        public void CamsResolutionHeightLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.ResolutionHeight))).Returns("1920");
            configService.CamsResolutionHeight = 1;

            configService.LoadSettings();

            configService.CamsResolutionHeight.Should().Be(1920);
        }

        [Test]
        public void MovesExtractionValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.MovesExtraction))).Returns("200");
            configService.MovesExtractionValue = 1;

            configService.LoadSettings();

            configService.MovesExtractionValue.Should().Be(200);
        }

        [Test]
        public void MovesDartValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.MovesDart))).Returns("200");
            configService.MovesDartValue = 1;

            configService.LoadSettings();

            configService.MovesDartValue.Should().Be(200);
        }

        [Test]
        public void MovesNoiseValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.MovesNoise))).Returns("200");
            configService.MovesNoiseValue = 1;

            configService.LoadSettings();

            configService.MovesNoiseValue.Should().Be(200);
        }

        [Test]
        public void SmoothGaussValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.SmoothGauss))).Returns("5");
            configService.SmoothGaussValue = 1;

            configService.LoadSettings();

            configService.SmoothGaussValue.Should().Be(5);
        }

        [Test]
        public void MinContourArcValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.MinContourArc))).Returns("150");
            configService.MinContourArcValue = 1;

            configService.LoadSettings();

            configService.MinContourArcValue.Should().Be(150);
        }

        [Test]
        public void CamsFovAngleLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.CamFovAngle))).Returns("75.5");
            configService.CamsFovAngle = 1;

            configService.LoadSettings();

            configService.CamsFovAngle.Should().Be(75.5);
        }

        [Test]
        public void Cam1IdLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam1Id))).Returns("r4rt54");
            configService.Cam1Id = "SomeTrash";

            configService.LoadSettings();

            configService.Cam1Id.Should().Be("r4rt54");
        }

        [Test]
        public void Cam2IdLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam2Id))).Returns("r4rt54");
            configService.Cam2Id = "SomeTrash";

            configService.LoadSettings();

            configService.Cam2Id.Should().Be("r4rt54");
        }

        [Test]
        public void Cam3IdLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam3Id))).Returns("r4rt54");
            configService.Cam3Id = "SomeTrash";

            configService.LoadSettings();

            configService.Cam3Id.Should().Be("r4rt54");
        }

        [Test]
        public void Cam4IdLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam4Id))).Returns("r4rt54");
            configService.Cam4Id = "SomeTrash";

            configService.LoadSettings();

            configService.Cam4Id.Should().Be("r4rt54");
        }

        [Test]
        public void ExtractionSleepTimeValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.ExtractionSleepTime))).Returns("0.25");
            configService.ExtractionSleepTimeValue = 0.1;

            configService.LoadSettings();

            configService.ExtractionSleepTimeValue.Should().Be(0.25);
        }

        [Test]
        public void ThresholdSleepTimeValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.ThresholdSleepTime))).Returns("0.25");
            configService.ThresholdSleepTimeValue = 0.1;

            configService.LoadSettings();

            configService.ThresholdSleepTimeValue.Should().Be(0.25);
        }

        [Test]
        public void MovesDetectedSleepTimeValueLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.MoveDetectedSleepTime))).Returns("0.25");
            configService.MovesDetectedSleepTimeValue = 0.1;

            configService.LoadSettings();

            configService.MovesDetectedSleepTimeValue.Should().Be(0.25);
        }

        [Test]
        public void DetectionEnabledLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.WithDetectionCheckBox))).Returns("True");
            configService.DetectionEnabled = false;

            configService.LoadSettings();

            configService.DetectionEnabled.Should().BeTrue();
        }

        [Test]
        public void CamsDetectionWindowPositionLeftLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.CamsDetectionWindowPositionLeft))).Returns("546.5");
            configService.CamsDetectionWindowPositionLeft = 1.0;

            configService.LoadSettings();

            configService.CamsDetectionWindowPositionLeft.Should().Be(546.5);
        }

        [Test]
        public void CamsDetectionWindowPositionTopLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.CamsDetectionWindowPositionTop))).Returns("546.5");
            configService.CamsDetectionWindowPositionTop = 1.0;

            configService.LoadSettings();

            configService.CamsDetectionWindowPositionTop.Should().Be(546.5);
        }

        [Test]
        public void CamsDetectionWindowHeightLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.CamsDetectionWindowHeight))).Returns("546.5");
            configService.CamsDetectionWindowHeight = 1.0;
            configService.LoadSettings();

            configService.CamsDetectionWindowHeight.Should().Be(546.5);
        }

        [Test]
        public void CamsDetectionWindowWidthLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.CamsDetectionWindowWidth))).Returns("546.5");
            configService.CamsDetectionWindowWidth = 1.0;

            configService.LoadSettings();

            configService.CamsDetectionWindowWidth.Should().Be(546.5);
        }

        [Test]
        public void Cam1EnabledLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam1CheckBox))).Returns("True");
            configService.Cam1Enabled = false;

            configService.LoadSettings();

            configService.Cam1Enabled.Should().BeTrue();
        }

        [Test]
        public void Cam2EnabledLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam2CheckBox))).Returns("True");
            configService.Cam2Enabled = false;

            configService.LoadSettings();

            configService.Cam2Enabled.Should().BeTrue();
        }

        [Test]
        public void Cam3EnabledLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam3CheckBox))).Returns("True");
            configService.Cam3Enabled = false;

            configService.LoadSettings();

            configService.Cam3Enabled.Should().BeTrue();
        }

        [Test]
        public void Cam4EnabledLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam4CheckBox))).Returns("True");
            configService.Cam4Enabled = false;

            configService.LoadSettings();

            configService.Cam4Enabled.Should().BeTrue();
        }

        [Test]
        public void ToCam1DistanceLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.ToCam1Distance))).Returns("35.5");
            configService.ToCam1Distance = 1.0;

            configService.LoadSettings();

            configService.ToCam1Distance.Should().Be(35.5);
        }

        [Test]
        public void ToCam2DistanceLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.ToCam2Distance))).Returns("35.5");
            configService.ToCam2Distance = 1.0;

            configService.LoadSettings();

            configService.ToCam2Distance.Should().Be(35.5);
        }

        [Test]
        public void ToCam3DistanceLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.ToCam3Distance))).Returns("35.5");
            configService.ToCam3Distance = 1.0;

            configService.LoadSettings();

            configService.ToCam3Distance.Should().Be(35.5);
        }

        [Test]
        public void ToCam4DistanceLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.ToCam4Distance))).Returns("35.5");
            configService.ToCam4Distance = 1.0;

            configService.LoadSettings();

            configService.ToCam4Distance.Should().Be(35.5);
        }

        [Test]
        public void Cam1SetupSectorLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam1SetupSector))).Returns("20/1");
            configService.Cam1SetupSector = "SomeTrash";

            configService.LoadSettings();

            configService.Cam1SetupSector.Should().Be("20/1");
        }

        [Test]
        public void Cam2SetupSectorLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam2SetupSector))).Returns("20/1");
            configService.Cam2SetupSector = "SomeTrash";

            configService.LoadSettings();

            configService.Cam2SetupSector.Should().Be("20/1");
        }

        [Test]
        public void Cam3SetupSectorLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam3SetupSector))).Returns("20/1");
            configService.Cam3SetupSector = "SomeTrash";

            configService.LoadSettings();

            configService.Cam3SetupSector.Should().Be("20/1");
        }

        [Test]
        public void Cam4SetupSectorLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam4SetupSector))).Returns("20/1");
            configService.Cam4SetupSector = "SomeTrash";

            configService.LoadSettings();

            configService.Cam4SetupSector.Should().Be("20/1");
        }

        [Test]
        public void FreeThrowsSingleScoreWindowPositionLeftLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.FreeThrowsSingleScoreWindowPositionLeft))).Returns("546.5");
            configService.FreeThrowsSingleScoreWindowPositionLeft = 1.0;

            configService.LoadSettings();

            configService.FreeThrowsSingleScoreWindowPositionLeft.Should().Be(546.5);
        }

        [Test]
        public void FreeThrowsSingleScoreWindowPositionTopLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.FreeThrowsSingleScoreWindowPositionTop))).Returns("546.5");
            configService.FreeThrowsSingleScoreWindowPositionTop = 1.0;

            configService.LoadSettings();

            configService.FreeThrowsSingleScoreWindowPositionTop.Should().Be(546.5);
        }

        [Test]
        public void FreeThrowsSingleScoreWindowHeightLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.FreeThrowsSingleScoreWindowHeight))).Returns("546.5");
            configService.FreeThrowsSingleScoreWindowHeight = 1.0;

            configService.LoadSettings();

            configService.FreeThrowsSingleScoreWindowHeight.Should().Be(546.5);
        }

        [Test]
        public void FreeThrowsSingleScoreWindowWidthLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.FreeThrowsSingleScoreWindowWidth))).Returns("546.5");
            configService.FreeThrowsSingleScoreWindowWidth = 1.0;

            configService.LoadSettings();

            configService.FreeThrowsSingleScoreWindowWidth.Should().Be(546.5);
        }

        [Test]
        public void FreeThrowsDoubleScoreWindowPositionLeftLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.FreeThrowsDoubleScoreWindowPositionLeft))).Returns("546.5");
            configService.FreeThrowsDoubleScoreWindowPositionLeft = 1.0;

            configService.LoadSettings();

            configService.FreeThrowsDoubleScoreWindowPositionLeft.Should().Be(546.5);
        }

        [Test]
        public void FreeThrowsDoubleScoreWindowPositionTopLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.FreeThrowsDoubleScoreWindowPositionTop))).Returns("546.5");
            configService.FreeThrowsDoubleScoreWindowPositionTop = 1.0;

            configService.LoadSettings();

            configService.FreeThrowsDoubleScoreWindowPositionTop.Should().Be(546.5);
        }

        [Test]
        public void FreeThrowsDoubleScoreWindowHeightLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.FreeThrowsDoubleScoreWindowHeight))).Returns("546.5");
            configService.FreeThrowsDoubleScoreWindowHeight = 1.0;

            configService.LoadSettings();

            configService.FreeThrowsDoubleScoreWindowHeight.Should().Be(546.5);
        }

        [Test]
        public void FreeThrowsDoubleScoreWindowWidthLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.FreeThrowsDoubleScoreWindowWidth))).Returns("546.5");
            configService.FreeThrowsDoubleScoreWindowWidth = 1.0;

            configService.LoadSettings();

            configService.FreeThrowsDoubleScoreWindowWidth.Should().Be(546.5);
        }

        [Test]
        public void ClassicScoreWindowPositionLeftLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.ClassicScoreWindowPositionLeft))).Returns("546.5");
            configService.ClassicScoreWindowPositionLeft = 1.0;

            configService.LoadSettings();

            configService.ClassicScoreWindowPositionLeft.Should().Be(546.5);
        }

        [Test]
        public void ClassicScoreWindowPositionTopLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.ClassicScoreWindowPositionTop))).Returns("546.5");
            configService.ClassicScoreWindowPositionTop = 1.0;

            configService.LoadSettings();

            configService.ClassicScoreWindowPositionTop.Should().Be(546.5);
        }

        [Test]
        public void ClassicScoreWindowHeightLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.ClassicScoreWindowHeight))).Returns("546.5");
            configService.ClassicScoreWindowHeight = 1.0;

            configService.LoadSettings();

            configService.ClassicScoreWindowHeight.Should().Be(546.5);
        }

        [Test]
        public void ClassicScoreWindowWidthLoaded()
        {
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.ClassicScoreWindowWidth))).Returns("546.5");
            configService.ClassicScoreWindowWidth = 1.0;

            configService.LoadSettings();

            configService.ClassicScoreWindowWidth.Should().Be(546.5);
        }
    }
}