#region Usings

using System.Linq;
using FluentAssertions;
using NUnit.Framework;

#endregion

namespace OneHundredAndEightyCore.Tests.Converter
{
    public class WhenConvertingLoadedPlayers : ConverterTestBase
    {
        [Test]
        public void PlayersIdsConvertedCorrectly()
        {
            var player1Id = 55;
            var player2Id = 69;
            PlayersDataTableFromDb.Rows.Add(player1Id, "doesNotMatter", "doesNotMatter", base64ImageString);
            PlayersDataTableFromDb.Rows.Add(player2Id, "doesNotMatter", "doesNotMatter", base64ImageString);

            var players = Common.Converter.PlayersFromTable(PlayersDataTableFromDb);

            players.ElementAt(0).Id.Should().Be(player1Id);
            players.ElementAt(1).Id.Should().Be(player2Id);
        }

        [Test]
        public void PlayersNamesConvertedCorrectly()
        {
            var player1name = "Phil";
            var player2name = "Mike";
            PlayersDataTableFromDb.Rows.Add(1, player1name, "doesNotMatter", base64ImageString);
            PlayersDataTableFromDb.Rows.Add(2, player2name, "doesNotMatter", base64ImageString);

            var players = Common.Converter.PlayersFromTable(PlayersDataTableFromDb);

            players.ElementAt(0).Name.Should().Be(player1name);
            players.ElementAt(1).Name.Should().Be(player2name);
        }

        [Test]
        public void PlayersNickNamesConvertedCorrectly()
        {
            var player1Nickname = "The Power";
            var player2Nickname = "Mighty Mike";
            PlayersDataTableFromDb.Rows.Add(1, "doesNotMatter", player1Nickname, base64ImageString);
            PlayersDataTableFromDb.Rows.Add(2, "doesNotMatter", player2Nickname, base64ImageString);

            var players = Common.Converter.PlayersFromTable(PlayersDataTableFromDb);

            players.ElementAt(0).NickName.Should().Be(player1Nickname);
            players.ElementAt(1).NickName.Should().Be(player2Nickname);
        }

        [Test]
        public void PlayersAvatarsConvertedCorrectly()
        {
            PlayersDataTableFromDb.Rows.Add(1, "doesNotMatter", "doesNotMatter", base64ImageString);
            PlayersDataTableFromDb.Rows.Add(2, "doesNotMatter", "doesNotMatter", base64ImageString);

            var players = Common.Converter.PlayersFromTable(PlayersDataTableFromDb);

            players.ElementAt(0).Avatar.Should().NotBeNull();
            players.ElementAt(1).Avatar.Should().NotBeNull();
        }
    }
}