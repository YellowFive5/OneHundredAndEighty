using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Input;

namespace OneHundredAndEighty
{
    public static class DBwork  //  Класс работы с БД
    {
        static MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Cсылка на главное окно
        static string connectionstring = @"Data Source =(LocalDB)\MSSQLLocalDB; AttachDbFilename = |DataDirectory|\DB.mdf; Integrated Security = True; Pooling=True";

        public static void SaveSettings()   //  Сохранение настроек в БД
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlCommand player1namecombobox = new SqlCommand("UPDATE Settings SET IntValue=@value WHERE SettingName='Player1NameBoxSelectedItem'", connection);
                player1namecombobox.Parameters.AddWithValue("@value", MainWindow.Player1NameCombobox.SelectedIndex);
                player1namecombobox.ExecuteNonQuery();
                SqlCommand player2namecombobox = new SqlCommand("UPDATE Settings SET IntValue=@value WHERE SettingName='Player2NameBoxSelectedItem'", connection);
                player2namecombobox.Parameters.AddWithValue("@value", MainWindow.Player2NameCombobox.SelectedIndex);
                player2namecombobox.ExecuteNonQuery();
                SqlCommand setbox = new SqlCommand("UPDATE Settings SET IntValue=@value WHERE SettingName='SetsBoxSelectedItem'", connection);
                setbox.Parameters.AddWithValue("@value", MainWindow.SetBox.SelectedIndex);
                setbox.ExecuteNonQuery();
                SqlCommand legbox = new SqlCommand("UPDATE Settings SET IntValue=@value WHERE SettingName='LegsBoxSelectedItem'", connection);
                legbox.Parameters.AddWithValue("@value", MainWindow.LegBox.SelectedIndex);
                legbox.ExecuteNonQuery();
                SqlCommand pointsbox = new SqlCommand("UPDATE Settings SET IntValue=@value WHERE SettingName='PointsBoxSelectedItem'", connection);
                pointsbox.Parameters.AddWithValue("@value", MainWindow.PointsBox.SelectedIndex);
                pointsbox.ExecuteNonQuery();
                SqlCommand player1radio = new SqlCommand("UPDATE Settings SET BoolValue=@value WHERE SettingName='Player1RadioButtonIsChecked'", connection);
                player1radio.Parameters.AddWithValue("@value", MainWindow.Player1Radiobutton.IsChecked);
                player1radio.ExecuteNonQuery();
                SqlCommand player2radio = new SqlCommand("UPDATE Settings SET BoolValue=@value WHERE SettingName='Player2RadioButtonIsChecked'", connection);
                player2radio.Parameters.AddWithValue("@value", MainWindow.Player2Radiobutton.IsChecked);
                player2radio.ExecuteNonQuery();
                connection.Close();
            }
        }
        public static void LoadSettings()   //  Загрузка настроек из БД
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                MainWindow.Player1NameCombobox.SelectedIndex = (int)new SqlCommand("SELECT IntValue FROM Settings WHERE SettingName='Player1NameBoxSelectedItem'", connection).ExecuteScalar();
                MainWindow.Player2NameCombobox.SelectedIndex = (int)new SqlCommand("SELECT IntValue FROM Settings WHERE SettingName='Player2NameBoxSelectedItem'", connection).ExecuteScalar();
                MainWindow.SetBox.SelectedIndex = (int)new SqlCommand("SELECT IntValue FROM Settings WHERE SettingName='SetsBoxSelectedItem'", connection).ExecuteScalar();
                MainWindow.LegBox.SelectedIndex = (int)new SqlCommand("SELECT IntValue FROM Settings WHERE SettingName='LegsBoxSelectedItem'", connection).ExecuteScalar();
                MainWindow.PointsBox.SelectedIndex = (int)new SqlCommand("SELECT IntValue FROM Settings WHERE SettingName='PointsBoxSelectedItem'", connection).ExecuteScalar();
                MainWindow.Player1Radiobutton.IsChecked = (bool)new SqlCommand("SELECT BoolValue FROM Settings WHERE SettingName='Player1RadioButtonIsChecked'", connection).ExecuteScalar();
                MainWindow.Player2Radiobutton.IsChecked = (bool)new SqlCommand("SELECT BoolValue FROM Settings WHERE SettingName='Player2RadioButtonIsChecked'", connection).ExecuteScalar();
                connection.Close();
            }
        }
        public static void LoadPlayers()    //  Подгрузка имен игроков из ДБ в комбобоксы
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT Id,Name,Nickname FROM Players", connection);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                MainWindow.Player1NameCombobox.ItemsSource = dt.DefaultView;
                MainWindow.Player1NameCombobox.DisplayMemberPath = "Nickname";
                MainWindow.Player1NameCombobox.SelectedValuePath = "Id";
                MainWindow.Player1NameCombobox.SelectedIndex = (int)new SqlCommand("SELECT IntValue FROM Settings WHERE SettingName='Player1NameBoxSelectedItem'", connection).ExecuteScalar();
                MainWindow.Player2NameCombobox.ItemsSource = dt.DefaultView;
                MainWindow.Player2NameCombobox.DisplayMemberPath = "Nickname";
                MainWindow.Player2NameCombobox.SelectedValuePath = "Id";
                MainWindow.Player2NameCombobox.SelectedIndex = (int)new SqlCommand("SELECT IntValue FROM Settings WHERE SettingName='Player2NameBoxSelectedItem'", connection).ExecuteScalar();

                MainWindow.Player1TabNameCombobox.ItemsSource = dt.DefaultView;
                MainWindow.Player1TabNameCombobox.DisplayMemberPath = "Nickname";
                MainWindow.Player1TabNameCombobox.SelectedValuePath = "Id";
                MainWindow.Player2TabNameCombobox.ItemsSource = dt.DefaultView;
                MainWindow.Player2TabNameCombobox.DisplayMemberPath = "Nickname";
                MainWindow.Player2TabNameCombobox.SelectedValuePath = "Id";
                MainWindow.Player1TabNameCombobox.SelectedIndex = -1;
                MainWindow.Player2TabNameCombobox.SelectedIndex = -1;
                connection.Close();
            }
        }
        public static void SaveNewPlayer(string name, string nickname)  //  Сохранение нового игрока в БД
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Players (Name,Nickname,RegistrationDate) VALUES(@name,@nickname,@date)", connection);
                cmd.Parameters.Add(new SqlParameter("@name", name));
                cmd.Parameters.Add(new SqlParameter("@nickname", nickname));
                cmd.Parameters.Add(new SqlParameter("@date", DateTime.Now));
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            LoadPlayers();
        }
        public static void AftermatchSave(StatisticsWindowLogic game)   //  Сохранение данных матча в БД
        {
            MainWindow.Cursor = Cursors.Wait;   //  Изменяем курсор
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();  //  Открываем подключение
                UpdateRow("Players", game.WinnerId, "GamesWon", 1, connection);  //  Добавляем победителю выигранную игру
                UpdateRow("Players", game.LooserId, "GamesLoose", 1, connection);    //  Добавляем проигравшему проиграннуб игру
                //  Игрок 1
                //  Обновляем данные игрока в БД
                UpdateRow("Players", game.Player1Id, "GamesPlayed", 1, connection);
                UpdateRow("Players", game.Player1Id, "SetsPlayed", game.SetsPlayed, connection);
                UpdateRow("Players", game.Player1Id, "SetsWon", game.Player1SetsWon, connection);
                UpdateRow("Players", game.Player1Id, "LegsPlayed", game.LegsPlayed, connection);
                UpdateRow("Players", game.Player1Id, "LegsWon", game.Player1LegsWon, connection);
                UpdateRow("Players", game.Player1Id, "Throws", game.Player1Throws, connection);
                UpdateRow("Players", game.Player1Id, "Points", game.Player1Points, connection);
                UpdateRow("Players", game.Player1Id, "_180", game.Player1_180, connection);
                UpdateRow("Players", game.Player1Id, "Trembles", game.Player1TrembleThrows, connection);
                UpdateRow("Players", game.Player1Id, "Bulleyes", game.Player1BulleyeThrows, connection);
                UpdateRow("Players", game.Player1Id, "Doubles", game.Player1DoubleThrows, connection);
                UpdateRow("Players", game.Player1Id, "Singles", game.Player1SingleThrows, connection);
                UpdateRow("Players", game.Player1Id, "_25", game.Player1_25Throws, connection);
                UpdateRow("Players", game.Player1Id, "Zeroes", game.Player1ZeroThrows, connection);
                UpdateRow("Players", game.Player1Id, "Faults", game.Player1FaultThrows, connection);
                //  Игрок 2
                //  Обновляем данные игрока в БД
                UpdateRow("Players", game.Player2Id, "GamesPlayed", 1, connection);
                UpdateRow("Players", game.Player2Id, "SetsPlayed", game.SetsPlayed, connection);
                UpdateRow("Players", game.Player2Id, "SetsWon", game.Player2SetsWon, connection);
                UpdateRow("Players", game.Player2Id, "LegsPlayed", game.LegsPlayed, connection);
                UpdateRow("Players", game.Player2Id, "LegsWon", game.Player2LegsWon, connection);
                UpdateRow("Players", game.Player2Id, "Throws", game.Player2Throws, connection);
                UpdateRow("Players", game.Player2Id, "Points", game.Player2Points, connection);
                UpdateRow("Players", game.Player2Id, "_180", game.Player2_180, connection);
                UpdateRow("Players", game.Player2Id, "Trembles", game.Player2TrembleThrows, connection);
                UpdateRow("Players", game.Player2Id, "Bulleyes", game.Player2BulleyeThrows, connection);
                UpdateRow("Players", game.Player2Id, "Doubles", game.Player2DoubleThrows, connection);
                UpdateRow("Players", game.Player2Id, "Singles", game.Player2SingleThrows, connection);
                UpdateRow("Players", game.Player2Id, "_25", game.Player2_25Throws, connection);
                UpdateRow("Players", game.Player2Id, "Zeroes", game.Player2ZeroThrows, connection);
                UpdateRow("Players", game.Player2Id, "Faults", game.Player2FaultThrows, connection);
                //  Среднее игроков
                UpdatePlayersAvarages(game.Player1Id, game.Player2Id, connection);    //  Вычисляем и обновляем Avarages игроков
                //  Проверяем лучший средний набор и обновляем если нужно
                CheckAndUpdateBestHand(game.Player1Id, game.AveragePlayer1Points * 3);
                CheckAndUpdateBestHand(game.Player2Id, game.AveragePlayer2Points * 3);
                //  Матч    //  Сохраняем матч в БД 
                SqlCommand savematch = new SqlCommand("INSERT INTO Games VALUES (@Player1ID,@Player1Name,@Player2ID,@Player2Name,@Datetime,@WinnerID,@WinnerName,@LooserID,@LooserName,@SetsPlayed,@Player1SetsWon,@Player2SetsWon,@LegsPlayed,@Player1LegsWon,@Player2LegsWon,@Throws,@Player1Throws,@Player2Throws,@Points,@Player1Points,@Player2Points,@AvarageThrowPoints,@Player1AvaragePoints,@Player1AvarageHand,@Player2AvaragePoints,@Player2AvarageHand,@_180,@Player1180,@Player2180,@Trembles,@Player1Trembles,@Player2Trembles,@Bulleyes,@Player1Bulleyes,@Player2Bulleyes,@Doubles,@Player1Doubles,@Player2Doubles,@Singles,@Player1Singles,@Player2Singles,@_25,@Player125,@Player225,@Zeroes,@Player1Zeroes,@Player2Zeroes,@Faults,@Player1Faults,@Player2Faults,@Log)", connection);
                savematch.Parameters.AddWithValue("@Player1ID", game.Player1Id);
                savematch.Parameters.AddWithValue("@Player1Name", game.Player1Name);
                savematch.Parameters.AddWithValue("@Player2ID", game.Player2Id);
                savematch.Parameters.AddWithValue("@Player2Name", game.Player2Name);
                savematch.Parameters.AddWithValue("@Datetime", DateTime.Now);
                savematch.Parameters.AddWithValue("@WinnerID", game.WinnerId);
                savematch.Parameters.AddWithValue("@WinnerName", game.WinnerName);
                savematch.Parameters.AddWithValue("@LooserID", game.LooserId);
                savematch.Parameters.AddWithValue("@LooserName", game.LooserName);
                savematch.Parameters.AddWithValue("@SetsPlayed", game.SetsPlayed);
                savematch.Parameters.AddWithValue("@Player1SetsWon", game.Player1SetsWon);
                savematch.Parameters.AddWithValue("@Player2SetsWon", game.Player2SetsWon);
                savematch.Parameters.AddWithValue("@LegsPlayed", game.LegsPlayed);
                savematch.Parameters.AddWithValue("@Player1LegsWon", game.Player1LegsWon);
                savematch.Parameters.AddWithValue("@Player2LegsWon", game.Player2LegsWon);
                savematch.Parameters.AddWithValue("@Throws", game.Throws);
                savematch.Parameters.AddWithValue("@Player1Throws", game.Player1Throws);
                savematch.Parameters.AddWithValue("@Player2Throws", game.Player2Throws);
                savematch.Parameters.AddWithValue("@Points", game.Points);
                savematch.Parameters.AddWithValue("@Player1Points", game.Player1Points);
                savematch.Parameters.AddWithValue("@Player2Points", game.Player2Points);
                savematch.Parameters.AddWithValue("@AvarageThrowPoints", game.AveragePoints);
                savematch.Parameters.AddWithValue("@Player1AvaragePoints", game.AveragePlayer1Points);
                savematch.Parameters.AddWithValue("@Player1AvarageHand", game.AveragePlayer1Points * 3);
                savematch.Parameters.AddWithValue("@Player2AvaragePoints", game.AveragePlayer2Points);
                savematch.Parameters.AddWithValue("@Player2AvarageHand", game.AveragePlayer2Points * 3);
                savematch.Parameters.AddWithValue("@_180", game._180);
                savematch.Parameters.AddWithValue("@Player1180", game.Player1_180);
                savematch.Parameters.AddWithValue("@Player2180", game.Player2_180);
                savematch.Parameters.AddWithValue("@Trembles", game.TrembleThrows);
                savematch.Parameters.AddWithValue("@Player1Trembles", game.Player1TrembleThrows);
                savematch.Parameters.AddWithValue("@Player2Trembles", game.Player2TrembleThrows);
                savematch.Parameters.AddWithValue("@Bulleyes", game.BulleyeThrows);
                savematch.Parameters.AddWithValue("@Player1Bulleyes", game.Player1BulleyeThrows);
                savematch.Parameters.AddWithValue("@Player2Bulleyes", game.Player2BulleyeThrows);
                savematch.Parameters.AddWithValue("@Doubles", game.DoubleThrows);
                savematch.Parameters.AddWithValue("@Player1Doubles", game.Player1DoubleThrows);
                savematch.Parameters.AddWithValue("@Player2Doubles", game.Player2DoubleThrows);
                savematch.Parameters.AddWithValue("@Singles", game.SingleThrows);
                savematch.Parameters.AddWithValue("@Player1Singles", game.Player1SingleThrows);
                savematch.Parameters.AddWithValue("@Player2Singles", game.Player2SingleThrows);
                savematch.Parameters.AddWithValue("@_25", game._25Throws);
                savematch.Parameters.AddWithValue("@Player125", game.Player1_25Throws);
                savematch.Parameters.AddWithValue("@Player225", game.Player2_25Throws);
                savematch.Parameters.AddWithValue("@Zeroes", game.ZeroThrows);
                savematch.Parameters.AddWithValue("@Player1Zeroes", game.Player1ZeroThrows);
                savematch.Parameters.AddWithValue("@Player2Zeroes", game.Player2ZeroThrows);
                savematch.Parameters.AddWithValue("@Faults", game.FaultThrows);
                savematch.Parameters.AddWithValue("@Player1Faults", game.Player1FaultThrows);
                savematch.Parameters.AddWithValue("@Player2Faults", game.Player2FaultThrows);
                savematch.Parameters.AddWithValue("@Log", MainWindow.TextLog.Text);
                savematch.ExecuteNonQuery();
                //  Закрываем подключения
                connection.Close();
                MainWindow.Cursor = Cursors.Arrow;
            }
            void UpdateRow(string Table, int PlayerId, string TableRow, double Add, SqlConnection connection)   //  Обновление данных у игрока
            {
                SqlCommand getdatacmd = new SqlCommand(string.Format("SELECT {2} FROM {0} WHERE Id = {1}", Table, PlayerId, TableRow), connection);
                int Data = (int)getdatacmd.ExecuteScalar(); //  Получаем старые данные
                SqlCommand setdatacmd = new SqlCommand(string.Format("UPDATE {0} SET {2} = {3} WHERE Id={1}", Table, PlayerId, TableRow, Data + Add), connection);
                setdatacmd.ExecuteNonQuery();   //  Добавляем к старым данным новые данные
            }
            void UpdatePlayersAvarages(int id1, int id2, SqlConnection connection)  //  Рассчет и обновление Avarages игроков
            {   //  Player 1
                SqlCommand GetPlayer1Throws = new SqlCommand(string.Format("SELECT Throws FROM Players WHERE Id = {0}", id1), connection);
                int Player1Throws = (int)GetPlayer1Throws.ExecuteScalar();  //  Получение бросков игрока 1
                SqlCommand GetPlayer1Points = new SqlCommand(string.Format("SELECT Points FROM Players WHERE Id = {0}", id1), connection);
                int Player1Points = (int)GetPlayer1Points.ExecuteScalar();  //  Получение очков игрока 1
                double Player1AvarageThrowPoints = (Player1Throws != 0) ? Math.Round((double)Player1Points / Player1Throws, 2) : 0; //  Рассчет среднего броска
                SqlCommand SetPlayer1AvarageThrowPoints = new SqlCommand(string.Format("UPDATE Players SET AvarageThrowPoints = @Player1AvarageThrowPoints WHERE Id={0}", id1), connection);
                SetPlayer1AvarageThrowPoints.Parameters.AddWithValue("@Player1AvarageThrowPoints", Player1AvarageThrowPoints);
                SetPlayer1AvarageThrowPoints.ExecuteNonQuery(); //  Запись среднего броска
                SqlCommand SetPlayer1AvarageHandPoints = new SqlCommand(string.Format("UPDATE Players SET AvarageHand = @Player1AvarageHandPoints WHERE Id={0}", id1), connection);
                SetPlayer1AvarageHandPoints.Parameters.AddWithValue("@Player1AvarageHandPoints", Player1AvarageThrowPoints * 3);
                SetPlayer1AvarageHandPoints.ExecuteNonQuery();  //  Запись средних очков подхода
                //  Player 2
                SqlCommand GetPlayer2Throws = new SqlCommand(string.Format("SELECT Throws FROM Players WHERE Id = {0}", id2), connection);
                int Player2Throws = (int)GetPlayer2Throws.ExecuteScalar();  //  Получение бросков игрока 2
                SqlCommand GetPlayer2Points = new SqlCommand(string.Format("SELECT Points FROM Players WHERE Id = {0}", id2), connection);
                int Player2Points = (int)GetPlayer2Points.ExecuteScalar();  //  Получение очков игрока 2
                double Player2AvarageThrowPoints = (Player2Throws != 0) ? Math.Round((double)Player2Points / Player2Throws, 2) : 0; //  Рассчет среднего броска
                SqlCommand SetPlayer2AvarageThrowPoints = new SqlCommand(string.Format("UPDATE Players SET AvarageThrowPoints = @Player2AvarageThrowPoints WHERE Id={0}", id2), connection);
                SetPlayer2AvarageThrowPoints.Parameters.AddWithValue("@Player2AvarageThrowPoints", Player2AvarageThrowPoints);
                SetPlayer2AvarageThrowPoints.ExecuteNonQuery(); //  Запись среднего броска
                SqlCommand SetPlayer2AvarageHandPoints = new SqlCommand(string.Format("UPDATE Players SET AvarageHand = @Player2AvarageHandPoints WHERE Id={0}", id2), connection);
                SetPlayer2AvarageHandPoints.Parameters.AddWithValue("@Player2AvarageHandPoints", Player2AvarageThrowPoints * 3);
                SetPlayer2AvarageHandPoints.ExecuteNonQuery();  //  Запись средних очков подхода
            }
            void CheckAndUpdateBestHand(int Id, double AvHand)   //  Проверяем лучший средний набор и обновляем если нужно
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    SqlCommand GetPlayerBestHand = new SqlCommand(string.Format("SELECT AvarageHandMatchRecord FROM Players WHERE Id = {0}", Id), connection);
                    double PlayerBestHand = (double)GetPlayerBestHand.ExecuteScalar();  //  Получение лучшего среднего набора игрока
                    if (AvHand > PlayerBestHand)    //  Если в матче установлен новый рекорд - записываем
                    {
                        SqlCommand SetPlayerBestHand = new SqlCommand(string.Format("UPDATE Players SET AvarageHandMatchRecord = @PlayerBestHand WHERE Id={0}", Id), connection);
                        SetPlayerBestHand.Parameters.AddWithValue("@PlayerBestHand", AvHand);
                        SetPlayerBestHand.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
        }
        public static void UpdateAchieves(StatisticsWindowLogic game)   //  Проверка ачивок
        {
            //  Проверка стандартных ачивок
            CheckPlayer(game.Player1Id);
            CheckPlayer(game.Player2Id);
            //  Проверка спецачивок
            if (!CheckPlayerHasAchieve(game.Player1Id, "AFirst180"))
            {
                if (game.Player1_180 > 0)
                {
                    using (SqlConnection connection = new SqlConnection(connectionstring))
                    {
                        connection.Open();
                        SqlCommand cmd2 = new SqlCommand(string.Format("UPDATE Players SET AFirst180 = 'True' WHERE Id = {0}", game.Player1Id), connection);
                        cmd2.ExecuteNonQuery();
                        string name = (string)new SqlCommand(string.Format("SELECT Nickname FROM Players WHERE Id = {0}", game.Player1Id), connection).ExecuteScalar();
                        connection.Close();
                        NewAchieve.ShowNewAchieveWindow(name, "AFirst180");
                    }
                }
            }
            if (!CheckPlayerHasAchieve(game.Player2Id, "AFirst180"))
            {
                if (game.Player2_180 > 0)
                {
                    using (SqlConnection connection = new SqlConnection(connectionstring))
                    {
                        connection.Open();
                        SqlCommand cmd2 = new SqlCommand(string.Format("UPDATE Players SET AFirst180 = 'True' WHERE Id = {0}", game.Player2Id), connection);
                        cmd2.ExecuteNonQuery();
                        string name = (string)new SqlCommand(string.Format("SELECT Nickname FROM Players WHERE Id = {0}", game.Player2Id), connection).ExecuteScalar();
                        connection.Close();
                        NewAchieve.ShowNewAchieveWindow(name, "AFirst180");
                    }
                }
            }
            if (!CheckPlayerHasAchieve(game.Player1Id, "A3Bull"))
            {
                if (game.Player1Is3Bull)
                {
                    using (SqlConnection connection = new SqlConnection(connectionstring))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand(string.Format("UPDATE Players SET A3Bull = 'True' WHERE Id = {0}", game.Player1Id), connection);
                        cmd.ExecuteNonQuery();
                        string name = (string)new SqlCommand(string.Format("SELECT Nickname FROM Players WHERE Id = {0}", game.Player1Id), connection).ExecuteScalar();
                        connection.Close();
                        NewAchieve.ShowNewAchieveWindow(name, "A3Bull");
                        connection.Close();
                    }
                }
            }
            if (!CheckPlayerHasAchieve(game.Player2Id, "A3Bull"))
            {
                if (game.Player2Is3Bull)
                {
                    using (SqlConnection connection = new SqlConnection(connectionstring))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand(string.Format("UPDATE Players SET A3Bull = 'True' WHERE Id = {0}", game.Player2Id), connection);
                        cmd.ExecuteNonQuery();
                        string name = (string)new SqlCommand(string.Format("SELECT Nickname FROM Players WHERE Id = {0}", game.Player2Id), connection).ExecuteScalar();
                        connection.Close();
                        NewAchieve.ShowNewAchieveWindow(name, "A3Bull");
                        connection.Close();
                    }
                }
            }
            if (!CheckPlayerHasAchieve(game.Player1Id, "AmrZ"))
            {
                if (game.Player1IsmrZ)
                {
                    using (SqlConnection connection = new SqlConnection(connectionstring))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand(string.Format("UPDATE Players SET AmrZ = 'True' WHERE Id = {0}", game.Player1Id), connection);
                        cmd.ExecuteNonQuery();
                        string name = (string)new SqlCommand(string.Format("SELECT Nickname FROM Players WHERE Id = {0}", game.Player1Id), connection).ExecuteScalar();
                        connection.Close();
                        NewAchieve.ShowNewAchieveWindow(name, "AmrZ");
                        connection.Close();
                    }
                }
            }
            if (!CheckPlayerHasAchieve(game.Player2Id, "AmrZ"))
            {
                if (game.Player2IsmrZ)
                {
                    using (SqlConnection connection = new SqlConnection(connectionstring))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand(string.Format("UPDATE Players SET AmrZ = 'True' WHERE Id = {0}", game.Player2Id), connection);
                        cmd.ExecuteNonQuery();
                        string name = (string)new SqlCommand(string.Format("SELECT Nickname FROM Players WHERE Id = {0}", game.Player2Id), connection).ExecuteScalar();
                        connection.Close();
                        NewAchieve.ShowNewAchieveWindow(name, "AmrZ");
                        connection.Close();
                    }
                }
            }

            void CheckPlayer(int Id)
            {   //  Если ачивка не получена - проверяем на получение в матче
                if (!CheckPlayerHasAchieve(Id, "A10matchespalyed"))
                    CheckPlayerGetAchieve(Id, "GamesPlayed", "A10matchespalyed", 10);
                if (!CheckPlayerHasAchieve(Id, "A100MatchesPalyed"))
                    CheckPlayerGetAchieve(Id, "GamesPlayed", "A100MatchesPalyed", 100);
                if (!CheckPlayerHasAchieve(Id, "A1000MatchesPalyed"))
                    CheckPlayerGetAchieve(Id, "GamesPlayed", "A1000MatchesPalyed", 1000);
                if (!CheckPlayerHasAchieve(Id, "A10MatchesWon"))
                    CheckPlayerGetAchieve(Id, "GamesWon", "A10MatchesWon", 10);
                if (!CheckPlayerHasAchieve(Id, "A100MatchesWon"))
                    CheckPlayerGetAchieve(Id, "GamesWon", "A100MatchesWon", 100);
                if (!CheckPlayerHasAchieve(Id, "A1000MatchesWon"))
                    CheckPlayerGetAchieve(Id, "GamesWon", "A1000MatchesWon", 1000);
                if (!CheckPlayerHasAchieve(Id, "A1000Throws"))
                    CheckPlayerGetAchieve(Id, "Throws", "A1000Throws", 1000);
                if (!CheckPlayerHasAchieve(Id, "A10000Throws"))
                    CheckPlayerGetAchieve(Id, "Throws", "A10000Throws", 10000);
                if (!CheckPlayerHasAchieve(Id, "A100000Throws"))
                    CheckPlayerGetAchieve(Id, "Throws", "A100000Throws", 100000);
                if (!CheckPlayerHasAchieve(Id, "A10000Points"))
                    CheckPlayerGetAchieve(Id, "Points", "A10000Points", 10000);
                if (!CheckPlayerHasAchieve(Id, "A100000Points"))
                    CheckPlayerGetAchieve(Id, "Points", "A100000Points", 100000);
                if (!CheckPlayerHasAchieve(Id, "A1000000Points"))
                    CheckPlayerGetAchieve(Id, "Points", "A1000000Points", 1000000);
                if (!CheckPlayerHasAchieve(Id, "A180x10"))
                    CheckPlayerGetAchieve(Id, "_180", "A180x10", 10);
                if (!CheckPlayerHasAchieve(Id, "A180x100"))
                    CheckPlayerGetAchieve(Id, "_180", "A180x100", 100);
                if (!CheckPlayerHasAchieve(Id, "A180x1000"))
                    CheckPlayerGetAchieve(Id, "_180", "A180x1000", 1000);
            }
            bool CheckPlayerHasAchieve(int Id, string achieve)    //  Проверка игрока на наличие ачивки
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(string.Format("SELECT {0} FROM Players WHERE Id = {1}", achieve, Id), connection);
                    bool result = (bool)cmd.ExecuteScalar();
                    connection.Close();
                    return result;
                }
            }
            void CheckPlayerGetAchieve(int Id, string playerrow, string achieve, int number)
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(string.Format("SELECT {0} FROM Players WHERE Id = {1}", playerrow, Id), connection);
                    int result = (int)cmd.ExecuteScalar();
                    connection.Close();
                    if (result >= number)   //  Ачивка получена в процессе матча
                    {
                        connection.Open();
                        SqlCommand cmd2 = new SqlCommand(string.Format("UPDATE Players SET {0} = 'True' WHERE Id = {1}", achieve, Id), connection);
                        cmd2.ExecuteNonQuery();
                        string name = (string)new SqlCommand(string.Format("SELECT Nickname FROM Players WHERE Id = {0}", Id), connection).ExecuteScalar();
                        connection.Close();
                        NewAchieve.ShowNewAchieveWindow(name, achieve);
                    }
                }
            }
        }
        public static bool IsPlayerExist(string name, string nickname)  //  Проверяем БД на наличие регистрируемого игрока
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT Id FROM Players WHERE Name=@name AND Nickname=@nickname", connection);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@nickname", nickname);
                var res = cmd.ExecuteScalar();  //  Результат запроса
                connection.Close();
                if (res != null)  //  Если вернулся не null значит запись есть
                    return true;
                else
                    return false;   //  Иначе такого игрока нет и можно сохранять в БД
            }

        }
        public static DataTable LoadPlayerData(int PlayerId)    //  Загрузка данных игрока
        {

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Players WHERE Id=@Id", connection);
                cmd.Parameters.AddWithValue("@Id", PlayerId);
                DataTable PlayerData = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(PlayerData);
                connection.Close();
                return PlayerData;
            }
        }
    }
}
