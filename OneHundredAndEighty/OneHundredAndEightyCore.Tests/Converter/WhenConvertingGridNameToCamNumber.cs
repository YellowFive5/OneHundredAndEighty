#region Usings

using System;
using FluentAssertions;
using NUnit.Framework;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Tests.Converter
{
    public class WhenConvertingGridNameToCamNumber : ConverterTestBase
    {
        [TestCase("Cam1Grid", ExpectedResult = CamNumber._1)]
        [TestCase("Cam2Grid", ExpectedResult = CamNumber._2)]
        [TestCase("Cam3Grid", ExpectedResult = CamNumber._3)]
        [TestCase("Cam4Grid", ExpectedResult = CamNumber._4)]
        public CamNumber ConvertsWhenInputValueCorrect(string gridName)
        {
            var convertedValue = Common.Converter.GridNameToCamNumber(gridName);

            return convertedValue;
        }

        [Test]
        public void ErrorOccuredWhenInputValueNotCorrect()
        {
            Action act = () => { Common.Converter.GridNameToCamNumber("SomeTrash"); };

            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}