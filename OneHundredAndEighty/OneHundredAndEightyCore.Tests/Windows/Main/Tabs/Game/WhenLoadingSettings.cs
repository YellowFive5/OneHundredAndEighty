#region Usings

using FluentAssertions;
using NUnit.Framework;
using OneHundredAndEightyCore.Enums;
using OneHundredAndEightyCore.Windows.Main.Tabs.Game;

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main.Tabs.Game
{
    public class WhenLoadingSettings : GameTabViewModelTestBase
    {
        [Test]
        public void NewGameSetsSetToDefault()
        {
            GameTabViewModel.NewGameSets = 99;

            GameTabViewModel.LoadSettings();

            GameTabViewModel.NewGameSets.Should().Be(GameTabViewModel.DefaultNewGameSetsValue);
        }

        [Test]
        public void NewGameLegsSetToDefault()
        {
            GameTabViewModel.NewGameLegs = 99;

            GameTabViewModel.LoadSettings();

            GameTabViewModel.NewGameLegs.Should().Be(GameTabViewModel.DefaultNewGameLegsValue);
        }

        [Test]
        public void NewGameTypeSetToDefault()
        {
            GameTabViewModel.NewGameType = GameType.FreeThrowsSingle;

            GameTabViewModel.LoadSettings();

            GameTabViewModel.NewGameType.Should().Be(GameTabViewModel.DefaultNewGameType);
        }

        [Test]
        public void NewGamePointsSetToDefault()
        {
            GameTabViewModel.NewGamePoints = GamePoints.Free;

            GameTabViewModel.LoadSettings();

            GameTabViewModel.NewGamePoints.Should().Be(GameTabViewModel.DefaultNewGamePoints);
        }
    }
}