#region Usings

using NUnit.Framework;
using OneHundredAndEightyCore.Game;

#endregion

namespace OneHundredAndEightyCore.Tests.Converter
{
    public class WhenConvertingNewGameControls : ConverterTestBase
    {
        [Ignore("temp - because don't know how to test")]
        [TestCase("FreeThrowsSingle", ExpectedResult = GameType.FreeThrowsSingle)]
        [TestCase("FreeThrowsDouble", ExpectedResult = GameType.FreeThrowsDouble)]
        [TestCase("Classic", ExpectedResult = GameType.Classic)]
        public GameType ConvertsCorrectly(string selectedItem)
        {
            return GameType.Classic;
        }
    }
}