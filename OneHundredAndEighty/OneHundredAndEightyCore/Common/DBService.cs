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
                case GameType.FreeThrowsSingleFreePoints:
                case GameType.FreeThrowsSingle301Points:
                case GameType.FreeThrowsSingle501Points:
                case GameType.FreeThrowsSingle701Points:
                case GameType.FreeThrowsSingle1001Points:
                case GameType.FreeThrowsDoubleFreePoints:
                case GameType.FreeThrowsDouble301Points:
                case GameType.FreeThrowsDouble501Points:
                case GameType.FreeThrowsDouble701Points:
                case GameType.FreeThrowsDouble1001Points:

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
                case GameType.Classic301Points:
                    break;
                case GameType.Classic501Points:
                    break;
                case GameType.Classic701Points:
                    break;
                case GameType.Classic1001Points:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ExecuteNonQueryInternal(incrementThrowGameStatisticsQuery);
            ExecuteNonQueryInternal(incrementPointsGameStatisticsQuery);
            ExecuteNonQueryInternal(incrementThrowTypeGameStatisticsQuery);

            var incrementThrowPlayerStatisticsQuery = $"UPDATE [{Table.PlayerStatistics}] SET [{Column.Throws}] = [{Column.Throws}] + 1 " +
                                                      $"WHERE [{Column.Id}] = (SELECT [{Column.Statistics}] FROM [{Table.Players}] WHERE [{Column.Id}] = {thrw.Player.Id})";
            ExecuteNonQueryInternal(incrementThrowPlayerStatisticsQuery);

            var incrementPointsPlayerStatisticsQuery = $"UPDATE [{Table.PlayerStatistics}] SET [{Column.Points}] = [{Column.Points}] + {thrw.Points} " +
                                                       $"WHERE [{Column.Id}] = (SELECT [{Column.Statistics}] FROM [{Table.Players}] WHERE [{Column.Id}] = {thrw.Player.Id})";
            ExecuteNonQueryInternal(incrementPointsPlayerStatisticsQuery);

            var incrementThrowTypePlayerStatisticsQuery = $"UPDATE [{Table.PlayerStatistics}] SET [{thrw.Type}] = [{thrw.Type}] + 1 " +
                                                          $"WHERE [{Column.Id}] = (SELECT [{Column.Statistics}] FROM [{Table.Players}] WHERE [{Column.Id}] = {thrw.Player.Id})";
            ExecuteNonQueryInternal(incrementThrowTypePlayerStatisticsQuery);
        }

        #endregion

        #region Game

        public void SaveNewGame(Game.Game game, List<Player> players)
        {
            var newGameQuery = $"INSERT INTO [{Table.Games}] ({Column.StartTimestamp},{Column.EndTimestamp},{Column.Type})" +
                               $" VALUES ('{game.StartTimeStamp}','','{(int) game.Type}')";
            ExecuteNonQueryInternal(newGameQuery);

            var newGameId = Convert.ToInt32(ExecuteScalarInternal($"SELECT MAX({Column.Id}) FROM [{Table.Games}]"));
            game.SetId(newGameId);

            switch (game.Type)
            {
                case GameType.FreeThrowsSingleFreePoints:
                case GameType.FreeThrowsSingle301Points:
                case GameType.FreeThrowsSingle501Points:
                case GameType.FreeThrowsSingle701Points:
                case GameType.FreeThrowsSingle1001Points:
                case GameType.FreeThrowsDoubleFreePoints:
                case GameType.FreeThrowsDouble301Points:
                case GameType.FreeThrowsDouble501Points:
                case GameType.FreeThrowsDouble701Points:
                case GameType.FreeThrowsDouble1001Points:
                    SaveNewFreeThrowsStatistics(newGameId, players);
                    break;
                case GameType.Classic301Points:
                case GameType.Classic501Points:
                case GameType.Classic701Points:
                case GameType.Classic1001Points:
                    SaveNewClassicStatistics(newGameId, players);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void EndGame(Game.Game game, GameResultType gameResultType = GameResultType.NotDefined, Player winner = null)
        {
            WriteGameEndTimeStamp(game);

            if (gameResultType == GameResultType.Aborted ||
                gameResultType == GameResultType.Error)
            {
                switch (game.Type)
                {
                    case GameType.FreeThrowsSingleFreePoints:
                    case GameType.FreeThrowsSingle301Points:
                    case GameType.FreeThrowsSingle501Points:
                    case GameType.FreeThrowsSingle701Points:
                    case GameType.FreeThrowsSingle1001Points:
                    case GameType.FreeThrowsDoubleFreePoints:
                    case GameType.FreeThrowsDouble301Points:
                    case GameType.FreeThrowsDouble501Points:
                    case GameType.FreeThrowsDouble701Points:
                    case GameType.FreeThrowsDouble1001Points:
                        UpdateFreeThrowsStatistics(game.Id, gameResultType);
                        break;
                    case GameType.Classic301Points:
                        break;
                    case GameType.Classic501Points:
                        break;
                    case GameType.Classic701Points:
                        break;
                    case GameType.Classic1001Points:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (winner != null)
            {
                switch (game.Type)
                {
                    case GameType.FreeThrowsSingleFreePoints:
                    case GameType.FreeThrowsSingle301Points:
                    case GameType.FreeThrowsSingle501Points:
                    case GameType.FreeThrowsSingle701Points:
                    case GameType.FreeThrowsSingle1001Points:
                    case GameType.FreeThrowsDoubleFreePoints:
                    case GameType.FreeThrowsDouble301Points:
                    case GameType.FreeThrowsDouble501Points:
                    case GameType.FreeThrowsDouble701Points:
                    case GameType.FreeThrowsDouble1001Points:
                        UpdateFreeThrowsStatisticsForWinnerAndLoosers(game.Id, winner.Id);
                        break;
                    case GameType.Classic301Points:
                        break;
                    case GameType.Classic501Points:
                        break;
                    case GameType.Classic701Points:
                        break;
                    case GameType.Classic1001Points:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                UpdateWinnerStatistics(winner.Id);
            }

            switch (game.Type)
            {
                case GameType.FreeThrowsSingleFreePoints:
                case GameType.FreeThrowsSingle301Points:
                case GameType.FreeThrowsSingle501Points:
                case GameType.FreeThrowsSingle701Points:
                case GameType.FreeThrowsSingle1001Points:
                case GameType.FreeThrowsDoubleFreePoints:
                case GameType.FreeThrowsDouble301Points:
                case GameType.FreeThrowsDouble501Points:
                case GameType.FreeThrowsDouble701Points:
                case GameType.FreeThrowsDouble1001Points:
                    UpdatePlayerStatistics(game.Id);
                    break;
                case GameType.Classic301Points:
                    break;
                case GameType.Classic501Points:
                    break;
                case GameType.Classic701Points:
                    break;
                case GameType.Classic1001Points:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void WriteGameEndTimeStamp(Game.Game game)
        {
            game.SetEndTimeStamp();
            var gameEndTimestampQuery = $"UPDATE [{Table.Games}] SET [{Column.EndTimestamp}] = '{game.EndTimeStamp}' " +
                                        $"WHERE [Id] = {game.Id}";
            ExecuteNonQueryInternal(gameEndTimestampQuery);
        }

        #endregion

        #region Statistics

        private void UpdateFreeThrowsStatistics(int gameId, GameResultType gameResultType)
        {
            var playersGameStatisticsResultQuery = $"UPDATE [{Table.StatisticsFreeThrows}] SET [{Column.GameResult}] = {(int) gameResultType} " +
                                                   $"WHERE [{Column.Id}] IN (SELECT [{Column.Id}] FROM [{Table.StatisticsFreeThrows}] AS [SFT] " +
                                                   $"INNER JOIN [{Table.GameStatistics}] AS [GS] " +
                                                   $"ON [GS].[{Column.Game}] = {gameId} " +
                                                   $"AND [GS].[{Column.Statistics}] = [SFT].[{Column.Id}])";

            ExecuteNonQueryInternal(playersGameStatisticsResultQuery);
        }

        private void UpdatePlayerStatistics(int gameId)
        {
            var playersStatisticsQuery = $"UPDATE [{Table.PlayerStatistics}] SET [{Column.GamesPlayed}] = [{Column.GamesPlayed}] + 1 " +
                                         $"WHERE [{Column.Id}] IN (SELECT [P].[{Column.Statistics}] FROM [{Table.Players}] AS [P] " +
                                         $"INNER JOIN [{Table.StatisticsFreeThrows}] AS [SFT] " +
                                         $"ON [SFT].[{Column.Player}] = [P].[{Column.Id}]" +
                                         $"INNER JOIN [{Table.GameStatistics}] AS [GS] " +
                                         $"ON [GS].[{Column.Game}] = {gameId} " +
                                         $"AND [GS].[{Column.Statistics}] = [SFT].[{Column.Id}])";

            ExecuteNonQueryInternal(playersStatisticsQuery);
        }

        private void UpdateWinnerStatistics(int winnerId)
        {
            var winnerPlayerStatisticsQuery = $"UPDATE [{Table.PlayerStatistics}] SET [{Column.GamesWon}] = [{Column.GamesWon}] + 1 " +
                                              $"WHERE [{Column.Id}] = (SELECT [{Column.Statistics}] FROM [{Table.Players}] WHERE [{Column.Id}] = {winnerId})";
            ExecuteNonQueryInternal(winnerPlayerStatisticsQuery);
        }

        private void UpdateFreeThrowsStatisticsForWinnerAndLoosers(int gameId, int winnerId)
        {
            var winnerGameStatisticsResultQuery = $"UPDATE [{Table.StatisticsFreeThrows}] SET [{Column.GameResult}] = {(int) GameResultType.Win} " +
                                                  $"WHERE [{Column.Id}] = (SELECT [{Column.Id}] FROM [{Table.StatisticsFreeThrows}] AS [SFT] " +
                                                  $"INNER JOIN [{Table.GameStatistics}] AS [GS] " +
                                                  $"ON [GS].[{Column.Game}] = {gameId} " +
                                                  $"AND [GS].[{Column.Statistics}] = [SFT].[{Column.Id}] " +
                                                  $"WHERE[{Column.Player}] = {winnerId})";
            var loosersGameStatisticsResultQuery = $"UPDATE [{Table.StatisticsFreeThrows}] SET [{Column.GameResult}] = {(int) GameResultType.Loose} " +
                                                   $"WHERE [{Column.Id}] IN (SELECT [{Column.Id}] FROM [{Table.StatisticsFreeThrows}] AS [SFT] " +
                                                   $"INNER JOIN [{Table.GameStatistics}] AS [GS] " +
                                                   $"ON [GS].[{Column.Game}] = {gameId} " +
                                                   $"AND [GS].[{Column.Statistics}] = [SFT].[{Column.Id}] " +
                                                   $"WHERE[{Column.Player}] <> {winnerId})";

            ExecuteNonQueryInternal(winnerGameStatisticsResultQuery);
            ExecuteNonQueryInternal(loosersGameStatisticsResultQuery);
        }

        private void SaveNewFreeThrowsStatistics(int newGameId, List<Player> players)
        {
            var newGameStatisticsIds = new List<object>();

            foreach (var player in players)
            {
                var newGameStatisticsQuery = $"INSERT INTO [{Table.StatisticsFreeThrows}] " +
                                             $"({Column.Player},{Column.GameResult},{Column.Throws}," +
                                             $"{Column.Points},{Column._180},{Column.Tremble},{Column.Double}," +
                                             $"{Column.Single},{Column.Bulleye},{Column._25},{Column.Zero},{Column.Fault})" +
                                             $"VALUES ({player.Id},{(int) GameResultType.NotDefined},0,0,0,0,0,0,0,0,0,0)";
                ExecuteNonQueryInternal(newGameStatisticsQuery);

                var newGameStatisticsId = ExecuteScalarInternal($"SELECT MAX({Column.Id}) FROM [{Table.StatisticsFreeThrows}]");
                newGameStatisticsIds.Add(newGameStatisticsId);
            }

            foreach (var id in newGameStatisticsIds)
            {
                ExecuteNonQueryInternal($"INSERT INTO [{Table.GameStatistics}] ({Column.Game},{Column.Statistics})" +
                                        $" VALUES ({newGameId},{id})");
            }
        }

        private void SaveNewClassicStatistics(int newGameId, List<Player> players)
        {
            throw new NotImplementedException(); // todo
        }

        #endregion

        #region Player

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

        public DataTable LoadPlayers()
        {
            return ExecuteReaderInternal($"SELECT * FROM [{Table.Players}]");
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

        #endregion

        #region Settings

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