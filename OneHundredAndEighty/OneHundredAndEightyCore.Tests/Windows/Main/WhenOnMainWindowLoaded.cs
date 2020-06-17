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

        private MainWindowViewModel CreateViewModel(IConfigService configService)
        {
            return new MainWindowViewModel(logger.Object,
                                           null,
                                           dbService.Object,
                                           versionChecker.Object,
                                           null,
                                           null,
                                           null,
                                           detectionService.Object,
                                           null,
                                           null,
                                           configService,
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
            configService.Object.MainWindowHeight = 1555.55;
            viewModel = CreateViewModel(configService.Object);
            viewModel.MainWindowHeight = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.MainWindowHeight.Should().Be(1555.55);
        }

        [Test]
        public void MainWindowWidthSets()
        {
            configService.Object.MainWindowWidth = 1555.55;
            viewModel = CreateViewModel(configService.Object);
            viewModel.MainWindowWidth = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.MainWindowWidth.Should().Be(1555.55);
        }

        [Test]
        public void MainWindowPositionLeftSets()
        {
            configService.Object.MainWindowPositionLeft = 1555.55;
            viewModel = CreateViewModel(configService.Object);
            viewModel.MainWindowPositionLeft = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.MainWindowPositionLeft.Should().Be(1555.55);
        }

        [Test]
        public void MainWindowPositionTopSets()
        {
            configService.Object.MainWindowPositionTop = 1555.55;
            viewModel = CreateViewModel(configService.Object);
            viewModel.MainWindowPositionTop = 0.999;

            viewModel.OnMainWindowLoaded();

            viewModel.MainWindowPositionTop.Should().Be(1555.55);
        }

        [Test]
        public void Cam1ThresholdSliderValueSets()
        {
            configService.Object.Cam1ThresholdSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1ThresholdSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam2ThresholdSliderValueSets()
        {
            configService.Object.Cam2ThresholdSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2ThresholdSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam3ThresholdSliderValueSets()
        {
            configService.Object.Cam3ThresholdSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3ThresholdSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam4ThresholdSliderValueSets()
        {
            configService.Object.Cam4ThresholdSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4ThresholdSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam1SurfaceSliderValueSets()
        {
            configService.Object.Cam1SurfaceSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1SurfaceSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam2SurfaceSliderValueSets()
        {
            configService.Object.Cam2SurfaceSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2SurfaceSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam3SurfaceSliderValueSets()
        {
            configService.Object.Cam3SurfaceSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3SurfaceSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam4SurfaceSliderValueSets()
        {
            configService.Object.Cam4SurfaceSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4SurfaceSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam1SurfaceCenterSliderValueSets()
        {
            configService.Object.Cam1SurfaceCenterSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1SurfaceCenterSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam2SurfaceCenterSliderValueSets()
        {
            configService.Object.Cam2SurfaceCenterSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2SurfaceCenterSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam3SurfaceCenterSliderValueSets()
        {
            configService.Object.Cam3SurfaceCenterSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3SurfaceCenterSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam4SurfaceCenterSliderValueSets()
        {
            configService.Object.Cam4SurfaceCenterSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4SurfaceCenterSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam1RoiPosYSliderValueSets()
        {
            configService.Object.Cam1RoiPosYSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1RoiPosYSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam2RoiPosYSliderValueSets()
        {
            configService.Object.Cam2RoiPosYSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2RoiPosYSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam3RoiPosYSliderValueSets()
        {
            configService.Object.Cam3RoiPosYSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3RoiPosYSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam4RoiPosYSliderValueSets()
        {
            configService.Object.Cam4RoiPosYSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4RoiPosYSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam1RoiHeightSliderValueSets()
        {
            configService.Object.Cam1RoiHeightSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1RoiHeightSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam2RoiHeightSliderValueSets()
        {
            configService.Object.Cam2RoiHeightSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2RoiHeightSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam3RoiHeightSliderValueSets()
        {
            configService.Object.Cam3RoiHeightSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3RoiHeightSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam4RoiHeightSliderValueSets()
        {
            configService.Object.Cam4RoiHeightSliderValue = 1555.55;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4RoiHeightSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam1EnabledSets()
        {
            configService.Object.Cam1Enabled = true;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1Enabled.Should().BeTrue();
        }

        [Test]
        public void Cam2EnabledSets()
        {
            configService.Object.Cam2Enabled = true;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2Enabled.Should().BeTrue();
        }

        [Test]
        public void Cam3EnabledSets()
        {
            configService.Object.Cam3Enabled = true;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3Enabled.Should().BeTrue();
        }

        [Test]
        public void Cam4EnabledSets()
        {
            configService.Object.Cam4Enabled = true;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4Enabled.Should().BeTrue();
        }

        [Test]
        public void DetectionEnabledSets()
        {
            configService.Object.DetectionEnabled = true;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.DetectionEnabled.Should().BeTrue();
        }

        [Test]
        public void Cam1IdSets()
        {
            configService.Object.Cam1Id = "34rre5";
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1Id.Should().Be("34rre5");
        }

        [Test]
        public void Cam2IdSets()
        {
            configService.Object.Cam2Id = "34rre5";
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2Id.Should().Be("34rre5");
        }

        [Test]
        public void Cam3IdSets()
        {
            configService.Object.Cam3Id = "34rre5";
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3Id.Should().Be("34rre5");
        }

        [Test]
        public void Cam4IdSets()
        {
            configService.Object.Cam4Id = "34rre5";
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4Id.Should().Be("34rre5");
        }

        [Test]
        public void CamsFovAngleSets()
        {
            configService.Object.CamsFovAngle = 75.5;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.CamsFovAngle.Should().Be(75.5);
        }

        [Test]
        public void CamsResolutionWidthSets()
        {
            configService.Object.CamsResolutionWidth = 1920;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.CamsResolutionWidth.Should().Be(1920);
        }

        [Test]
        public void CamsResolutionHeightSets()
        {
            configService.Object.CamsResolutionHeight = 1028;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.CamsResolutionHeight.Should().Be(1028);
        }

        [Test]
        public void MovesExtractionValueSets()
        {
            configService.Object.MovesExtractionValue = 500;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.MovesExtractionValue.Should().Be(500);
        }

        [Test]
        public void MovesDetectedSleepTimeValueSets()
        {
            configService.Object.MovesDetectedSleepTimeValue = 0.25;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.MovesDetectedSleepTimeValue.Should().Be(0.25);
        }

        [Test]
        public void MovesNoiseValueSets()
        {
            configService.Object.MovesNoiseValue = 200;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.MovesNoiseValue.Should().Be(200);
        }

        [Test]
        public void SmoothGaussValueSets()
        {
            configService.Object.SmoothGaussValue = 5;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.SmoothGaussValue.Should().Be(5);
        }

        [Test]
        public void ThresholdSleepTimeValueSets()
        {
            configService.Object.ThresholdSleepTimeValue = 5.5;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.ThresholdSleepTimeValue.Should().Be(5.5);
        }

        [Test]
        public void ExtractionSleepTimeValueSets()
        {
            configService.Object.ExtractionSleepTimeValue = 8.5;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.ExtractionSleepTimeValue.Should().Be(8.5);
        }

        [Test]
        public void MinContourArcValueSets()
        {
            configService.Object.MinContourArcValue = 100;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.MinContourArcValue.Should().Be(100);
        }

        [Test]
        public void MovesDartValueSets()
        {
            configService.Object.MovesDartValue = 300;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.MovesDartValue.Should().Be(300);
        }

        [Test]
        public void ToCam1DistanceSets()
        {
            configService.Object.ToCam1Distance = 35.5;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.ToCam1Distance.Should().Be(35.5);
        }

        [Test]
        public void ToCam2DistanceSets()
        {
            configService.Object.ToCam2Distance = 35.5;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.ToCam2Distance.Should().Be(35.5);
        }

        [Test]
        public void ToCam3DistanceSets()
        {
            configService.Object.ToCam3Distance = 35.5;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.ToCam3Distance.Should().Be(35.5);
        }

        [Test]
        public void ToCam4DistanceSets()
        {
            configService.Object.ToCam4Distance = 35.5;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.ToCam4Distance.Should().Be(35.5);
        }

        [Test]
        public void Cam1XSetupValueSets()
        {
            configService.Object.Cam1XSetupValue = 579;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1XSetupValue.Should().Be(579);
        }

        [Test]
        public void Cam1YSetupValueSets()
        {
            configService.Object.Cam1YSetupValue = 579;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1YSetupValue.Should().Be(579);
        }

        [Test]
        public void Cam2XSetupValueSets()
        {
            configService.Object.Cam2XSetupValue = 579;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2XSetupValue.Should().Be(579);
        }

        [Test]
        public void Cam2YSetupValueSets()
        {
            configService.Object.Cam2YSetupValue = 579;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2YSetupValue.Should().Be(579);
        }

        [Test]
        public void Cam3XSetupValueSets()
        {
            configService.Object.Cam3XSetupValue = 579;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3XSetupValue.Should().Be(579);
        }

        [Test]
        public void Cam3YSetupValueSets()
        {
            configService.Object.Cam3YSetupValue = 579;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3YSetupValue.Should().Be(579);
        }

        [Test]
        public void Cam4XSetupValueSets()
        {
            configService.Object.Cam4XSetupValue = 579;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4XSetupValue.Should().Be(579);
        }

        [Test]
        public void Cam4YSetupValueSets()
        {
            configService.Object.Cam4YSetupValue = 579;
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4YSetupValue.Should().Be(579);
        }

        [Test]
        public void Cam1SetupSectorSets()
        {
            configService.Object.Cam1SetupSector = "20/1";
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam1SetupSector.Should().Be("20/1");
        }

        [Test]
        public void Cam2SetupSectorSets()
        {
            configService.Object.Cam2SetupSector = "20/1";
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam2SetupSector.Should().Be("20/1");
        }

        [Test]
        public void Cam3SetupSectorSets()
        {
            configService.Object.Cam3SetupSector = "20/1";
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam3SetupSector.Should().Be("20/1");
        }

        [Test]
        public void Cam4SetupSectorSets()
        {
            configService.Object.Cam4SetupSector = "20/1";
            viewModel = CreateViewModel(configService.Object);

            viewModel.OnMainWindowLoaded();

            viewModel.Cam4SetupSector.Should().Be("20/1");
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