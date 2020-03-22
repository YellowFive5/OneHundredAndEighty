#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using OneHundredAndEightyCore.Game;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public class DBService : IDisposable
    {
        private readonly SQLiteConnection connection;
        private const string DatabaseName = "Database.db";

        public DBService()
        {
            connection = new SQLiteConnection($"Data Source={DatabaseName}; Pooling=true;");
        }

        #region Throws

        public void ThrowSaveNew(Throw thrw)
        {
            var newThrowQuery = $"INSERT INTO [{Table.Throws}] ({Column.Player},{Column.Game},{Column.Sector},{Column.Type},{Column.Result},{Column.Number},{Column.Points},{Column.PoiX},{Column.PoiY},{Column.ProjectionResolution},{Column.Timestamp})" +
                                $"VALUES ({thrw.Player.Id},{thrw.Game.Id},{thrw.Sector},{(int) thrw.Type},{(int) thrw.Result},{thrw.Number}," +
                                $"{thrw.Points},{thrw.Poi.X.ToString(CultureInfo.InvariantCulture)},{thrw.Poi.Y.ToString(CultureInfo.InvariantCulture)},{thrw.ProjectionResolution},'{thrw.TimeStamp}')";
            ExecuteNonQueryInternal(newThrowQuery);

            var newThrowId = ExecuteScalarInternal($"SELECT MAX({Column.Id}) FROM [{Table.Throws}]");
            thrw.SetId(Convert.ToInt32(newThrowId));

            var incrementThrowGameStatisticsQuery = $"UPDATE [{Table.Statistic}] SET [{Column.Throws}] = [{Column.Throws}] + 1 " +
                                                    $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [SFT] " +
                                                    $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                                    $"ON [GS].[{Column.Game}] = {thrw.Game.Id} " +
                                                    $"AND [GS].[{Column.Statistic}] = [SFT].[{Column.Id}] " +
                                                    $"WHERE[Player] = {thrw.Player.Id})";
            var incrementPointsGameStatisticsQuery = $"UPDATE [{Table.Statistic}] SET [{Column.Points}] = [{Column.Points}] + {thrw.Points} " +
                                                     $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [SFT] " +
                                                     $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                                     $"ON [GS].[{Column.Game}] = {thrw.Game.Id} " +
                                                     $"AND [GS].[{Column.Statistic}] = [SFT].[{Column.Id}] " +
                                                     $"WHERE[{Column.Player}] = {thrw.Player.Id})";
            var incrementThrowTypeGameStatisticsQuery = $"UPDATE [{Table.Statistic}] SET [{thrw.Type}] = [{thrw.Type}] + 1 " +
                                                        $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [SFT] " +
                                                        $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                                        $"ON [GS].[{Column.Game}] = {thrw.Game.Id} " +
                                                        $"AND [GS].[{Column.Statistic}] = [SFT].[{Column.Id}] " +
                                                        $"WHERE[{Column.Player}] = {thrw.Player.Id})";
            // todo fault add
            // todo _180 add
            // todo another game types

            ExecuteNonQueryInternal(incrementThrowGameStatisticsQuery);
            ExecuteNonQueryInternal(incrementPointsGameStatisticsQuery);
            ExecuteNonQueryInternal(incrementThrowTypeGameStatisticsQuery);

            var incrementThrowPlayerStatisticsQuery = $"UPDATE [{Table.PlayerStatistic}] SET [{Column.Throws}] = [{Column.Throws}] + 1 " +
                                                      $"WHERE [{Column.Id}] = (SELECT [{Column.Statistic}] FROM [{Table.Players}] WHERE [{Column.Id}] = {thrw.Player.Id})";
            ExecuteNonQueryInternal(incrementThrowPlayerStatisticsQuery);

            var incrementPointsPlayerStatisticsQuery = $"UPDATE [{Table.PlayerStatistic}] SET [{Column.Points}] = [{Column.Points}] + {thrw.Points} " +
                                                       $"WHERE [{Column.Id}] = (SELECT [{Column.Statistic}] FROM [{Table.Players}] WHERE [{Column.Id}] = {thrw.Player.Id})";
            ExecuteNonQueryInternal(incrementPointsPlayerStatisticsQuery);

            var incrementThrowTypePlayerStatisticsQuery = $"UPDATE [{Table.PlayerStatistic}] SET [{thrw.Type}] = [{thrw.Type}] + 1 " +
                                                          $"WHERE [{Column.Id}] = (SELECT [{Column.Statistic}] FROM [{Table.Players}] WHERE [{Column.Id}] = {thrw.Player.Id})";
            ExecuteNonQueryInternal(incrementThrowTypePlayerStatisticsQuery);
        }

        #endregion

        #region Game

        public void GameSaveNew(Game.Game game, List<Player> players)
        {
            var newGameQuery = $"INSERT INTO [{Table.Games}] ({Column.StartTimestamp},{Column.EndTimestamp},{Column.Type})" +
                               $" VALUES ('{game.StartTimeStamp}','','{(int) game.Type}')";
            ExecuteNonQueryInternal(newGameQuery);

            var newGameId = Convert.ToInt32(ExecuteScalarInternal($"SELECT MAX({Column.Id}) FROM [{Table.Games}]"));
            game.SetId(newGameId);

            StatisticSaveNew(newGameId, players);
        }

        public void GameEnd(Game.Game game, GameResultType gameResultType = GameResultType.NotDefined, Player winner = null)
        {
            GameSaveEndTimeStamp(game);

            if (gameResultType == GameResultType.Aborted ||
                gameResultType == GameResultType.Error)
            {
                StatisticUpdateAllPlayersSetGameResultAbortedOrError(game.Id, gameResultType);
            }
            else if (winner != null)
            {
                StatisticUpdateAllPlayersSetGameResultForWinnersAndLosers(game.Id, winner.Id);

                StatisticUpdateIncrementWinnerGamesWon(winner.Id);
            }

            PlayerStatisticUpdateIncrementAllPlayersGamesPlayed(game.Id);
        }

        private void GameSaveEndTimeStamp(Game.Game game)
        {
            game.SetEndTimeStamp();
            var gameEndTimestampQuery = $"UPDATE [{Table.Games}] SET [{Column.EndTimestamp}] = '{game.EndTimeStamp}' " +
                                        $"WHERE [Id] = {game.Id}";
            ExecuteNonQueryInternal(gameEndTimestampQuery);
        }

        #endregion

        #region Statistics

        private void StatisticUpdateAllPlayersSetGameResultAbortedOrError(int gameId, GameResultType gameResultType)
        {
            var playersGameStatisticsResultQuery = $"UPDATE [{Table.Statistic}] SET [{Column.GameResult}] = {(int) gameResultType} " +
                                                   $"WHERE [{Column.Id}] IN (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                                   $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                                   $"ON [GS].[{Column.Game}] = {gameId} " +
                                                   $"AND [GS].[{Column.Statistic}] = [S].[{Column.Id}])";

            ExecuteNonQueryInternal(playersGameStatisticsResultQuery);
        }

        private void StatisticUpdateIncrementWinnerGamesWon(int winnerId)
        {
            var winnerPlayerStatisticsQuery = $"UPDATE [{Table.PlayerStatistic}] SET [{Column.GamesWon}] = [{Column.GamesWon}] + 1 " +
                                              $"WHERE [{Column.Id}] = (SELECT [{Column.Statistic}] FROM [{Table.Players}] WHERE [{Column.Id}] = {winnerId})";
            ExecuteNonQueryInternal(winnerPlayerStatisticsQuery);
        }

        private void StatisticUpdateAllPlayersSetGameResultForWinnersAndLosers(int gameId, int winnerId)
        {
            var winnerGameStatisticsResultQuery = $"UPDATE [{Table.Statistic}] SET [{Column.GameResult}] = {(int) GameResultType.Win} " +
                                                  $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                                  $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                                  $"ON [GS].[{Column.Game}] = {gameId} " +
                                                  $"AND [GS].[{Column.Statistic}] = [S].[{Column.Id}] " +
                                                  $"WHERE[{Column.Player}] = {winnerId})";
            var loosersGameStatisticsResultQuery = $"UPDATE [{Table.Statistic}] SET [{Column.GameResult}] = {(int) GameResultType.Loose} " +
                                                   $"WHERE [{Column.Id}] IN (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                                   $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                                   $"ON [GS].[{Column.Game}] = {gameId} " +
                                                   $"AND [GS].[{Column.Statistic}] = [S].[{Column.Id}] " +
                                                   $"WHERE[{Column.Player}] <> {winnerId})";

            ExecuteNonQueryInternal(winnerGameStatisticsResultQuery);
            ExecuteNonQueryInternal(loosersGameStatisticsResultQuery);
        }

        private void StatisticSaveNew(int newGameId, List<Player> players)
        {
            var newGameStatisticsIds = new List<object>();

            foreach (var player in players)
            {
                var newGameStatisticsQuery = $"INSERT INTO [{Table.Statistic}] " +
                                             $"({Column.Player},{Column.GameResult})" +
                                             $"VALUES ({player.Id},{(int) GameResultType.NotDefined})";
                ExecuteNonQueryInternal(newGameStatisticsQuery);

                var newGameStatisticsId = ExecuteScalarInternal($"SELECT MAX({Column.Id}) FROM [{Table.Statistic}]");
                newGameStatisticsIds.Add(newGameStatisticsId);
            }

            foreach (var id in newGameStatisticsIds)
            {
                ExecuteNonQueryInternal($"INSERT INTO [{Table.GameStatistic}] ({Column.Game},{Column.Statistic})" +
                                        $" VALUES ({newGameId},{id})");
            }
        }

        #endregion

        #region PlayerStatistic

        private void PlayerStatisticUpdateIncrementAllPlayersGamesPlayed(int gameId)
        {
            var playersStatisticsQuery = $"UPDATE [{Table.PlayerStatistic}] SET [{Column.GamesPlayed}] = [{Column.GamesPlayed}] + 1 " +
                                         $"WHERE [{Column.Id}] IN (SELECT [P].[{Column.Statistic}] FROM [{Table.Players}] AS [P] " +
                                         $"INNER JOIN [{Table.Statistic}] AS [S] " +
                                         $"ON [S].[{Column.Player}] = [P].[{Column.Id}]" +
                                         $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                         $"ON [GS].[{Column.Game}] = {gameId} " +
                                         $"AND [GS].[{Column.Statistic}] = [S].[{Column.Id}])";

            ExecuteNonQueryInternal(playersStatisticsQuery);
        }

        private object PlayerStatisticSaveNew()
        {
            var newPlayerStatisticsQuery = $"INSERT INTO [{Table.PlayerStatistic}] DEFAULT VALUES";
            ExecuteNonQueryInternal(newPlayerStatisticsQuery);
            return ExecuteScalarInternal($"SELECT MAX({Column.Id}) FROM [{Table.PlayerStatistic}]");
        }

        #endregion

        #region Player

        public void PlayerSaveNew(Player player)
        {
            var playerWithNickNameId = ExecuteScalarInternal($"SELECT {Column.Id} FROM [{Table.Players}] WHERE {Column.NickName} = '{player.NickName}'");
            if (playerWithNickNameId != null)
            {
                throw new Exception($"Player with nickname: '{player.NickName}' is already exists in DB");
            }

            var newPlayerStatisticsId = PlayerStatisticSaveNew();
            var newPlayerAchievesId = PlayerAchievesSaveNew();

            var newPlayerQuery = $"INSERT INTO [{Table.Players}] ({Column.Name}, {Column.NickName}, {Column.RegistrationTimestamp}, {Column.Statistic}, {Column.Achieves}, {Column.Avatar})" +
                                 $" VALUES ('{player.Name}','{player.NickName}','{DateTime.Now}', '{newPlayerStatisticsId}', '{newPlayerAchievesId}', '{Converter.BitmapImageToBase64(player.Avatar)}')";
            try
            {
                ExecuteNonQueryInternal(newPlayerQuery);
            }
            catch (Exception e)
            {
                //todo errorMessage
                ExecuteNonQueryInternal($"DELETE FROM [{Table.PlayerStatistic}] WHERE [{Column.Id}]={newPlayerStatisticsId}");
                ExecuteNonQueryInternal($"DELETE FROM [{Table.PlayerAchieves}] WHERE [{Column.Id}]={newPlayerAchievesId}");
                throw e;
            }
        }

        public DataTable PlayersLoadAll()
        {
            return ExecuteReaderInternal($"SELECT * FROM [{Table.Players}]");
        }

        #endregion

        #region PlayerAchieves

        private object PlayerAchievesSaveNew()
        {
            var newPlayerAchievesQuery = $"INSERT INTO [{Table.PlayerAchieves}] DEFAULT VALUES";
            ExecuteNonQueryInternal(newPlayerAchievesQuery);
            return ExecuteScalarInternal($"SELECT MAX({Column.Id}) FROM [{Table.PlayerAchieves}]");
        }

        #endregion

        #region Settings

        public string SettingsGetValue(SettingsType name)
        {
            var query = $"SELECT [{Column.Value}] FROM [{Table.Settings}] WHERE [{Column.Name}] = '{name}'";
            return (string) ExecuteScalarInternal(query);
        }

        public void SettingsSetValue(SettingsType name, string value)
        {
            var query = $"UPDATE [{Table.Settings}] SET [{Column.Value}] = '{value}' WHERE [{Column.Name}] = '{name}'";
            ExecuteNonQueryInternal(query);
        }

        #endregion

        #region Low level internals

        private object ExecuteScalarInternal(string query)
        {
            var cmd = new SQLiteCommand(query) {Connection = connection};
            try
            {
                connection.Open();
                var result = cmd.ExecuteScalar();
                return result;
            }
            catch (Exception e)
            {
                //todo errorMessage
                throw e;
            }
            finally
            {
                connection.Close();
            }
        }

        private void ExecuteNonQueryInternal(string query)
        {
            var cmd = new SQLiteCommand(query) {Connection = connection};
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //todo errorMessage
                throw e;
            }
            finally
            {
                connection.Close();
            }
        }

        private DataTable ExecuteReaderInternal(string query)
        {
            var cmd = new SQLiteCommand(query) {Connection = connection};

            try
            {
                connection.Open();
                var dataReader = cmd.ExecuteReader();
                var dataTable = new DataTable();
                dataTable.Load(dataReader);
                return dataTable;
            }
            catch (Exception e)
            {
                //todo errorMessage
                throw e;
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion

        public void Dispose()
        {
            // todo: checkThis
            connection.Close();
            connection?.Dispose();
        }
    }
}