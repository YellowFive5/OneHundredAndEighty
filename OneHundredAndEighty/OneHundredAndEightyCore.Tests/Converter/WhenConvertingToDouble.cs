#region Usings

using System;
using FluentAssertions;
using NUnit.Framework;

#endregion

namespace OneHundredAndEightyCore.Tests.Converter
{
    public class WhenConvertingToDouble : ConverterTestBase
    {
        [TestCase("5.5", 5.5d)]
        [TestCase("-9.9", -9.9d)]
        [TestCase("0", 0d)]
        [TestCase("5", 5d)]
        [TestCase("5.0", 5d)]
        [TestCase("-9", -9d)]
        [TestCase("-9.0", -9d)]
        public void ConvertsWhenInputValueCorrect(string inputValue, double expected)
        {
            var convertedValue = Common.Converter.ToDouble(inputValue);

            convertedValue.Should().Be(expected);
        }

        [TestCase("trash_!")]
        [TestCase("True")]
        public void ErrorOccuresWhenInputValueNotCorrect(string inputValue)
        {
            Action act = () => { Common.Converter.ToDouble(inputValue); };

            act.Should().Throw<FormatException>();
        }

        [TestCase("9,7")]
        public void WrongConvertBecauseOfInvariantCulture(string inputValue)
        {
            var convertedValue = Common.Converter.ToDouble(inputValue);

            convertedValue.Should().Be(97);
        }
    }
}