#region Usings

using NUnit.Framework;

#endregion

namespace OneHundredAndEightyCore.Tests.Converter
{
    public class WhenConvertingCamSetupSectorSettingValue : ConverterTestBase
    {
        [TestCase("11", ExpectedResult = 0)]
        [TestCase("11/14", ExpectedResult = 1)]
        [TestCase("14", ExpectedResult = 2)]
        [TestCase("14/9", ExpectedResult = 3)]
        [TestCase("9", ExpectedResult = 4)]
        [TestCase("9/12", ExpectedResult = 5)]
        [TestCase("12", ExpectedResult = 6)]
        [TestCase("12/5", ExpectedResult = 7)]
        [TestCase("5", ExpectedResult = 8)]
        [TestCase("5/20", ExpectedResult = 9)]
        [TestCase("20", ExpectedResult = 10)]
        [TestCase("20/1", ExpectedResult = 11)]
        [TestCase("1", ExpectedResult = 12)]
        [TestCase("1/18", ExpectedResult = 13)]
        [TestCase("18", ExpectedResult = 14)]
        [TestCase("18/4", ExpectedResult = 15)]
        [TestCase("4", ExpectedResult = 16)]
        [TestCase("4/13", ExpectedResult = 17)]
        [TestCase("13", ExpectedResult = 18)]
        [TestCase("13/6", ExpectedResult = 19)]
        [TestCase("6", ExpectedResult = 20)]
        [TestCase("6/10", ExpectedResult = 21)]
        [TestCase("10", ExpectedResult = 22)]
        [TestCase("10/15", ExpectedResult = 23)]
        [TestCase("15", ExpectedResult = 24)]
        [TestCase("15/2", ExpectedResult = 25)]
        [TestCase("2", ExpectedResult = 26)]
        [TestCase("2/17", ExpectedResult = 27)]
        [TestCase("17", ExpectedResult = 28)]
        [TestCase("17/3", ExpectedResult = 29)]
        [TestCase("3", ExpectedResult = 30)]
        [TestCase("3/19", ExpectedResult = 31)]
        [TestCase("19", ExpectedResult = 32)]
        [TestCase("19/7", ExpectedResult = 33)]
        [TestCase("7", ExpectedResult = 34)]
        [TestCase("7/16", ExpectedResult = 35)]
        [TestCase("16", ExpectedResult = 36)]
        [TestCase("16/8", ExpectedResult = 37)]
        [TestCase("8", ExpectedResult = 38)]
        [TestCase("8/11", ExpectedResult = 39)]
        [TestCase("trash", ExpectedResult = -1)]
        public int ConvertsWhenInputValueCorrect(string camSetupSectorSettingValue)
        {
            var convertedValue = Common.Converter.CamSetupSectorSettingValueToComboboxSelectedIndex(camSetupSectorSettingValue);

            return convertedValue;
        }
    }
}