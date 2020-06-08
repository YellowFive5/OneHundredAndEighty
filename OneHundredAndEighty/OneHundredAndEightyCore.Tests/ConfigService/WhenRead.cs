#region Usings

using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Tests.ConfigService
{
    public class WhenRead : ConfigServiceTestBase
    {
        protected override void Setup()
        {
            base.Setup();

            dbService = new Mock<IDBService>();
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam1ThresholdSlider))).Returns("1.2");
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam1SurfaceCenterSlider))).Returns("6.232");
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam1RoiHeightSlider))).Returns("445.2326656");
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam1X))).Returns("55");
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam1CheckBox))).Returns("True");
            dbService.Setup(x => x.SettingsGetValue(It.Is<SettingsType>(s => s == SettingsType.Cam1Id))).Returns("82TUV3");

            configService = new Common.ConfigService(logger.Object,
                                                     dbService.Object);
        }

        [Test]
        public void DoubleSettingsReturns()
        {
            var doubleValue = configService.Read<double>(SettingsType.Cam1ThresholdSlider);

            doubleValue.Should().Be(1.2d);
        }

        [Test]
        public void DecimalSettingsReturns()
        {
            var decimalValue = configService.Read<decimal>(SettingsType.Cam1SurfaceCenterSlider);

            decimalValue.Should().Be(6.232m);
        }

        [Test]
        public void FloatSettingsReturns()
        {
            var floatValue = configService.Read<float>(SettingsType.Cam1RoiHeightSlider);

            floatValue.Should().Be(445.2326656f);
        }

        [Test]
        public void IntSettingsReturns()
        {
            var intValue = configService.Read<int>(SettingsType.Cam1X);

            intValue.Should().Be(55);
        }

        [Test]
        public void BoolSettingsReturns()
        {
            var boolValue = configService.Read<bool>(SettingsType.Cam1CheckBox);

            boolValue.Should().Be(true);
        }

        [Test]
        public void StringSettingsReturns()
        {
            var stringValue = configService.Read<string>(SettingsType.Cam1Id);

            stringValue.Should().Be("82TUV3");
        }

        [Test]
        public void ErrorOccuresWhenNotSupportedTypePassed()
        {
            Action act = () => { configService.Read<byte>(SettingsType.Cam1Id); };

            act.Should().Throw<FormatException>();
        }
    }
}