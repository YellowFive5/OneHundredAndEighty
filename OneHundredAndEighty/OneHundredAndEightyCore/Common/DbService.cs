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
            var newThrowQuery = $"INSERT INTO [{Table.Throws}] ({Column.PlayerId},{Column.GameId},{Column.Sector},{Column.ThrowTypeId},{Column.ThrowResultId},{Column.Number},{Column.Points},{Column.PoiX},{Column.PoiY},{Column.ProjectionResolution},{Column.DateTime})" +
                                $"VALUES ({thrw.Player.Id},{game.Id},{thrw.Sector},{(int) thrw.Type},{(int) thrw.Result},{thrw.Number}," +
                                $"{thrw.Points},{thrw.Poi.X.ToString(CultureInfo.InvariantCulture)},{thrw.Poi.Y.ToString(CultureInfo.InvariantCulture)},{thrw.ProjectionResolution},'{thrw.TimeStamp}')";
            ExecuteNonQueryInternal(newThrowQuery);

            return Convert.ToInt32(ExecuteScalarInternal($"SELECT MAX({Column.Id}) FROM [{Table.Throws}]"));
        }

        #endregion

        #region Game

        public int GameSaveNew(Domain.Game game)
        {
            var newGameQuery = $"INSERT INTO [{Table.Games}] ({Column.StartDateTime},{Column.EndDateTime},{Column.GameTypeId})" +
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
                                           $"ON [GS].[{Column.GameId}] = {game.Id} " +
                                           $"AND [GS].[{Column.StatisticId}] = [S].[{Column.Id}] " +
                                           $"WHERE[{Column.PlayerId}] = {player.Id})";

            ExecuteNonQueryInternal(setLegsWonForPlayerQuery);
        }

        public void StatisticUpdateSetSetsWonForPlayer(Player player, Domain.Game game)
        {
            var setSetsWonForPlayerQuery = $"UPDATE [{Table.Statistic}] SET [{Column.SetsWon}] = {player.GameData.SetsWon} " +
                                           $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                           $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                           $"ON [GS].[{Column.GameId}] = {game.Id} " +
                                           $"AND [GS].[{Column.StatisticId}] = [S].[{Column.Id}] " +
                                           $"WHERE[{Column.PlayerId}] = {player.Id})";

            ExecuteNonQueryInternal(setSetsWonForPlayerQuery);
        }

        public void StatisticUpdateSetLegsPlayedForPlayer(Domain.Game game)
        {
            var setLegsPlayedForPlayerQuery = $"UPDATE [{Table.Statistic}] SET [{Column.LegsPlayed}] = {game.Players.Sum(p => p.GameData.LegsWon)} " +
                                              $"WHERE [{Column.Id}] IN (SELECT [{Column.StatisticId}] FROM [{Table.GameStatistic}] " +
                                              $"WHERE [{Column.GameId}] = {game.Id})";

            ExecuteNonQueryInternal(setLegsPlayedForPlayerQuery);
        }

        public void StatisticUpdateSetSetsPlayedForPlayer(Domain.Game game)
        {
            var setSetsPlayedForPlayerQuery = $"UPDATE [{Table.Statistic}] SET [{Column.SetsPlayed}] = {game.Players.Sum(p => p.GameData.SetsWon)} " +
                                              $"WHERE [{Column.Id}] IN (SELECT [{Column.StatisticId}] FROM [{Table.GameStatistic}] " +
                                              $"WHERE [{Column.GameId}] = {game.Id})";

            ExecuteNonQueryInternal(setSetsPlayedForPlayerQuery);
        }

        public void StatisticUpdateAllPlayersSetGameResultAbortedOrError(Domain.Game game)
        {
            var playersGameStatisticsResultQuery = $"UPDATE [{Table.Statistic}] SET [{Column.GameResultTypeId}] = {(int) game.Result} " +
                                                   $"WHERE [{Column.Id}] IN (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                                   $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                                   $"ON [GS].[{Column.GameId}] = {game.Id} " +
                                                   $"AND [GS].[{Column.StatisticId}] = [S].[{Column.Id}])";

            ExecuteNonQueryInternal(playersGameStatisticsResultQuery);
        }

        public void StatisticUpdateAllPlayersSetGameResultForWinnersAndLosers(Domain.Game game)
        {
            var winnerGameStatisticsResultQuery = $"UPDATE [{Table.Statistic}] SET [{Column.GameResultTypeId}] = {(int) GameResultType.Win} " +
                                                  $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                                  $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                                  $"ON [GS].[{Column.GameId}] = {game.Id} " +
                                                  $"AND [GS].[{Column.StatisticId}] = [S].[{Column.Id}] " +
                                                  $"WHERE[{Column.PlayerId}] = {game.Winner.Id})";
            var losersGameStatisticsResultQuery = $"UPDATE [{Table.Statistic}] SET [{Column.GameResultTypeId}] = {(int) GameResultType.Loose} " +
                                                  $"WHERE [{Column.Id}] IN (SELECT [{Column.Id}] FROM [{Table.Statistic}] AS [S] " +
                                                  $"INNER JOIN [{Table.GameStatistic}] AS [GS] " +
                                                  $"ON [GS].[{Column.GameId}] = {game.Id} " +
                                                  $"AND [GS].[{Column.StatisticId}] = [S].[{Column.Id}] " +
                                                  $"WHERE[{Column.PlayerId}] <> {game.Winner.Id})";

            ExecuteNonQueryInternal(winnerGameStatisticsResultQuery);
            ExecuteNonQueryInternal(losersGameStatisticsResultQuery);
        }

        private void StatisticSaveNew(int newGameId, List<Player> players)
        {
            var newGameStatisticsIds = new List<object>();

            foreach (var player in players)
            {
                var newGameStatisticsQuery = $"INSERT INTO [{Table.Statistic}] " +
                                             $"({Column.PlayerId},{Column.GameResultTypeId})" +
                                             $"VALUES ({player.Id},{(int) GameResultType.NotDefined})";
                ExecuteNonQueryInternal(newGameStatisticsQuery);

                var newGameStatisticsId = ExecuteScalarInternal($"SELECT MAX({Column.Id}) FROM [{Table.Statistic}]");
                newGameStatisticsIds.Add(newGameStatisticsId);
            }

            foreach (var id in newGameStatisticsIds)
            {
                ExecuteNonQueryInternal($"INSERT INTO [{Table.GameStatistic}] ({Column.GameId},{Column.StatisticId})" +
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

            var newPlayerQuery = $"INSERT INTO [{Table.Players}] ({Column.Name}, {Column.NickName}, {Column.RegistrationDateTime}, {Column.Avatar})" +
                                 $" VALUES ('{player.Name}','{player.NickName}','{DateTime.Now}', '{Converter.BitmapImageToBase64(player.Avatar)}')";
            ExecuteNonQueryInternal(newPlayerQuery);
        }

        public DataTable PlayersAllLoad()
        {
            return ExecuteDataTableInternal($"SELECT * FROM [{Table.Players}]");
        }

        public DataSet PlayerLoad(int playerId)
        {
            return new DataSet(); // todo
        }

        #endregion

        #region PlayerStatistics

        public DataTable StatisticsGetForPlayer(int playerId)
        {
            var playerStatisticsQuery = $"SELECT P.{Column.Name}, P.{Column.NickName}, P.{Column.RegistrationDateTime}, " +
                                        $"COUNT(S.{Column.Id}) AS GamesPlayed, " +
                                        $"IFNULL((SELECT COUNT(S.{Column.Id}) WHERE S.{Column.GameResultTypeId} = {(int) GameResultType.Win}),0)  AS GamesWon, " +
                                        $"IFNULL((SELECT COUNT(S.{Column.Id}) WHERE S.{Column.GameResultTypeId} = {(int) GameResultType.Loose}),0)  AS GamesLoose, " +
                                        $"IFNULL((SELECT COUNT(S.{Column.Id}) WHERE G.{Column.GameTypeId} = {(int) GameType.FreeThrowsSingle}),0)  AS FreeThrowsSingleGamesPlayed, " +
                                        $"IFNULL((SELECT COUNT(S.{Column.Id}) WHERE G.{Column.GameTypeId} = {(int) GameType.FreeThrowsDouble}),0)  AS FreeThrowsDoubleGamesPlayed, " +
                                        $"IFNULL((SELECT COUNT(S.{Column.Id}) WHERE G.{Column.GameTypeId} = {(int) GameType.FreeThrowsDouble} AND S.{Column.GameResultTypeId} = {(int) GameResultType.Win}),0)  AS FreeThrowsDoubleGamesWon, " +
                                        $"IFNULL((SELECT COUNT(S.{Column.Id}) WHERE G.{Column.GameTypeId} = {(int) GameType.Classic}),0)  AS ClassicGamesPlayed, " +
                                        $"IFNULL((SELECT COUNT(S.{Column.Id}) WHERE G.{Column.GameTypeId} = {(int) GameType.Classic} AND S.{Column.GameResultTypeId} = {(int) GameResultType.Win}),0)  AS ClassicGamesGamesWon, " +
                                        $"IFNULL(SUM(S.{Column.LegsPlayed}),0)  AS LegsPlayed, " +
                                        $"IFNULL(SUM(S.{Column.LegsWon}),0)  AS LegsWon, " +
                                        $"IFNULL(SUM(S.{Column.SetsPlayed}),0)  AS SetsPlayed, " +
                                        $"IFNULL(SUM(S.{Column.SetsWon}),0)  AS SetsWon, " +
                                        $"IFNULL((SELECT COUNT(T.{Column.Id}) FROM {Table.Throws} AS T WHERE T.{Column.PlayerId} = {playerId}),0)  AS TotalThrows, " +
                                        $"IFNULL((SELECT SUM(T.Points) FROM {Table.Throws} AS T WHERE T.{Column.PlayerId} = {playerId}),0)  AS TotalPoints, " +
                                        $"IFNULL((SELECT ROUND(AVG(T.Points),3) FROM {Table.Throws} AS T WHERE T.{Column.PlayerId} = {playerId}),0)  AS AvgPointsPerThrow, " +
                                        $"IFNULL((SELECT COUNT(_180.{Column.Id}) WHERE _180.{Column.PlayerId} = {playerId}),0)  AS _180, " +
                                        $"IFNULL((SELECT COUNT(T.{Column.Id}) FROM {Table.Throws} AS T WHERE T.{Column.PlayerId} = {playerId} AND T.{Column.ThrowTypeId} = {(int) ThrowType.Single}),0)  AS SingleThrows, " +
                                        $"IFNULL((SELECT COUNT(T.{Column.Id}) FROM {Table.Throws} AS T WHERE T.{Column.PlayerId} = {playerId} AND T.{Column.ThrowTypeId} = {(int) ThrowType.Double}),0)  AS DoubleThrows, " +
                                        $"IFNULL((SELECT COUNT(T.{Column.Id}) FROM {Table.Throws} AS T WHERE T.{Column.PlayerId} = {playerId} AND T.{Column.ThrowTypeId} = {(int) ThrowType.Tremble}),0)  AS TrembleThrows, " +
                                        $"IFNULL((SELECT COUNT(T.{Column.Id}) FROM {Table.Throws} AS T WHERE T.{Column.PlayerId} = {playerId} AND T.{Column.ThrowTypeId} = {(int) ThrowType.Bull}),0)  AS BullThrows, " +
                                        $"IFNULL((SELECT COUNT(T.{Column.Id}) FROM {Table.Throws} AS T WHERE T.{Column.PlayerId} = {playerId} AND T.{Column.ThrowTypeId} = {(int) ThrowType._25}),0)  AS _25Throws, " +
                                        $"IFNULL((SELECT COUNT(T.{Column.Id}) FROM {Table.Throws} AS T WHERE T.{Column.PlayerId} = {playerId} AND T.{Column.ThrowTypeId} = {(int) ThrowType.Zero}),0)  AS ZeroThrows, " +
                                        $"COUNT(PA.{Column.AchieveId}) AS TotalAchieves " +
                                        $"FROM {Table.Players} AS P " +
                                        $"LEFT JOIN {Table.Statistic} AS S " +
                                        $"ON S.{Column.PlayerId} = P.{Column.Id} " +
                                        $"LEFT JOIN {Table.GameStatistic} AS GS " +
                                        $"ON GS.{Column.StatisticId} = S.{Column.Id} " +
                                        $"LEFT JOIN {Table.Games} AS G " +
                                        $"ON G.{Column.Id} = GS.{Column.GameId} " +
                                        $"LEFT JOIN {Table._180} AS _180 " +
                                        $"ON _180.{Column.PlayerId} = P.{Column.Id} " +
                                        $"LEFT JOIN {Table.PlayerAchieves} AS PA " +
                                        $"ON PA.{Column.PlayerId} = P.{Column.Id} " +
                                        $"WHERE P.{Column.Id} = {playerId}";

            return ExecuteDataTableInternal(playerStatisticsQuery);
        }

        #endregion

        #region _180

        public void _180SaveNew(Hand180 _180Hand, Domain.Game game)
        {
            var new180Query = $"INSERT INTO [{Table._180}] ({Column.PlayerId},{Column.GameId},{Column.Throw1Id},{Column.Throw2Id},{Column.Throw3Id}, {Column.DateTime})" +
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
            var renameSettings = $"UPDATE [Settings] SET [Name] = 'Cam1ThresholdSlider' " +
                                 $"WHERE [Name] = 'Cam1TresholdSlider' ";
            ExecuteNonQueryInternal(renameSettings);

            renameSettings = $"UPDATE [Settings] SET [Name] = 'Cam2ThresholdSlider' " +
                             $"WHERE [Name] = 'Cam2TresholdSlider'";
            ExecuteNonQueryInternal(renameSettings);

            renameSettings = $"UPDATE [Settings] SET [Name] = 'Cam3ThresholdSlider' " +
                             $"WHERE [Name] = 'Cam3TresholdSlider'";
            ExecuteNonQueryInternal(renameSettings);

            renameSettings = $"UPDATE [Settings] SET [Name] = 'Cam4ThresholdSlider' " +
                             $"WHERE [Name] = 'Cam4TresholdSlider'";
            ExecuteNonQueryInternal(renameSettings);

            UpdateDbVersion("2.1");
        }

        public void MigrateFrom2_1to2_2()
        {
            // ThrowTypeBugFix
            var addTempThrowTypeForShuffle = $"INSERT INTO [ThrowType] values (7, 'temp', 0)";
            ExecuteNonQueryInternal(addTempThrowTypeForShuffle);

            var tremblesToTemp = $"UPDATE [Throws] SET [Type] = 7 " +
                                 $"WHERE [Type] = 4";
            ExecuteNonQueryInternal(tremblesToTemp);
            var zeroesFix = $"UPDATE [Throws] SET [Type] = 4 " +
                            $"WHERE [Type] = 1";
            ExecuteNonQueryInternal(zeroesFix);
            var singlesFix = $"UPDATE [Throws] SET [Type] = 1 " +
                             $"WHERE [Type] = 2";
            ExecuteNonQueryInternal(singlesFix);
            var doublesFix = $"UPDATE [Throws] SET [Type] = 2 " +
                             $"WHERE [Type] = 3";
            ExecuteNonQueryInternal(doublesFix);
            var tremblesFix = $"UPDATE [Throws] SET [Type] = 3 " +
                              $"WHERE [Type] = 7";
            ExecuteNonQueryInternal(tremblesFix);

            var deleteTempThrowTypeForShuffle = $"DELETE FROM [ThrowType] WHERE [Id] = 7";
            ExecuteNonQueryInternal(deleteTempThrowTypeForShuffle);
            // ThrowTypeBugFix

            // GameTypes simplify
            var toFreeThrowsSingle = $"UPDATE [Games] SET [Type] = 1 " +
                                     $"WHERE [Type] IN (1,2,3,4,5)";
            ExecuteNonQueryInternal(toFreeThrowsSingle);

            var toFreeThrowsDouble = $"UPDATE [Games] SET [Type] = 2 " +
                                     $"WHERE [Type] IN (6,7,8,9,10)";
            ExecuteNonQueryInternal(toFreeThrowsDouble);

            var toClassic = $"UPDATE [Games] SET [Type] = 3 " +
                            $"WHERE [Type] IN (11,12,13,14)";
            ExecuteNonQueryInternal(toClassic);

            var deleteTypes = $"DELETE FROM [GameType] WHERE [Id] > 3";
            ExecuteNonQueryInternal(deleteTypes);

            var renameFreeThrowsSingle = $"UPDATE [GameType] SET [Type] = 'FreeThrowsSingle' " +
                                         $"WHERE [Id] = 1";
            ExecuteNonQueryInternal(renameFreeThrowsSingle);

            var renameFreeThrowsDouble = $"UPDATE [GameType] SET [Type] = 'FreeThrowsDouble' " +
                                         $"WHERE [Id] = 2";
            ExecuteNonQueryInternal(renameFreeThrowsDouble);

            var renameClassic = $"UPDATE [GameType] SET [Type] = 'Classic' " +
                                $"WHERE [Id] = 3";
            ExecuteNonQueryInternal(renameClassic);
            // GameTypes simplify

            // add some settings
            var addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                               "VALUES ('CamsDetectionWindowPositionLeft',50)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('CamsDetectionWindowPositionTop',50)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('CamsDetectionWindowHeight',1056)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('CamsDetectionWindowWidth',1944)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('MainWindowHeight',638)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('MainWindowWidth',1197)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('FreeThrowsSingleScoreWindowPositionLeft',1046)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('FreeThrowsSingleScoreWindowPositionTop',906)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('FreeThrowsSingleScoreWindowHeight',293)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('FreeThrowsSingleScoreWindowWidth',1406)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('FreeThrowsDoubleScoreWindowPositionLeft',1056)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('FreeThrowsDoubleScoreWindowPositionTop',828)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('FreeThrowsDoubleScoreWindowHeight',376)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('FreeThrowsDoubleScoreWindowWidth',1473)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('ClassicScoreWindowPositionLeft',1056)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('ClassicScoreWindowPositionTop',815)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('ClassicScoreWindowHeight',386)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('ClassicScoreWindowWidth',1472)";
            ExecuteNonQueryInternal(addParameter);
            // add some settings

            UpdateDbVersion("2.2");
        }

        public void MigrateFrom2_2to2_3()
        {
            var deleteParameter = $"DELETE FROM [Settings] WHERE [Name] = 'MovesExtraction'";
            ExecuteNonQueryInternal(deleteParameter);
            deleteParameter = $"DELETE FROM [Settings] WHERE [Name] = 'MovesDart'";
            ExecuteNonQueryInternal(deleteParameter);
            deleteParameter = $"DELETE FROM [Settings] WHERE [Name] = 'MovesNoise'";
            ExecuteNonQueryInternal(deleteParameter);

            var addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                               "VALUES ('MaxContourArc',265)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('MinContourArea',336)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('MaxContourArea',3300)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('MinContourWidth',8)";
            ExecuteNonQueryInternal(addParameter);
            addParameter = $"INSERT INTO [Settings] (Name, Value) " +
                           "VALUES ('MaxContourWidth',44)";
            ExecuteNonQueryInternal(addParameter);

            UpdateDbVersion("2.3");
        }

        public void MigrateFrom2_3to2_4()
        {
            // Achieves scheme update
            var achievesUpdate = "PRAGMA foreign_keys = OFF; " +
                                 $"CREATE TABLE [Achieves] ('Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 'Name' INTEGER NOT NULL UNIQUE); " +
                                 $"DROP TABLE [PlayerAchieves]; " +
                                 $"CREATE TABLE [PlayerAchieves] ('AchieveId' INTEGER NOT NULL, 'PlayerId' INTEGER NOT NULL, 'ObtainedDateTime' TEXT NOT NULL, FOREIGN KEY('PlayerId') REFERENCES 'Players'('Id'), FOREIGN KEY('AchieveId') REFERENCES 'Achieves'('Id')); " +
                                 $"CREATE TABLE PlayersTemp ('Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, 'Name' TEXT NOT NULL CHECK(Name!=''), 'NickName' TEXT NOT NULL CHECK(NickName!='') UNIQUE, 'RegistrationDateTime' TEXT NOT NULL, 'Avatar' TEXT); " +
                                 $"INSERT INTO PlayersTemp (Id, Name, NickName, RegistrationDateTime, Avatar) SELECT Id, Name, NickName, RegistrationTimestamp, Avatar FROM Players; " +
                                 $"DROP TABLE [Players]; " +
                                 $"ALTER TABLE PlayersTemp RENAME TO [Players]; " +
                                 $"PRAGMA foreign_keys = ON; " +
                                 $"INSERT INTO [Achieves] (Name) VALUES ('MatchesPlayed10'),('MatchesPlayed100'),('MatchesPlayed1000'),('MatchesWon10'),('MatchesWon100'),('MatchesWon1000'),('Throws1000'),('Throws10000'),('Throws100000'),('Points10000'),('Points100000'),('Points1000000'),('_180x10'),('_180x100'),('_180x1000'),('First180'),('Bullx3'),('MrZ')";
            ExecuteNonQueryInternal(achievesUpdate);

            var columnsRename = $"PRAGMA foreign_keys = OFF; " +
                                $"CREATE TABLE [ThrowsTemp] ('Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, 'PlayerId' INTEGER NOT NULL, 'GameId' INTEGER NOT NULL, 'Sector' INTEGER NOT NULL, 'ThrowTypeId' INTEGER NOT NULL, 'ThrowResultId' INTEGER NOT NULL, 'Number' INTEGER NOT NULL, 'Points' INTEGER NOT NULL, 'PoiX' INTEGER NOT NULL, 'PoiY' INTEGER NOT NULL, 'ProjectionResolution' INTEGER NOT NULL, 'DateTime' TEXT NOT NULL, FOREIGN KEY('GameId') REFERENCES 'Games'('Id'), FOREIGN KEY('ThrowTypeId') REFERENCES 'ThrowType'('Id'), FOREIGN KEY('PlayerId') REFERENCES 'Players'('Id'), FOREIGN KEY('ThrowResultId') REFERENCES 'ThrowResult'('Id')); " +
                                $"INSERT INTO [ThrowsTemp] (Id, PlayerId, GameId, Sector, ThrowTypeId, ThrowResultId, Number, Points, PoiX, PoiY, ProjectionResolution, DateTime) " +
                                $"SELECT Id, Player, Game, Sector, Type, Result, Number, Points, PoiX, PoiY, ProjectionResolution, Timestamp FROM [Throws]; " +
                                $"DROP TABLE [Throws]; " +
                                $"ALTER TABLE [ThrowsTemp] RENAME TO [Throws]; " +
                                $"CREATE TABLE [GamesTemp] ('Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, 'StartDateTime' TEXT NOT NULL, 'EndDateTime' TEXT NOT NULL, 'GameTypeId' INTEGER NOT NULL, FOREIGN KEY('GameTypeId') REFERENCES 'GameType'('Id')); " +
                                $"INSERT INTO [GamesTemp] (Id, StartDateTime, EndDateTime, GameTypeId) " +
                                $"SELECT Id, StartTimestamp, EndTimestamp, Type FROM [Games]; " +
                                $"DROP TABLE [Games]; " +
                                $"ALTER TABLE [GamesTemp] RENAME TO [Games]; " +
                                $"CREATE TABLE 'GameStatisticTemp' ('GameId' INTEGER NOT NULL, 'StatisticId' INTEGER NOT NULL, FOREIGN KEY('GameId') REFERENCES 'Games'('Id'), FOREIGN KEY('StatisticId') REFERENCES 'Statistic'('Id')); " +
                                $"INSERT INTO [GameStatisticTemp] (GameId, StatisticId) " +
                                $"SELECT Game, Statistic FROM [GameStatistic]; " +
                                $"DROP TABLE [GameStatistic]; " +
                                $"ALTER TABLE [GameStatisticTemp] RENAME TO [GameStatistic]; " +
                                $"CREATE TABLE [StatisticTemp] ('Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 'PlayerId' INTEGER NOT NULL, 'GameResultTypeId' INTEGER NOT NULL, 'LegsPlayed' INTEGER NOT NULL DEFAULT 0, 'LegsWon' INTEGER NOT NULL DEFAULT 0, 'SetsPlayed' INTEGER NOT NULL DEFAULT 0, 'SetsWon' INTEGER NOT NULL DEFAULT 0, FOREIGN KEY('PlayerId') REFERENCES 'Players'('Id'), FOREIGN KEY('GameResultTypeId') REFERENCES 'GameResultType'('Id')); " +
                                $"INSERT INTO [StatisticTemp] (Id, PlayerId, GameResultTypeId, LegsPlayed, LegsWon, SetsPlayed, SetsWon) " +
                                $"SELECT Id, Player, GameResult, LegsPlayed, LegsWon, SetsPlayed, SetsWon FROM [Statistic]; " +
                                $"DROP TABLE [Statistic]; " +
                                $"ALTER TABLE [StatisticTemp] RENAME TO [Statistic]; " +
                                $"CREATE TABLE [_180Temp] ('Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 'PlayerId' INTEGER NOT NULL, 'GameId' INTEGER NOT NULL, 'Throw1Id' INTEGER NOT NULL, 'Throw2Id' INTEGER NOT NULL, 'Throw3Id' INTEGER NOT NULL, 'DateTime' TEXT NOT NULL, FOREIGN KEY('PlayerId') REFERENCES 'Players'('Id'), FOREIGN KEY('GameId') REFERENCES 'Games'('Id'), FOREIGN KEY('Throw1Id') REFERENCES 'Throws'('Id'), FOREIGN KEY('Throw2Id') REFERENCES 'Throws'('Id'), FOREIGN KEY('Throw3Id') REFERENCES 'Throws'('Id')); " +
                                $"INSERT INTO [_180Temp] (Id, PlayerId, GameId, Throw1Id, Throw2Id, Throw3Id, DateTime) " +
                                $"SELECT Id, Player, Game, Throw1, Throw2, Throw3, TimeStamp FROM [_180]; " +
                                $"DROP TABLE [_180]; " +
                                $"ALTER TABLE [_180Temp] RENAME TO [_180]; " +
                                $"PRAGMA foreign_keys = ON; ";
            ExecuteNonQueryInternal(columnsRename);
            
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