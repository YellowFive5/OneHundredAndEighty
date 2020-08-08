#region Usings

using FluentAssertions;
using NUnit.Framework;

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main.Tabs.Settings
{
    public class WhenLoadingSettings : SettingsTabViewModelTestBase
    {
        [Test]
        public void Cam1ThresholdSliderValueSets()
        {
            ConfigServiceMock.Object.Cam1ThresholdSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam1ThresholdSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam2ThresholdSliderValueSets()
        {
            ConfigServiceMock.Object.Cam2ThresholdSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam2ThresholdSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam3ThresholdSliderValueSets()
        {
            ConfigServiceMock.Object.Cam3ThresholdSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam3ThresholdSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam4ThresholdSliderValueSets()
        {
            ConfigServiceMock.Object.Cam4ThresholdSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam4ThresholdSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam1SurfaceSliderValueSets()
        {
            ConfigServiceMock.Object.Cam1SurfaceSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam1SurfaceSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam2SurfaceSliderValueSets()
        {
            ConfigServiceMock.Object.Cam2SurfaceSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam2SurfaceSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam3SurfaceSliderValueSets()
        {
            ConfigServiceMock.Object.Cam3SurfaceSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam3SurfaceSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam4SurfaceSliderValueSets()
        {
            ConfigServiceMock.Object.Cam4SurfaceSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam4SurfaceSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam1SurfaceCenterSliderValueSets()
        {
            ConfigServiceMock.Object.Cam1SurfaceCenterSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam1SurfaceCenterSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam2SurfaceCenterSliderValueSets()
        {
            ConfigServiceMock.Object.Cam2SurfaceCenterSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam2SurfaceCenterSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam3SurfaceCenterSliderValueSets()
        {
            ConfigServiceMock.Object.Cam3SurfaceCenterSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam3SurfaceCenterSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam4SurfaceCenterSliderValueSets()
        {
            ConfigServiceMock.Object.Cam4SurfaceCenterSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam4SurfaceCenterSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam1RoiPosYSliderValueSets()
        {
            ConfigServiceMock.Object.Cam1RoiPosYSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam1RoiPosYSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam2RoiPosYSliderValueSets()
        {
            ConfigServiceMock.Object.Cam2RoiPosYSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam2RoiPosYSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam3RoiPosYSliderValueSets()
        {
            ConfigServiceMock.Object.Cam3RoiPosYSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam3RoiPosYSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam4RoiPosYSliderValueSets()
        {
            ConfigServiceMock.Object.Cam4RoiPosYSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam4RoiPosYSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam1RoiHeightSliderValueSets()
        {
            ConfigServiceMock.Object.Cam1RoiHeightSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam1RoiHeightSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam2RoiHeightSliderValueSets()
        {
            ConfigServiceMock.Object.Cam2RoiHeightSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam2RoiHeightSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam3RoiHeightSliderValueSets()
        {
            ConfigServiceMock.Object.Cam3RoiHeightSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam3RoiHeightSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam4RoiHeightSliderValueSets()
        {
            ConfigServiceMock.Object.Cam4RoiHeightSliderValue = 1555.55;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam4RoiHeightSliderValue.Should().Be(1555.55);
        }

        [Test]
        public void Cam1EnabledSets()
        {
            ConfigServiceMock.Object.Cam1Enabled = true;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam1Enabled.Should().BeTrue();
        }

        [Test]
        public void Cam2EnabledSets()
        {
            ConfigServiceMock.Object.Cam2Enabled = true;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam2Enabled.Should().BeTrue();
        }

        [Test]
        public void Cam3EnabledSets()
        {
            ConfigServiceMock.Object.Cam3Enabled = true;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam3Enabled.Should().BeTrue();
        }

        [Test]
        public void Cam4EnabledSets()
        {
            ConfigServiceMock.Object.Cam4Enabled = true;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam4Enabled.Should().BeTrue();
        }

        [Test]
        public void DetectionEnabledSets()
        {
            ConfigServiceMock.Object.DetectionEnabled = true;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.DetectionEnabled.Should().BeTrue();
        }

        [Test]
        public void Cam1IdSets()
        {
            ConfigServiceMock.Object.Cam1Id = "34rre5";

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam1Id.Should().Be("34rre5");
        }

        [Test]
        public void Cam2IdSets()
        {
            ConfigServiceMock.Object.Cam2Id = "34rre5";

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam2Id.Should().Be("34rre5");
        }

        [Test]
        public void Cam3IdSets()
        {
            ConfigServiceMock.Object.Cam3Id = "34rre5";

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam3Id.Should().Be("34rre5");
        }

        [Test]
        public void Cam4IdSets()
        {
            ConfigServiceMock.Object.Cam4Id = "34rre5";

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam4Id.Should().Be("34rre5");
        }

        [Test]
        public void CamsFovAngleSets()
        {
            ConfigServiceMock.Object.CamsFovAngle = 75.5;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.CamsFovAngle.Should().Be(75.5);
        }

        [Test]
        public void CamsResolutionWidthSets()
        {
            ConfigServiceMock.Object.CamsResolutionWidth = 1920;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.CamsResolutionWidth.Should().Be(1920);
        }

        [Test]
        public void CamsResolutionHeightSets()
        {
            ConfigServiceMock.Object.CamsResolutionHeight = 1028;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.CamsResolutionHeight.Should().Be(1028);
        }


        [Test]
        public void MovesDetectedSleepTimeValueSets()
        {
            ConfigServiceMock.Object.MovesDetectedSleepTimeValue = 0.25;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.MovesDetectedSleepTimeValue.Should().Be(0.25);
        }

        [Test]
        public void SmoothGaussValueSets()
        {
            ConfigServiceMock.Object.SmoothGaussValue = 5;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.SmoothGaussValue.Should().Be(5);
        }

        [Test]
        public void ThresholdSleepTimeValueSets()
        {
            ConfigServiceMock.Object.ThresholdSleepTimeValue = 5.5;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.ThresholdSleepTimeValue.Should().Be(5.5);
        }

        [Test]
        public void ExtractionSleepTimeValueSets()
        {
            ConfigServiceMock.Object.ExtractionSleepTimeValue = 8.5;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.ExtractionSleepTimeValue.Should().Be(8.5);
        }

        [Test]
        public void MinContourArcValueSets()
        {
            ConfigServiceMock.Object.MinContourArcValue = 100;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.MinContourArcValue.Should().Be(100);
        }

        [Test]
        public void MaxContourArcValueSets()
        {
            ConfigServiceMock.Object.MaxContourArcValue = 100;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.MaxContourArcValue.Should().Be(100);
        }

        [Test]
        public void MaxContourAreaValueSets()
        {
            ConfigServiceMock.Object.MaxContourAreaValue = 100;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.MaxContourAreaValue.Should().Be(100);
        }

        [Test]
        public void MinContourAreaValueSets()
        {
            ConfigServiceMock.Object.MinContourAreaValue = 100;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.MinContourAreaValue.Should().Be(100);
        }

        [Test]
        public void MinContourWidthValueSets()
        {
            ConfigServiceMock.Object.MinContourWidthValue = 100;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.MinContourWidthValue.Should().Be(100);
        }

        [Test]
        public void MaxContourWidthValueSets()
        {
            ConfigServiceMock.Object.MaxContourWidthValue = 100;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.MaxContourWidthValue.Should().Be(100);
        }

        [Test]
        public void ToCam1DistanceSets()
        {
            ConfigServiceMock.Object.ToCam1Distance = 35.5;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.ToCam1Distance.Should().Be(35.5);
        }

        [Test]
        public void ToCam2DistanceSets()
        {
            ConfigServiceMock.Object.ToCam2Distance = 35.5;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.ToCam2Distance.Should().Be(35.5);
        }

        [Test]
        public void ToCam3DistanceSets()
        {
            ConfigServiceMock.Object.ToCam3Distance = 35.5;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.ToCam3Distance.Should().Be(35.5);
        }

        [Test]
        public void ToCam4DistanceSets()
        {
            ConfigServiceMock.Object.ToCam4Distance = 35.5;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.ToCam4Distance.Should().Be(35.5);
        }

        [Test]
        public void Cam1XSetupValueSets()
        {
            ConfigServiceMock.Object.Cam1XSetupValue = 579;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam1XSetupValue.Should().Be(579);
        }

        [Test]
        public void Cam1YSetupValueSets()
        {
            ConfigServiceMock.Object.Cam1YSetupValue = 579;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam1YSetupValue.Should().Be(579);
        }

        [Test]
        public void Cam2XSetupValueSets()
        {
            ConfigServiceMock.Object.Cam2XSetupValue = 579;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam2XSetupValue.Should().Be(579);
        }

        [Test]
        public void Cam2YSetupValueSets()
        {
            ConfigServiceMock.Object.Cam2YSetupValue = 579;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam2YSetupValue.Should().Be(579);
        }

        [Test]
        public void Cam3XSetupValueSets()
        {
            ConfigServiceMock.Object.Cam3XSetupValue = 579;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam3XSetupValue.Should().Be(579);
        }

        [Test]
        public void Cam3YSetupValueSets()
        {
            ConfigServiceMock.Object.Cam3YSetupValue = 579;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam3YSetupValue.Should().Be(579);
        }

        [Test]
        public void Cam4XSetupValueSets()
        {
            ConfigServiceMock.Object.Cam4XSetupValue = 579;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam4XSetupValue.Should().Be(579);
        }

        [Test]
        public void Cam4YSetupValueSets()
        {
            ConfigServiceMock.Object.Cam4YSetupValue = 579;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam4YSetupValue.Should().Be(579);
        }

        [Test]
        public void Cam1SetupSectorSets()
        {
            ConfigServiceMock.Object.Cam1SetupSector = "20/1";

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam1SetupSector.Should().Be("20/1");
        }

        [Test]
        public void Cam2SetupSectorSets()
        {
            ConfigServiceMock.Object.Cam2SetupSector = "20/1";

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam2SetupSector.Should().Be("20/1");
        }

        [Test]
        public void Cam3SetupSectorSets()
        {
            ConfigServiceMock.Object.Cam3SetupSector = "20/1";

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam3SetupSector.Should().Be("20/1");
        }

        [Test]
        public void Cam4SetupSectorSets()
        {
            ConfigServiceMock.Object.Cam4SetupSector = "20/1";

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.Cam4SetupSector.Should().Be("20/1");
        }

        [Test]
        public void SetupTabsIsEnabled()
        {
            SettingsTabViewModel.IsSetupTabsEnabled = false;

            SettingsTabViewModel.LoadSettings();

            SettingsTabViewModel.IsSetupTabsEnabled.Should().BeTrue();
        }
    }
}