#region Usings

using NUnit.Framework;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Tests.Converter
{
    public class WhenConvertingFromGamePointsToInt : ConverterTestBase
    {
        [TestCase(GamePoints.Free, ExpectedResult = 0)]
        [TestCase(GamePoints._301, ExpectedResult = 301)]
        [TestCase(GamePoints._501, ExpectedResult = 501)]
        [TestCase(GamePoints._701, ExpectedResult = 701)]
        [TestCase(GamePoints._1001, ExpectedResult = 1001)]
        public int ConvertsCorrectly(GamePoints points)
        {
            var convertedValue = Common.Converter.GamePointsToInt(points);

            return convertedValue;
        }
    }
}