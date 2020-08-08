#region Usings

using FluentAssertions;
using NUnit.Framework;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main.Tabs.Game
{
    public class WhenSettingBindableProperties : GameTabViewModelTestBase
    {
        private NotifyPropertyChangedTester tester;

        protected override void Setup()
        {
            base.Setup();
            tester = new NotifyPropertyChangedTester(GameTabViewModel);
        }

        [TestCase(GameType.FreeThrowsSingle, GameType.FreeThrowsDouble, false, true, false)]
        [TestCase(GameType.FreeThrowsSingle, GameType.Classic, false, true, true)]
        [TestCase(GameType.FreeThrowsDouble, GameType.FreeThrowsSingle, true, false, false)]
        [TestCase(GameType.FreeThrowsDouble, GameType.Classic, false, true, true)]
        [TestCase(GameType.Classic, GameType.FreeThrowsSingle, true, false, false)]
        [TestCase(GameType.Classic, GameType.FreeThrowsDouble, false, true, false)]
        public void NewGameTypeSetsAndAllAdditionalChangesFiredAndPropertiesSetsWhenNewValueNotEqualsOld(GameType oldGameTypeValue,
                                                                                                         GameType newGameTypeValue,
                                                                                                         bool isNewGameForSingleExpectedValue,
                                                                                                         bool isNewGameForPairExpectedValue,
                                                                                                         bool isNewGameIsClassicExpectedValue)
        {
            GameTabViewModel.NewGameType = oldGameTypeValue;
            tester.ChangesInvokes.Clear();

            GameTabViewModel.NewGameType = newGameTypeValue;
            var newValue = GameTabViewModel.NewGameType;

            newValue.Should().NotBe(oldGameTypeValue);
            newValue.Should().Be(newGameTypeValue);
            tester.AssertOnPropertyChangedInvoke(0, nameof(GameTabViewModel.NewGameType));
            tester.AssertOnPropertyChangedInvoke(1, nameof(GameTabViewModel.IsNewGameForSingle));
            tester.AssertOnPropertyChangedInvoke(2, nameof(GameTabViewModel.IsNewGameForPair));
            tester.AssertOnPropertyChangedInvoke(3, nameof(GameTabViewModel.IsNewGameIsClassic));
            GameTabViewModel.IsNewGameForSingle = isNewGameForSingleExpectedValue;
            GameTabViewModel.IsNewGameForPair = isNewGameForPairExpectedValue;
            GameTabViewModel.IsNewGameIsClassic = isNewGameIsClassicExpectedValue;
        }

        [TestCase(GameType.FreeThrowsSingle, GameType.FreeThrowsSingle)]
        [TestCase(GameType.FreeThrowsDouble, GameType.FreeThrowsDouble)]
        [TestCase(GameType.Classic, GameType.Classic)]
        public void NewGameTypeNotSetsAndAllAdditionalChangesNotFiredAndPropertiesNotSetsWhenNewValueEqualsOld(GameType oldGameTypeValue,
                                                                                                               GameType newGameTypeValue)
        {
            GameTabViewModel.NewGameType = oldGameTypeValue;
            tester.ChangesInvokes.Clear();
            var oldValue = GameTabViewModel.NewGameType;
            GameTabViewModel.IsNewGameForSingle = false;
            GameTabViewModel.IsNewGameForPair = false;
            GameTabViewModel.IsNewGameIsClassic = false;

            GameTabViewModel.NewGameType = newGameTypeValue;
            var newValue = GameTabViewModel.NewGameType;

            newValue.Should().Be(oldValue);
            tester.ChangesInvokes.Should().BeEmpty();
            GameTabViewModel.IsNewGameForSingle.Should().BeFalse();
            GameTabViewModel.IsNewGameForPair.Should().BeFalse();
            GameTabViewModel.IsNewGameIsClassic.Should().BeFalse();
        }

        [Test]
        public void NewGamePointsSetsAndChangeFired()
        {
            var oldValue = GameTabViewModel.NewGamePoints;

            GameTabViewModel.NewGamePoints = GamePoints._1001;
            var newValue = GameTabViewModel.NewGamePoints;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(GamePoints._1001);
            tester.AssertOnPropertyChangedInvoke(0, nameof(GameTabViewModel.NewGamePoints));
        }

        [Test]
        public void NewGameSetsSetsAndChangeFired()
        {
            var oldValue = GameTabViewModel.NewGameSets;

            GameTabViewModel.NewGameSets = 9;
            var newValue = GameTabViewModel.NewGameSets;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(9);
            tester.AssertOnPropertyChangedInvoke(0, nameof(GameTabViewModel.NewGameSets));
        }

        [Test]
        public void NewGameLegsSetsAndChangeFired()
        {
            var oldValue = GameTabViewModel.NewGameLegs;

            GameTabViewModel.NewGameLegs = 9;
            var newValue = GameTabViewModel.NewGameLegs;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(9);
            tester.AssertOnPropertyChangedInvoke(0, nameof(GameTabViewModel.NewGameLegs));
        }

        [Test]
        public void IsGameRunningSetsAndChangeFired()
        {
            var oldValue = GameTabViewModel.IsGameRunning;

            GameTabViewModel.IsGameRunning = true;
            var newValue = GameTabViewModel.IsGameRunning;

            oldValue.Should().BeFalse();
            newValue.Should().BeTrue();
            tester.AssertOnPropertyChangedInvoke(0, nameof(GameTabViewModel.IsGameRunning));
        }
    }
}