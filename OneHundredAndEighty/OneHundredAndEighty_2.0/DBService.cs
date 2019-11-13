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

        public object GetSettingsValue(string name)
        {
            var query = $"SELECT [Value] FROM [Settings] WHERE [Name] = '{name}'";
            return ExecuteScalarInternal(query);
        }

        public void SaveSettingsValue(string name, object value)
        {
            var query = $"UPDATE [Settings] SET [Value] = '{value}' WHERE [Name] = '{name}'";
            ExecuteNonQueryInternal(query);
        }

        public void SaveNewPlayer(string name, string nickName)
        {
            if (name == "" || nickName == "")
            {
                //todo errorMessage
                return;
            }

            const string newPlayerStatisticsQuery = "INSERT INTO [PlayerStatistics] DEFAULT VALUES";
            ExecuteNonQueryInternal(newPlayerStatisticsQuery);
            var newPlayerStatisticsId = ExecuteScalarInternal("SELECT MAX(Id) FROM [PlayerStatistics]");

            const string newPlayerAchievesQuery = "INSERT INTO [PlayerAchieves] DEFAULT VALUES";
            ExecuteNonQueryInternal(newPlayerAchievesQuery);
            var newPlayerAchievesId = ExecuteScalarInternal("SELECT MAX(Id) FROM [PlayerAchieves]");


            var newPlayerQuery = $"INSERT INTO [Players] (Name, NickName, RegistrationTimestamp, Statistics, Achieves)" +
                                 $" VALUES ('{name}','{nickName}','{DateTime.Now}', '{newPlayerStatisticsId}', '{newPlayerAchievesId}')";
            try
            {
                ExecuteNonQueryInternal(newPlayerQuery);
            }
            catch (Exception e)
            {
                //todo errorMessage
                ExecuteNonQueryInternal($"DELETE FROM [PlayerStatistics] WHERE [Id]={newPlayerStatisticsId}");
                ExecuteNonQueryInternal($"DELETE FROM [PlayerAchieves] WHERE [Id]={newPlayerAchievesId}");
                throw e;
            }
        }

        public void StartNewGame(GameType type, List<string> players)
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
                    case GameType.FreeThrows_1:
                    case GameType.FreeThrows_2:
                        var newGameStatisticsQuery = $"INSERT INTO [StatisticsFreeThrows] (Player,GameResult,Throws,Points,_180,Trembles,Doubles,Singles,Bulleyes,_25,Zeroes,Faults)" +
                                                     $"VALUES ((SELECT [id] FROM [Players] WHERE [NickName]='{player}'),1,0,0,0,0,0,0,0,0,0,0)";
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
            connection.Close();
            connection?.Dispose();
        }
    }
}