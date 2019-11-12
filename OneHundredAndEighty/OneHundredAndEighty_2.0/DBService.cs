#region Usings

using System;
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
                throw;
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
                throw;
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
            const string newPlayerStatisticsQuery = "INSERT INTO [PlayerStatistics] DEFAULT VALUES";
            ExecuteNonQueryInternal(newPlayerStatisticsQuery);
            var newPlayerStatisticsId = ExecuteScalarInternal("SELECT MAX(Id) FROM [PlayerStatistics]");

            const string newPlayerAchievesQuery = "INSERT INTO [PlayerAchieves] DEFAULT VALUES";
            ExecuteNonQueryInternal(newPlayerAchievesQuery);
            var newPlayerAchievesId = ExecuteScalarInternal("SELECT MAX(Id) FROM [PlayerAchieves]");


            var newPlayerQuery = $"INSERT INTO [Players] (Name, NickName, RegistrationTimestamp, Statistics, Achieves)" +
                                 $" VALUES ('{name}','{nickName}','{DateTime.Now}', '{newPlayerStatisticsId}', '{newPlayerAchievesId}')";
            ExecuteNonQueryInternal(newPlayerQuery);
        }

        public void Dispose()
        {
            connection.Close();
            connection?.Dispose();
        }
    }
}