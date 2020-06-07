#region Usings

using System;
using FluentAssertions;
using NUnit.Framework;

#endregion

namespace OneHundredAndEightyCore.Tests.Converter
{
    public class WhenConvertingToDecimal : ConverterTestBase
    {
        [TestCase("5.5", 5.5)]
        [TestCase("-9.9", -9.9)]
        [TestCase("0", 0)]
        [TestCase("5", 5)]
        [TestCase("5.0", 5)]
        [TestCase("-9", -9)]
        [TestCase("-9.0", -9)]
        public void ConvertsWhenInputValueCorrect(string inputValue, decimal expected)
        {
            var convertedValue = Common.Converter.ToDecimal(inputValue);

            convertedValue.Should().Be(expected);
        }

        [TestCase("trash_!")]
        [TestCase("True")]
        public void ErrorOccuresWhenInputValueNotCorrect(string inputValue)
        {
            Action act = () => { Common.Converter.ToDecimal(inputValue); };

            act.Should().Throw<FormatException>();
        }

        [TestCase("9,7")]
        public void WrongConvertBecauseOfInvariantDecimal(string inputValue)
        {
            var convertedValue = Common.Converter.ToDecimal(inputValue);

            convertedValue.Should().Be(97);
        }
    }
}