#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public class DbService : IDbService, IDisposable
    {
        private readonly SQLiteConnection connection;
        private readonly object locker;

        public const string DatabaseCopyName = "Database_old.db";
        public const string DatabaseName = "Database.db";

        public DbService()
        {
            locker = new object();
            connection = new SQLiteConnection($"Data Source={DatabaseName}; Pooling=true;");
        }

        #region Throws

        public int ThrowSaveNew(Throw thrw, Domain.Game game)
        {
            var newThrowQuery = $"INSERT INTO [{Table.Throws}] ({Column.Player},{Column.Game},{Column.Sector},{Column.Type},{Column.Result},{Column.Number},{Column.Points},{Column.PoiX},{Column.PoiY},{Column.ProjectionResolution},{Column.Timestamp})" +
                                $"VALUES ({thrw.Player.Id},{game.Id},{thrw.Sector},{(int) thrw.Type},{(int) thrw.Result},{thrw.Number}," +
                                $"{thrw.Points},{thrw.Poi.X.ToString(CultureInfo.InvariantCulture)},{thrw.Poi.Y.ToString(CultureInfo.InvariantCulture)},{thrw.ProjectionResolution},'{thrw.TimeStamp}')";
            ExecuteNonQueryInternal(newThrowQuery);

            return Convert.ToInt32(ExecuteScalarInternal($"SELECT MAX({Column.Id}) FROM [{Table.Throws}]"));
        }

        #endregion

        #region Game

        public int GameSaveNew(Domain.Game game)
        {
            var newGameQuery = $"INSERT INTO [{Table.Games}] ({Column.StartTimestamp},{Column.EndTimestamp},{Column.Type})" +
                               $" VALUES ('{game.StartTimeStamp}','{game.EndTimeStamp}','{(int) game.Type}')";
            ExecuteNonQueryInternal(newGameQuery);

            var newGameId = Convert.ToInt32(ExecuteScalarInternal($"SELECT MAX({Column.Id}) FROM [{Table.Games}]"));

            StatisticSaveNew(newGameId, game.Players);

            return newGameId;
        }

        #endregion

        #region Statistics

        public void StatisticUpdateSetLegsWonForPlayer(Player player, Domain.Game game)
        {
            var setLegsWonForPlayerQuery = $"UPDATE [{Table.Statistic}] SET [{Column.LegsWon}] = {player.GameData.LegsWon} " +
                                           $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                           $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                           $"ON [GS].[{Column.Game}] = {game.Id} " +
                                           $"AND [GS].[{Column.Statistic}] = [S].[{Column.Id}] " +
                                           $"WHERE[{Column.Player}] = {player.Id})";

            ExecuteNonQueryInternal(setLegsWonForPlayerQuery);
        }

        public void StatisticUpdateSetSetsWonForPlayer(Player player, Domain.Game game)
        {
            var setSetsWonForPlayerQuery = $"UPDATE [{Table.Statistic}] SET [{Column.SetsWon}] = {player.GameData.SetsWon} " +
                                           $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                           $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                           $"ON [GS].[{Column.Game}] = {game.Id} " +
                                           $"AND [GS].[{Column.Statistic}] = [S].[{Column.Id}] " +
                                           $"WHERE[{Column.Player}] = {player.Id})";

            ExecuteNonQueryInternal(setSetsWonForPlayerQuery);
        }

        public void StatisticUpdateSetLegsPlayedForPlayer(Domain.Game game)
        {
            var setLegsPlayedForPlayerQuery = $"UPDATE [{Table.Statistic}] SET [{Column.LegsPlayed}] = {game.Players.Sum(p => p.GameData.LegsWon)} " +
                                              $"WHERE [{Column.Id}] IN (SELECT [{Column.Statistic}] FROM [{Table.GameStatistic}] " +
                                              $"WHERE [{Column.Game}] = {game.Id})";

            ExecuteNonQueryInternal(setLegsPlayedForPlayerQuery);
        }

        public void StatisticUpdateSetSetsPlayedForPlayer(Domain.Game game)
        {
            var setSetsPlayedForPlayerQuery = $"UPDATE [{Table.Statistic}] SET [{Column.SetsPlayed}] = {game.Players.Sum(p => p.GameData.SetsWon)} " +
                                              $"WHERE [{Column.Id}] IN (SELECT [{Column.Statistic}] FROM [{Table.GameStatistic}] " +
                                              $"WHERE [{Column.Game}] = {game.Id})";

            ExecuteNonQueryInternal(setSetsPlayedForPlayerQuery);
        }

        public void StatisticUpdateAllPlayersSetGameResultAbortedOrError(Domain.Game game)
        {
            var playersGameStatisticsResultQuery = $"UPDATE [{Table.Statistic}] SET [{Column.GameResult}] = {(int) game.Result} " +
                                                   $"WHERE [{Column.Id}] IN (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                                   $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                                   $"ON [GS].[{Column.Game}] = {game.Id} " +
                                                   $"AND [GS].[{Column.Statistic}] = [S].[{Column.Id}])";

            ExecuteNonQueryInternal(playersGameStatisticsResultQuery);
        }

        public void StatisticUpdateAllPlayersSetGameResultForWinnersAndLosers(Domain.Game game)
        {
            var winnerGameStatisticsResultQuery = $"UPDATE [{Table.Statistic}] SET [{Column.GameResult}] = {(int) GameResultType.Win} " +
                                                  $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                                  $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                                  $"ON [GS].[{Column.Game}] = {game.Id} " +
                                                  $"AND [GS].[{Column.Statistic}] = [S].[{Column.Id}] " +
                                                  $"WHERE[{Column.Player}] = {game.Winner.Id})";
            var losersGameStatisticsResultQuery = $"UPDATE [{Table.Statistic}] SET [{Column.GameResult}] = {(int) GameResultType.Loose} " +
                                                  $"WHERE [{Column.Id}] IN (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                                  $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                                  $"ON [GS].[{Column.Game}] = {game.Id} " +
                                                  $"AND [GS].[{Column.Statistic}] = [S].[{Column.Id}] " +
                                                  $"WHERE[{Column.Player}] <> {game.Winner.Id})";

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

            var newPlayerQuery = $"INSERT INTO [{Table.Players}] ({Column.Name}, {Column.NickName}, {Column.RegistrationTimestamp}, {Column.Avatar})" +
                                 $" VALUES ('{player.Name}','{player.NickName}','{DateTime.Now}', '{Converter.BitmapImageToBase64(player.Avatar)}')";
            ExecuteNonQueryInternal(newPlayerQuery);
        }

        public DataTable PlayersLoadAll()
        {
            return ExecuteDataTableInternal($"SELECT * FROM [{Table.Players}]");
        }

        #endregion

        #region PlayerStatistics

        public DataTable StatisticsGetForPlayer(int playerId)
        {
            var playerStatisticsQuery = $"SELECT P.{Column.Name}, P.{Column.NickName}, P.{Column.RegistrationTimestamp}, " +
                                        $"COUNT(S.{Column.Id}) AS GamesPlayed, " +
                                        $"IFNULL((SELECT COUNT(S.{Column.Id}) WHERE S.{Column.GameResult} = {(int) GameResultType.Win}),0)  AS GamesWon, " +
                                        $"IFNULL((SELECT COUNT(S.{Column.Id}) WHERE S.{Column.GameResult} = {(int) GameResultType.Loose}),0)  AS GamesLoose, " +
                                        $"IFNULL((SELECT COUNT(S.{Column.Id}) WHERE G.{Column.Type} = {(int) GameType.FreeThrowsSingle}),0)  AS FreeThrowsSingleGamesPlayed, " +
                                        $"IFNULL((SELECT COUNT(S.{Column.Id}) WHERE G.{Column.Type} = {(int) GameType.FreeThrowsDouble}),0)  AS FreeThrowsDoubleGamesPlayed, " +
                                        $"IFNULL((SELECT COUNT(S.{Column.Id}) WHERE G.{Column.Type} = {(int) GameType.FreeThrowsDouble} AND S.{Column.GameResult} = {(int) GameResultType.Win}),0)  AS FreeThrowsDoubleGamesWon, " +
                                        $"IFNULL((SELECT COUNT(S.{Column.Id}) WHERE G.{Column.Type} = {(int) GameType.Classic}),0)  AS ClassicGamesPlayed, " +
                                        $"IFNULL((SELECT COUNT(S.{Column.Id}) WHERE G.{Column.Type} = {(int) GameType.Classic} AND S.{Column.GameResult} = {(int) GameResultType.Win}),0)  AS ClassicGamesGamesWon, " +
                                        $"IFNULL(SUM(S.{Column.LegsPlayed}),0)  AS LegsPlayed, " +
                                        $"IFNULL(SUM(S.{Column.LegsWon}),0)  AS LegsWon, " +
                                        $"IFNULL(SUM(S.{Column.SetsPlayed}),0)  AS SetsPlayed, " +
                                        $"IFNULL(SUM(S.{Column.SetsWon}),0)  AS SetsWon, " +
                                        $"IFNULL((SELECT COUNT(T.{Column.Id}) FROM {Table.Throws} AS T WHERE T.{Column.Player} = {playerId}),0)  AS TotalThrows, " +
                                        $"IFNULL((SELECT SUM(T.Points) FROM {Table.Throws} AS T WHERE T.{Column.Player} = {playerId}),0)  AS TotalPoints, " +
                                        $"IFNULL((SELECT ROUND(AVG(T.Points),3) FROM {Table.Throws} AS T WHERE T.{Column.Player} = {playerId}),0)  AS AvgPointsPerThrow, " +
                                        $"IFNULL((SELECT COUNT(_180.{Column.Id}) WHERE _180.{Column.Player} = {playerId}),0)  AS _180, " +
                                        $"IFNULL((SELECT COUNT(T.{Column.Id}) FROM {Table.Throws} AS T WHERE T.{Column.Player} = {playerId} AND T.{Column.Type} = {(int) ThrowType.Single}),0)  AS SingleThrows, " +
                                        $"IFNULL((SELECT COUNT(T.{Column.Id}) FROM {Table.Throws} AS T WHERE T.{Column.Player} = {playerId} AND T.{Column.Type} = {(int) ThrowType.Double}),0)  AS DoubleThrows, " +
                                        $"IFNULL((SELECT COUNT(T.{Column.Id}) FROM {Table.Throws} AS T WHERE T.{Column.Player} = {playerId} AND T.{Column.Type} = {(int) ThrowType.Tremble}),0)  AS TrembleThrows, " +
                                        $"IFNULL((SELECT COUNT(T.{Column.Id}) FROM {Table.Throws} AS T WHERE T.{Column.Player} = {playerId} AND T.{Column.Type} = {(int) ThrowType.Bull}),0)  AS BullThrows, " +
                                        $"IFNULL((SELECT COUNT(T.{Column.Id}) FROM {Table.Throws} AS T WHERE T.{Column.Player} = {playerId} AND T.{Column.Type} = {(int) ThrowType._25}),0)  AS _25Throws, " +
                                        $"IFNULL((SELECT COUNT(T.{Column.Id}) FROM {Table.Throws} AS T WHERE T.{Column.Player} = {playerId} AND T.{Column.Type} = {(int) ThrowType.Zero}),0)  AS ZeroThrows, " +
                                        $"COUNT(PA.{Column.AchieveId}) AS TotalAchieves " +
                                        $"FROM {Table.Players} AS P " +
                                        $"LEFT JOIN {Table.Statistic} AS S " +
                                        $"ON S.{Column.Player} = P.{Column.Id} " +
                                        $"LEFT JOIN {Table.GameStatistic} AS GS " +
                                        $"ON GS.{Column.Statistic} = S.{Column.Id} " +
                                        $"LEFT JOIN {Table.Games} AS G " +
                                        $"ON G.{Column.Id} = GS.{Column.Game} " +
                                        $"LEFT JOIN {Table._180} AS _180 " +
                                        $"ON _180.{Column.Player} = P.{Column.Id} " +
                                        $"LEFT JOIN {Table.PlayerAchieves} AS PA " +
                                        $"ON PA.{Column.PlayerId} = P.{Column.Id} " +
                                        $"WHERE P.{Column.Id} = {playerId}";

            return ExecuteDataTableInternal(playerStatisticsQuery);
        }

        #endregion

        #region _180

        public void _180SaveNew(Hand180 _180Hand, Domain.Game game)
        {
            var new180Query = $"INSERT INTO [{Table._180}] ({Column.Player},{Column.Game},{Column.Throw1},{Column.Throw2},{Column.Throw3}, {Column.Timestamp})" +
                              $" VALUES ('{_180Hand.Player.Id}','{game.Id}'," +
                              $"'{_180Hand.HandThrows.ElementAt(0).Id}'," +
                              $"'{_180Hand.HandThrows.ElementAt(1).Id}'," +
                              $"'{_180Hand.HandThrows.ElementAt(2).Id}'," +
                              $"'{_180Hand.HandThrows.ElementAt(2).TimeStamp}')";
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

        #region Low level internals

        private object ExecuteScalarInternal(string query)
        {
            var cmd = new SQLiteCommand(query) {Connection = connection};
            try
            {
                lock (locker)
                {
                    connection.Open();
                    var result = cmd.ExecuteScalar();
                    return result;
                }
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
                lock (locker)
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
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

        private DataTable ExecuteDataTableInternal(string query)
        {
            var cmd = new SQLiteCommand(query) {Connection = connection};

            try
            {
                lock (locker)
                {
                    connection.Open();
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        using (var dataSet = new DataSet())
                        {
                            using (var dataTable = new DataTable())
                            {
                                dataSet.Tables.Add(dataTable);
                                dataSet.EnforceConstraints = false;
                                dataTable.Load(dataReader);
                                dataReader.Close();
                                return dataTable;
                            }
                        }
                    }
                }
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
                               "VALUES ('CamsDetectionWindowPositionLeft',50)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('CamsDetectionWindowPositionTop',50)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('CamsDetectionWindowHeight',1056)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('CamsDetectionWindowWidth',1944)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('MainWindowHeight',638)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('MainWindowWidth',1197)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('FreeThrowsSingleScoreWindowPositionLeft',1046)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('FreeThrowsSingleScoreWindowPositionTop',906)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('FreeThrowsSingleScoreWindowHeight',293)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('FreeThrowsSingleScoreWindowWidth',1406)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('FreeThrowsDoubleScoreWindowPositionLeft',1056)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('FreeThrowsDoubleScoreWindowPositionTop',828)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('FreeThrowsDoubleScoreWindowHeight',376)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('FreeThrowsDoubleScoreWindowWidth',1473)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('ClassicScoreWindowPositionLeft',1056)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('ClassicScoreWindowPositionTop',815)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('ClassicScoreWindowHeight',386)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('ClassicScoreWindowWidth',1472)";
            ExecuteNonQueryInternal(addParameter);
            // add some settings

            UpdateDbVersion("2.2");
        }

        public void MigrateFrom2_2to2_3()
        {
            var deleteParameter = $"DELETE FROM [{Table.Settings}] WHERE [{Column.Name}] = 'MovesExtraction'";
            ExecuteNonQueryInternal(deleteParameter);
            deleteParameter = $"DELETE FROM [{Table.Settings}] WHERE [{Column.Name}] = 'MovesDart'";
            ExecuteNonQueryInternal(deleteParameter);
            deleteParameter = $"DELETE FROM [{Table.Settings}] WHERE [{Column.Name}] = 'MovesNoise'";
            ExecuteNonQueryInternal(deleteParameter);

            var addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                               "VALUES ('MaxContourArc',265)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('MinContourArea',336)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('MaxContourArea',3300)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('MinContourWidth',8)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [{Table.Settings}] ({Column.Name},{Column.Value}) " +
                           "VALUES ('MaxContourWidth',44)";
            ExecuteNonQueryInternal(addParameter);

            UpdateDbVersion("2.3");
        }

        public void MigrateFrom2_3to2_4()
        {
            // Achieves scheme update
            var achievesUpdate = "PRAGMA foreign_keys = OFF; " +
                                 $"CREATE TABLE [{Table.Achieves}] ('{Column.Id}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, '{Column.Name}' INTEGER NOT NULL UNIQUE); " +
                                 $"DROP TABLE [{Table.PlayerAchieves}]; " +
                                 $"CREATE TABLE [{Table.PlayerAchieves}] ('{Column.AchieveId}' INTEGER NOT NULL, '{Column.PlayerId}' INTEGER NOT NULL, '{Column.ObtainedTimeStamp}' TEXT NOT NULL, FOREIGN KEY('{Column.PlayerId}') REFERENCES '{Table.Players}'('{Column.Id}'), FOREIGN KEY('{Column.AchieveId}') REFERENCES '{Table.Achieves}'('{Column.Id}')); " +
                                 $"CREATE TABLE PlayersTemp ('Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, 'Name' TEXT NOT NULL CHECK(Name!=''), 'NickName' TEXT NOT NULL CHECK(NickName!='') UNIQUE, 'RegistrationTimestamp' TEXT NOT NULL, 'Avatar' TEXT); " +
                                 $"INSERT INTO PlayersTemp (Id, Name, NickName, RegistrationTimestamp, Avatar) SELECT Id, Name, NickName, RegistrationTimestamp, Avatar FROM Players; " +
                                 $"DROP TABLE [{Table.Players}]; " +
                                 $"ALTER TABLE PlayersTemp RENAME TO [{Table.Players}]; " +
                                 $"PRAGMA foreign_keys = ON; " +
                                 $"INSERT INTO [{Table.Achieves}] ({Column.Name}) VALUES ('{Achieve.MatchesPlayed10}'),('{Achieve.MatchesPlayed100}'),('{Achieve.MatchesPlayed1000}'),('{Achieve.MatchesWon10}'),('{Achieve.MatchesWon100}'),('{Achieve.MatchesWon1000}'),('{Achieve.Throws1000}'),('{Achieve.Throws10000}'),('{Achieve.Throws100000}'),('{Achieve.Points10000}'),('{Achieve.Points100000}'),('{Achieve.Points1000000}'),('{Achieve._180x10}'),('{Achieve._180x100}'),('{Achieve._180x1000}'),('{Achieve.First180}'),('{Achieve.Bullx3}'),('{Achieve.MrZ}')";
            ExecuteNonQueryInternal(achievesUpdate);

            UpdateDbVersion("2.4");
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