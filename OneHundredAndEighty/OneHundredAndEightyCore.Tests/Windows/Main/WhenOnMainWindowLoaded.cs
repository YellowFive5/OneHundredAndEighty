#region Usings

using FluentAssertions;
using Moq;
using NUnit.Framework;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Windows.Main;

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main
{
    public class WhenOnMainWindowLoaded : MainWindowViewModelTestBase
    {
        protected override void Setup()
        {
            base.Setup();
            PlayersDataTableFromDb.Rows.Add(1, "doesNotMatter", "doesNotMatter", base64ImageString);
            PlayersDataTableFromDb.Rows.Add(2, "doesNotMatter", "doesNotMatter", base64ImageString);

            dbService.Setup(x => x.PlayersLoadAll()).Returns(PlayersDataTableFromDb);

            detectionService.Setup(x => x.FindConnectedCams()).Returns("[HBV HD CAMERA]-[ID:'8&2f223cfb']");
            detectionService.SetupAdd(m => m.OnErrorOccurred += e => { });
        }

        [Test]
        public void VersionIsChecked()
        {
            viewModel.OnMainWindowLoaded();

            versionChecker.Verify(v => v.CheckVersions(), Times.Once);
        }

        [Test]
        public void NewPlayerAvatarLoaded()
        {
            viewModel.NewPlayerAvatar = null;

            viewModel.OnMainWindowLoaded();

            viewModel.NewPlayerAvatar.Should().NotBeNull();
        }

        [Test]
        public void MainTabsIsEnabled()
        {
            viewModel.IsMainTabsEnabled = false;

            viewModel.OnMainWindowLoaded();

            viewModel.IsMainTabsEnabled.Should().BeTrue();
        }

        [Test]
        public void SetupTabsIsEnabled()
        {
            viewModel.IsSetupTabsEnabled = false;

            viewModel.OnMainWindowLoaded();

            viewModel.IsSetupTabsEnabled.Should().BeTrue();
        }

        [Test]
        public void NewGameSetsSetToDefault()
        {
            viewModel.NewGameSets = 99;

            viewModel.OnMainWindowLoaded();

            viewModel.NewGameSets.Should().Be(MainWindowViewModel.DefaultNewGameSetsValue);
        }

        [Test]
        public void NewGameLegsSetToDefault()
        {
            viewModel.NewGameLegs = 99;

            viewModel.OnMainWindowLoaded();

            viewModel.NewGameLegs.Should().Be(MainWindowViewModel.DefaultNewGameLegsValue);
        }

        [Test]
        public void NewGameTypeSetToDefault()
        {
            viewModel.NewGameType = GameType.FreeThrowsSingle;

            viewModel.OnMainWindowLoaded();

            viewModel.NewGameType.Should().Be(MainWindowViewModel.DefaultNewGameType);
        }

        [Test]
        public void NewGamePointsSetToDefault()
        {
            viewModel.NewGamePoints = GamePoints.Free;

            viewModel.OnMainWindowLoaded();

            viewModel.NewGamePoints.Should().Be(MainWindowViewModel.DefaultNewGamePoints);
        }

        [Test]
        public void MainWindowHeightSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.MainWindowHeight))).Returns(1555.55);
            viewModel.MainWindowHeight = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.MainWindowHeight.Should().Be(1555.55);
        }

        [Test]
        public void MainWindowWidthSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.MainWindowWidth))).Returns(755.88);
            viewModel.MainWindowWidth = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.MainWindowWidth.Should().Be(755.88);
        }

        [Test]
        public void MainWindowPositionTopSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.MainWindowPositionTop))).Returns(99.99);
            viewModel.MainWindowPositionTop = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.MainWindowPositionTop.Should().Be(99.99);
        }

        [Test]
        public void MainWindowPositionLeftSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.MainWindowPositionLeft))).Returns(133.03);
            viewModel.MainWindowPositionLeft = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.MainWindowPositionLeft.Should().Be(133.03);
        }

        [Test]
        public void Cam1ThresholdSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam1ThresholdSlider))).Returns(77.88);
            viewModel.Cam1ThresholdSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1ThresholdSliderValue.Should().Be(77.88);
        }

        [Test]
        public void Cam2ThresholdSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam2ThresholdSlider))).Returns(14.88);
            viewModel.Cam2ThresholdSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2ThresholdSliderValue.Should().Be(14.88);
        }

        [Test]
        public void Cam3ThresholdSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam3ThresholdSlider))).Returns(78.89);
            viewModel.Cam3ThresholdSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3ThresholdSliderValue.Should().Be(78.89);
        }

        [Test]
        public void Cam4ThresholdSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam4ThresholdSlider))).Returns(44.11);
            viewModel.Cam4ThresholdSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4ThresholdSliderValue.Should().Be(44.11);
        }

        [Test]
        public void Cam1SurfaceSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam1SurfaceSlider))).Returns(100.11);
            viewModel.Cam1SurfaceSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1SurfaceSliderValue.Should().Be(100.11);
        }

        [Test]
        public void Cam2SurfaceSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam2SurfaceSlider))).Returns(222.11);
            viewModel.Cam2SurfaceSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2SurfaceSliderValue.Should().Be(222.11);
        }

        [Test]
        public void Cam3SurfaceSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam3SurfaceSlider))).Returns(333.11);
            viewModel.Cam3SurfaceSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3SurfaceSliderValue.Should().Be(333.11);
        }

        [Test]
        public void Cam4SurfaceSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam4SurfaceSlider))).Returns(444.11);
            viewModel.Cam4SurfaceSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4SurfaceSliderValue.Should().Be(444.11);
        }

        [Test]
        public void Cam1SurfaceCenterSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam1SurfaceCenterSlider))).Returns(101.11);
            viewModel.Cam1SurfaceCenterSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1SurfaceCenterSliderValue.Should().Be(101.11);
        }

        [Test]
        public void Cam2SurfaceCenterSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam2SurfaceCenterSlider))).Returns(102.11);
            viewModel.Cam2SurfaceCenterSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2SurfaceCenterSliderValue.Should().Be(102.11);
        }

        [Test]
        public void Cam3SurfaceCenterSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam3SurfaceCenterSlider))).Returns(103.11);
            viewModel.Cam3SurfaceCenterSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3SurfaceCenterSliderValue.Should().Be(103.11);
        }

        [Test]
        public void Cam4SurfaceCenterSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam4SurfaceCenterSlider))).Returns(104.11);
            viewModel.Cam4SurfaceCenterSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4SurfaceCenterSliderValue.Should().Be(104.11);
        }

        [Test]
        public void Cam1RoiPosYSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam1RoiPosYSlider))).Returns(10.88);
            viewModel.Cam1RoiPosYSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1RoiPosYSliderValue.Should().Be(10.88);
        }

        [Test]
        public void Cam2RoiPosYSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam2RoiPosYSlider))).Returns(20.88);
            viewModel.Cam2RoiPosYSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2RoiPosYSliderValue.Should().Be(20.88);
        }

        [Test]
        public void Cam3RoiPosYSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam3RoiPosYSlider))).Returns(30.88);
            viewModel.Cam3RoiPosYSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3RoiPosYSliderValue.Should().Be(30.88);
        }

        [Test]
        public void Cam4RoiPosYSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam4RoiPosYSlider))).Returns(40.88);
            viewModel.Cam4RoiPosYSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4RoiPosYSliderValue.Should().Be(40.88);
        }

        [Test]
        public void Cam1RoiHeightSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam1RoiHeightSlider))).Returns(11.11);
            viewModel.Cam1RoiHeightSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1RoiHeightSliderValue.Should().Be(11.11);
        }

        [Test]
        public void Cam2RoiHeightSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam2RoiHeightSlider))).Returns(21.11);
            viewModel.Cam2RoiHeightSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2RoiHeightSliderValue.Should().Be(21.11);
        }

        [Test]
        public void Cam3RoiHeightSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam3RoiHeightSlider))).Returns(31.11);
            viewModel.Cam3RoiHeightSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3RoiHeightSliderValue.Should().Be(31.11);
        }

        [Test]
        public void Cam4RoiHeightSliderValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam4RoiHeightSlider))).Returns(41.11);
            viewModel.Cam4RoiHeightSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4RoiHeightSliderValue.Should().Be(41.11);
        }

        [Test]
        public void Cam1EnabledSets()
        {
            configService.Setup(x => x.Read<bool>(It.Is<SettingsType>(s => s == SettingsType.Cam1CheckBox))).Returns(true);
            viewModel.Cam1Enabled = false;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1Enabled.Should().Be(true);
        }

        [Test]
        public void Cam2EnabledSets()
        {
            configService.Setup(x => x.Read<bool>(It.Is<SettingsType>(s => s == SettingsType.Cam2CheckBox))).Returns(true);
            viewModel.Cam2Enabled = false;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2Enabled.Should().Be(true);
        }

        [Test]
        public void Cam3EnabledSets()
        {
            configService.Setup(x => x.Read<bool>(It.Is<SettingsType>(s => s == SettingsType.Cam3CheckBox))).Returns(true);
            viewModel.Cam3Enabled = false;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3Enabled.Should().Be(true);
        }

        [Test]
        public void Cam4EnabledSets()
        {
            configService.Setup(x => x.Read<bool>(It.Is<SettingsType>(s => s == SettingsType.Cam4CheckBox))).Returns(true);
            viewModel.Cam4Enabled = false;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4Enabled.Should().Be(true);
        }

        [Test]
        public void DetectionEnabledSets()
        {
            configService.Setup(x => x.Read<bool>(It.Is<SettingsType>(s => s == SettingsType.WithDetectionCheckBox))).Returns(true);
            viewModel.DetectionEnabled = false;

            viewModel.OnMainWindowLoaded();

            viewModel.DetectionEnabled.Should().Be(true);
        }

        [Test]
        public void Cam1IdSets()
        {
            configService.Setup(x => x.Read<string>(It.Is<SettingsType>(s => s == SettingsType.Cam1Id))).Returns("13G343");
            viewModel.Cam1Id = "trash";

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1Id.Should().Be("13G343");
        }

        [Test]
        public void Cam2IdSets()
        {
            configService.Setup(x => x.Read<string>(It.Is<SettingsType>(s => s == SettingsType.Cam2Id))).Returns("7644bf");
            viewModel.Cam2Id = "trash";

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2Id.Should().Be("7644bf");
        }

        [Test]
        public void Cam3IdSets()
        {
            configService.Setup(x => x.Read<string>(It.Is<SettingsType>(s => s == SettingsType.Cam3Id))).Returns("8uty67");
            viewModel.Cam3Id = "trash";

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3Id.Should().Be("8uty67");
        }

        [Test]
        public void Cam4IdSets()
        {
            configService.Setup(x => x.Read<string>(It.Is<SettingsType>(s => s == SettingsType.Cam4Id))).Returns("we32dg");
            viewModel.Cam4Id = "trash";

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4Id.Should().Be("we32dg");
        }

        [Test]
        public void CamsFovAngleSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.CamFovAngle))).Returns(75.8);
            viewModel.CamsFovAngle = 0.01;

            viewModel.OnMainWindowLoaded();

            viewModel.CamsFovAngle.Should().Be(75.8);
        }

        [Test]
        public void CamsResolutionWidthSets()
        {
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.ResolutionWidth))).Returns(1280);
            viewModel.CamsResolutionWidth = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.CamsResolutionWidth.Should().Be(1280);
        }

        [Test]
        public void CamsResolutionHeightSets()
        {
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.ResolutionHeight))).Returns(768);
            viewModel.CamsResolutionHeight = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.CamsResolutionHeight.Should().Be(768);
        }

        [Test]
        public void MovesExtractionValueSets()
        {
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.MovesExtraction))).Returns(5000);
            viewModel.MovesExtractionValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.MovesExtractionValue.Should().Be(5000);
        }

        [Test]
        public void MovesDetectedSleepTimeValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.MoveDetectedSleepTime))).Returns(0.25);
            viewModel.MovesDetectedSleepTimeValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.MovesDetectedSleepTimeValue.Should().Be(0.25);
        }

        [Test]
        public void MovesNoiseValueSets()
        {
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.MovesNoise))).Returns(1500);
            viewModel.MovesNoiseValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.MovesNoiseValue.Should().Be(1500);
        }

        [Test]
        public void SmoothGaussValueSets()
        {
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.SmoothGauss))).Returns(5);
            viewModel.SmoothGaussValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.SmoothGaussValue.Should().Be(5);
        }

        [Test]
        public void ThresholdSleepTimeValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.ThresholdSleepTime))).Returns(0.55);
            viewModel.ThresholdSleepTimeValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.ThresholdSleepTimeValue.Should().Be(0.55);
        }

        [Test]
        public void ExtractionSleepTimeValueSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.ExtractionSleepTime))).Returns(0.88);
            viewModel.ExtractionSleepTimeValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.ExtractionSleepTimeValue.Should().Be(0.88);
        }

        [Test]
        public void MinContourArcValueSets()
        {
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.MinContourArc))).Returns(500);
            viewModel.MinContourArcValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.MinContourArcValue.Should().Be(500);
        }

        [Test]
        public void MovesDartValueSets()
        {
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.MovesDart))).Returns(700);
            viewModel.MovesDartValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.MovesDartValue.Should().Be(700);
        }

        [Test]
        public void ToCam1DistanceSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.ToCam1Distance))).Returns(15.5);
            viewModel.ToCam1Distance = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.ToCam1Distance.Should().Be(15.5);
        }

        [Test]
        public void ToCam2DistanceSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.ToCam2Distance))).Returns(25.5);
            viewModel.ToCam2Distance = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.ToCam2Distance.Should().Be(25.5);
        }

        [Test]
        public void ToCam3DistanceSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.ToCam3Distance))).Returns(35.5);
            viewModel.ToCam3Distance = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.ToCam3Distance.Should().Be(35.5);
        }

        [Test]
        public void ToCam4DistanceSets()
        {
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.ToCam4Distance))).Returns(45.5);
            viewModel.ToCam4Distance = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.ToCam4Distance.Should().Be(45.5);
        }

        [Test]
        public void Cam1SetupXValueSets()
        {
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.Cam1X))).Returns(100);
            viewModel.Cam1SetupXValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1SetupXValue.Should().Be(100);
        }

        [Test]
        public void Cam1SetupYValueSets()
        {
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.Cam1Y))).Returns(101);
            viewModel.Cam1SetupYValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1SetupYValue.Should().Be(101);
        }

        [Test]
        public void Cam2SetupXValueSets()
        {
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.Cam2X))).Returns(200);
            viewModel.Cam2SetupXValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2SetupXValue.Should().Be(200);
        }

        [Test]
        public void Cam2SetupYValueSets()
        {
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.Cam2Y))).Returns(201);
            viewModel.Cam2SetupYValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2SetupYValue.Should().Be(201);
        }

        [Test]
        public void Cam3SetupXValueSets()
        {
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.Cam3X))).Returns(300);
            viewModel.Cam3SetupXValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3SetupXValue.Should().Be(300);
        }

        [Test]
        public void Cam3SetupYValueSets()
        {
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.Cam3Y))).Returns(301);
            viewModel.Cam3SetupYValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3SetupYValue.Should().Be(301);
        }

        [Test]
        public void Cam4SetupXValueSets()
        {
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.Cam4X))).Returns(400);
            viewModel.Cam4SetupXValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4SetupXValue.Should().Be(400);
        }

        [Test]
        public void Cam4SetupYValueSets()
        {
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.Cam4Y))).Returns(401);
            viewModel.Cam4SetupYValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4SetupYValue.Should().Be(401);
        }

        [Test]
        public void Cam1SetupSectorSets()
        {
            configService.Setup(x => x.Read<string>(It.Is<SettingsType>(s => s == SettingsType.Cam1SetupSector))).Returns("5/20");
            viewModel.Cam1SetupSector = "trash";

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1SetupSector.Should().Be("5/20");
        }

        [Test]
        public void Cam2SetupSectorSets()
        {
            configService.Setup(x => x.Read<string>(It.Is<SettingsType>(s => s == SettingsType.Cam2SetupSector))).Returns("17/3");
            viewModel.Cam2SetupSector = "trash";

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2SetupSector.Should().Be("17/3");
        }

        [Test]
        public void Cam3SetupSectorSets()
        {
            configService.Setup(x => x.Read<string>(It.Is<SettingsType>(s => s == SettingsType.Cam3SetupSector))).Returns("3/19");
            viewModel.Cam3SetupSector = "trash";

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3SetupSector.Should().Be("3/19");
        }

        [Test]
        public void Cam4SetupSectorSets()
        {
            configService.Setup(x => x.Read<string>(It.Is<SettingsType>(s => s == SettingsType.Cam4SetupSector))).Returns("16/8");
            viewModel.Cam4SetupSector = "trash";

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4SetupSector.Should().Be("16/8");
        }

        [Test]
        public void PlayersSets()
        {
            viewModel.OnMainWindowLoaded();

            viewModel.Players.Should().NotBeNull();
        }

        [Test]
        public void CheckCamsBoxTextSets()
        {
            viewModel.OnMainWindowLoaded();

            viewModel.CheckCamsBoxText.Should().Be("[HBV HD CAMERA]-[ID:'8&2f223cfb']");
        }

        [Test]
        public void OnErrorOccurredSubscribed()
        {
            viewModel.OnMainWindowLoaded();

            detectionService.VerifyAdd(m => m.OnErrorOccurred += It.IsAny<DetectionService.ExceptionOccurredDelegate>(), Times.Once);
        }
    }
}