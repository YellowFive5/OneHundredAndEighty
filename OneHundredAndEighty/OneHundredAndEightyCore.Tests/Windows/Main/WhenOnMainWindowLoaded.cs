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

            configService = new Mock<IConfigService>();
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.MainWindowHeight))).Returns(1555.55);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.MainWindowWidth))).Returns(755.88);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.MainWindowPositionLeft))).Returns(100.01);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.MainWindowPositionTop))).Returns(99.99);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam1ThresholdSlider))).Returns(77.88);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam2ThresholdSlider))).Returns(14.88);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam3ThresholdSlider))).Returns(78.89);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam4ThresholdSlider))).Returns(44.11);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam1SurfaceSlider))).Returns(100.11);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam2SurfaceSlider))).Returns(222.11);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam3SurfaceSlider))).Returns(333.11);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam4SurfaceSlider))).Returns(444.11);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam1SurfaceCenterSlider))).Returns(101.11);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam2SurfaceCenterSlider))).Returns(102.11);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam3SurfaceCenterSlider))).Returns(103.11);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam4SurfaceCenterSlider))).Returns(104.11);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam1RoiPosYSlider))).Returns(10.88);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam2RoiPosYSlider))).Returns(20.88);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam3RoiPosYSlider))).Returns(30.88);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam4RoiPosYSlider))).Returns(40.88);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam1RoiHeightSlider))).Returns(11.11);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam2RoiHeightSlider))).Returns(21.11);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam3RoiHeightSlider))).Returns(31.11);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.Cam4RoiHeightSlider))).Returns(41.11);
            configService.Setup(x => x.Read<bool>(It.Is<SettingsType>(s => s == SettingsType.Cam1CheckBox))).Returns(true);
            configService.Setup(x => x.Read<bool>(It.Is<SettingsType>(s => s == SettingsType.Cam2CheckBox))).Returns(true);
            configService.Setup(x => x.Read<bool>(It.Is<SettingsType>(s => s == SettingsType.Cam3CheckBox))).Returns(true);
            configService.Setup(x => x.Read<bool>(It.Is<SettingsType>(s => s == SettingsType.Cam4CheckBox))).Returns(true);
            configService.Setup(x => x.Read<bool>(It.Is<SettingsType>(s => s == SettingsType.WithDetectionCheckBox))).Returns(true);
            configService.Setup(x => x.Read<string>(It.Is<SettingsType>(s => s == SettingsType.Cam1Id))).Returns("13G343");
            configService.Setup(x => x.Read<string>(It.Is<SettingsType>(s => s == SettingsType.Cam2Id))).Returns("7644bf");
            configService.Setup(x => x.Read<string>(It.Is<SettingsType>(s => s == SettingsType.Cam3Id))).Returns("8uty67");
            configService.Setup(x => x.Read<string>(It.Is<SettingsType>(s => s == SettingsType.Cam4Id))).Returns("we32dg");
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.CamFovAngle))).Returns(75.8);
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.ResolutionWidth))).Returns(1280);
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.ResolutionHeight))).Returns(768);
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.MovesExtraction))).Returns(5000);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.MoveDetectedSleepTime))).Returns(0.25);
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.MovesNoise))).Returns(1500);
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.SmoothGauss))).Returns(5);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.ThresholdSleepTime))).Returns(0.55);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.ExtractionSleepTime))).Returns(0.88);
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.MinContourArc))).Returns(500);
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.MovesDart))).Returns(700);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.ToCam1Distance))).Returns(15.5);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.ToCam2Distance))).Returns(25.5);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.ToCam3Distance))).Returns(35.5);
            configService.Setup(x => x.Read<double>(It.Is<SettingsType>(s => s == SettingsType.ToCam4Distance))).Returns(45.5);
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.Cam1X))).Returns(100);
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.Cam1Y))).Returns(101);
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.Cam2X))).Returns(200);
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.Cam2Y))).Returns(201);
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.Cam3X))).Returns(300);
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.Cam3Y))).Returns(301);
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.Cam4X))).Returns(400);
            configService.Setup(x => x.Read<int>(It.Is<SettingsType>(s => s == SettingsType.Cam4Y))).Returns(401);
            configService.Setup(x => x.Read<string>(It.Is<SettingsType>(s => s == SettingsType.Cam1SetupSector))).Returns("5/20");
            configService.Setup(x => x.Read<string>(It.Is<SettingsType>(s => s == SettingsType.Cam2SetupSector))).Returns("17/3");
            configService.Setup(x => x.Read<string>(It.Is<SettingsType>(s => s == SettingsType.Cam3SetupSector))).Returns("3/19");
            configService.Setup(x => x.Read<string>(It.Is<SettingsType>(s => s == SettingsType.Cam4SetupSector))).Returns("16/8");

            PlayersDataTableFromDb.Rows.Add(1, "doesNotMatter", "doesNotMatter", base64ImageString);
            PlayersDataTableFromDb.Rows.Add(2, "doesNotMatter", "doesNotMatter", base64ImageString);

            dbService = new Mock<IDBService>();
            dbService.Setup(x => x.PlayersLoadAll()).Returns(PlayersDataTableFromDb);

            detectionService.Setup(x => x.FindConnectedCams()).Returns("[HBV HD CAMERA]-[ID:'8&2f223cfb']");
            detectionService.SetupAdd(m => m.OnErrorOccurred += e => { });

            viewModel = new MainWindowViewModel(null,
                                                null,
                                                dbService.Object,
                                                versionChecker.Object,
                                                null,
                                                null,
                                                null,
                                                detectionService.Object,
                                                null,
                                                null,
                                                configService.Object,
                                                null);
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
            viewModel.MainWindowHeight = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.MainWindowHeight.Should().Be(1555.55);
        }

        [Test]
        public void MainWindowWidthSets()
        {
            viewModel.MainWindowWidth = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.MainWindowWidth.Should().Be(755.88);
        }

        [Test]
        public void MainWindowPositionTopSets()
        {
            viewModel.MainWindowPositionTop = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.MainWindowPositionTop.Should().Be(99.99);
        }

        [Test]
        public void Cam1ThresholdSliderValueSets()
        {
            viewModel.Cam1ThresholdSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1ThresholdSliderValue.Should().Be(77.88);
        }

        [Test]
        public void Cam2ThresholdSliderValueSets()
        {
            viewModel.Cam2ThresholdSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2ThresholdSliderValue.Should().Be(14.88);
        }

        [Test]
        public void Cam3ThresholdSliderValueSets()
        {
            viewModel.Cam3ThresholdSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3ThresholdSliderValue.Should().Be(78.89);
        }

        [Test]
        public void Cam4ThresholdSliderValueSets()
        {
            viewModel.Cam4ThresholdSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4ThresholdSliderValue.Should().Be(44.11);
        }

        [Test]
        public void Cam1SurfaceSliderValueSets()
        {
            viewModel.Cam1SurfaceSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1SurfaceSliderValue.Should().Be(100.11);
        }

        [Test]
        public void Cam2SurfaceSliderValueSets()
        {
            viewModel.Cam2SurfaceSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2SurfaceSliderValue.Should().Be(222.11);
        }

        [Test]
        public void Cam3SurfaceSliderValueSets()
        {
            viewModel.Cam3SurfaceSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3SurfaceSliderValue.Should().Be(333.11);
        }

        [Test]
        public void Cam4SurfaceSliderValueSets()
        {
            viewModel.Cam4SurfaceSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4SurfaceSliderValue.Should().Be(444.11);
        }

        [Test]
        public void Cam1SurfaceCenterSliderValueSets()
        {
            viewModel.Cam1SurfaceCenterSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1SurfaceCenterSliderValue.Should().Be(101.11);
        }

        [Test]
        public void Cam2SurfaceCenterSliderValueSets()
        {
            viewModel.Cam2SurfaceCenterSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2SurfaceCenterSliderValue.Should().Be(102.11);
        }

        [Test]
        public void Cam3SurfaceCenterSliderValueSets()
        {
            viewModel.Cam3SurfaceCenterSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3SurfaceCenterSliderValue.Should().Be(103.11);
        }

        [Test]
        public void Cam4SurfaceCenterSliderValueSets()
        {
            viewModel.Cam4SurfaceCenterSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4SurfaceCenterSliderValue.Should().Be(104.11);
        }

        [Test]
        public void Cam1RoiPosYSliderValueSets()
        {
            viewModel.Cam1RoiPosYSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1RoiPosYSliderValue.Should().Be(10.88);
        }

        [Test]
        public void Cam2RoiPosYSliderValueSets()
        {
            viewModel.Cam2RoiPosYSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2RoiPosYSliderValue.Should().Be(20.88);
        }

        [Test]
        public void Cam3RoiPosYSliderValueSets()
        {
            viewModel.Cam3RoiPosYSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3RoiPosYSliderValue.Should().Be(30.88);
        }

        [Test]
        public void Cam4RoiPosYSliderValueSets()
        {
            viewModel.Cam4RoiPosYSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4RoiPosYSliderValue.Should().Be(40.88);
        }

        [Test]
        public void Cam1RoiHeightSliderValueSets()
        {
            viewModel.Cam1RoiHeightSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1RoiHeightSliderValue.Should().Be(11.11);
        }

        [Test]
        public void Cam2RoiHeightSliderValueSets()
        {
            viewModel.Cam2RoiHeightSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2RoiHeightSliderValue.Should().Be(21.11);
        }

        [Test]
        public void Cam3RoiHeightSliderValueSets()
        {
            viewModel.Cam3RoiHeightSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3RoiHeightSliderValue.Should().Be(31.11);
        }

        [Test]
        public void Cam4RoiHeightSliderValueSets()
        {
            viewModel.Cam4RoiHeightSliderValue = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4RoiHeightSliderValue.Should().Be(41.11);
        }

        [Test]
        public void Cam1EnabledSets()
        {
            viewModel.Cam1Enabled = false;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1Enabled.Should().Be(true);
        }

        [Test]
        public void Cam2EnabledSets()
        {
            viewModel.Cam2Enabled = false;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2Enabled.Should().Be(true);
        }

        [Test]
        public void Cam3EnabledSets()
        {
            viewModel.Cam3Enabled = false;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3Enabled.Should().Be(true);
        }

        [Test]
        public void Cam4EnabledSets()
        {
            viewModel.Cam4Enabled = false;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4Enabled.Should().Be(true);
        }

        [Test]
        public void DetectionEnabledSets()
        {
            viewModel.DetectionEnabled = false;

            viewModel.OnMainWindowLoaded();

            viewModel.DetectionEnabled.Should().Be(true);
        }

        [Test]
        public void Cam1IdSets()
        {
            viewModel.Cam1Id = "trash";

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1Id.Should().Be("13G343");
        }

        [Test]
        public void Cam2IdSets()
        {
            viewModel.Cam2Id = "trash";

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2Id.Should().Be("7644bf");
        }

        [Test]
        public void Cam3IdSets()
        {
            viewModel.Cam3Id = "trash";

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3Id.Should().Be("8uty67");
        }

        [Test]
        public void Cam4IdSets()
        {
            viewModel.Cam4Id = "trash";

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4Id.Should().Be("we32dg");
        }

        [Test]
        public void CamsFovAngleSets()
        {
            viewModel.CamsFovAngle = 0.01;

            viewModel.OnMainWindowLoaded();

            viewModel.CamsFovAngle.Should().Be(75.8);
        }

        [Test]
        public void CamsResolutionWidthSets()
        {
            viewModel.CamsResolutionWidth = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.CamsResolutionWidth.Should().Be(1280);
        }

        [Test]
        public void CamsResolutionHeightSets()
        {
            viewModel.CamsResolutionHeight = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.CamsResolutionHeight.Should().Be(768);
        }

        [Test]
        public void MovesExtractionValueSets()
        {
            viewModel.MovesExtractionValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.MovesExtractionValue.Should().Be(5000);
        }

        [Test]
        public void MovesDetectedSleepTimeValueSets()
        {
            viewModel.MovesDetectedSleepTimeValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.MovesDetectedSleepTimeValue.Should().Be(0.25);
        }

        [Test]
        public void MovesNoiseValueSets()
        {
            viewModel.MovesNoiseValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.MovesNoiseValue.Should().Be(1500);
        }

        [Test]
        public void SmoothGaussValueSets()
        {
            viewModel.SmoothGaussValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.SmoothGaussValue.Should().Be(5);
        }

        [Test]
        public void ThresholdSleepTimeValueSets()
        {
            viewModel.ThresholdSleepTimeValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.ThresholdSleepTimeValue.Should().Be(0.55);
        }

        [Test]
        public void ExtractionSleepTimeValueSets()
        {
            viewModel.ExtractionSleepTimeValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.ExtractionSleepTimeValue.Should().Be(0.88);
        }

        [Test]
        public void MinContourArcValueSets()
        {
            viewModel.MinContourArcValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.MinContourArcValue.Should().Be(500);
        }

        [Test]
        public void MovesDartValueSets()
        {
            viewModel.MovesDartValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.MovesDartValue.Should().Be(700);
        }

        [Test]
        public void ToCam1DistanceSets()
        {
            viewModel.ToCam1Distance = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.ToCam1Distance.Should().Be(15.5);
        }

        [Test]
        public void ToCam2DistanceSets()
        {
            viewModel.ToCam2Distance = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.ToCam2Distance.Should().Be(25.5);
        }

        [Test]
        public void ToCam3DistanceSets()
        {
            viewModel.ToCam3Distance = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.ToCam3Distance.Should().Be(35.5);
        }

        [Test]
        public void ToCam4DistanceSets()
        {
            viewModel.ToCam4Distance = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.ToCam4Distance.Should().Be(45.5);
        }

        [Test]
        public void Cam1SetupXValueSets()
        {
            viewModel.Cam1SetupXValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1SetupXValue.Should().Be(100);
        }

        [Test]
        public void Cam1SetupYValueSets()
        {
            viewModel.Cam1SetupYValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1SetupYValue.Should().Be(101);
        }

        [Test]
        public void Cam2SetupXValueSets()
        {
            viewModel.Cam2SetupXValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2SetupXValue.Should().Be(200);
        }

        [Test]
        public void Cam2SetupYValueSets()
        {
            viewModel.Cam2SetupYValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2SetupYValue.Should().Be(201);
        }

        [Test]
        public void Cam3SetupXValueSets()
        {
            viewModel.Cam3SetupXValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3SetupXValue.Should().Be(300);
        }

        [Test]
        public void Cam3SetupYValueSets()
        {
            viewModel.Cam3SetupYValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3SetupYValue.Should().Be(301);
        }

        [Test]
        public void Cam4SetupXValueSets()
        {
            viewModel.Cam4SetupXValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4SetupXValue.Should().Be(400);
        }

        [Test]
        public void Cam4SetupYValueSets()
        {
            viewModel.Cam4SetupYValue = 0;

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4SetupYValue.Should().Be(401);
        }

        [Test]
        public void Cam1SetupSectorSets()
        {
            viewModel.Cam1SetupSector = "trash";

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1SetupSector.Should().Be("5/20");
        }

        [Test]
        public void Cam2SetupSectorSets()
        {
            viewModel.Cam2SetupSector = "trash";

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2SetupSector.Should().Be("17/3");
        }

        [Test]
        public void Cam3SetupSectorSets()
        {
            viewModel.Cam3SetupSector = "trash";

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3SetupSector.Should().Be("3/19");
        }

        [Test]
        public void Cam4SetupSectorSets()
        {
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