#region Usings

using System.Collections.Generic;
using System.Windows.Media.Imaging;
using FluentAssertions;
using NUnit.Framework;
using OneHundredAndEightyCore.Game;

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main
{
    public class WhenSettingBindableProperties : MainWindowViewModelTestBase
    {
        private NotifyPropertyChangedTester tester;

        protected override void Setup()
        {
            base.Setup();
            tester = new NotifyPropertyChangedTester(viewModel);
        }

        [Test]
        public void MainWindowPositionLeftSetsAndChangeFired()
        {
            var oldValue = viewModel.MainWindowPositionLeft;

            viewModel.MainWindowPositionLeft = 555.05;
            var newValue = viewModel.MainWindowPositionLeft;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.MainWindowPositionLeft));
        }

        [Test]
        public void MainWindowPositionTopSetsAndChangeFired()
        {
            var oldValue = viewModel.MainWindowPositionTop;

            viewModel.MainWindowPositionTop = 555.05;
            var newValue = viewModel.MainWindowPositionTop;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.MainWindowPositionTop));
        }

        [Test]
        public void MainWindowHeightSetsAndChangeFired()
        {
            var oldValue = viewModel.MainWindowHeight;

            viewModel.MainWindowHeight = 555.05;
            var newValue = viewModel.MainWindowHeight;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.MainWindowHeight));
        }

        [Test]
        public void MainWindowWidthSetsAndChangeFired()
        {
            var oldValue = viewModel.MainWindowWidth;

            viewModel.MainWindowWidth = 555.05;
            var newValue = viewModel.MainWindowWidth;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.MainWindowWidth));
        }

        [Test]
        public void CamImageSetsAndChangeFired()
        {
            var oldValue = viewModel.CamImage;

            viewModel.CamImage = new BitmapImage();
            var newValue = viewModel.CamImage;

            newValue.Should().NotBe(oldValue);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.CamImage));
        }

        [Test]
        public void CamRoiImageSetsAndChangeFired()
        {
            var oldValue = viewModel.CamRoiImage;

            viewModel.CamRoiImage = new BitmapImage();
            var newValue = viewModel.CamRoiImage;

            newValue.Should().NotBe(oldValue);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.CamRoiImage));
        }

        [Test]
        public void Cam1ThresholdSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam1ThresholdSliderValue;
            configService.Object.Cam1ThresholdSliderValue = oldValue;

            viewModel.Cam1ThresholdSliderValue = 555.05;
            var newValue = viewModel.Cam1ThresholdSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1ThresholdSliderValue));
            configService.Object.Cam1ThresholdSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam2ThresholdSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam2ThresholdSliderValue;
            configService.Object.Cam2ThresholdSliderValue = oldValue;

            viewModel.Cam2ThresholdSliderValue = 555.05;
            var newValue = viewModel.Cam2ThresholdSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2ThresholdSliderValue));
            configService.Object.Cam2ThresholdSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam3ThresholdSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam3ThresholdSliderValue;
            configService.Object.Cam3ThresholdSliderValue = oldValue;

            viewModel.Cam3ThresholdSliderValue = 555.05;
            var newValue = viewModel.Cam3ThresholdSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3ThresholdSliderValue));
            configService.Object.Cam3ThresholdSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam4ThresholdSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam4ThresholdSliderValue;
            configService.Object.Cam4ThresholdSliderValue = oldValue;

            viewModel.Cam4ThresholdSliderValue = 555.05;
            var newValue = viewModel.Cam4ThresholdSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4ThresholdSliderValue));
            configService.Object.Cam4ThresholdSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam1SurfaceSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam1SurfaceSliderValue;
            configService.Object.Cam1SurfaceSliderValue = oldValue;

            viewModel.Cam1SurfaceSliderValue = 555.05;
            var newValue = viewModel.Cam1SurfaceSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1SurfaceSliderValue));
            configService.Object.Cam1SurfaceSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam2SurfaceSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam2SurfaceSliderValue;
            configService.Object.Cam2SurfaceSliderValue = oldValue;

            viewModel.Cam2SurfaceSliderValue = 555.05;
            var newValue = viewModel.Cam2SurfaceSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2SurfaceSliderValue));
            configService.Object.Cam2SurfaceSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam3SurfaceSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam3SurfaceSliderValue;
            configService.Object.Cam3SurfaceSliderValue = oldValue;

            viewModel.Cam3SurfaceSliderValue = 555.05;
            var newValue = viewModel.Cam3SurfaceSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3SurfaceSliderValue));
            configService.Object.Cam3SurfaceSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam4SurfaceSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam4SurfaceSliderValue;
            configService.Object.Cam4SurfaceSliderValue = oldValue;

            viewModel.Cam4SurfaceSliderValue = 555.05;
            var newValue = viewModel.Cam4SurfaceSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4SurfaceSliderValue));
            configService.Object.Cam4SurfaceSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam1SurfaceCenterSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam1SurfaceCenterSliderValue;
            configService.Object.Cam1SurfaceCenterSliderValue = oldValue;

            viewModel.Cam1SurfaceCenterSliderValue = 555.05;
            var newValue = viewModel.Cam1SurfaceCenterSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1SurfaceCenterSliderValue));
            configService.Object.Cam1SurfaceCenterSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam2SurfaceCenterSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam2SurfaceCenterSliderValue;
            configService.Object.Cam2SurfaceCenterSliderValue = oldValue;

            viewModel.Cam2SurfaceCenterSliderValue = 555.05;
            var newValue = viewModel.Cam2SurfaceCenterSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2SurfaceCenterSliderValue));
            configService.Object.Cam2SurfaceCenterSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam3SurfaceCenterSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam3SurfaceCenterSliderValue;
            configService.Object.Cam3SurfaceCenterSliderValue = oldValue;

            viewModel.Cam3SurfaceCenterSliderValue = 555.05;
            var newValue = viewModel.Cam3SurfaceCenterSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3SurfaceCenterSliderValue));
            configService.Object.Cam3SurfaceCenterSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam4SurfaceCenterSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam4SurfaceCenterSliderValue;
            configService.Object.Cam4SurfaceCenterSliderValue = oldValue;

            viewModel.Cam4SurfaceCenterSliderValue = 555.05;
            var newValue = viewModel.Cam4SurfaceCenterSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4SurfaceCenterSliderValue));
            configService.Object.Cam4SurfaceCenterSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam1RoiPosYSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam1RoiPosYSliderValue;
            configService.Object.Cam1RoiPosYSliderValue = oldValue;

            viewModel.Cam1RoiPosYSliderValue = 555.05;
            var newValue = viewModel.Cam1RoiPosYSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1RoiPosYSliderValue));
            configService.Object.Cam1RoiPosYSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam2RoiPosYSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam2RoiPosYSliderValue;
            configService.Object.Cam2RoiPosYSliderValue = oldValue;

            viewModel.Cam2RoiPosYSliderValue = 555.05;
            var newValue = viewModel.Cam2RoiPosYSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2RoiPosYSliderValue));
            configService.Object.Cam2RoiPosYSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam3RoiPosYSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam3RoiPosYSliderValue;
            configService.Object.Cam3RoiPosYSliderValue = oldValue;

            viewModel.Cam3RoiPosYSliderValue = 555.05;
            var newValue = viewModel.Cam3RoiPosYSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3RoiPosYSliderValue));
            configService.Object.Cam3RoiPosYSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam4RoiPosYSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam4RoiPosYSliderValue;
            configService.Object.Cam4RoiPosYSliderValue = oldValue;

            viewModel.Cam4RoiPosYSliderValue = 555.05;
            var newValue = viewModel.Cam4RoiPosYSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4RoiPosYSliderValue));
            configService.Object.Cam4RoiPosYSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam1RoiHeightSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam1RoiHeightSliderValue;
            configService.Object.Cam1RoiHeightSliderValue = oldValue;

            viewModel.Cam1RoiHeightSliderValue = 555.05;
            var newValue = viewModel.Cam1RoiHeightSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1RoiHeightSliderValue));
            configService.Object.Cam1RoiHeightSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam2RoiHeightSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam2RoiHeightSliderValue;
            configService.Object.Cam2RoiHeightSliderValue = oldValue;

            viewModel.Cam2RoiHeightSliderValue = 555.05;
            var newValue = viewModel.Cam2RoiHeightSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2RoiHeightSliderValue));
            configService.Object.Cam2RoiHeightSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam3RoiHeightSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam3RoiHeightSliderValue;
            configService.Object.Cam3RoiHeightSliderValue = oldValue;

            viewModel.Cam3RoiHeightSliderValue = 555.05;
            var newValue = viewModel.Cam3RoiHeightSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3RoiHeightSliderValue));
            configService.Object.Cam3RoiHeightSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam4RoiHeightSliderValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam4RoiHeightSliderValue;
            configService.Object.Cam4RoiHeightSliderValue = oldValue;

            viewModel.Cam4RoiHeightSliderValue = 555.05;
            var newValue = viewModel.Cam4RoiHeightSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4RoiHeightSliderValue));
            configService.Object.Cam4RoiHeightSliderValue.Should().Be(newValue);
        }

        [Test]
        public void Cam1EnabledSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam1Enabled;
            configService.Object.Cam1Enabled = oldValue;

            viewModel.Cam1Enabled = true;
            var newValue = viewModel.Cam1Enabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1Enabled));
            configService.Object.Cam1Enabled.Should().BeTrue();
        }

        [Test]
        public void Cam2EnabledSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam2Enabled;
            configService.Object.Cam2Enabled = oldValue;

            viewModel.Cam2Enabled = true;
            var newValue = viewModel.Cam2Enabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2Enabled));
            configService.Object.Cam2Enabled.Should().BeTrue();
        }

        [Test]
        public void Cam3EnabledSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam3Enabled;
            configService.Object.Cam3Enabled = oldValue;

            viewModel.Cam3Enabled = true;
            var newValue = viewModel.Cam3Enabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3Enabled));
            configService.Object.Cam3Enabled.Should().BeTrue();
        }

        [Test]
        public void Cam4EnabledSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam4Enabled;
            configService.Object.Cam4Enabled = oldValue;

            viewModel.Cam4Enabled = true;
            var newValue = viewModel.Cam4Enabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4Enabled));
            configService.Object.Cam4Enabled.Should().BeTrue();
        }

        [Test]
        public void DetectionEnabledSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.DetectionEnabled;
            configService.Object.DetectionEnabled = oldValue;

            viewModel.DetectionEnabled = true;
            var newValue = viewModel.DetectionEnabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.DetectionEnabled));
            configService.Object.DetectionEnabled.Should().BeTrue();
        }

        [Test]
        public void Cam1IdSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam1Id;
            configService.Object.Cam1Id = oldValue;

            viewModel.Cam1Id = "2dr4e45";
            var newValue = viewModel.Cam1Id;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("2dr4e45");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1Id));
            configService.Object.Cam1Id.Should().Be(newValue);
        }

        [Test]
        public void Cam2IdSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam2Id;
            configService.Object.Cam2Id = oldValue;

            viewModel.Cam2Id = "2dr4e45";
            var newValue = viewModel.Cam2Id;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("2dr4e45");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2Id));
            configService.Object.Cam2Id.Should().Be(newValue);
        }

        [Test]
        public void Cam3IdSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam3Id;
            configService.Object.Cam3Id = oldValue;

            viewModel.Cam3Id = "2dr4e45";
            var newValue = viewModel.Cam3Id;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("2dr4e45");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3Id));
            configService.Object.Cam3Id.Should().Be(newValue);
        }

        [Test]
        public void Cam4IdSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam4Id;
            configService.Object.Cam4Id = oldValue;

            viewModel.Cam4Id = "2dr4e45";
            var newValue = viewModel.Cam4Id;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("2dr4e45");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4Id));
            configService.Object.Cam4Id.Should().Be(newValue);
        }

        [Test]
        public void CamsFovAngleSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.CamsFovAngle;
            configService.Object.CamsFovAngle = oldValue;

            viewModel.CamsFovAngle = 555.05;
            var newValue = viewModel.CamsFovAngle;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.CamsFovAngle));
            configService.Object.CamsFovAngle.Should().Be(newValue);
        }

        [Test]
        public void CamsResolutionHeightSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.CamsResolutionHeight;
            configService.Object.CamsResolutionHeight = oldValue;

            viewModel.CamsResolutionHeight = 1920;
            var newValue = viewModel.CamsResolutionHeight;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(1920);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.CamsResolutionHeight));
            configService.Object.CamsResolutionHeight.Should().Be(newValue);
        }

        [Test]
        public void CamsResolutionWidthSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.CamsResolutionWidth;
            configService.Object.CamsResolutionWidth = oldValue;

            viewModel.CamsResolutionWidth = 1920;
            var newValue = viewModel.CamsResolutionWidth;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(1920);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.CamsResolutionWidth));
            configService.Object.CamsResolutionWidth.Should().Be(newValue);
        }

        [Test]
        public void MovesExtractionValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.MovesExtractionValue;
            configService.Object.MovesExtractionValue = oldValue;

            viewModel.MovesExtractionValue = 500;
            var newValue = viewModel.MovesExtractionValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(500);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.MovesExtractionValue));
            configService.Object.MovesExtractionValue.Should().Be(newValue);
        }

        [Test]
        public void MovesDetectedSleepTimeValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.MovesDetectedSleepTimeValue;
            configService.Object.MovesDetectedSleepTimeValue = oldValue;

            viewModel.MovesDetectedSleepTimeValue = 0.25;
            var newValue = viewModel.MovesDetectedSleepTimeValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(0.25);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.MovesDetectedSleepTimeValue));
            configService.Object.MovesDetectedSleepTimeValue.Should().Be(newValue);
        }

        [Test]
        public void MovesNoiseValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.MovesNoiseValue;
            configService.Object.MovesNoiseValue = oldValue;

            viewModel.MovesNoiseValue = 200;
            var newValue = viewModel.MovesNoiseValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(200);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.MovesNoiseValue));
            configService.Object.MovesNoiseValue.Should().Be(newValue);
        }

        [Test]
        public void SmoothGaussValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.SmoothGaussValue;
            configService.Object.SmoothGaussValue = oldValue;

            viewModel.SmoothGaussValue = 5;
            var newValue = viewModel.SmoothGaussValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(5);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.SmoothGaussValue));
            configService.Object.SmoothGaussValue.Should().Be(newValue);
        }

        [Test]
        public void ThresholdSleepTimeValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.ThresholdSleepTimeValue;
            configService.Object.ThresholdSleepTimeValue = oldValue;

            viewModel.ThresholdSleepTimeValue = 555.05;
            var newValue = viewModel.ThresholdSleepTimeValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.ThresholdSleepTimeValue));
            configService.Object.ThresholdSleepTimeValue.Should().Be(newValue);
        }

        [Test]
        public void ExtractionSleepTimeValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.ExtractionSleepTimeValue;
            configService.Object.ExtractionSleepTimeValue = oldValue;

            viewModel.ExtractionSleepTimeValue = 555.05;
            var newValue = viewModel.ExtractionSleepTimeValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.ExtractionSleepTimeValue));
            configService.Object.ExtractionSleepTimeValue.Should().Be(newValue);
        }

        [Test]
        public void MinContourArcValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.MinContourArcValue;
            configService.Object.MinContourArcValue = oldValue;

            viewModel.MinContourArcValue = 500;
            var newValue = viewModel.MinContourArcValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(500);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.MinContourArcValue));
            configService.Object.MinContourArcValue.Should().Be(newValue);
        }

        [Test]
        public void MovesDartValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.MovesDartValue;
            configService.Object.MovesDartValue = oldValue;

            viewModel.MovesDartValue = 500;
            var newValue = viewModel.MovesDartValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(500);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.MovesDartValue));
            configService.Object.MovesDartValue.Should().Be(newValue);
        }

        [Test]
        public void ToCam1DistanceSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.ToCam1Distance;
            configService.Object.ToCam1Distance = oldValue;

            viewModel.ToCam1Distance = 33.5;
            var newValue = viewModel.ToCam1Distance;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(33.5);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.ToCam1Distance));
            configService.Object.ToCam1Distance.Should().Be(newValue);
        }

        [Test]
        public void ToCam2DistanceSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.ToCam2Distance;
            configService.Object.ToCam2Distance = oldValue;

            viewModel.ToCam2Distance = 33.5;
            var newValue = viewModel.ToCam2Distance;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(33.5);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.ToCam2Distance));
            configService.Object.ToCam2Distance.Should().Be(newValue);
        }

        [Test]
        public void ToCam3DistanceSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.ToCam3Distance;
            configService.Object.ToCam3Distance = oldValue;

            viewModel.ToCam3Distance = 33.5;
            var newValue = viewModel.ToCam3Distance;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(33.5);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.ToCam3Distance));
            configService.Object.ToCam3Distance.Should().Be(newValue);
        }

        [Test]
        public void ToCam4DistanceSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.ToCam4Distance;
            configService.Object.ToCam4Distance = oldValue;

            viewModel.ToCam4Distance = 33.5;
            var newValue = viewModel.ToCam4Distance;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(33.5);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.ToCam4Distance));
            configService.Object.ToCam4Distance.Should().Be(newValue);
        }

        [Test]
        public void Cam1XSetupValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam1XSetupValue;
            configService.Object.Cam1XSetupValue = oldValue;

            viewModel.Cam1XSetupValue = 589;
            var newValue = viewModel.Cam1XSetupValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(589);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1XSetupValue));
            configService.Object.Cam1XSetupValue.Should().Be(newValue);
        }

        [Test]
        public void Cam1YSetupValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam1YSetupValue;
            configService.Object.Cam1YSetupValue = oldValue;

            viewModel.Cam1YSetupValue = 589;
            var newValue = viewModel.Cam1YSetupValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(589);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1YSetupValue));
            configService.Object.Cam1YSetupValue.Should().Be(newValue);
        }

        [Test]
        public void Cam2XSetupValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam2XSetupValue;
            configService.Object.Cam2XSetupValue = oldValue;

            viewModel.Cam2XSetupValue = 589;
            var newValue = viewModel.Cam2XSetupValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(589);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2XSetupValue));
            configService.Object.Cam2XSetupValue.Should().Be(newValue);
        }

        [Test]
        public void Cam2YSetupValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam2YSetupValue;
            configService.Object.Cam2YSetupValue = oldValue;

            viewModel.Cam2YSetupValue = 589;
            var newValue = viewModel.Cam2YSetupValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(589);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2YSetupValue));
            configService.Object.Cam2YSetupValue.Should().Be(newValue);
        }

        [Test]
        public void Cam3XSetupValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam3XSetupValue;
            configService.Object.Cam3XSetupValue = oldValue;

            viewModel.Cam3XSetupValue = 589;
            var newValue = viewModel.Cam3XSetupValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(589);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3XSetupValue));
            configService.Object.Cam3XSetupValue.Should().Be(newValue);
        }

        [Test]
        public void Cam3YSetupValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam3YSetupValue;
            configService.Object.Cam3YSetupValue = oldValue;

            viewModel.Cam3YSetupValue = 589;
            var newValue = viewModel.Cam3YSetupValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(589);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3YSetupValue));
            configService.Object.Cam3YSetupValue.Should().Be(newValue);
        }

        [Test]
        public void Cam4XSetupValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam4XSetupValue;
            configService.Object.Cam4XSetupValue = oldValue;

            viewModel.Cam4XSetupValue = 589;
            var newValue = viewModel.Cam4XSetupValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(589);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4XSetupValue));
            configService.Object.Cam4XSetupValue.Should().Be(newValue);
        }

        [Test]
        public void Cam4YSetupValueSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam4YSetupValue;
            configService.Object.Cam4YSetupValue = oldValue;

            viewModel.Cam4YSetupValue = 589;
            var newValue = viewModel.Cam4YSetupValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(589);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4YSetupValue));
            configService.Object.Cam4YSetupValue.Should().Be(newValue);
        }

        [Test]
        public void Cam1SetupSectorSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam1SetupSector;
            configService.Object.Cam1SetupSector = oldValue;

            viewModel.Cam1SetupSector = "18/4";
            var newValue = viewModel.Cam1SetupSector;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("18/4");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1SetupSector));
            configService.Object.Cam1SetupSector.Should().Be(newValue);
        }

        [Test]
        public void Cam2SetupSectorSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam2SetupSector;
            configService.Object.Cam2SetupSector = oldValue;

            viewModel.Cam2SetupSector = "18/4";
            var newValue = viewModel.Cam2SetupSector;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("18/4");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2SetupSector));
            configService.Object.Cam2SetupSector.Should().Be(newValue);
        }

        [Test]
        public void Cam3SetupSectorSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam3SetupSector;
            configService.Object.Cam3SetupSector = oldValue;

            viewModel.Cam3SetupSector = "18/4";
            var newValue = viewModel.Cam3SetupSector;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("18/4");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3SetupSector));
            configService.Object.Cam3SetupSector.Should().Be(newValue);
        }

        [Test]
        public void Cam4SetupSectorSetsAndChangeFiredAndConfigItemSaved()
        {
            var oldValue = viewModel.Cam4SetupSector;
            configService.Object.Cam4SetupSector = oldValue;

            viewModel.Cam4SetupSector = "18/4";
            var newValue = viewModel.Cam4SetupSector;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("18/4");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4SetupSector));
            configService.Object.Cam4SetupSector.Should().Be(newValue);
        }

        [Test]
        public void CheckCamsBoxTextSetsAndChangeFired()
        {
            var oldValue = viewModel.CheckCamsBoxText;

            viewModel.CheckCamsBoxText = "[HBV HD CAMERA]-[ID:'8&2f223cfb']";
            var newValue = viewModel.CheckCamsBoxText;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("[HBV HD CAMERA]-[ID:'8&2f223cfb']");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.CheckCamsBoxText));
        }

        [Test]
        public void PlayersSetsAndChangeFired()
        {
            var oldValue = viewModel.Players;
            var list = new List<Player>()
                       {
                           new Player("Phil", "The Power"),
                           new Player("Michael", "Mighty Mike")
                       };

            viewModel.Players = list;
            var newValue = viewModel.Players;

            oldValue.Should().BeNullOrEmpty();
            newValue.Should().BeEquivalentTo(list);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Players));
        }

        [TestCase(GameType.FreeThrowsSingle, GameType.FreeThrowsDouble, false, true, false)]
        [TestCase(GameType.FreeThrowsSingle, GameType.Classic, false, true, true)]
        [TestCase(GameType.FreeThrowsDouble, GameType.FreeThrowsSingle, true, false, false)]
        [TestCase(GameType.FreeThrowsDouble, GameType.Classic, false, true, true)]
        [TestCase(GameType.Classic, GameType.FreeThrowsSingle, true, false, false)]
        [TestCase(GameType.Classic, GameType.FreeThrowsDouble, false, true, false)]
        public void NewGameTypeSetsAndAllAdditionalChangesFiredAndPropertiesSetsWhenNewValueNotEqualsOld(GameType oldGameTypeValue,
                                                                                                         GameType newGameTypeValue,
                                                                                                         bool isNewGameForSingleExpectedValue,
                                                                                                         bool isNewGameForPairExpectedValue,
                                                                                                         bool isNewGameIsClassicExpectedValue)
        {
            viewModel.NewGameType = oldGameTypeValue;
            tester.ChangesInvokes.Clear();

            viewModel.NewGameType = newGameTypeValue;
            var newValue = viewModel.NewGameType;

            newValue.Should().NotBe(oldGameTypeValue);
            newValue.Should().Be(newGameTypeValue);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.NewGameType));
            tester.AssertOnPropertyChangedInvoke(1, nameof(viewModel.IsNewGameForSingle));
            tester.AssertOnPropertyChangedInvoke(2, nameof(viewModel.IsNewGameForPair));
            tester.AssertOnPropertyChangedInvoke(3, nameof(viewModel.IsNewGameIsClassic));
            viewModel.IsNewGameForSingle = isNewGameForSingleExpectedValue;
            viewModel.IsNewGameForPair = isNewGameForPairExpectedValue;
            viewModel.IsNewGameIsClassic = isNewGameIsClassicExpectedValue;
        }

        [TestCase(GameType.FreeThrowsSingle, GameType.FreeThrowsSingle)]
        [TestCase(GameType.FreeThrowsDouble, GameType.FreeThrowsDouble)]
        [TestCase(GameType.Classic, GameType.Classic)]
        public void NewGameTypeNotSetsAndAllAdditionalChangesNotFiredAndPropertiesNotSetsWhenNewValueEqualsOld(GameType oldGameTypeValue,
                                                                                                               GameType newGameTypeValue)
        {
            viewModel.NewGameType = oldGameTypeValue;
            tester.ChangesInvokes.Clear();
            var oldValue = viewModel.NewGameType;
            viewModel.IsNewGameForSingle = false;
            viewModel.IsNewGameForPair = false;
            viewModel.IsNewGameIsClassic = false;

            viewModel.NewGameType = newGameTypeValue;
            var newValue = viewModel.NewGameType;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            viewModel.IsNewGameForSingle.Should().BeFalse();
            viewModel.IsNewGameForPair.Should().BeFalse();
            viewModel.IsNewGameIsClassic.Should().BeFalse();
        }

        [Test]
        public void NewGamePointsSetsAndChangeFired()
        {
            var oldValue = viewModel.NewGamePoints;

            viewModel.NewGamePoints = GamePoints._1001;
            var newValue = viewModel.NewGamePoints;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(GamePoints._1001);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.NewGamePoints));
        }

        [Test]
        public void NewGameSetsSetsAndChangeFired()
        {
            var oldValue = viewModel.NewGameSets;

            viewModel.NewGameSets = 9;
            var newValue = viewModel.NewGameSets;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(9);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.NewGameSets));
        }

        [Test]
        public void NewGameLegsSetsAndChangeFired()
        {
            var oldValue = viewModel.NewGameLegs;

            viewModel.NewGameLegs = 9;
            var newValue = viewModel.NewGameLegs;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(9);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.NewGameLegs));
        }

        [Test]
        public void NewPlayerNameTextSetsAndChangeFired()
        {
            var oldValue = viewModel.NewPlayerNameText;

            viewModel.NewPlayerNameText = "SomeName";
            var newValue = viewModel.NewPlayerNameText;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("SomeName");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.NewPlayerNameText));
        }

        [Test]
        public void NewPlayerNickNameTextSetsAndChangeFired()
        {
            var oldValue = viewModel.NewPlayerNickNameText;

            viewModel.NewPlayerNickNameText = "SomeNickName";
            var newValue = viewModel.NewPlayerNickNameText;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("SomeNickName");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.NewPlayerNickNameText));
        }

        [Test]
        public void NewPlayerAvatarSetsAndChangeFired()
        {
            var oldValue = viewModel.NewPlayerAvatar;

            viewModel.NewPlayerAvatar = new BitmapImage();
            var newValue = viewModel.NewPlayerAvatar;

            newValue.Should().NotBe(oldValue);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.NewPlayerAvatar));
        }

        [Test]
        public void IsMainTabsEnabledSetsAndChangeFired()
        {
            var oldValue = viewModel.IsMainTabsEnabled;

            viewModel.IsMainTabsEnabled = true;
            var newValue = viewModel.IsMainTabsEnabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.IsMainTabsEnabled));
        }

        [Test]
        public void IsSetupTabsEnabledSetsAndChangeFired()
        {
            var oldValue = viewModel.IsSetupTabsEnabled;

            viewModel.IsSetupTabsEnabled = true;
            var newValue = viewModel.IsSetupTabsEnabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.IsSetupTabsEnabled));
        }

        [Test]
        public void IsCamsSetupRunningSetsAndChangeFired()
        {
            var oldValue = viewModel.IsCamsSetupRunning;

            viewModel.IsCamsSetupRunning = true;
            var newValue = viewModel.IsCamsSetupRunning;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.IsCamsSetupRunning));
        }

        [Test]
        public void IsGameRunningSetsAndChangeFired()
        {
            var oldValue = viewModel.IsGameRunning;

            viewModel.IsGameRunning = true;
            var newValue = viewModel.IsGameRunning;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.IsGameRunning));
        }

        [Test]
        public void IsRuntimeCrossingRunningSetsAndChangeFired()
        {
            var oldValue = viewModel.IsRuntimeCrossingRunning;

            viewModel.IsRuntimeCrossingRunning = true;
            var newValue = viewModel.IsRuntimeCrossingRunning;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.IsRuntimeCrossingRunning));
        }
    }
}