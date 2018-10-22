using System;
using System.Data;
using System.Data.SqlClient;

namespace OneHundredAndEighty
{
    public static class DBwork  //  Класс работы с БД
    {
        public static void LoadPlayers()    //  Подгрузка имен игроков из ДБ
        {
            MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Cсылка на главное окно
            string connectionstring = @"Data Source =(LocalDB)\MSSQLLocalDB; AttachDbFilename = |DataDirectory|\DB.mdf; Integrated Security = True; Pooling=True";
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Players", connection);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                MainWindow.Player1NameCombobox.ItemsSource = dt.DefaultView;
                MainWindow.Player1NameCombobox.DisplayMemberPath = "Name";
                MainWindow.Player1NameCombobox.SelectedValuePath = "Id";
                MainWindow.Player2NameCombobox.ItemsSource = dt.DefaultView;
                MainWindow.Player2NameCombobox.DisplayMemberPath = "Name";
                MainWindow.Player2NameCombobox.SelectedValuePath = "Id";
                connection.Close();
            }
        }
        public static void SaveNewPlayer(string name, string nickname, DateTime date)  //  Сохранение нового игрока в БД
        {
            MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Cсылка на главное окно
            string connectionstring = @"Data Source =(LocalDB)\MSSQLLocalDB; AttachDbFilename = |DataDirectory|\DB.mdf; Integrated Security = True; Pooling=True";
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Players (Name,Nickname,RegistrationDate) VALUES(@name,@nickname,@date)", connection);
                cmd.Parameters.Add(new SqlParameter("@name", name));
                cmd.Parameters.Add(new SqlParameter("@nickname", nickname));
                cmd.Parameters.Add(new SqlParameter("@date", date));
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            LoadPlayers();
        }
        public static void SaveGameData() { }
        public static void SavePlayerData() { }

    }
}
