#region Usings

using System;
using System.Collections.Generic;
using System.Data.SQLite;

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

        public void StartNewGame(GameType type, List<Player> players)
        {
            var newGameQuery = $"INSERT INTO [Games] (StartTimestamp,EndTimestamp,Type)" +
                               $" VALUES ('{DateTime.Now}','','{(int) type}')";
            ExecuteNonQueryInternal(newGameQuery);

            var newGameId = ExecuteScalarInternal("SELECT MAX(Id) FROM [Games]");
            var newGameStatisticsIds = new List<object>();

            foreach (var player in players)
            {
                switch (type)
                {
                    case GameType.FreeThrows:
                        var newGameStatisticsQuery = $"INSERT INTO [StatisticsFreeThrows] (Player,GameResult,Throws,Points,_180,Trembles,Doubles,Singles,Bulleyes,_25,Zeroes,Faults)" +
                                                     $"VALUES ({player.Id},1,0,0,0,0,0,0,0,0,0,0)";
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