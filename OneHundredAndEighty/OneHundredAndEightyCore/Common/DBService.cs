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
    public enum Table
    {
        Settings,
        PlayerStatistics,
        PlayerAchieves,
        Players,
        Games,
        StatisticsFreeThrows,
        GameStatistics,
        Throws,
    }

    public enum Column
    {
        Value,
        Name,
        Id,
        NickName,
        RegistrationTimestamp,
        Statistics,
        Achieves,
        StartTimestamp,
        EndTimestamp,
        Type,
        Player,
        GameResult,
        Throws,
        Points,
        _180,
        Tremble,
        Double,
        Single,
        Bulleye,
        _25,
        Zero,
        Fault,
        Game,
        GamesWon,
        GamesPlayed,
        Sector,
        Resultativity,
        Number,
        PoiX,
        PoiY,
        ProjectionResolution,
        Timestamp,
        Avatar
    }

    public class DBService : IDisposable
    {
        private readonly SQLiteConnection connection;
        private const string DatabaseName = "Database.db";

        public DBService()
        {
            connection = new SQLiteConnection($"Data Source={DatabaseName}; Pooling=true;");
        }

        public string GetSettingsValue(SettingsType name)
        {
            var query = $"SELECT [{Column.Value}] FROM [{Table.Settings}] WHERE [{Column.Name}] = '{name}'";
            return (string) ExecuteScalarInternal(query);
        }

        public void SaveSettingsValue(SettingsType name, string value)
        {
            var query = $"UPDATE [{Table.Settings}] SET [{Column.Value}] = '{value}' WHERE [{Column.Name}] = '{name}'";
            ExecuteNonQueryInternal(query);
        }

        public void SaveNewPlayer(Player player)
        {
            var playerWithNickNameId = ExecuteScalarInternal($"SELECT {Column.Id} FROM [{Table.Players}] WHERE {Column.NickName} = '{player.NickName}'");
            if (playerWithNickNameId != null)
            {
                throw new Exception($"Player with nickname: '{player.NickName}' is already exists in DB");
            }

            var newPlayerStatisticsId = CreateNewPlayerStatistics();
            var newPlayerAchievesId = CreateNewPlayerAchieves();

            var newPlayerQuery = $"INSERT INTO [{Table.Players}] ({Column.Name}, {Column.NickName}, {Column.RegistrationTimestamp}, {Column.Statistics}, {Column.Achieves}, {Column.Avatar})" +
                                 $" VALUES ('{player.Name}','{player.NickName}','{DateTime.Now}', '{newPlayerStatisticsId}', '{newPlayerAchievesId}', '{Converter.BitmapImageToBase64(player.Avatar)}')";
            try
            {
                ExecuteNonQueryInternal(newPlayerQuery);
            }
            catch (Exception e)
            {
                //todo errorMessage
                ExecuteNonQueryInternal($"DELETE FROM [{Table.PlayerStatistics}] WHERE [{Column.Id}]={newPlayerStatisticsId}");
                ExecuteNonQueryInternal($"DELETE FROM [{Table.PlayerAchieves}] WHERE [{Column.Id}]={newPlayerAchievesId}");
                throw e;
            }
        }

        public void SaveNewGame(Game.Game game, List<Player> players)
        {
            var newGameQuery = $"INSERT INTO [{Table.Games}] ({Column.StartTimestamp},{Column.EndTimestamp},{Column.Type})" +
                               $" VALUES ('{game.StartTimeStamp}','','{(int) game.Type}')";
            ExecuteNonQueryInternal(newGameQuery);

            var newGameId = ExecuteScalarInternal($"SELECT MAX({Column.Id}) FROM [{Table.Games}]");
            game.SetId(Convert.ToInt32(newGameId));
            var newGameStatisticsIds = new List<object>();

            foreach (var player in players)
            {
                switch (game.Type)
                {
                    case GameType.FreeThrows:
                        var newGameStatisticsQuery = $"INSERT INTO [{Table.StatisticsFreeThrows}] " +
                                                     $"({Column.Player},{Column.GameResult},{Column.Throws}," +
                                                     $"{Column.Points},{Column._180},{Column.Tremble},{Column.Double}," +
                                                     $"{Column.Single},{Column.Bulleye},{Column._25},{Column.Zero},{Column.Fault})" +
                                                     $"VALUES ({player.Id},{(int) GameResultType.NotDefined},0,0,0,0,0,0,0,0,0,0)";
                        ExecuteNonQueryInternal(newGameStatisticsQuery);

                        var newGameStatisticsId = ExecuteScalarInternal($"SELECT MAX({Column.Id}) FROM [{Table.StatisticsFreeThrows}]");
                        newGameStatisticsIds.Add(newGameStatisticsId);
                        break;

                    // todo another game types
                }
            }

            foreach (var id in newGameStatisticsIds)
            {
                ExecuteNonQueryInternal($"INSERT INTO [{Table.GameStatistics}] ({Column.Game},{Column.Statistics})" +
                                        $" VALUES ({newGameId},{id})");
            }
        }

        public void EndGame(Game.Game game, Player winner = null)
        {
            game.SetEndTimeStamp();
            var gameEndTimestampQuery = $"UPDATE [{Table.Games}] SET [{Column.EndTimestamp}] = '{game.EndTimeStamp}' " +
                                        $"WHERE [Id] = {game.Id}";
            ExecuteNonQueryInternal(gameEndTimestampQuery);

            if (winner != null)
            {
                var winnerGameStatisticsResultQuery = string.Empty;
                var loosersGameStatisticsResultQuery = string.Empty;

                switch (game.Type)
                {
                    case GameType.FreeThrows:
                        winnerGameStatisticsResultQuery = $"UPDATE [{Table.StatisticsFreeThrows}] SET [{Column.GameResult}] = {(int) GameResultType.Win} " +
                                                          $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.StatisticsFreeThrows}] AS [SFT] " +
                                                          $"INNER JOIN [{Table.GameStatistics}] AS [GS] " +
                                                          $"ON [GS].[{Column.Game}] = {game.Id} " +
                                                          $"AND [GS].[{Column.Statistics}] = [SFT].[{Column.Id}] " +
                                                          $"WHERE[{Column.Player}] = {winner.Id})";
                        loosersGameStatisticsResultQuery = $"UPDATE [{Table.StatisticsFreeThrows}] SET [{Column.GameResult}] = {(int) GameResultType.Loose} " +
                                                           $"WHERE [{Column.Id}] IN (SELECT [{Column.Id}] FROM [{Table.StatisticsFreeThrows}] AS [SFT] " +
                                                           $"INNER JOIN [{Table.GameStatistics}] AS [GS] " +
                                                           $"ON [GS].[{Column.Game}] = {game.Id} " +
                                                           $"AND [GS].[{Column.Statistics}] = [SFT].[{Column.Id}] " +
                                                           $"WHERE[{Column.Player}] <> {winner.Id})";
                        break;
                    // todo another game types
                }

                ExecuteNonQueryInternal(winnerGameStatisticsResultQuery);
                ExecuteNonQueryInternal(loosersGameStatisticsResultQuery);

                var winnerPlayerStatisticsQuery = $"UPDATE [{Table.PlayerStatistics}] SET [{Column.GamesWon}] = [{Column.GamesWon}] + 1 " +
                                                  $"WHERE [{Column.Id}] = (SELECT [{Column.Statistics}] FROM [{Table.Players}] WHERE [{Column.Id}] = {winner.Id})";
                ExecuteNonQueryInternal(winnerPlayerStatisticsQuery);
            }

            var playersStatisticsQuery = $"UPDATE [{Table.PlayerStatistics}] SET [{Column.GamesPlayed}] = [{Column.GamesPlayed}] + 1 " +
                                         $"WHERE [{Column.Id}] IN (SELECT [P].[{Column.Statistics}] FROM [{Table.Players}] AS [P] " +
                                         $"INNER JOIN [{Table.StatisticsFreeThrows}] AS [SFT] " +
                                         $"ON [SFT].[{Column.Player}] = [P].[{Column.Id}]" +
                                         $"INNER JOIN [{Table.GameStatistics}] AS [GS] " +
                                         $"ON [GS].[{Column.Game}] = {game.Id} " +
                                         $"AND [GS].[{Column.Statistics}] = [SFT].[{Column.Id}])";
            // todo another game types
            ExecuteNonQueryInternal(playersStatisticsQuery);
        }

        public void SaveThrow(Throw thrw)
        {
            var newThrowQuery = $"INSERT INTO [{Table.Throws}] ({Column.Player},{Column.Game},{Column.Sector},{Column.Type},{Column.Resultativity},{Column.Number},{Column.Points},{Column.PoiX},{Column.PoiY},{Column.ProjectionResolution},{Column.Timestamp})" +
                                $"VALUES ({thrw.Player.Id},{thrw.Game.Id},{thrw.Sector},{(int) thrw.Type},{(int) thrw.Resultativity},{thrw.Number}," +
                                $"{thrw.Points},{thrw.Poi.X.ToString(CultureInfo.InvariantCulture)},{thrw.Poi.Y.ToString(CultureInfo.InvariantCulture)},{thrw.ProjectionResolution},'{thrw.TimeStamp}')";
            ExecuteNonQueryInternal(newThrowQuery);

            var newThrowId = ExecuteScalarInternal($"SELECT MAX({Column.Id}) FROM [{Table.Throws}]");
            thrw.SetId(Convert.ToInt32(newThrowId));
            var incrementThrowGameStatisticsQuery = string.Empty;
            var incrementPointsGameStatisticsQuery = string.Empty;
            var incrementThrowTypeGameStatisticsQuery = string.Empty;
            switch (thrw.Game.Type)
            {
                case GameType.FreeThrows:
                    incrementThrowGameStatisticsQuery = $"UPDATE [{Table.StatisticsFreeThrows}] SET [{Column.Throws}] = [{Column.Throws}] + 1 " +
                                                        $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.StatisticsFreeThrows}] AS [SFT] " +
                                                        $"INNER JOIN [{Table.GameStatistics}] AS [GS] " +
                                                        $"ON [GS].[{Column.Game}] = {thrw.Game.Id} " +
                                                        $"AND [GS].[{Column.Statistics}] = [SFT].[{Column.Id}] " +
                                                        $"WHERE[Player] = {thrw.Player.Id})";
                    incrementPointsGameStatisticsQuery = $"UPDATE [{Table.StatisticsFreeThrows}] SET [{Column.Points}] = [{Column.Points}] + {thrw.Points} " +
                                                         $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.StatisticsFreeThrows}] AS [SFT] " +
                                                         $"INNER JOIN [{Table.GameStatistics}] AS [GS] " +
                                                         $"ON [GS].[{Column.Game}] = {thrw.Game.Id} " +
                                                         $"AND [GS].[{Column.Statistics}] = [SFT].[{Column.Id}] " +
                                                         $"WHERE[{Column.Player}] = {thrw.Player.Id})";
                    incrementThrowTypeGameStatisticsQuery = $"UPDATE [{Table.StatisticsFreeThrows}] SET [{thrw.Type}] = [{thrw.Type}] + 1 " +
                                                            $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.StatisticsFreeThrows}] AS [SFT] " +
                                                            $"INNER JOIN [{Table.GameStatistics}] AS [GS] " +
                                                            $"ON [GS].[{Column.Game}] = {thrw.Game.Id} " +
                                                            $"AND [GS].[{Column.Statistics}] = [SFT].[{Column.Id}] " +
                                                            $"WHERE[{Column.Player}] = {thrw.Player.Id})";
                    // todo fault add
                    // todo _180 add
                    // todo another game types
                    break;
            }

            ExecuteNonQueryInternal(incrementThrowGameStatisticsQuery);
            ExecuteNonQueryInternal(incrementPointsGameStatisticsQuery);
            ExecuteNonQueryInternal(incrementThrowTypeGameStatisticsQuery);

            var incrementThrowPlayerStatisticsQuery = $"UPDATE [PlayerStatistics] SET [Throws] = [Throws] + 1 " +
                                                      $"WHERE [Id] = (SELECT [Statistics] FROM [Players] WHERE [Id] = {thrw.Player.Id})";
            ExecuteNonQueryInternal(incrementThrowPlayerStatisticsQuery);

            var incrementPointsPlayerStatisticsQuery = $"UPDATE [PlayerStatistics] SET [Points] = [Points] + {thrw.Points} " +
                                                       $"WHERE [Id] = (SELECT [Statistics] FROM [Players] WHERE [Id] = {thrw.Player.Id})";
            ExecuteNonQueryInternal(incrementPointsPlayerStatisticsQuery);

            var incrementThrowTypePlayerStatisticsQuery = $"UPDATE [PlayerStatistics] SET [{thrw.Type}] = [{thrw.Type}] + 1 " +
                                                          $"WHERE [Id] = (SELECT [Statistics] FROM [Players] WHERE [Id] = {thrw.Player.Id})";
            ExecuteNonQueryInternal(incrementThrowTypePlayerStatisticsQuery);
        }

        private object CreateNewPlayerStatistics()
        {
            var newPlayerStatisticsQuery = $"INSERT INTO [{Table.PlayerStatistics}] DEFAULT VALUES";
            ExecuteNonQueryInternal(newPlayerStatisticsQuery);
            return ExecuteScalarInternal($"SELECT MAX({Column.Id}) FROM [{Table.PlayerStatistics}]");
        }

        private object CreateNewPlayerAchieves()
        {
            var newPlayerAchievesQuery = $"INSERT INTO [{Table.PlayerAchieves}] DEFAULT VALUES";
            ExecuteNonQueryInternal(newPlayerAchievesQuery);
            return ExecuteScalarInternal($"SELECT MAX({Column.Id}) FROM [{Table.PlayerAchieves}]");
        }

        public DataTable LoadPlayers()
        {
            return ExecuteReaderInternal($"SELECT * FROM [{Table.Players}]");
        }

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