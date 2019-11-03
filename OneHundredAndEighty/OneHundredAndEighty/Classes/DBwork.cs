#region Usings

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

#endregion

namespace OneHundredAndEighty
{
    public static class DBwork //  Класс работы с БД
    {
        private static readonly MainWindow MainWindow = (MainWindow) Application.Current.MainWindow;
        private static string connectionstring = @"Data Source =(LocalDB)\MSSQLLocalDB; AttachDbFilename = |DataDirectory|\DB.mdf; Integrated Security = True; Pooling=True";

        public static void SaveSettings()
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();

                var player1NameComboBox = new SqlCommand("UPDATE Settings SET IntValue=@value WHERE SettingName='Player1NameBoxSelectedItem'", connection);
                player1NameComboBox.Parameters.AddWithValue("@value", MainWindow.Player1NameCombobox.SelectedIndex);
                player1NameComboBox.ExecuteNonQuery();
                var player2NameComboBox = new SqlCommand("UPDATE Settings SET IntValue=@value WHERE SettingName='Player2NameBoxSelectedItem'", connection);
                player2NameComboBox.Parameters.AddWithValue("@value", MainWindow.Player2NameCombobox.SelectedIndex);
                player2NameComboBox.ExecuteNonQuery();
                var setBox = new SqlCommand("UPDATE Settings SET IntValue=@value WHERE SettingName='SetsBoxSelectedItem'", connection);
                setBox.Parameters.AddWithValue("@value", MainWindow.SetBox.SelectedIndex);
                setBox.ExecuteNonQuery();
                var legBox = new SqlCommand("UPDATE Settings SET IntValue=@value WHERE SettingName='LegsBoxSelectedItem'", connection);
                legBox.Parameters.AddWithValue("@value", MainWindow.LegBox.SelectedIndex);
                legBox.ExecuteNonQuery();
                var pointsBox = new SqlCommand("UPDATE Settings SET IntValue=@value WHERE SettingName='PointsBoxSelectedItem'", connection);
                pointsBox.Parameters.AddWithValue("@value", MainWindow.PointsBox.SelectedIndex);
                pointsBox.ExecuteNonQuery();
                var player1RadioButton = new SqlCommand("UPDATE Settings SET BoolValue=@value WHERE SettingName='Player1RadioButtonIsChecked'", connection);
                player1RadioButton.Parameters.AddWithValue("@value", MainWindow.Player1Radiobutton.IsChecked);
                player1RadioButton.ExecuteNonQuery();
                var player2RadioButton = new SqlCommand("UPDATE Settings SET BoolValue=@value WHERE SettingName='Player2RadioButtonIsChecked'", connection);
                player2RadioButton.Parameters.AddWithValue("@value", MainWindow.Player2Radiobutton.IsChecked);
                player2RadioButton.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static void LoadSettings()
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();

                MainWindow.Player1NameCombobox.SelectedIndex = (int) new SqlCommand("SELECT IntValue FROM Settings WHERE SettingName='Player1NameBoxSelectedItem'", connection).ExecuteScalar();
                MainWindow.Player2NameCombobox.SelectedIndex = (int) new SqlCommand("SELECT IntValue FROM Settings WHERE SettingName='Player2NameBoxSelectedItem'", connection).ExecuteScalar();
                MainWindow.SetBox.SelectedIndex = (int) new SqlCommand("SELECT IntValue FROM Settings WHERE SettingName='SetsBoxSelectedItem'", connection).ExecuteScalar();
                MainWindow.LegBox.SelectedIndex = (int) new SqlCommand("SELECT IntValue FROM Settings WHERE SettingName='LegsBoxSelectedItem'", connection).ExecuteScalar();
                MainWindow.PointsBox.SelectedIndex = (int) new SqlCommand("SELECT IntValue FROM Settings WHERE SettingName='PointsBoxSelectedItem'", connection).ExecuteScalar();
                MainWindow.Player1Radiobutton.IsChecked = (bool) new SqlCommand("SELECT BoolValue FROM Settings WHERE SettingName='Player1RadioButtonIsChecked'", connection).ExecuteScalar();
                MainWindow.Player2Radiobutton.IsChecked = (bool) new SqlCommand("SELECT BoolValue FROM Settings WHERE SettingName='Player2RadioButtonIsChecked'", connection).ExecuteScalar();

                connection.Close();
            }
        }

        public static void LoadPlayers() //  Подгрузка имен игроков из ДБ в комбобоксы
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                var cmd = new SqlCommand("SELECT Id,Name,Nickname FROM Players", connection);
                var dt = new DataTable();
                var da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                MainWindow.Player1NameCombobox.ItemsSource = dt.DefaultView;
                MainWindow.Player1NameCombobox.DisplayMemberPath = "Nickname";
                MainWindow.Player1NameCombobox.SelectedValuePath = "Id";
                MainWindow.Player1NameCombobox.SelectedIndex = (int) new SqlCommand("SELECT IntValue FROM Settings WHERE SettingName='Player1NameBoxSelectedItem'", connection).ExecuteScalar();
                MainWindow.Player2NameCombobox.ItemsSource = dt.DefaultView;
                MainWindow.Player2NameCombobox.DisplayMemberPath = "Nickname";
                MainWindow.Player2NameCombobox.SelectedValuePath = "Id";
                MainWindow.Player2NameCombobox.SelectedIndex = (int) new SqlCommand("SELECT IntValue FROM Settings WHERE SettingName='Player2NameBoxSelectedItem'", connection).ExecuteScalar();

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

        public static void SaveNewPlayer(string name, string nickname)
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                var cmd = new SqlCommand("INSERT INTO Players (Name,Nickname,RegistrationDate) VALUES(@name,@nickname,@date)", connection);
                cmd.Parameters.Add(new SqlParameter("@name", name));
                cmd.Parameters.Add(new SqlParameter("@nickname", nickname));
                cmd.Parameters.Add(new SqlParameter("@date", DateTime.Now));
                cmd.ExecuteNonQuery();
                connection.Close();
            }

            LoadPlayers();
        }

        public static void AfterMatchSave(StatisticsWindowLogic game) //  Сохранение данных матча в БД
        {
            MainWindow.Cursor = Cursors.Wait;
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                UpdateRow("Players", game.WinnerId, "GamesWon", 1, connection);
                UpdateRow("Players", game.LooserId, "GamesLoose", 1, connection);
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
                UpdatePlayersAverages(game.Player1Id, game.Player2Id, connection); //  Вычисляем и обновляем Avarages игроков
                //  Проверяем лучший средний набор и обновляем если нужно
                CheckAndUpdateBestHand(game.Player1Id, game.AveragePlayer1Points * 3);
                CheckAndUpdateBestHand(game.Player2Id, game.AveragePlayer2Points * 3);
                //  Матч    //  Сохраняем матч в БД 
                var savematch = new SqlCommand("INSERT INTO Games VALUES (@Player1ID,@Player1Name,@Player2ID,@Player2Name,@Datetime,@WinnerID,@WinnerName,@LooserID,@LooserName,@SetsPlayed,@Player1SetsWon,@Player2SetsWon,@LegsPlayed,@Player1LegsWon,@Player2LegsWon,@Throws,@Player1Throws,@Player2Throws,@Points,@Player1Points,@Player2Points,@AvarageThrowPoints,@Player1AvaragePoints,@Player1AvarageHand,@Player2AvaragePoints,@Player2AvarageHand,@_180,@Player1180,@Player2180,@Trembles,@Player1Trembles,@Player2Trembles,@Bulleyes,@Player1Bulleyes,@Player2Bulleyes,@Doubles,@Player1Doubles,@Player2Doubles,@Singles,@Player1Singles,@Player2Singles,@_25,@Player125,@Player225,@Zeroes,@Player1Zeroes,@Player2Zeroes,@Faults,@Player1Faults,@Player2Faults,@Log)", connection);
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

            void UpdateRow(string Table, int PlayerId, string TableRow, double Add, SqlConnection connection) //  Обновление данных у игрока
            {
                var getDataCmd = new SqlCommand($"SELECT {TableRow} FROM {Table} WHERE Id = {PlayerId}", connection);
                var data = (int) getDataCmd.ExecuteScalar(); //  Получаем старые данные
                var setDataCmd = new SqlCommand($"UPDATE {Table} SET {TableRow} = {data + Add} WHERE Id={PlayerId}", connection);
                setDataCmd.ExecuteNonQuery(); //  Добавляем к старым данным новые данные
            }

            void UpdatePlayersAverages(int id1, int id2, SqlConnection connection) //  Рассчет и обновление Avarages игроков
            {
                //  Player 1
                var getPlayer1Throws = new SqlCommand($"SELECT Throws FROM Players WHERE Id = {id1}", connection);
                var player1Throws = (int) getPlayer1Throws.ExecuteScalar(); //  Получение бросков игрока 1
                var getPlayer1Points = new SqlCommand($"SELECT Points FROM Players WHERE Id = {id1}", connection);
                var player1Points = (int) getPlayer1Points.ExecuteScalar(); //  Получение очков игрока 1
                var player1AverageThrowPoints = player1Throws != 0
                                                    ? Math.Round((double) player1Points / player1Throws, 2)
                                                    : 0; //  Рассчет среднего броска
                var setPlayer1AverageThrowPoints = new SqlCommand($"UPDATE Players SET AvarageThrowPoints = @Player1AvarageThrowPoints WHERE Id={id1}", connection);
                setPlayer1AverageThrowPoints.Parameters.AddWithValue("@Player1AvarageThrowPoints", player1AverageThrowPoints);
                setPlayer1AverageThrowPoints.ExecuteNonQuery(); //  Запись среднего броска
                var setPlayer1AverageHandPoints = new SqlCommand($"UPDATE Players SET AvarageHand = @Player1AvarageHandPoints WHERE Id={id1}", connection);
                setPlayer1AverageHandPoints.Parameters.AddWithValue("@Player1AvarageHandPoints", player1AverageThrowPoints * 3);
                setPlayer1AverageHandPoints.ExecuteNonQuery(); //  Запись средних очков подхода
                //  Player 2
                var getPlayer2Throws = new SqlCommand($"SELECT Throws FROM Players WHERE Id = {id2}", connection);
                var player2Throws = (int) getPlayer2Throws.ExecuteScalar(); //  Получение бросков игрока 2
                var getPlayer2Points = new SqlCommand($"SELECT Points FROM Players WHERE Id = {id2}", connection);
                var player2Points = (int) getPlayer2Points.ExecuteScalar(); //  Получение очков игрока 2
                var player2AverageThrowPoints = player2Throws != 0
                                                    ? Math.Round((double) player2Points / player2Throws, 2)
                                                    : 0; //  Рассчет среднего броска
                var setPlayer2AverageThrowPoints = new SqlCommand($"UPDATE Players SET AvarageThrowPoints = @Player2AvarageThrowPoints WHERE Id={id2}", connection);
                setPlayer2AverageThrowPoints.Parameters.AddWithValue("@Player2AvarageThrowPoints", player2AverageThrowPoints);
                setPlayer2AverageThrowPoints.ExecuteNonQuery(); //  Запись среднего броска
                var setPlayer2AverageHandPoints = new SqlCommand($"UPDATE Players SET AvarageHand = @Player2AvarageHandPoints WHERE Id={id2}", connection);
                setPlayer2AverageHandPoints.Parameters.AddWithValue("@Player2AvarageHandPoints", player2AverageThrowPoints * 3);
                setPlayer2AverageHandPoints.ExecuteNonQuery(); //  Запись средних очков подхода
            }

            void CheckAndUpdateBestHand(int Id, double AvHand) //  Проверяем лучший средний набор и обновляем если нужно
            {
                using (var connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    var getPlayerBestHand = new SqlCommand($"SELECT AvarageHandMatchRecord FROM Players WHERE Id = {Id}", connection);
                    var playerBestHand = (double) getPlayerBestHand.ExecuteScalar(); //  Получение лучшего среднего набора игрока
                    if (AvHand > playerBestHand) //  Если в матче установлен новый рекорд - записываем
                    {
                        var setPlayerBestHand = new SqlCommand($"UPDATE Players SET AvarageHandMatchRecord = @PlayerBestHand WHERE Id={Id}", connection);
                        setPlayerBestHand.Parameters.AddWithValue("@PlayerBestHand", AvHand);
                        setPlayerBestHand.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
        }

        public static void UpdateAchieves(StatisticsWindowLogic game) //  Проверка ачивок
        {
            //  Проверка стандартных ачивок
            CheckPlayer(game.Player1Id);
            CheckPlayer(game.Player2Id);
            //  Проверка спецачивок
            if (!CheckPlayerHasAchieve(game.Player1Id, "AFirst180"))
            {
                if (game.Player1_180 > 0)
                {
                    using (var connection = new SqlConnection(connectionstring))
                    {
                        connection.Open();
                        var cmd2 = new SqlCommand($"UPDATE Players SET AFirst180 = 'True' WHERE Id = {game.Player1Id}", connection);
                        cmd2.ExecuteNonQuery();
                        var name = (string) new SqlCommand($"SELECT Nickname FROM Players WHERE Id = {game.Player1Id}", connection).ExecuteScalar();
                        connection.Close();
                        NewAchieve.ShowNewAchieveWindow(name, "AFirst180");
                    }
                }
            }

            if (!CheckPlayerHasAchieve(game.Player2Id, "AFirst180"))
            {
                if (game.Player2_180 > 0)
                {
                    using (var connection = new SqlConnection(connectionstring))
                    {
                        connection.Open();
                        var cmd2 = new SqlCommand($"UPDATE Players SET AFirst180 = 'True' WHERE Id = {game.Player2Id}", connection);
                        cmd2.ExecuteNonQuery();
                        var name = (string) new SqlCommand($"SELECT Nickname FROM Players WHERE Id = {game.Player2Id}", connection).ExecuteScalar();
                        connection.Close();
                        NewAchieve.ShowNewAchieveWindow(name, "AFirst180");
                    }
                }
            }

            if (!CheckPlayerHasAchieve(game.Player1Id, "A3Bull"))
            {
                if (game.Player1Is3Bull)
                {
                    using (var connection = new SqlConnection(connectionstring))
                    {
                        connection.Open();
                        var cmd = new SqlCommand($"UPDATE Players SET A3Bull = 'True' WHERE Id = {game.Player1Id}", connection);
                        cmd.ExecuteNonQuery();
                        var name = (string) new SqlCommand($"SELECT Nickname FROM Players WHERE Id = {game.Player1Id}", connection).ExecuteScalar();
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
                    using (var connection = new SqlConnection(connectionstring))
                    {
                        connection.Open();
                        var cmd = new SqlCommand($"UPDATE Players SET A3Bull = 'True' WHERE Id = {game.Player2Id}", connection);
                        cmd.ExecuteNonQuery();
                        var name = (string) new SqlCommand($"SELECT Nickname FROM Players WHERE Id = {game.Player2Id}", connection).ExecuteScalar();
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
                    using (var connection = new SqlConnection(connectionstring))
                    {
                        connection.Open();
                        var cmd = new SqlCommand($"UPDATE Players SET AmrZ = 'True' WHERE Id = {game.Player1Id}", connection);
                        cmd.ExecuteNonQuery();
                        var name = (string) new SqlCommand($"SELECT Nickname FROM Players WHERE Id = {game.Player1Id}", connection).ExecuteScalar();
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
                    using (var connection = new SqlConnection(connectionstring))
                    {
                        connection.Open();
                        var cmd = new SqlCommand($"UPDATE Players SET AmrZ = 'True' WHERE Id = {game.Player2Id}", connection);
                        cmd.ExecuteNonQuery();
                        var name = (string) new SqlCommand($"SELECT Nickname FROM Players WHERE Id = {game.Player2Id}", connection).ExecuteScalar();
                        connection.Close();
                        NewAchieve.ShowNewAchieveWindow(name, "AmrZ");
                        connection.Close();
                    }
                }
            }

            void CheckPlayer(int Id)
            {
                //  Если ачивка не получена - проверяем на получение в матче
                if (!CheckPlayerHasAchieve(Id, "A10matchespalyed"))
                {
                    CheckPlayerGetAchieve(Id, "GamesPlayed", "A10matchespalyed", 10);
                }

                if (!CheckPlayerHasAchieve(Id, "A100MatchesPalyed"))
                {
                    CheckPlayerGetAchieve(Id, "GamesPlayed", "A100MatchesPalyed", 100);
                }

                if (!CheckPlayerHasAchieve(Id, "A1000MatchesPalyed"))
                {
                    CheckPlayerGetAchieve(Id, "GamesPlayed", "A1000MatchesPalyed", 1000);
                }

                if (!CheckPlayerHasAchieve(Id, "A10MatchesWon"))
                {
                    CheckPlayerGetAchieve(Id, "GamesWon", "A10MatchesWon", 10);
                }

                if (!CheckPlayerHasAchieve(Id, "A100MatchesWon"))
                {
                    CheckPlayerGetAchieve(Id, "GamesWon", "A100MatchesWon", 100);
                }

                if (!CheckPlayerHasAchieve(Id, "A1000MatchesWon"))
                {
                    CheckPlayerGetAchieve(Id, "GamesWon", "A1000MatchesWon", 1000);
                }

                if (!CheckPlayerHasAchieve(Id, "A1000Throws"))
                {
                    CheckPlayerGetAchieve(Id, "Throws", "A1000Throws", 1000);
                }

                if (!CheckPlayerHasAchieve(Id, "A10000Throws"))
                {
                    CheckPlayerGetAchieve(Id, "Throws", "A10000Throws", 10000);
                }

                if (!CheckPlayerHasAchieve(Id, "A100000Throws"))
                {
                    CheckPlayerGetAchieve(Id, "Throws", "A100000Throws", 100000);
                }

                if (!CheckPlayerHasAchieve(Id, "A10000Points"))
                {
                    CheckPlayerGetAchieve(Id, "Points", "A10000Points", 10000);
                }

                if (!CheckPlayerHasAchieve(Id, "A100000Points"))
                {
                    CheckPlayerGetAchieve(Id, "Points", "A100000Points", 100000);
                }

                if (!CheckPlayerHasAchieve(Id, "A1000000Points"))
                {
                    CheckPlayerGetAchieve(Id, "Points", "A1000000Points", 1000000);
                }

                if (!CheckPlayerHasAchieve(Id, "A180x10"))
                {
                    CheckPlayerGetAchieve(Id, "_180", "A180x10", 10);
                }

                if (!CheckPlayerHasAchieve(Id, "A180x100"))
                {
                    CheckPlayerGetAchieve(Id, "_180", "A180x100", 100);
                }

                if (!CheckPlayerHasAchieve(Id, "A180x1000"))
                {
                    CheckPlayerGetAchieve(Id, "_180", "A180x1000", 1000);
                }
            }

            bool CheckPlayerHasAchieve(int id, string achieve) //  Проверка игрока на наличие ачивки
            {
                using (var connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    var cmd = new SqlCommand($"SELECT {achieve} FROM Players WHERE Id = {id}", connection);
                    var result = (bool) cmd.ExecuteScalar();
                    connection.Close();
                    return result;
                }
            }

            void CheckPlayerGetAchieve(int id, string playerRow, string achieve, int number)
            {
                using (var connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    var cmd = new SqlCommand($"SELECT {playerRow} FROM Players WHERE Id = {id}", connection);
                    var result = (int) cmd.ExecuteScalar();
                    connection.Close();
                    if (result >= number) //  Ачивка получена в процессе матча
                    {
                        connection.Open();
                        var cmd2 = new SqlCommand($"UPDATE Players SET {achieve} = 'True' WHERE Id = {id}", connection);
                        cmd2.ExecuteNonQuery();
                        var name = (string) new SqlCommand($"SELECT Nickname FROM Players WHERE Id = {id}", connection).ExecuteScalar();
                        connection.Close();
                        NewAchieve.ShowNewAchieveWindow(name, achieve);
                    }
                }
            }
        }

        public static bool IsPlayerExist(string name, string nickname) //  Проверяем БД на наличие регистрируемого игрока
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                var cmd = new SqlCommand("SELECT Id FROM Players WHERE Name=@name AND Nickname=@nickname", connection);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@nickname", nickname);
                var res = cmd.ExecuteScalar(); //  Результат запроса
                connection.Close();
                return res != null;
            }
        }

        public static DataTable LoadPlayerData(int playerId) //  Загрузка данных игрока
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                var cmd = new SqlCommand("SELECT * FROM Players WHERE Id=@Id", connection);
                cmd.Parameters.AddWithValue("@Id", playerId);
                var playerData = new DataTable();
                var da = new SqlDataAdapter(cmd);
                da.Fill(playerData);
                connection.Close();
                return playerData;
            }
        }

        public static int[] FindPvP(int player1Id, int player2Id) //  Ищем инфу PvP
        {
            var arr = new int[39]; //  Массив для найденных данных
            using (var connection = new SqlConnection(connectionstring)) //  Считаем всего игр и побед
            {
                connection.Open();
                arr[0] = (int) new SqlCommand($"SELECT COUNT(*) FROM Games WHERE (Player1Id={player1Id} AND Player2Id={player2Id}) OR (Player1Id={player2Id} AND Player2Id={player1Id})", connection).ExecuteScalar();
                arr[1] = (int) new SqlCommand($"SELECT COUNT(*) FROM Games WHERE (Player1Id={player1Id} AND Player2Id={player2Id} AND WinnerId={player1Id}) OR (Player1Id={player2Id} AND Player2Id={player1Id} AND WinnerId={player1Id})", connection).ExecuteScalar();
                arr[2] = (int) new SqlCommand($"SELECT COUNT(*) FROM Games WHERE (Player1Id={player1Id} AND Player2Id={player2Id} AND WinnerId={player2Id}) OR (Player1Id={player2Id} AND Player2Id={player1Id} AND WinnerId={player2Id})", connection).ExecuteScalar();
                connection.Close();
            }

            //  Считаем остальное
            arr[3] = GamesData("LegsPlayed");
            PlayerData(out arr[4], out arr[5], "LegsPlayer1Won", "LegsPlayer2Won");
            arr[6] = GamesData("SetsPlayed");
            PlayerData(out arr[7], out arr[8], "SetsPlayer1Won", "SetsPlayer2Won");
            arr[9] = GamesData("Throws");
            PlayerData(out arr[10], out arr[11], "Player1Throws", "Player2Throws");
            arr[12] = GamesData("Points");
            PlayerData(out arr[13], out arr[14], "Player1Points", "Player2Points");
            arr[15] = GamesData("_180");
            PlayerData(out arr[16], out arr[17], "Player1180", "Player2180");
            arr[18] = GamesData("Trembles");
            PlayerData(out arr[19], out arr[20], "Player1Trembles", "Player2Trembles");
            arr[21] = GamesData("Bulleyes");
            PlayerData(out arr[22], out arr[23], "Player1Bulleyes", "Player2Bulleyes");
            arr[24] = GamesData("Doubles");
            PlayerData(out arr[25], out arr[26], "Player1Doubles", "Player2Doubles");
            arr[27] = GamesData("_25");
            PlayerData(out arr[28], out arr[29], "Player125", "Player225");
            arr[30] = GamesData("Singles");
            PlayerData(out arr[31], out arr[32], "Player1Singles", "Player2Singles");
            arr[33] = GamesData("Zeroes");
            PlayerData(out arr[34], out arr[35], "Player1Zeroes", "Player2Zeroes");
            arr[36] = GamesData("Faults");
            PlayerData(out arr[37], out arr[38], "Player1Faults", "Player2Faults");

            void PlayerData(out int p1, out int p2, string row1, string row2) //  Считаем данные общие данные матча
            {
                using (var connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    var p1d1 = new SqlCommand($"SELECT SUM({row1}) FROM Games WHERE (Player1Id={player1Id} AND Player2Id={player2Id})", connection).ExecuteScalar();
                    var player1data = p1d1 == DBNull.Value
                                          ? 0
                                          : (int) p1d1;
                    var p1d2 = new SqlCommand($"SELECT SUM({row2}) FROM Games WHERE (Player1Id={player2Id} AND Player2Id={player1Id})", connection).ExecuteScalar();
                    player1data += p1d2 == DBNull.Value
                                       ? 0
                                       : (int) p1d2;
                    p1 = player1data;

                    var p2d1 = new SqlCommand($"SELECT SUM({row2}) FROM Games WHERE (Player1Id={player1Id} AND Player2Id={player2Id})", connection).ExecuteScalar();
                    var player2data = p2d1 == DBNull.Value
                                          ? 0
                                          : (int) p2d1;
                    var p2d2 = new SqlCommand($"SELECT SUM({row1}) FROM Games WHERE (Player1Id={player2Id} AND Player2Id={player1Id})", connection).ExecuteScalar();
                    player2data += p2d2 == DBNull.Value
                                       ? 0
                                       : (int) p2d2;
                    p2 = player2data;
                    connection.Close();
                }
            }

            int GamesData(string row) //  Считаем за каждого игрока
            {
                using (var connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    var res = new SqlCommand($"SELECT SUM({row}) FROM Games WHERE (Player1Id={player1Id} AND Player2Id={player2Id}) OR (Player1Id={player2Id} AND Player2Id={player1Id})", connection).ExecuteScalar();
                    var result = res == DBNull.Value
                                     ? 0
                                     : (int) res;
                    connection.Close();
                    return result;
                }
            }

            return arr;
        }
    }
}