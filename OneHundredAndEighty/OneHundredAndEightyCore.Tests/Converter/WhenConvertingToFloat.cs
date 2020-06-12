#region Usings

using System;
using FluentAssertions;
using NUnit.Framework;

#endregion

namespace OneHundredAndEightyCore.Tests.Converter
{
    public class WhenConvertingToFloat : ConverterTestBase
    {
        [TestCase("5.5", 5.5f)]
        [TestCase("-9.9", -9.9f)]
        [TestCase("0", 0f)]
        [TestCase("5", 5f)]
        [TestCase("5.0", 5f)]
        [TestCase("-9", -9f)]
        [TestCase("-9.0", -9f)]
        public void ConvertsWhenInputValueCorrect(string inputValue, float expected)
        {
            var convertedValue = Common.Converter.ToFloat(inputValue);

            convertedValue.Should().Be(expected);
        }

        [TestCase("trash_!")]
        [TestCase("True")]
        public void ErrorOccuredWhenInputValueNotCorrect(string inputValue)
        {
            Action act = () => { Common.Converter.ToFloat(inputValue); };

            act.Should().Throw<FormatException>();
        }

        [TestCase("9,7")]
        public void WrongConvertBecauseOfInvariantCulture(string inputValue)
        {
            var convertedValue = Common.Converter.ToFloat(inputValue);

            convertedValue.Should().Be(97);
        }
    }
}