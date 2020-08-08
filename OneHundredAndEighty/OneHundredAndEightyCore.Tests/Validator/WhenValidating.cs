#region Usings

using FluentAssertions;
using NUnit.Framework;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Enums;

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

            var isValid = Common.Validator.ValidateStartNewGamePlayersSelected(GameType.FreeThrowsSingle, player, null);

            isValid.Should().BeTrue();
        }

        [Test]
        public void StartNewGamePlayersSelectedIsNotValidForFreeThrowsSingleGameTypeWhenPlayerNotSelected()
        {
            var isValid = Common.Validator.ValidateStartNewGamePlayersSelected(GameType.FreeThrowsSingle, null, null);

            isValid.Should().BeFalse();
        }

        [TestCase(GameType.FreeThrowsDouble)]
        [TestCase(GameType.Classic)]
        public void StartNewGamePlayersSelectedIsValidWhenPlayersSelectedAndNotTheSame(GameType type)
        {
            var player1 = new Player("Does not matter", "Does not matter");
            var player2 = new Player("Does not matter too", "Does not matter too");

            var isValid = Common.Validator.ValidateStartNewGamePlayersSelected(type, player1, player2);

            isValid.Should().BeTrue();
        }

        [TestCase(GameType.FreeThrowsDouble)]
        [TestCase(GameType.Classic)]
        public void StartNewGamePlayersSelectedIsNotValidWhenPlayersNotSelected(GameType type)
        {
            var isValid = Common.Validator.ValidateStartNewGamePlayersSelected(type, null, null);

            isValid.Should().BeFalse();
        }

        [TestCase(GameType.FreeThrowsDouble)]
        [TestCase(GameType.Classic)]
        public void StartNewGamePlayersSelectedIsNotValidWhenFirstPlayerNotSelected(GameType type)
        {
            var player2 = new Player("Does not matter too", "Does not matter too");

            var isValid = Common.Validator.ValidateStartNewGamePlayersSelected(type, null, player2);

            isValid.Should().BeFalse();
        }

        [TestCase(GameType.FreeThrowsDouble)]
        [TestCase(GameType.Classic)]
        public void StartNewGamePlayersSelectedIsNotValidWhenSecondPlayerNotSelected(GameType type)
        {
            var player1 = new Player("Does not matter too", "Does not matter too");

            var isValid = Common.Validator.ValidateStartNewGamePlayersSelected(type, player1, null);

            isValid.Should().BeFalse();
        }

        [TestCase(GameType.FreeThrowsDouble)]
        [TestCase(GameType.Classic)]
        public void StartNewGamePlayersSelectedIsNotValidWhenPlayersAreTheSame(GameType type)
        {
            var player = new Player("Does not matter", "Does not matter");

            var isValid = Common.Validator.ValidateStartNewGamePlayersSelected(type, player, player);

            isValid.Should().BeFalse();
        }

        [Test]
        public void ImplementedGameTypesValidatesCorrectly([Values] GameType type)
        {
            var isValid = Common.Validator.ValidateImplementedGameTypes(type);

            isValid.Should().BeTrue();
        }

        [TestCase(GameType.FreeThrowsSingle, GamePoints._501, ExpectedResult = true)]
        [TestCase(GameType.Classic, GamePoints._501, ExpectedResult = true)]
        [TestCase(GameType.FreeThrowsSingle, GamePoints.Free, ExpectedResult = true)]
        [TestCase(GameType.Classic, GamePoints.Free, ExpectedResult = false)]
        public bool StartNewClassicGameValidatesCorrectly(GameType type, GamePoints points)
        {
            var isValid = Common.Validator.ValidateStartNewClassicGame(type, points);

            return isValid;
        }

        [TestCase("someText", ExpectedResult = false)]
        [TestCase("-1", ExpectedResult = false)]
        [TestCase("-1.5", ExpectedResult = false)]
        [TestCase("0", ExpectedResult = true)]
        [TestCase("5", ExpectedResult = true)]
        [TestCase("5.3", ExpectedResult = false)]
        [TestCase("5,3", ExpectedResult = false)]
        public bool IntInputValidatesCorrectly(string input)
        {
            var isValid = Common.Validator.ValidateIntInput(input);

            return isValid;
        }

        [TestCase("someText", ExpectedResult = false)]
        [TestCase("-1", ExpectedResult = false)]
        [TestCase("-1.5", ExpectedResult = false)]
        [TestCase("-1,5", ExpectedResult = false)]
        [TestCase("0", ExpectedResult = true)]
        [TestCase("5", ExpectedResult = true)]
        [TestCase("5.3", ExpectedResult = true)]
        [TestCase("5,3", ExpectedResult = false)]
        public bool DoubleInputValidatesCorrectly(string input)
        {
            var isValid = Common.Validator.ValidateDoubleInput(input);

            return isValid;
        }
    }
}