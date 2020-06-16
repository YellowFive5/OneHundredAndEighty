#region Usings

using System.Collections.Generic;
using System.Windows.Media.Imaging;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Windows.Score;

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
        public void Cam1ImageSetsAndChangeFired()
        {
            var oldValue = viewModel.CamImage;

            viewModel.CamImage = new BitmapImage();
            var newValue = viewModel.CamImage;

            newValue.Should().NotBe(oldValue);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.CamImage));
        }

        [Test]
        public void Cam2ImageSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam2Image;

            viewModel.Cam2Image = new BitmapImage();
            var newValue = viewModel.Cam2Image;

            newValue.Should().NotBe(oldValue);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2Image));
        }

        [Test]
        public void Cam3ImageSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam3Image;

            viewModel.Cam3Image = new BitmapImage();
            var newValue = viewModel.Cam3Image;

            newValue.Should().NotBe(oldValue);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3Image));
        }

        [Test]
        public void Cam4ImageSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam4Image;

            viewModel.Cam4Image = new BitmapImage();
            var newValue = viewModel.Cam4Image;

            newValue.Should().NotBe(oldValue);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4Image));
        }

        [Test]
        public void Cam1RoiImageSetsAndChangeFired()
        {
            var oldValue = viewModel.CamRoiImage;

            viewModel.CamRoiImage = new BitmapImage();
            var newValue = viewModel.CamRoiImage;

            newValue.Should().NotBe(oldValue);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.CamRoiImage));
        }

        [Test]
        public void Cam2RoiImageSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam2RoiImage;

            viewModel.Cam2RoiImage = new BitmapImage();
            var newValue = viewModel.Cam2RoiImage;

            newValue.Should().NotBe(oldValue);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2RoiImage));
        }

        [Test]
        public void Cam3RoiImageSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam3RoiImage;

            viewModel.Cam3RoiImage = new BitmapImage();
            var newValue = viewModel.Cam3RoiImage;

            newValue.Should().NotBe(oldValue);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3RoiImage));
        }

        [Test]
        public void Cam4RoiImageSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam4RoiImage;

            viewModel.Cam4RoiImage = new BitmapImage();
            var newValue = viewModel.Cam4RoiImage;

            newValue.Should().NotBe(oldValue);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4RoiImage));
        }

        [Test]
        public void Cam1ThresholdSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam1ThresholdSliderValue;

            viewModel.Cam1ThresholdSliderValue = 555.05;
            var newValue = viewModel.Cam1ThresholdSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1ThresholdSliderValue));
        }

        [Test]
        public void Cam2ThresholdSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam2ThresholdSliderValue;

            viewModel.Cam2ThresholdSliderValue = 555.05;
            var newValue = viewModel.Cam2ThresholdSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2ThresholdSliderValue));
        }

        [Test]
        public void Cam3ThresholdSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam3ThresholdSliderValue;

            viewModel.Cam3ThresholdSliderValue = 555.05;
            var newValue = viewModel.Cam3ThresholdSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3ThresholdSliderValue));
        }

        [Test]
        public void Cam4ThresholdSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam4ThresholdSliderValue;

            viewModel.Cam4ThresholdSliderValue = 555.05;
            var newValue = viewModel.Cam4ThresholdSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4ThresholdSliderValue));
        }

        [Test]
        public void Cam1SurfaceSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam1SurfaceSliderValue;

            viewModel.Cam1SurfaceSliderValue = 555.05;
            var newValue = viewModel.Cam1SurfaceSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1SurfaceSliderValue));
        }

        [Test]
        public void Cam2SurfaceSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam2SurfaceSliderValue;

            viewModel.Cam2SurfaceSliderValue = 555.05;
            var newValue = viewModel.Cam2SurfaceSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2SurfaceSliderValue));
        }

        [Test]
        public void Cam3SurfaceSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam3SurfaceSliderValue;

            viewModel.Cam3SurfaceSliderValue = 555.05;
            var newValue = viewModel.Cam3SurfaceSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3SurfaceSliderValue));
        }

        [Test]
        public void Cam4SurfaceSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam4SurfaceSliderValue;

            viewModel.Cam4SurfaceSliderValue = 555.05;
            var newValue = viewModel.Cam4SurfaceSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4SurfaceSliderValue));
        }

        [Test]
        public void Cam1SurfaceCenterSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam1SurfaceCenterSliderValue;

            viewModel.Cam1SurfaceCenterSliderValue = 555.05;
            var newValue = viewModel.Cam1SurfaceCenterSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1SurfaceCenterSliderValue));
        }

        [Test]
        public void Cam2SurfaceCenterSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam2SurfaceCenterSliderValue;

            viewModel.Cam2SurfaceCenterSliderValue = 555.05;
            var newValue = viewModel.Cam2SurfaceCenterSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2SurfaceCenterSliderValue));
        }

        [Test]
        public void Cam3SurfaceCenterSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam3SurfaceCenterSliderValue;

            viewModel.Cam3SurfaceCenterSliderValue = 555.05;
            var newValue = viewModel.Cam3SurfaceCenterSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3SurfaceCenterSliderValue));
        }

        [Test]
        public void Cam4SurfaceCenterSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam4SurfaceCenterSliderValue;

            viewModel.Cam4SurfaceCenterSliderValue = 555.05;
            var newValue = viewModel.Cam4SurfaceCenterSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4SurfaceCenterSliderValue));
        }

        [Test]
        public void Cam1RoiPosYSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam1RoiPosYSliderValue;

            viewModel.Cam1RoiPosYSliderValue = 555.05;
            var newValue = viewModel.Cam1RoiPosYSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1RoiPosYSliderValue));
        }

        [Test]
        public void Cam2RoiPosYSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam2RoiPosYSliderValue;

            viewModel.Cam2RoiPosYSliderValue = 555.05;
            var newValue = viewModel.Cam2RoiPosYSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2RoiPosYSliderValue));
        }

        [Test]
        public void Cam3RoiPosYSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam3RoiPosYSliderValue;

            viewModel.Cam3RoiPosYSliderValue = 555.05;
            var newValue = viewModel.Cam3RoiPosYSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3RoiPosYSliderValue));
        }

        [Test]
        public void Cam4RoiPosYSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam4RoiPosYSliderValue;

            viewModel.Cam4RoiPosYSliderValue = 555.05;
            var newValue = viewModel.Cam4RoiPosYSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4RoiPosYSliderValue));
        }

        [Test]
        public void Cam1RoiHeightSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam1RoiHeightSliderValue;

            viewModel.Cam1RoiHeightSliderValue = 555.05;
            var newValue = viewModel.Cam1RoiHeightSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1RoiHeightSliderValue));
        }

        [Test]
        public void Cam2RoiHeightSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam2RoiHeightSliderValue;

            viewModel.Cam2RoiHeightSliderValue = 555.05;
            var newValue = viewModel.Cam2RoiHeightSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2RoiHeightSliderValue));
        }

        [Test]
        public void Cam3RoiHeightSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam3RoiHeightSliderValue;

            viewModel.Cam3RoiHeightSliderValue = 555.05;
            var newValue = viewModel.Cam3RoiHeightSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3RoiHeightSliderValue));
        }

        [Test]
        public void Cam4RoiHeightSliderValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam4RoiHeightSliderValue;

            viewModel.Cam4RoiHeightSliderValue = 555.05;
            var newValue = viewModel.Cam4RoiHeightSliderValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4RoiHeightSliderValue));
        }

        [Test]
        public void Cam1EnabledValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.Cam1Enabled;

            viewModel.Cam1Enabled = true;
            var newValue = viewModel.Cam1Enabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1Enabled));
            configService.Verify(w => w.Write(SettingsType.Cam1CheckBox, true), Times.Once);
        }

        [Test]
        public void Cam1EnabledValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.Cam1Enabled;

            viewModel.Cam1Enabled = oldValue;
            var newValue = viewModel.Cam1Enabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeFalse();
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void Cam2EnabledValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.Cam2Enabled;

            viewModel.Cam2Enabled = true;
            var newValue = viewModel.Cam2Enabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2Enabled));
            configService.Verify(w => w.Write(SettingsType.Cam2CheckBox, true), Times.Once);
        }

        [Test]
        public void Cam2EnabledValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.Cam2Enabled;

            viewModel.Cam2Enabled = oldValue;
            var newValue = viewModel.Cam2Enabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeFalse();
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void Cam3EnabledValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.Cam3Enabled;

            viewModel.Cam3Enabled = true;
            var newValue = viewModel.Cam3Enabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3Enabled));
            configService.Verify(w => w.Write(SettingsType.Cam3CheckBox, true), Times.Once);
        }

        [Test]
        public void Cam3EnabledValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.Cam3Enabled;

            viewModel.Cam3Enabled = oldValue;
            var newValue = viewModel.Cam3Enabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeFalse();
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void Cam4EnabledValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.Cam4Enabled;

            viewModel.Cam4Enabled = true;
            var newValue = viewModel.Cam4Enabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4Enabled));
            configService.Verify(w => w.Write(SettingsType.Cam4CheckBox, true), Times.Once);
        }

        [Test]
        public void Cam4EnabledValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.Cam4Enabled;

            viewModel.Cam4Enabled = oldValue;
            var newValue = viewModel.Cam4Enabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeFalse();
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void DetectionEnabledValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.DetectionEnabled;

            viewModel.DetectionEnabled = true;
            var newValue = viewModel.DetectionEnabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.DetectionEnabled));
            configService.Verify(w => w.Write(SettingsType.WithDetectionCheckBox, true), Times.Once);
        }

        [Test]
        public void DetectionEnabledValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.DetectionEnabled;

            viewModel.DetectionEnabled = oldValue;
            var newValue = viewModel.DetectionEnabled;

            oldValue.Should().BeFalse();
            newValue.Should().BeFalse();
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void Cam1IdValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.Cam1Id;

            viewModel.Cam1Id = "23fer45";
            var newValue = viewModel.Cam1Id;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("23fer45");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1Id));
            configService.Verify(w => w.Write(SettingsType.Cam1Id, "23fer45"), Times.Once);
        }

        [Test]
        public void Cam1IdValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.Cam1Id;

            viewModel.Cam1Id = oldValue;
            var newValue = viewModel.Cam1Id;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void Cam2IdValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.Cam2Id;

            viewModel.Cam2Id = "23fer45";
            var newValue = viewModel.Cam2Id;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("23fer45");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2Id));
            configService.Verify(w => w.Write(SettingsType.Cam2Id, "23fer45"), Times.Once);
        }

        [Test]
        public void Cam2IdValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.Cam2Id;

            viewModel.Cam2Id = oldValue;
            var newValue = viewModel.Cam2Id;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void Cam3IdValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.Cam3Id;

            viewModel.Cam3Id = "23fer45";
            var newValue = viewModel.Cam3Id;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("23fer45");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3Id));
            configService.Verify(w => w.Write(SettingsType.Cam3Id, "23fer45"), Times.Once);
        }

        [Test]
        public void Cam3IdValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.Cam3Id;

            viewModel.Cam3Id = oldValue;
            var newValue = viewModel.Cam3Id;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void Cam4IdValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.Cam3Id;

            viewModel.Cam4Id = "23fer45";
            var newValue = viewModel.Cam4Id;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("23fer45");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4Id));
            configService.Verify(w => w.Write(SettingsType.Cam4Id, "23fer45"), Times.Once);
        }

        [Test]
        public void Cam4IdValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.Cam4Id;

            viewModel.Cam4Id = oldValue;
            var newValue = viewModel.Cam4Id;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void CamsFovAngleValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.CamsFovAngle;

            viewModel.CamsFovAngle = 155.25;
            var newValue = viewModel.CamsFovAngle;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(155.25);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.CamsFovAngle));
            configService.Verify(w => w.Write(SettingsType.CamFovAngle, 155.25), Times.Once);
        }

        [Test]
        public void CamsFovAngleValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.CamsFovAngle;

            viewModel.CamsFovAngle = oldValue;
            var newValue = viewModel.CamsFovAngle;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void CamsResolutionHeightValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.CamsResolutionHeight;

            viewModel.CamsResolutionHeight = 1028;
            var newValue = viewModel.CamsResolutionHeight;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(1028);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.CamsResolutionHeight));
            configService.Verify(w => w.Write(SettingsType.ResolutionHeight, 1028), Times.Once);
        }

        [Test]
        public void CamsResolutionHeightValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.CamsResolutionHeight;

            viewModel.CamsResolutionHeight = oldValue;
            var newValue = viewModel.CamsResolutionHeight;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void CamsResolutionWidthValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.CamsResolutionWidth;

            viewModel.CamsResolutionWidth = 768;
            var newValue = viewModel.CamsResolutionWidth;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(768);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.CamsResolutionWidth));
            configService.Verify(w => w.Write(SettingsType.ResolutionWidth, 768), Times.Once);
        }

        [Test]
        public void CamsResolutionWidthValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.CamsResolutionWidth;

            viewModel.CamsResolutionWidth = oldValue;
            var newValue = viewModel.CamsResolutionWidth;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void MovesExtractionValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.MovesExtractionValue;

            viewModel.MovesExtractionValue = 1000;
            var newValue = viewModel.MovesExtractionValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(1000);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.MovesExtractionValue));
            configService.Verify(w => w.Write(SettingsType.MovesExtraction, 1000), Times.Once);
        }

        [Test]
        public void MovesExtractionValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.MovesExtractionValue;

            viewModel.MovesExtractionValue = oldValue;
            var newValue = viewModel.MovesExtractionValue;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void MovesDetectedSleepTimeValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.MovesDetectedSleepTimeValue;

            viewModel.MovesDetectedSleepTimeValue = 0.25;
            var newValue = viewModel.MovesDetectedSleepTimeValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(0.25);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.MovesDetectedSleepTimeValue));
            configService.Verify(w => w.Write(SettingsType.MoveDetectedSleepTime, 0.25), Times.Once);
        }

        [Test]
        public void MovesDetectedSleepTimeValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.MovesDetectedSleepTimeValue;

            viewModel.MovesDetectedSleepTimeValue = oldValue;
            var newValue = viewModel.MovesDetectedSleepTimeValue;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void MovesNoiseValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.MovesNoiseValue;

            viewModel.MovesNoiseValue = 2500;
            var newValue = viewModel.MovesNoiseValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(2500);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.MovesNoiseValue));
            configService.Verify(w => w.Write(SettingsType.MovesNoise, 2500), Times.Once);
        }

        [Test]
        public void MovesNoiseValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.MovesNoiseValue;

            viewModel.MovesNoiseValue = oldValue;
            var newValue = viewModel.MovesNoiseValue;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void SmoothGaussValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.SmoothGaussValue;

            viewModel.SmoothGaussValue = 5;
            var newValue = viewModel.SmoothGaussValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(5);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.SmoothGaussValue));
            configService.Verify(w => w.Write(SettingsType.SmoothGauss, 5), Times.Once);
        }

        [Test]
        public void SmoothGaussValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.SmoothGaussValue;

            viewModel.SmoothGaussValue = oldValue;
            var newValue = viewModel.SmoothGaussValue;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void ThresholdSleepTimeValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.ThresholdSleepTimeValue;

            viewModel.ThresholdSleepTimeValue = 0.5;
            var newValue = viewModel.ThresholdSleepTimeValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(0.5);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.ThresholdSleepTimeValue));
            configService.Verify(w => w.Write(SettingsType.ThresholdSleepTime, 0.5), Times.Once);
        }

        [Test]
        public void ThresholdSleepTimeValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.ThresholdSleepTimeValue;

            viewModel.ThresholdSleepTimeValue = oldValue;
            var newValue = viewModel.ThresholdSleepTimeValue;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void ExtractionSleepTimeValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.ExtractionSleepTimeValue;

            viewModel.ExtractionSleepTimeValue = 0.56;
            var newValue = viewModel.ExtractionSleepTimeValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(0.56);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.ExtractionSleepTimeValue));
            configService.Verify(w => w.Write(SettingsType.ExtractionSleepTime, 0.56), Times.Once);
        }

        [Test]
        public void ExtractionSleepTimeValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.ExtractionSleepTimeValue;

            viewModel.ExtractionSleepTimeValue = oldValue;
            var newValue = viewModel.ExtractionSleepTimeValue;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void MinContourArcValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.MinContourArcValue;

            viewModel.MinContourArcValue = 700;
            var newValue = viewModel.MinContourArcValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(700);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.MinContourArcValue));
            configService.Verify(w => w.Write(SettingsType.MinContourArc, 700), Times.Once);
        }

        [Test]
        public void MinContourArcValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.MinContourArcValue;

            viewModel.MinContourArcValue = oldValue;
            var newValue = viewModel.MinContourArcValue;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void MovesDartValueSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.MovesDartValue;

            viewModel.MovesDartValue = 500;
            var newValue = viewModel.MovesDartValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(500);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.MovesDartValue));
            configService.Verify(w => w.Write(SettingsType.MovesDart, 500), Times.Once);
        }

        [Test]
        public void MovesDartValueNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.MovesDartValue;

            viewModel.MovesDartValue = oldValue;
            var newValue = viewModel.MovesDartValue;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void ToCam1DistanceSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.ToCam1Distance;

            viewModel.ToCam1Distance = 35.5;
            var newValue = viewModel.ToCam1Distance;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(35.5);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.ToCam1Distance));
            configService.Verify(w => w.Write(SettingsType.ToCam1Distance, 35.5), Times.Once);
        }

        [Test]
        public void ToCam1DistanceNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.ToCam1Distance;

            viewModel.ToCam1Distance = oldValue;
            var newValue = viewModel.ToCam1Distance;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void ToCam2DistanceSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.ToCam2Distance;

            viewModel.ToCam2Distance = 35.5;
            var newValue = viewModel.ToCam2Distance;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(35.5);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.ToCam2Distance));
            configService.Verify(w => w.Write(SettingsType.ToCam2Distance, 35.5), Times.Once);
        }

        [Test]
        public void ToCam2DistanceNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.ToCam2Distance;

            viewModel.ToCam2Distance = oldValue;
            var newValue = viewModel.ToCam2Distance;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void ToCam3DistanceSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.ToCam3Distance;

            viewModel.ToCam3Distance = 35.5;
            var newValue = viewModel.ToCam3Distance;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(35.5);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.ToCam3Distance));
            configService.Verify(w => w.Write(SettingsType.ToCam3Distance, 35.5), Times.Once);
        }

        [Test]
        public void ToCam3DistanceNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.ToCam3Distance;

            viewModel.ToCam3Distance = oldValue;
            var newValue = viewModel.ToCam3Distance;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void ToCam4DistanceSetsAndChangeFiredAndSettingSavedWhenNewValueNotEqualsOldValue()
        {
            var oldValue = viewModel.ToCam4Distance;

            viewModel.ToCam4Distance = 35.5;
            var newValue = viewModel.ToCam4Distance;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(35.5);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.ToCam4Distance));
            configService.Verify(w => w.Write(SettingsType.ToCam4Distance, 35.5), Times.Once);
        }

        [Test]
        public void ToCam4DistanceNotSetsAndNotChangeFiredAndSettingNotSavedWhenNewValueEqualsOldValue()
        {
            var oldValue = viewModel.ToCam4Distance;

            viewModel.ToCam4Distance = oldValue;
            var newValue = viewModel.ToCam4Distance;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            configService.Verify(w => w.Write(It.IsAny<SettingsType>(),
                                              It.IsAny<object>()),
                                 Times.Never);
        }

        [Test]
        public void Cam1SetupXValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam1SetupXValue;

            viewModel.Cam1SetupXValue = 89;
            var newValue = viewModel.Cam1SetupXValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(89);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1SetupXValue));
        }

        [Test]
        public void Cam1SetupYValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam1SetupYValue;

            viewModel.Cam1SetupYValue = 89;
            var newValue = viewModel.Cam1SetupYValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(89);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1SetupYValue));
        }

        [Test]
        public void Cam2SetupXValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam2SetupXValue;

            viewModel.Cam2SetupXValue = 89;
            var newValue = viewModel.Cam2SetupXValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(89);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2SetupXValue));
        }

        [Test]
        public void Cam2SetupYValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam2SetupXValue;

            viewModel.Cam2SetupYValue = 89;
            var newValue = viewModel.Cam2SetupYValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(89);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2SetupYValue));
        }

        [Test]
        public void Cam3SetupXValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam3SetupXValue;

            viewModel.Cam3SetupXValue = 89;
            var newValue = viewModel.Cam3SetupXValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(89);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3SetupXValue));
        }

        [Test]
        public void Cam3SetupYValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam3SetupYValue;

            viewModel.Cam3SetupYValue = 89;
            var newValue = viewModel.Cam3SetupYValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(89);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3SetupYValue));
        }

        [Test]
        public void Cam4SetupXValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam4SetupXValue;

            viewModel.Cam4SetupXValue = 89;
            var newValue = viewModel.Cam4SetupXValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(89);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4SetupXValue));
        }

        [Test]
        public void Cam4SetupYValueSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam4SetupYValue;

            viewModel.Cam4SetupYValue = 89;
            var newValue = viewModel.Cam4SetupYValue;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(89);
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4SetupYValue));
        }

        [Test]
        public void Cam1SetupSectorSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam1SetupSector;

            viewModel.Cam1SetupSector = "5/20";
            var newValue = viewModel.Cam1SetupSector;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("5/20");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam1SetupSector));
        }

        [Test]
        public void Cam2SetupSectorSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam2SetupSector;

            viewModel.Cam2SetupSector = "5/20";
            var newValue = viewModel.Cam2SetupSector;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("5/20");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam2SetupSector));
        }

        [Test]
        public void Cam3SetupSectorSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam3SetupSector;

            viewModel.Cam3SetupSector = "5/20";
            var newValue = viewModel.Cam3SetupSector;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("5/20");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam3SetupSector));
        }

        [Test]
        public void Cam4SetupSectorSetsAndChangeFired()
        {
            var oldValue = viewModel.Cam4SetupSector;

            viewModel.Cam4SetupSector = "5/20";
            var newValue = viewModel.Cam4SetupSector;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("5/20");
            tester.AssertOnPropertyChangedInvoke(0, nameof(viewModel.Cam4SetupSector));
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