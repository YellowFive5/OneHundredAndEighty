#region Usings

using System;
using FluentAssertions;
using NUnit.Framework;

#endregion

namespace OneHundredAndEightyCore.Tests.Converter
{
    public class WhenConvertingToBool : ConverterTestBase
    {
        [TestCase("True", ExpectedResult = true)]
        [TestCase("False", ExpectedResult = false)]
        [TestCase("true", ExpectedResult = true)]
        [TestCase("false", ExpectedResult = false)]
        public bool ConvertsWhenInputValueCorrect(string inputValue)
        {
            var convertedValue = Common.Converter.ToBool(inputValue);

            return convertedValue;
        }

        [Test]
        public void ErrorOccuredWhenInputValueNotCorrect()
        {
            Action act = () => { Common.Converter.ToBool("trash_!"); };

            act.Should().Throw<FormatException>();
        }
    }
}