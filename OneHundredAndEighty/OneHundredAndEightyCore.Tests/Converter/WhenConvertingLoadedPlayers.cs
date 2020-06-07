#region Usings

using System.Data;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Tests.Converter
{
    public class WhenConvertingLoadedPlayers : ConverterTestBase
    {
        private DataTable inputDataTable;

        protected override void Setup()
        {
            base.Setup();
            inputDataTable = new DataTable();
            inputDataTable.Columns.Add(Column.Id.ToString(), typeof(int));
            inputDataTable.Columns.Add(Column.Name.ToString(), typeof(string));
            inputDataTable.Columns.Add(Column.NickName.ToString(), typeof(string));
            inputDataTable.Columns.Add(Column.Avatar.ToString(), typeof(string));
        }

        [Test]
        public void PlayersIdsConvertedCorrectly()
        {
            var player1Id = 55;
            var player2Id = 69;
            inputDataTable.Rows.Add(player1Id, "doesNotMatter", "doesNotMatter", base64ImageString);
            inputDataTable.Rows.Add(player2Id, "doesNotMatter", "doesNotMatter", base64ImageString);

            var players = Common.Converter.PlayersFromTable(inputDataTable);

            players.ElementAt(0).Id.Should().Be(player1Id);
            players.ElementAt(1).Id.Should().Be(player2Id);
        }

        [Test]
        public void PlayersNamesConvertedCorrectly()
        {
            var player1name = "Phil";
            var player2name = "Mike";
            inputDataTable.Rows.Add(1, player1name, "doesNotMatter", base64ImageString);
            inputDataTable.Rows.Add(2, player2name, "doesNotMatter", base64ImageString);

            var players = Common.Converter.PlayersFromTable(inputDataTable);

            players.ElementAt(0).Name.Should().Be(player1name);
            players.ElementAt(1).Name.Should().Be(player2name);
        }

        [Test]
        public void PlayersNickNamesConvertedCorrectly()
        {
            var player1Nickname = "The Power";
            var player2Nickname = "Mighty Mike";
            inputDataTable.Rows.Add(1, "doesNotMatter", player1Nickname, base64ImageString);
            inputDataTable.Rows.Add(2, "doesNotMatter", player2Nickname, base64ImageString);

            var players = Common.Converter.PlayersFromTable(inputDataTable);

            players.ElementAt(0).NickName.Should().Be(player1Nickname);
            players.ElementAt(1).NickName.Should().Be(player2Nickname);
        }

        [Test]
        public void PlayersAvatarsConvertedCorrectly()
        {
            inputDataTable.Rows.Add(1, "doesNotMatter", "doesNotMatter", base64ImageString);
            inputDataTable.Rows.Add(2, "doesNotMatter", "doesNotMatter", base64ImageString);

            var players = Common.Converter.PlayersFromTable(inputDataTable);

            players.ElementAt(0).Avatar.Should().NotBeNull();
            players.ElementAt(1).Avatar.Should().NotBeNull();
        }
    }
}