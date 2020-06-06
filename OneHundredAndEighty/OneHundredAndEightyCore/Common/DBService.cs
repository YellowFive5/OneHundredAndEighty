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

        public const string DatabaseCopyName = "Database_old.db";
        public const string DatabaseName = "Database.db";

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

        public void GameEnd(Game.Game game,
                            Player winner = null,
                            GameResultType gameResultType = GameResultType.NotDefined)
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
            }
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

        public void StatisticUpdateAddLegsPlayedForPlayers(int gameId)
        {
            var addLegsPlayedForPlayersQuery = $"UPDATE [{Table.Statistic}] SET [{Column.LegsPlayed}] = [{Column.LegsPlayed}] + 1 " +
                                               $"WHERE [{Column.Id}] IN (SELECT [{Column.Statistic}] FROM [{Table.GameStatistic}] " +
                                               $"WHERE [{Column.Game}] = {gameId})";

            ExecuteNonQueryInternal(addLegsPlayedForPlayersQuery);
        }

        public void StatisticUpdateAddLegsWonForPlayer(Player player, int gameId)
        {
            var addLegsWonForPlayerQuery = $"UPDATE [{Table.Statistic}] SET [{Column.LegsWon}] = [{Column.LegsWon}] + 1 " +
                                           $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                           $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                           $"ON [GS].[{Column.Game}] = {gameId} " +
                                           $"AND [GS].[{Column.Statistic}] = [S].[{Column.Id}] " +
                                           $"WHERE[{Column.Player}] = {player.Id})";

            ExecuteNonQueryInternal(addLegsWonForPlayerQuery);
        }

        public void StatisticUpdateAddSetsPlayedForPlayers(int gameId)
        {
            var addSetsPlayedForPlayersQuery = $"UPDATE [{Table.Statistic}] SET [{Column.SetsPlayed}] = [{Column.SetsPlayed}] + 1 " +
                                               $"WHERE [{Column.Id}] IN (SELECT [{Column.Statistic}] FROM [{Table.GameStatistic}] " +
                                               $"WHERE [{Column.Game}] = {gameId})";

            ExecuteNonQueryInternal(addSetsPlayedForPlayersQuery);
        }

        public void StatisticUpdateAddSetsWonForPlayer(Player player, int gameId)
        {
            var addSetsWonForPlayerQuery = $"UPDATE [{Table.Statistic}] SET [{Column.SetsWon}] = [{Column.SetsWon}] + 1 " +
                                           $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                           $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                           $"ON [GS].[{Column.Game}] = {gameId} " +
                                           $"AND [GS].[{Column.Statistic}] = [S].[{Column.Id}] " +
                                           $"WHERE[{Column.Player}] = {player.Id})";

            ExecuteNonQueryInternal(addSetsWonForPlayerQuery);
        }


        private void StatisticUpdateAllPlayersSetGameResultAbortedOrError(int gameId, GameResultType gameResultType)
        {
            var playersGameStatisticsResultQuery = $"UPDATE [{Table.Statistic}] SET [{Column.GameResult}] = {(int) gameResultType} " +
                                                   $"WHERE [{Column.Id}] IN (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                                   $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                                   $"ON [GS].[{Column.Game}] = {gameId} " +
                                                   $"AND [GS].[{Column.Statistic}] = [S].[{Column.Id}])";

            ExecuteNonQueryInternal(playersGameStatisticsResultQuery);
        }

        private void StatisticUpdateAllPlayersSetGameResultForWinnersAndLosers(int gameId, int winnerId)
        {
            var winnerGameStatisticsResultQuery = $"UPDATE [{Table.Statistic}] SET [{Column.GameResult}] = {(int) GameResultType.Win} " +
                                                  $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                                  $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                                  $"ON [GS].[{Column.Game}] = {gameId} " +
                                                  $"AND [GS].[{Column.Statistic}] = [S].[{Column.Id}] " +
                                                  $"WHERE[{Column.Player}] = {winnerId})";
            var losersGameStatisticsResultQuery = $"UPDATE [{Table.Statistic}] SET [{Column.GameResult}] = {(int) GameResultType.Loose} " +
                                                  $"WHERE [{Column.Id}] IN (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                                  $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                                  $"ON [GS].[{Column.Game}] = {gameId} " +
                                                  $"AND [GS].[{Column.Statistic}] = [S].[{Column.Id}] " +
                                                  $"WHERE[{Column.Player}] <> {winnerId})";

            ExecuteNonQueryInternal(winnerGameStatisticsResultQuery);
            ExecuteNonQueryInternal(losersGameStatisticsResultQuery);
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

        #region Player

        public void PlayerSaveNew(Player player)
        {
            var playerWithNickNameId = ExecuteScalarInternal($"SELECT {Column.Id} FROM [{Table.Players}] WHERE {Column.NickName} = '{player.NickName}'");
            if (playerWithNickNameId != null)
            {
                throw new Exception($"Player with nickname: '{player.NickName}' is already exists in DB");
            }

            var newPlayerAchievesId = PlayerAchievesSaveNew();

            var newPlayerQuery = $"INSERT INTO [{Table.Players}] ({Column.Name}, {Column.NickName}, {Column.RegistrationTimestamp}, {Column.Achieves}, {Column.Avatar})" +
                                 $" VALUES ('{player.Name}','{player.NickName}','{DateTime.Now}', '{newPlayerAchievesId}', '{Converter.BitmapImageToBase64(player.Avatar)}')";
            try
            {
                ExecuteNonQueryInternal(newPlayerQuery);
            }
            catch (Exception e)
            {
                //todo errorMessage
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

        #region _180

        public void _180SaveNew(Game.Game game, Player player)
        {
            var new180Query = $"INSERT INTO [{Table._180}] ({Column.Player},{Column.Game},{Column.Throw3},{Column.Throw2},{Column.Throw1}, {Column.Timestamp})" +
                              $" VALUES ('{player.Id}','{game.Id}','{player.HandThrows.Pop().Id}','{player.HandThrows.Pop().Id}','{player.HandThrows.Pop().Id}','{DateTime.Now}')";
            ExecuteNonQueryInternal(new180Query);
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

        #region Migrations

        private void UpdateDbVersion(string version)
        {
            SettingsSetValue(SettingsType.DBVersion, version);
        }

        public void MigrateFrom2_0to2_1()
        {
            var renameSettings = $"UPDATE [{Table.Settings}] SET [{Column.Name}] = '{SettingsType.Cam1ThresholdSlider}' " +
                                 $"WHERE [{Column.Name}] = 'Cam1TresholdSlider' ";
            ExecuteNonQueryInternal(renameSettings);

            renameSettings = $"UPDATE [{Table.Settings}] SET [{Column.Name}] = '{SettingsType.Cam2ThresholdSlider}' " +
                             $"WHERE [{Column.Name}] = 'Cam2TresholdSlider'";
            ExecuteNonQueryInternal(renameSettings);

            renameSettings = $"UPDATE [{Table.Settings}] SET [{Column.Name}] = '{SettingsType.Cam3ThresholdSlider}' " +
                             $"WHERE [{Column.Name}] = 'Cam3TresholdSlider'";
            ExecuteNonQueryInternal(renameSettings);

            renameSettings = $"UPDATE [{Table.Settings}] SET [{Column.Name}] = '{SettingsType.Cam4ThresholdSlider}' " +
                             $"WHERE [{Column.Name}] = 'Cam4TresholdSlider'";
            ExecuteNonQueryInternal(renameSettings);

            UpdateDbVersion("2.1");
        }

        public void MigrateFrom2_1to2_2()
        {
            // ThrowTypeBugFix
            var addTempThrowTypeForShuffle = $"INSERT INTO [{Table.ThrowType}] values (7, 'temp', 0)";
            ExecuteNonQueryInternal(addTempThrowTypeForShuffle);

            var tremblesToTemp = $"UPDATE [{Table.Throws}] SET [{Column.Type}] = 7 " +
                                 $"WHERE [{Column.Type}] = 4";
            ExecuteNonQueryInternal(tremblesToTemp);
            var zeroesFix = $"UPDATE [{Table.Throws}] SET [{Column.Type}] = 4 " +
                            $"WHERE [{Column.Type}] = 1";
            ExecuteNonQueryInternal(zeroesFix);
            var singlesFix = $"UPDATE [{Table.Throws}] SET [{Column.Type}] = 1 " +
                             $"WHERE [{Column.Type}] = 2";
            ExecuteNonQueryInternal(singlesFix);
            var doublesFix = $"UPDATE [{Table.Throws}] SET [{Column.Type}] = 2 " +
                             $"WHERE [{Column.Type}] = 3";
            ExecuteNonQueryInternal(doublesFix);
            var tremblesFix = $"UPDATE [{Table.Throws}] SET [{Column.Type}] = 3 " +
                              $"WHERE [{Column.Type}] = 7";
            ExecuteNonQueryInternal(tremblesFix);

            var deleteTempThrowTypeForShuffle = $"DELETE FROM [{Table.ThrowType}] WHERE [{Column.Id}] = 7";
            ExecuteNonQueryInternal(deleteTempThrowTypeForShuffle);
            // ThrowTypeBugFix

            // GameTypes simplify
            var toFreeThrowsSingle = $"UPDATE [{Table.Games}] SET [{Column.Type}] = 1 " +
                                     $"WHERE [{Column.Type}] IN (1,2,3,4,5)";
            ExecuteNonQueryInternal(toFreeThrowsSingle);

            var toFreeThrowsDouble = $"UPDATE [{Table.Games}] SET [{Column.Type}] = 2 " +
                                     $"WHERE [{Column.Type}] IN (6,7,8,9,10)";
            ExecuteNonQueryInternal(toFreeThrowsDouble);

            var toClassic = $"UPDATE [{Table.Games}] SET [{Column.Type}] = 3 " +
                            $"WHERE [{Column.Type}] IN (11,12,13,14)";
            ExecuteNonQueryInternal(toClassic);

            var deleteTypes = $"DELETE FROM [{Table.GameType}] WHERE [{Column.Id}] > 3";
            ExecuteNonQueryInternal(deleteTypes);

            var renameFreeThrowsSingle = $"UPDATE [{Table.GameType}] SET [{Column.Type}] = 'FreeThrowsSingle' " +
                                         $"WHERE [{Column.Id}] = 1";
            ExecuteNonQueryInternal(renameFreeThrowsSingle);

            var renameFreeThrowsDouble = $"UPDATE [{Table.GameType}] SET [{Column.Type}] = 'FreeThrowsDouble' " +
                                         $"WHERE [{Column.Id}] = 2";
            ExecuteNonQueryInternal(renameFreeThrowsDouble);

            var renameClassic = $"UPDATE [{Table.GameType}] SET [{Column.Type}] = 'Classic' " +
                                $"WHERE [{Column.Id}] = 3";
            ExecuteNonQueryInternal(renameClassic);
            // GameTypes simplify

            // add some settings
            var addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                               $"VALUES ('CamsDetectionWindowPositionLeft',50)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('CamsDetectionWindowPositionTop',50)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('CamsDetectionWindowHeight',1056)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('CamsDetectionWindowWidth',1944)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('MainWindowHeight',638)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('MainWindowWidth',1197)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('FreeThrowsSingleScoreWindowPositionLeft',1046)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('FreeThrowsSingleScoreWindowPositionTop',906)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('FreeThrowsSingleScoreWindowHeight',293)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('FreeThrowsSingleScoreWindowWidth',1406)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('FreeThrowsDoubleScoreWindowPositionLeft',1056)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('FreeThrowsDoubleScoreWindowPositionTop',828)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('FreeThrowsDoubleScoreWindowHeight',376)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('FreeThrowsDoubleScoreWindowWidth',1473)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('ClassicScoreWindowPositionLeft',1056)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('ClassicScoreWindowPositionTop',815)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('ClassicScoreWindowHeight',386)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           $"VALUES ('ClassicScoreWindowWidth',1472)";
            ExecuteNonQueryInternal(addParameter);
            // add some settings

            UpdateDbVersion("2.2");
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