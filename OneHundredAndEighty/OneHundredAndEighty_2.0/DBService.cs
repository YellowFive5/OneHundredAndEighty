#region Usings

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;

#endregion

namespace OneHundredAndEighty_2._0
{
    public class DBService : IDisposable
    {
        private readonly SQLiteConnection connection;
        private const string DbSource = "Database.db";

        public DBService()
        {
            connection = new SQLiteConnection($"Data Source={DbSource}; Pooling=true;");
        }

        public object GetSettingsValue(SettingsType name)
        {
            var query = $"SELECT [Value] FROM [Settings] WHERE [Name] = '{name}'";
            return ExecuteScalarInternal(query);
        }

        public void SaveSettingsValue(SettingsType name, object value)
        {
            var query = $"UPDATE [Settings] SET [Value] = '{value}' WHERE [Name] = '{name}'";
            ExecuteNonQueryInternal(query);
        }

        public void SaveNewPlayer(Player player)
        {
            const string newPlayerStatisticsQuery = "INSERT INTO [PlayerStatistics] DEFAULT VALUES";
            ExecuteNonQueryInternal(newPlayerStatisticsQuery);
            var newPlayerStatisticsId = ExecuteScalarInternal("SELECT MAX(Id) FROM [PlayerStatistics]");

            const string newPlayerAchievesQuery = "INSERT INTO [PlayerAchieves] DEFAULT VALUES";
            ExecuteNonQueryInternal(newPlayerAchievesQuery);
            var newPlayerAchievesId = ExecuteScalarInternal("SELECT MAX(Id) FROM [PlayerAchieves]");


            var newPlayerQuery = $"INSERT INTO [Players] (Name, NickName, RegistrationTimestamp, Statistics, Achieves)" +
                                 $" VALUES ('{player.Name}','{player.NickName}','{DateTime.Now}', '{newPlayerStatisticsId}', '{newPlayerAchievesId}')";
            try
            {
                ExecuteNonQueryInternal(newPlayerQuery);
                var newPlayerId = ExecuteScalarInternal("SELECT MAX(Id) FROM [Players]");
                player.SetId(Convert.ToInt32(newPlayerId));
            }
            catch (Exception e)
            {
                //todo errorMessage
                ExecuteNonQueryInternal($"DELETE FROM [PlayerStatistics] WHERE [Id]={newPlayerStatisticsId}");
                ExecuteNonQueryInternal($"DELETE FROM [PlayerAchieves] WHERE [Id]={newPlayerAchievesId}");
                throw e;
            }
        }

        public void StartNewGame(Game game, List<Player> players)
        {
            var newGameQuery = $"INSERT INTO [Games] (StartTimestamp,EndTimestamp,Type)" +
                               $" VALUES ('{game.StartTimeStamp}','','{(int) game.Type}')";
            ExecuteNonQueryInternal(newGameQuery);

            var newGameId = ExecuteScalarInternal("SELECT MAX(Id) FROM [Games]");
            game.SetId(Convert.ToInt32(newGameId));
            var newGameStatisticsIds = new List<object>();

            foreach (var player in players)
            {
                switch (game.Type)
                {
                    case GameType.FreeThrows:
                        var newGameStatisticsQuery = $"INSERT INTO [StatisticsFreeThrows] (Player,GameResult,Throws,Points,_180,Tremble,Double,Single,Bulleye,_25,Zero,Fault)" +
                                                     $"VALUES ({player.Id},{(int) GameResultType.NotDefined},0,0,0,0,0,0,0,0,0,0)";
                        ExecuteNonQueryInternal(newGameStatisticsQuery);

                        var newGameStatisticsId = ExecuteScalarInternal("SELECT MAX(Id) FROM [StatisticsFreeThrows]");
                        newGameStatisticsIds.Add(newGameStatisticsId);
                        break;

                    // todo another game types
                }
            }

            foreach (var id in newGameStatisticsIds)
            {
                ExecuteNonQueryInternal($"INSERT INTO [GameStatistics] (Game,Statistics)" +
                                        $" VALUES ({newGameId},{id})");
            }
        }

        public void EndGame(Game game, Player winner = null)
        {
            game.SetEndTimeStamp();
            var gameEndTimestampQuery = $"UPDATE [Games] SET [EndTimestamp] = '{game.EndTimeStamp}' " +
                                        $"WHERE [Id] = {game.Id}";
            ExecuteNonQueryInternal(gameEndTimestampQuery);

            if (winner != null)
            {
                var winnerGameStatisticsResultQuery = string.Empty;
                var loosersGameStatisticsResultQuery = string.Empty;

                switch (game.Type)
                {
                    case GameType.FreeThrows:
                        winnerGameStatisticsResultQuery = $"UPDATE [StatisticsFreeThrows] SET [GameResult] = {(int) GameResultType.Win} " +
                                                          $"WHERE [Id] = (SELECT [Id] FROM [StatisticsFreeThrows] AS [SFT] " +
                                                          $"INNER JOIN [GameStatistics] AS [GS] " +
                                                          $"ON [GS].[Game] = {game.Id} " +
                                                          $"AND [GS].[Statistics] = [SFT].[Id] " +
                                                          $"WHERE[Player] = {winner.Id})";
                        loosersGameStatisticsResultQuery = $"UPDATE [StatisticsFreeThrows] SET [GameResult] = {(int) GameResultType.Loose} " +
                                                           $"WHERE [Id] IN (SELECT [Id] FROM [StatisticsFreeThrows] AS [SFT] " +
                                                           $"INNER JOIN [GameStatistics] AS [GS] " +
                                                           $"ON [GS].[Game] = {game.Id} " +
                                                           $"AND [GS].[Statistics] = [SFT].[Id] " +
                                                           $"WHERE[Player] <> {winner.Id})";
                        break;
                    // todo another game types
                }

                ExecuteNonQueryInternal(winnerGameStatisticsResultQuery);
                ExecuteNonQueryInternal(loosersGameStatisticsResultQuery);

                var winnerPlayerStatisticsQuery = $"UPDATE [PlayerStatistics] SET [GamesWon] = [GamesWon] + 1 " +
                                                  $"WHERE [Id] = (SELECT [Statistics] FROM [Players] WHERE [Id] = {winner.Id})";
                ExecuteNonQueryInternal(winnerPlayerStatisticsQuery);
            }

            var playersStatisticsQuery = $"UPDATE [PlayerStatistics] SET [GamesPlayed] = [GamesPlayed] + 1 " +
                                         $"WHERE [Id] IN (SELECT [P].[Statistics] FROM [Players] AS [P] " +
                                         $"INNER JOIN [StatisticsFreeThrows] AS [SFT] " +
                                         $"ON [SFT].[Player] = [P].[Id]" +
                                         $"INNER JOIN [GameStatistics] AS [GS] " +
                                         $"ON [GS].[Game] = {game.Id} " +
                                         $"AND [GS].[Statistics] = [SFT].[Id])";
            // todo another game types
            ExecuteNonQueryInternal(playersStatisticsQuery);
        }

        public void SaveThrow(Throw thrw)
        {
            var newThrowQuery = $"INSERT INTO [Throws] (Player,Game,Sector,Type,Resultativity,Number,Points,PoiX,PoiY,ProjectionResolution,Timestamp)" +
                                $"VALUES ({thrw.Player.Id},{thrw.Game.Id},{thrw.Sector},{(int) thrw.Type},{(int) thrw.Resultativity},{thrw.Number}," +
                                $"{thrw.Points},{thrw.Poi.X.ToString(CultureInfo.InvariantCulture)},{thrw.Poi.Y.ToString(CultureInfo.InvariantCulture)},{thrw.ProjectionResolution},'{thrw.TimeStamp}')";
            ExecuteNonQueryInternal(newThrowQuery);

            var newThrowId = ExecuteScalarInternal("SELECT MAX(Id) FROM [Throws]");
            thrw.SetId(Convert.ToInt32(newThrowId));
            var incrementThrowGameStatisticsQuery = string.Empty;
            var incrementPointsGameStatisticsQuery = string.Empty;
            var incrementThrowTypeGameStatisticsQuery = string.Empty;
            switch (thrw.Game.Type)
            {
                case GameType.FreeThrows:
                    incrementThrowGameStatisticsQuery = $"UPDATE [StatisticsFreeThrows] SET [Throws] = [Throws] + 1 " +
                                                        $"WHERE [Id] = (SELECT [Id] FROM [StatisticsFreeThrows] AS [SFT] " +
                                                        $"INNER JOIN [GameStatistics] AS [GS] " +
                                                        $"ON [GS].[Game] = {thrw.Game.Id} " +
                                                        $"AND [GS].[Statistics] = [SFT].[Id] " +
                                                        $"WHERE[Player] = {thrw.Player.Id})";
                    incrementPointsGameStatisticsQuery = $"UPDATE [StatisticsFreeThrows] SET [Points] = [Points] + {thrw.Points} " +
                                                         $"WHERE [Id] = (SELECT [Id] FROM [StatisticsFreeThrows] AS [SFT] " +
                                                         $"INNER JOIN [GameStatistics] AS [GS] " +
                                                         $"ON [GS].[Game] = {thrw.Game.Id} " +
                                                         $"AND [GS].[Statistics] = [SFT].[Id] " +
                                                         $"WHERE[Player] = {thrw.Player.Id})";
                    incrementThrowTypeGameStatisticsQuery = $"UPDATE [StatisticsFreeThrows] SET [{thrw.Type}] = [{thrw.Type}] + 1 " +
                                                            $"WHERE [Id] = (SELECT [Id] FROM [StatisticsFreeThrows] AS [SFT] " +
                                                            $"INNER JOIN [GameStatistics] AS [GS] " +
                                                            $"ON [GS].[Game] = {thrw.Game.Id} " +
                                                            $"AND [GS].[Statistics] = [SFT].[Id] " +
                                                            $"WHERE[Player] = {thrw.Player.Id})";
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

        public void Dispose()
        {
            // todo: checkThis
            connection.Close();
            connection?.Dispose();
        }

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
    }
}