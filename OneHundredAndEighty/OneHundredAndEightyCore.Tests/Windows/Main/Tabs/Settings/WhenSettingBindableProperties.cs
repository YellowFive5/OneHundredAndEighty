#region Usings

using System.Windows.Media.Imaging;
using FluentAssertions;
using NUnit.Framework;

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main.Tabs.Settings
{
    public class WhenSettingBindableProperties : SettingsTabViewModelTestBase
    {
        private NotifyPropertyChangedTester tester;

        protected override void Setup()
        {
            base.Setup();
            tester = new NotifyPropertyChangedTester(SettingsTabViewModel);
        }

        [Test]
        public void CamImageSetsAndChangeFired()
        {
            var oldValue = SettingsTabViewModel.CamImage;

            SettingsTabViewModel.CamImage = new BitmapImage();
            var newValue = SettingsTabViewModel.CamImage;

            newValue.Should().NotBe(oldValue);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.CamImage));
        }

        [Test]
        public void CamRoiImageSetsAndChangeFired()
        {
            var oldValue = SettingsTabViewModel.CamRoiImage;

            SettingsTabViewModel.CamRoiImage = new BitmapImage();
            var newValue = SettingsTabViewModel.CamRoiImage;

            newValue.Should().NotBe(oldValue);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.CamRoiImage));
        }


        [Test]
        public void Cam1ThresholdSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam1ThresholdSliderValue;
            ConfigServiceMock.Object.Cam1ThresholdSliderValue = oldValue;

            SettingsTabViewModel.Cam1ThresholdSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam1ThresholdSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam1ThresholdSliderValue));
            ConfigServiceMock.Object.Cam1ThresholdSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam2ThresholdSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam2ThresholdSliderValue;
            ConfigServiceMock.Object.Cam2ThresholdSliderValue = oldValue;

            SettingsTabViewModel.Cam2ThresholdSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam2ThresholdSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam2ThresholdSliderValue));
            ConfigServiceMock.Object.Cam2ThresholdSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam3ThresholdSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam3ThresholdSliderValue;
            ConfigServiceMock.Object.Cam3ThresholdSliderValue = oldValue;

            SettingsTabViewModel.Cam3ThresholdSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam3ThresholdSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam3ThresholdSliderValue));
            ConfigServiceMock.Object.Cam3ThresholdSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam4ThresholdSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam4ThresholdSliderValue;
            ConfigServiceMock.Object.Cam4ThresholdSliderValue = oldValue;

            SettingsTabViewModel.Cam4ThresholdSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam4ThresholdSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam4ThresholdSliderValue));
            ConfigServiceMock.Object.Cam4ThresholdSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam1SurfaceSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam1SurfaceSliderValue;
            ConfigServiceMock.Object.Cam1SurfaceSliderValue = oldValue;

            SettingsTabViewModel.Cam1SurfaceSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam1SurfaceSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam1SurfaceSliderValue));
            ConfigServiceMock.Object.Cam1SurfaceSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam2SurfaceSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam2SurfaceSliderValue;
            ConfigServiceMock.Object.Cam2SurfaceSliderValue = oldValue;

            SettingsTabViewModel.Cam2SurfaceSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam2SurfaceSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam2SurfaceSliderValue));
            ConfigServiceMock.Object.Cam2SurfaceSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam3SurfaceSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam3SurfaceSliderValue;
            ConfigServiceMock.Object.Cam3SurfaceSliderValue = oldValue;

            SettingsTabViewModel.Cam3SurfaceSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam3SurfaceSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam3SurfaceSliderValue));
            ConfigServiceMock.Object.Cam3SurfaceSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam4SurfaceSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam4SurfaceSliderValue;
            ConfigServiceMock.Object.Cam4SurfaceSliderValue = oldValue;

            SettingsTabViewModel.Cam4SurfaceSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam4SurfaceSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam4SurfaceSliderValue));
            ConfigServiceMock.Object.Cam4SurfaceSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam1SurfaceCenterSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam1SurfaceCenterSliderValue;
            ConfigServiceMock.Object.Cam1SurfaceCenterSliderValue = oldValue;

            SettingsTabViewModel.Cam1SurfaceCenterSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam1SurfaceCenterSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam1SurfaceCenterSliderValue));
            ConfigServiceMock.Object.Cam1SurfaceCenterSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam2SurfaceCenterSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam2SurfaceCenterSliderValue;
            ConfigServiceMock.Object.Cam2SurfaceCenterSliderValue = oldValue;

            SettingsTabViewModel.Cam2SurfaceCenterSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam2SurfaceCenterSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam2SurfaceCenterSliderValue));
            ConfigServiceMock.Object.Cam2SurfaceCenterSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam3SurfaceCenterSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam3SurfaceCenterSliderValue;
            ConfigServiceMock.Object.Cam3SurfaceCenterSliderValue = oldValue;

            SettingsTabViewModel.Cam3SurfaceCenterSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam3SurfaceCenterSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam3SurfaceCenterSliderValue));
            ConfigServiceMock.Object.Cam3SurfaceCenterSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam4SurfaceCenterSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam4SurfaceCenterSliderValue;
            ConfigServiceMock.Object.Cam4SurfaceCenterSliderValue = oldValue;

            SettingsTabViewModel.Cam4SurfaceCenterSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam4SurfaceCenterSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam4SurfaceCenterSliderValue));
            ConfigServiceMock.Object.Cam4SurfaceCenterSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam1RoiPosYSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam1RoiPosYSliderValue;
            ConfigServiceMock.Object.Cam1RoiPosYSliderValue = oldValue;

            SettingsTabViewModel.Cam1RoiPosYSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam1RoiPosYSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam1RoiPosYSliderValue));
            ConfigServiceMock.Object.Cam1RoiPosYSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam2RoiPosYSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam2RoiPosYSliderValue;
            ConfigServiceMock.Object.Cam2RoiPosYSliderValue = oldValue;

            SettingsTabViewModel.Cam2RoiPosYSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam2RoiPosYSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam2RoiPosYSliderValue));
            ConfigServiceMock.Object.Cam2RoiPosYSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam3RoiPosYSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam3RoiPosYSliderValue;
            ConfigServiceMock.Object.Cam3RoiPosYSliderValue = oldValue;

            SettingsTabViewModel.Cam3RoiPosYSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam3RoiPosYSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam3RoiPosYSliderValue));
            ConfigServiceMock.Object.Cam3RoiPosYSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam4RoiPosYSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam4RoiPosYSliderValue;
            ConfigServiceMock.Object.Cam4RoiPosYSliderValue = oldValue;

            SettingsTabViewModel.Cam4RoiPosYSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam4RoiPosYSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam4RoiPosYSliderValue));
            ConfigServiceMock.Object.Cam4RoiPosYSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam1RoiHeightSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam1RoiHeightSliderValue;
            ConfigServiceMock.Object.Cam1RoiHeightSliderValue = oldValue;

            SettingsTabViewModel.Cam1RoiHeightSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam1RoiHeightSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam1RoiHeightSliderValue));
            ConfigServiceMock.Object.Cam1RoiHeightSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam2RoiHeightSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam2RoiHeightSliderValue;
            ConfigServiceMock.Object.Cam2RoiHeightSliderValue = oldValue;

            SettingsTabViewModel.Cam2RoiHeightSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam2RoiHeightSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam2RoiHeightSliderValue));
            ConfigServiceMock.Object.Cam2RoiHeightSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam3RoiHeightSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam3RoiHeightSliderValue;
            ConfigServiceMock.Object.Cam3RoiHeightSliderValue = oldValue;

            SettingsTabViewModel.Cam3RoiHeightSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam3RoiHeightSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam3RoiHeightSliderValue));
            ConfigServiceMock.Object.Cam3RoiHeightSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam4RoiHeightSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam4RoiHeightSliderValue;
            ConfigServiceMock.Object.Cam4RoiHeightSliderValue = oldValue;

            SettingsTabViewModel.Cam4RoiHeightSliderValue = 555.05;
            var newValue = SettingsTabViewModel.Cam4RoiHeightSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam4RoiHeightSliderValue));
            ConfigServiceMock.Object.Cam4RoiHeightSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam1EnabledSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam1Enabled;
            ConfigServiceMock.Object.Cam1Enabled = oldValue;

            SettingsTabViewModel.Cam1Enabled = true;
            var newValue = SettingsTabViewModel.Cam1Enabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam1Enabled));
            ConfigServiceMock.Object.Cam1Enabled.Should().BeTrue();
        }

        [Test]
        public void Cam2EnabledSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam2Enabled;
            ConfigServiceMock.Object.Cam2Enabled = oldValue;

            SettingsTabViewModel.Cam2Enabled = true;
            var newValue = SettingsTabViewModel.Cam2Enabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam2Enabled));
            ConfigServiceMock.Object.Cam2Enabled.Should().BeTrue();
        }

        [Test]
        public void Cam3EnabledSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam3Enabled;
            ConfigServiceMock.Object.Cam3Enabled = oldValue;

            SettingsTabViewModel.Cam3Enabled = true;
            var newValue = SettingsTabViewModel.Cam3Enabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam3Enabled));
            ConfigServiceMock.Object.Cam3Enabled.Should().BeTrue();
        }

        [Test]
        public void Cam4EnabledSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam4Enabled;
            ConfigServiceMock.Object.Cam4Enabled = oldValue;

            SettingsTabViewModel.Cam4Enabled = true;
            var newValue = SettingsTabViewModel.Cam4Enabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam4Enabled));
            ConfigServiceMock.Object.Cam4Enabled.Should().BeTrue();
        }

        [Test]
        public void DetectionEnabledSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.DetectionEnabled;
            ConfigServiceMock.Object.DetectionEnabled = oldValue;

            SettingsTabViewModel.DetectionEnabled = true;
            var newValue = SettingsTabViewModel.DetectionEnabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.DetectionEnabled));
            ConfigServiceMock.Object.DetectionEnabled.Should().BeTrue();
        }

        [Test]
        public void Cam1IdSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam1Id;
            ConfigServiceMock.Object.Cam1Id = oldValue;

            SettingsTabViewModel.Cam1Id = "2dr4e45";
            var newValue = SettingsTabViewModel.Cam1Id;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("2dr4e45");
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam1Id));
            ConfigServiceMock.Object.Cam1Id.Should().Be(newValue);
        }

        [Test]
        public void Cam2IdSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam2Id;
            ConfigServiceMock.Object.Cam2Id = oldValue;

            SettingsTabViewModel.Cam2Id = "2dr4e45";
            var newValue = SettingsTabViewModel.Cam2Id;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("2dr4e45");
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam2Id));
            ConfigServiceMock.Object.Cam2Id.Should().Be(newValue);
        }

        [Test]
        public void Cam3IdSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam3Id;
            ConfigServiceMock.Object.Cam3Id = oldValue;

            SettingsTabViewModel.Cam3Id = "2dr4e45";
            var newValue = SettingsTabViewModel.Cam3Id;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("2dr4e45");
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam3Id));
            ConfigServiceMock.Object.Cam3Id.Should().Be(newValue);
        }

        [Test]
        public void Cam4IdSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam4Id;
            ConfigServiceMock.Object.Cam4Id = oldValue;

            SettingsTabViewModel.Cam4Id = "2dr4e45";
            var newValue = SettingsTabViewModel.Cam4Id;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("2dr4e45");
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam4Id));
            ConfigServiceMock.Object.Cam4Id.Should().Be(newValue);
        }

        [Test]
        public void CamsFovAngleSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.CamsFovAngle;
            ConfigServiceMock.Object.CamsFovAngle = oldValue;

            SettingsTabViewModel.CamsFovAngle = 555.05;
            var newValue = SettingsTabViewModel.CamsFovAngle;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.CamsFovAngle));
            ConfigServiceMock.Object.CamsFovAngle.Should().Be(newValue);
        }

        [Test]
        public void CamsResolutionHeightSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.CamsResolutionHeight;
            ConfigServiceMock.Object.CamsResolutionHeight = oldValue;

            SettingsTabViewModel.CamsResolutionHeight = 1920;
            var newValue = SettingsTabViewModel.CamsResolutionHeight;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(1920);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.CamsResolutionHeight));
            ConfigServiceMock.Object.CamsResolutionHeight.Should().Be(newValue);
        }

        [Test]
        public void CamsResolutionWidthSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.CamsResolutionWidth;
            ConfigServiceMock.Object.CamsResolutionWidth = oldValue;

            SettingsTabViewModel.CamsResolutionWidth = 1920;
            var newValue = SettingsTabViewModel.CamsResolutionWidth;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(1920);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.CamsResolutionWidth));
            ConfigServiceMock.Object.CamsResolutionWidth.Should().Be(newValue);
        }

        [Test]
        public void MovesDetectedSleepTimeValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.MovesDetectedSleepTimeValue;
            ConfigServiceMock.Object.MovesDetectedSleepTimeValue = oldValue;

            SettingsTabViewModel.MovesDetectedSleepTimeValue = 0.25;
            var newValue = SettingsTabViewModel.MovesDetectedSleepTimeValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(0.25);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.MovesDetectedSleepTimeValue));
            ConfigServiceMock.Object.MovesDetectedSleepTimeValue.Should().Be(newValue);
        }

        [Test]
        public void SmoothGaussValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.SmoothGaussValue;
            ConfigServiceMock.Object.SmoothGaussValue = oldValue;

            SettingsTabViewModel.SmoothGaussValue = 5;
            var newValue = SettingsTabViewModel.SmoothGaussValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(5);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.SmoothGaussValue));
            ConfigServiceMock.Object.SmoothGaussValue.Should().Be(newValue);
        }

        [Test]
        public void ThresholdSleepTimeValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.ThresholdSleepTimeValue;
            ConfigServiceMock.Object.ThresholdSleepTimeValue = oldValue;

            SettingsTabViewModel.ThresholdSleepTimeValue = 555.05;
            var newValue = SettingsTabViewModel.ThresholdSleepTimeValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.ThresholdSleepTimeValue));
            ConfigServiceMock.Object.ThresholdSleepTimeValue.Should().Be(newValue);
        }

        [Test]
        public void ExtractionSleepTimeValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.ExtractionSleepTimeValue;
            ConfigServiceMock.Object.ExtractionSleepTimeValue = oldValue;

            SettingsTabViewModel.ExtractionSleepTimeValue = 555.05;
            var newValue = SettingsTabViewModel.ExtractionSleepTimeValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.ExtractionSleepTimeValue));
            ConfigServiceMock.Object.ExtractionSleepTimeValue.Should().Be(newValue);
        }

        [Test]
        public void MinContourArcValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.MinContourArcValue;
            ConfigServiceMock.Object.MinContourArcValue = oldValue;

            SettingsTabViewModel.MinContourArcValue = 500;
            var newValue = SettingsTabViewModel.MinContourArcValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(500);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.MinContourArcValue));
            ConfigServiceMock.Object.MinContourArcValue.Should().Be(newValue);
        }

        [Test]
        public void MaxContourArcValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.MaxContourArcValue;
            ConfigServiceMock.Object.MaxContourArcValue = oldValue;

            SettingsTabViewModel.MaxContourArcValue = 500;
            var newValue = SettingsTabViewModel.MaxContourArcValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(500);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.MaxContourArcValue));
            ConfigServiceMock.Object.MaxContourArcValue.Should().Be(newValue);
        }

        [Test]
        public void MaxContourAreaValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.MaxContourAreaValue;
            ConfigServiceMock.Object.MaxContourAreaValue = oldValue;

            SettingsTabViewModel.MaxContourAreaValue = 500;
            var newValue = SettingsTabViewModel.MaxContourAreaValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(500);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.MaxContourAreaValue));
            ConfigServiceMock.Object.MaxContourAreaValue.Should().Be(newValue);
        }

        [Test]
        public void MinContourAreaValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.MinContourAreaValue;
            ConfigServiceMock.Object.MinContourAreaValue = oldValue;

            SettingsTabViewModel.MinContourAreaValue = 500;
            var newValue = SettingsTabViewModel.MinContourAreaValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(500);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.MinContourAreaValue));
            ConfigServiceMock.Object.MinContourAreaValue.Should().Be(newValue);
        }

        [Test]
        public void MinContourWidthValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.MinContourWidthValue;
            ConfigServiceMock.Object.MinContourWidthValue = oldValue;

            SettingsTabViewModel.MinContourWidthValue = 500;
            var newValue = SettingsTabViewModel.MinContourWidthValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(500);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.MinContourWidthValue));
            ConfigServiceMock.Object.MinContourWidthValue.Should().Be(newValue);
        }

        [Test]
        public void MaxContourWidthValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.MaxContourWidthValue;
            ConfigServiceMock.Object.MaxContourWidthValue = oldValue;

            SettingsTabViewModel.MaxContourWidthValue = 500;
            var newValue = SettingsTabViewModel.MaxContourWidthValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(500);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.MaxContourWidthValue));
            ConfigServiceMock.Object.MaxContourWidthValue.Should().Be(newValue);
        }

        [Test]
        public void ToCam1DistanceSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.ToCam1Distance;
            ConfigServiceMock.Object.ToCam1Distance = oldValue;

            SettingsTabViewModel.ToCam1Distance = 33.5;
            var newValue = SettingsTabViewModel.ToCam1Distance;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(33.5);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.ToCam1Distance));
            ConfigServiceMock.Object.ToCam1Distance.Should().Be(newValue);
        }

        [Test]
        public void ToCam2DistanceSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.ToCam2Distance;
            ConfigServiceMock.Object.ToCam2Distance = oldValue;

            SettingsTabViewModel.ToCam2Distance = 33.5;
            var newValue = SettingsTabViewModel.ToCam2Distance;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(33.5);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.ToCam2Distance));
            ConfigServiceMock.Object.ToCam2Distance.Should().Be(newValue);
        }

        [Test]
        public void ToCam3DistanceSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.ToCam3Distance;
            ConfigServiceMock.Object.ToCam3Distance = oldValue;

            SettingsTabViewModel.ToCam3Distance = 33.5;
            var newValue = SettingsTabViewModel.ToCam3Distance;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(33.5);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.ToCam3Distance));
            ConfigServiceMock.Object.ToCam3Distance.Should().Be(newValue);
        }

        [Test]
        public void ToCam4DistanceSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.ToCam4Distance;
            ConfigServiceMock.Object.ToCam4Distance = oldValue;

            SettingsTabViewModel.ToCam4Distance = 33.5;
            var newValue = SettingsTabViewModel.ToCam4Distance;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(33.5);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.ToCam4Distance));
            ConfigServiceMock.Object.ToCam4Distance.Should().Be(newValue);
        }

        [Test]
        public void Cam1XSetupValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam1XSetupValue;
            ConfigServiceMock.Object.Cam1XSetupValue = oldValue;

            SettingsTabViewModel.Cam1XSetupValue = 589;
            var newValue = SettingsTabViewModel.Cam1XSetupValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(589);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam1XSetupValue));
            ConfigServiceMock.Object.Cam1XSetupValue.Should().Be(newValue);
        }

        [Test]
        public void Cam1YSetupValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam1YSetupValue;
            ConfigServiceMock.Object.Cam1YSetupValue = oldValue;

            SettingsTabViewModel.Cam1YSetupValue = 589;
            var newValue = SettingsTabViewModel.Cam1YSetupValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(589);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam1YSetupValue));
            ConfigServiceMock.Object.Cam1YSetupValue.Should().Be(newValue);
        }

        [Test]
        public void Cam2XSetupValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam2XSetupValue;
            ConfigServiceMock.Object.Cam2XSetupValue = oldValue;

            SettingsTabViewModel.Cam2XSetupValue = 589;
            var newValue = SettingsTabViewModel.Cam2XSetupValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(589);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam2XSetupValue));
            ConfigServiceMock.Object.Cam2XSetupValue.Should().Be(newValue);
        }

        [Test]
        public void Cam2YSetupValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam2YSetupValue;
            ConfigServiceMock.Object.Cam2YSetupValue = oldValue;

            SettingsTabViewModel.Cam2YSetupValue = 589;
            var newValue = SettingsTabViewModel.Cam2YSetupValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(589);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam2YSetupValue));
            ConfigServiceMock.Object.Cam2YSetupValue.Should().Be(newValue);
        }

        [Test]
        public void Cam3XSetupValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam3XSetupValue;
            ConfigServiceMock.Object.Cam3XSetupValue = oldValue;

            SettingsTabViewModel.Cam3XSetupValue = 589;
            var newValue = SettingsTabViewModel.Cam3XSetupValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(589);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam3XSetupValue));
            ConfigServiceMock.Object.Cam3XSetupValue.Should().Be(newValue);
        }

        [Test]
        public void Cam3YSetupValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam3YSetupValue;
            ConfigServiceMock.Object.Cam3YSetupValue = oldValue;

            SettingsTabViewModel.Cam3YSetupValue = 589;
            var newValue = SettingsTabViewModel.Cam3YSetupValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(589);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam3YSetupValue));
            ConfigServiceMock.Object.Cam3YSetupValue.Should().Be(newValue);
        }

        [Test]
        public void Cam4XSetupValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam4XSetupValue;
            ConfigServiceMock.Object.Cam4XSetupValue = oldValue;

            SettingsTabViewModel.Cam4XSetupValue = 589;
            var newValue = SettingsTabViewModel.Cam4XSetupValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(589);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam4XSetupValue));
            ConfigServiceMock.Object.Cam4XSetupValue.Should().Be(newValue);
        }

        [Test]
        public void Cam4YSetupValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam4YSetupValue;
            ConfigServiceMock.Object.Cam4YSetupValue = oldValue;

            SettingsTabViewModel.Cam4YSetupValue = 589;
            var newValue = SettingsTabViewModel.Cam4YSetupValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(589);
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam4YSetupValue));
            ConfigServiceMock.Object.Cam4YSetupValue.Should().Be(newValue);
        }

        [Test]
        public void Cam1SetupSectorSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam1SetupSector;
            ConfigServiceMock.Object.Cam1SetupSector = oldValue;

            SettingsTabViewModel.Cam1SetupSector = "18/4";
            var newValue = SettingsTabViewModel.Cam1SetupSector;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("18/4");
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam1SetupSector));
            ConfigServiceMock.Object.Cam1SetupSector.Should().Be(newValue);
        }

        [Test]
        public void Cam2SetupSectorSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam2SetupSector;
            ConfigServiceMock.Object.Cam2SetupSector = oldValue;

            SettingsTabViewModel.Cam2SetupSector = "18/4";
            var newValue = SettingsTabViewModel.Cam2SetupSector;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("18/4");
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam2SetupSector));
            ConfigServiceMock.Object.Cam2SetupSector.Should().Be(newValue);
        }

        [Test]
        public void Cam3SetupSectorSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam3SetupSector;
            ConfigServiceMock.Object.Cam3SetupSector = oldValue;

            SettingsTabViewModel.Cam3SetupSector = "18/4";
            var newValue = SettingsTabViewModel.Cam3SetupSector;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("18/4");
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam3SetupSector));
            ConfigServiceMock.Object.Cam3SetupSector.Should().Be(newValue);
        }

        [Test]
        public void Cam4SetupSectorSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = SettingsTabViewModel.Cam4SetupSector;
            ConfigServiceMock.Object.Cam4SetupSector = oldValue;

            SettingsTabViewModel.Cam4SetupSector = "18/4";
            var newValue = SettingsTabViewModel.Cam4SetupSector;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("18/4");
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.Cam4SetupSector));
            ConfigServiceMock.Object.Cam4SetupSector.Should().Be(newValue);
        }

        [Test]
        public void CheckCamsBoxTextSetsAndChangeFired()
        {
            var oldValue = SettingsTabViewModel.CheckCamsBoxText;

            SettingsTabViewModel.CheckCamsBoxText = "[HBV HD CAMERA]-[ID:'8&2f223cfb']";
            var newValue = SettingsTabViewModel.CheckCamsBoxText;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("[HBV HD CAMERA]-[ID:'8&2f223cfb']");
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.CheckCamsBoxText));
        }
        [Test]
        public void IsSetupTabsEnabledSetsAndChangeFired()
        {
            var oldValue = SettingsTabViewModel.IsSetupTabsEnabled;

            SettingsTabViewModel.IsSetupTabsEnabled = true;
            var newValue = SettingsTabViewModel.IsSetupTabsEnabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.IsSetupTabsEnabled));
        }

        [Test]
        public void IsCamsSetupRunningSetsAndChangeFired()
        {
            var oldValue = SettingsTabViewModel.IsCamsSetupRunning;

            SettingsTabViewModel.IsCamsSetupRunning = true;
            var newValue = SettingsTabViewModel.IsCamsSetupRunning;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.IsCamsSetupRunning));
        }


        [Test]
        public void IsRuntimeCrossingRunningSetsAndChangeFired()
        {
            var oldValue = SettingsTabViewModel.IsRuntimeCrossingRunning;

            SettingsTabViewModel.IsRuntimeCrossingRunning = true;
            var newValue = SettingsTabViewModel.IsRuntimeCrossingRunning;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(SettingsTabViewModel.IsRuntimeCrossingRunning));
        }

    }
}