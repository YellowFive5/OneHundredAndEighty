#region Usings

using System;
using FluentAssertions;
using NUnit.Framework;
using OneHundredAndEightyCore.Game;

#endregion

namespace OneHundredAndEightyCore.Tests.Validator
{
    public class WhenValidating : ValidatorTestBase
    {
        [TestCase("", "", ExpectedResult = false)]
        [TestCase(" ", " ", ExpectedResult = false)]
        [TestCase(null, null, ExpectedResult = false)]
        [TestCase("", " ", ExpectedResult = false)]
        [TestCase("", null, ExpectedResult = false)]
        [TestCase(" ", "", ExpectedResult = false)]
        [TestCase(" ", null, ExpectedResult = false)]
        [TestCase(null, "", ExpectedResult = false)]
        [TestCase(null, " ", ExpectedResult = false)]
        [TestCase("", "The Power", ExpectedResult = false)]
        [TestCase(" ", "The Power", ExpectedResult = false)]
        [TestCase(null, "The Power", ExpectedResult = false)]
        [TestCase("Phil", "", ExpectedResult = false)]
        [TestCase("Phil", " ", ExpectedResult = false)]
        [TestCase("Phil", null, ExpectedResult = false)]
        [TestCase("Phil", "The Power", ExpectedResult = true)]
        public bool NewPlayerNameAndNickNameValidatesCorrectly(string name, string nickname)
        {
            var isValid = Common.Validator.ValidateNewPlayerNameAndNickName(name, nickname);

            return isValid;
        }

        [TestCase(999, 999, ExpectedResult = true)]
        [TestCase(1000, 1000, ExpectedResult = true)]
        [TestCase(999, 1000, ExpectedResult = true)]
        [TestCase(1000, 999, ExpectedResult = true)]
        [TestCase(1001, 1001, ExpectedResult = false)]
        [TestCase(1001, 1000, ExpectedResult = false)]
        [TestCase(1000, 1001, ExpectedResult = false)]
        [TestCase(1001, 999, ExpectedResult = false)]
        [TestCase(999, 1001, ExpectedResult = false)]
        public bool NewPlayerAvatarValidatesCorrectly(int pixelHeight, int pixelWidth)
        {
            var isValid = Common.Validator.ValidateNewPlayerAvatar(pixelHeight, pixelWidth);

            return isValid;
        }

        [Test]
        public void StartNewGamePlayersSelectedIsValidForFreeThrowsSingleGameTypeWhenPlayerSelected()
        {
            var player = new Player("Does not matter", "Does not matter");
            var gameType = GameType.FreeThrowsSingle.ToString();

            var isValid = Common.Validator.ValidateStartNewGamePlayersSelected(gameType, player, null);

            isValid.Should().BeTrue();
        }

        [Test]
        public void StartNewGamePlayersSelectedIsNotValidForFreeThrowsSingleGameTypeWhenPlayerNotSelected()
        {
            var gameType = GameType.FreeThrowsSingle.ToString();

            var isValid = Common.Validator.ValidateStartNewGamePlayersSelected(gameType, null, null);

            isValid.Should().BeFalse();
        }

        [TestCase(GameType.FreeThrowsDouble)]
        [TestCase(GameType.Classic)]
        public void StartNewGamePlayersSelectedIsValidWhenPlayersSelectedAndNotTheSame(GameType type)
        {
            var player1 = new Player("Does not matter", "Does not matter");
            var player2 = new Player("Does not matter too", "Does not matter too");
            var gameType = type.ToString();

            var isValid = Common.Validator.ValidateStartNewGamePlayersSelected(gameType, player1, player2);

            isValid.Should().BeTrue();
        }

        [TestCase(GameType.FreeThrowsDouble)]
        [TestCase(GameType.Classic)]
        public void StartNewGamePlayersSelectedIsNotValidWhenPlayersNotSelected(GameType type)
        {
            var gameType = type.ToString();

            var isValid = Common.Validator.ValidateStartNewGamePlayersSelected(gameType, null, null);

            isValid.Should().BeFalse();
        }

        [TestCase(GameType.FreeThrowsDouble)]
        [TestCase(GameType.Classic)]
        public void StartNewGamePlayersSelectedIsNotValidWhenFirstPlayerNotSelected(GameType type)
        {
            var player2 = new Player("Does not matter too", "Does not matter too");
            var gameType = type.ToString();

            var isValid = Common.Validator.ValidateStartNewGamePlayersSelected(gameType, null, player2);

            isValid.Should().BeFalse();
        }

        [TestCase(GameType.FreeThrowsDouble)]
        [TestCase(GameType.Classic)]
        public void StartNewGamePlayersSelectedIsNotValidWhenSecondPlayerNotSelected(GameType type)
        {
            var player1 = new Player("Does not matter too", "Does not matter too");
            var gameType = type.ToString();

            var isValid = Common.Validator.ValidateStartNewGamePlayersSelected(gameType, player1, null);

            isValid.Should().BeFalse();
        }

        [TestCase(GameType.FreeThrowsDouble)]
        [TestCase(GameType.Classic)]
        public void StartNewGamePlayersSelectedIsNotValidWhenPlayersAreTheSame(GameType type)
        {
            var player = new Player("Does not matter", "Does not matter");
            var gameType = type.ToString();

            var isValid = Common.Validator.ValidateStartNewGamePlayersSelected(gameType, player, player);

            isValid.Should().BeFalse();
        }

        [Test]
        public void ImplementedGameTypesValidatesCorrectly([Values] GameType type)
        {
            var isValid = Common.Validator.ValidateImplementedGameTypes(type.ToString());

            isValid.Should().BeTrue();
        }

        [TestCase(GameType.FreeThrowsSingle, "501", ExpectedResult = true)]
        [TestCase(GameType.Classic, "501", ExpectedResult = true)]
        [TestCase(GameType.FreeThrowsSingle, "Free", ExpectedResult = true)]
        [TestCase(GameType.Classic, "Free", ExpectedResult = false)]
        public bool StartNewClassicGameValidatesCorrectly(GameType type, string pointsString)
        {
            var isValid = Common.Validator.ValidateStartNewClassicGame(type.ToString(), pointsString);

            return isValid;
        }
    }
}