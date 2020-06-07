#region Usings

using System;
using FluentAssertions;
using NUnit.Framework;

#endregion

namespace OneHundredAndEightyCore.Tests.Converter
{
    public class WhenConvertingToInt : ConverterTestBase
    {
        [TestCase("5", ExpectedResult = 5)]
        [TestCase("-9", ExpectedResult = -9)]
        [TestCase("0", ExpectedResult = 0)]
        public int ConvertsWhenInputValueCorrect(string inputValue)
        {
            var convertedValue = Common.Converter.ToInt(inputValue);

            return convertedValue;
        }

        [TestCase("5.5")]
        [TestCase("9,7")]
        [TestCase("trash_!")]
        [TestCase("True")]
        public void ErrorOccuresWhenInputValueNotCorrect(string inputValue)
        {
            Action act = () => { Common.Converter.ToInt(inputValue); };

            act.Should().Throw<FormatException>();
        }
    }
}