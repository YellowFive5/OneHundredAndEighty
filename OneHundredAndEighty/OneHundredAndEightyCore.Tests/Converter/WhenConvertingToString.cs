#region Usings

using System;
using FluentAssertions;
using NUnit.Framework;

#endregion

namespace OneHundredAndEightyCore.Tests.Converter
{
    public class WhenConvertingToString : ConverterTestBase
    {
        [TestCase(true, ExpectedResult = "True")]
        [TestCase(false, ExpectedResult = "False")]
        [TestCase(5, ExpectedResult = "5")]
        [TestCase(0, ExpectedResult = "0")]
        [TestCase(-9, ExpectedResult = "-9")]
        [TestCase(0.0f, ExpectedResult = "0")]
        [TestCase(5.0f, ExpectedResult = "5")]
        [TestCase(5.5f, ExpectedResult = "5.5")]
        [TestCase(-9.9f, ExpectedResult = "-9.9")]
        [TestCase(-9.0f, ExpectedResult = "-9")]
        [TestCase(0.0d, ExpectedResult = "0")]
        [TestCase(5.0d, ExpectedResult = "5")]
        [TestCase(5.5d, ExpectedResult = "5.5")]
        [TestCase(-9.9d, ExpectedResult = "-9.9")]
        [TestCase(-9.0d, ExpectedResult = "-9")]
        public string ConvertsWhenInputValueCorrect(object inputValue)
        {
            var convertedValue = Common.Converter.ToString(inputValue);

            return convertedValue;
        }
    }
}