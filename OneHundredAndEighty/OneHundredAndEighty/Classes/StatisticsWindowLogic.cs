#region Usings

using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace OneHundredAndEighty
{
    public class StatisticsWindowLogic //  Статистика матча
    {
        public MainWindow mainWindow = (MainWindow) System.Windows.Application.Current.MainWindow; //  Cсылка на главное окно
        private Windows.StatisticWindow statisticWindow;

        public int Player1Id { get; private set; } //  Id игрока 1
        public string Player1Name { get; private set; } //  Имя игрока 1
        public int Player2Id { get; private set; } //  Id игрока 2
        public string Player2Name { get; private set; } //  Имя игрока 2
        public int WinnerId { get; private set; } //  Id победителя
        public string WinnerName { get; private set; } //  Имя победителя
        public int LooserId { get; private set; } //  Id проигравшего
        public string LooserName { get; private set; } //  Имя проигравшего
        public int Throws { get; private set; } //  Всего бросков
        public int Player1Throws { get; private set; } //  Бросков у игрока 1
        public int Player2Throws { get; private set; } //  Бросков у игрока 2
        public int Points { get; private set; } //  Всего очков
        public int Player1Points { get; private set; } //  Очков у игрока 1
        public int Player2Points { get; private set; } //  Очков у игрока 2
        public double AveragePoints { get; private set; } //  Средние очки за бросок
        public double AveragePlayer1Points { get; private set; } //  Средние очки за бросок игрока 1
        public double AveragePlayer2Points { get; private set; } //  Средние очки за бросок игрока 2
        public int SetsPlayed { get; private set; } //  Сыграно сетов
        public int Player1SetsWon { get; private set; } //  Выиграно сетов игроком 1
        public int Player2SetsWon { get; private set; } //  Выиграно сетов игроком 2
        public int LegsPlayed { get; private set; } //  Сыграно легов
        public int Player1LegsWon { get; private set; } //  Выиграно легов игроком 1
        public int Player2LegsWon { get; private set; } //  Выиграно легов игроком 2
        public int FaultThrows { get; private set; } //  Штрафных бросков
        public int Player1FaultThrows { get; private set; } //  Штрафных бросков игрока 1
        public int Player2FaultThrows { get; private set; } //  Штрафных бросков игрока 2
        public int _180 { get; private set; } //  Количество набора 180 очков
        public int Player1_180 { get; private set; } //  180 у игрока 1
        public int Player2_180 { get; private set; } //  180 у игрока 2
        public int SingleThrows { get; private set; } //  Бросков в одинарный сектор
        public int Player1SingleThrows { get; private set; } //  Бросков в одинарный сектор игрока 1
        public int Player2SingleThrows { get; private set; } //  Бросков в одинарный сектор игрока 2
        public int DoubleThrows { get; private set; } //  Бросков в удвоение
        public int Player1DoubleThrows { get; private set; } //  Бросков в удвоение игрока 1
        public int Player2DoubleThrows { get; private set; } //  Бросков в удвоение игрока 2
        public int TrembleThrows { get; private set; } //  Бросков в утроение
        public int Player1TrembleThrows { get; private set; } //  Бросков в утроение игрока 1
        public int Player2TrembleThrows { get; private set; } //  Бросков в утроение игрока 2
        public int _25Throws { get; private set; } //  Бросков в 25
        public int Player1_25Throws { get; private set; } //  Бросков в 25 игрока 1
        public int Player2_25Throws { get; private set; } //  Бросков в 25 игрока 2
        public int BulleyeThrows { get; private set; } //  Бросков в булл
        public int Player1BulleyeThrows { get; private set; } //  Бросков в булл игрока 1
        public int Player2BulleyeThrows { get; private set; } //  Бросков в булл игрока 2
        public int ZeroThrows { get; private set; } //  Бросков в 0
        public int Player1ZeroThrows { get; private set; } //  Бросков в 0 игрока 1
        public int Player2ZeroThrows { get; private set; } //  Бросков в 0 игрока 2
        public bool Player1IsmrZ { get; private set; } //  Ачивка игрока 1
        public bool Player1Is3Bull { get; private set; } //  Ачивка игрока 1
        public bool Player2IsmrZ { get; private set; } //  Ачивка игрока 2
        public bool Player2Is3Bull { get; private set; } //  Ачивка игрока 2

        public void CountMatchStatistics(Player winner, Player player1, Player player2, Stack<Throw> allMatchThrows) //  Считаем статистику матча
        {
            statisticWindow = new Windows.StatisticWindow(); //  Ссылка на окно статистики матча
            Player1Id = player1.DbId;
            Player1Name = player1.Name;
            Player2Id = player2.DbId;
            Player2Name = player2.Name;
            WinnerName = winner.Name;
            WinnerId = winner.DbId;
            LooserName = WinnerName == player1.Name
                             ? player2.Name
                             : player1.Name;
            LooserId = WinnerName == player1.Name
                           ? player2.DbId
                           : player1.DbId;
            Player1IsmrZ = player1.ismrZ;
            Player2IsmrZ = player2.ismrZ;
            Player1Is3Bull = player1.is3Bull;
            Player2Is3Bull = player2.is3Bull;
            while (allMatchThrows.Count != 0) //  Разбираем бросок на запчасти
            {
                var T = allMatchThrows.Pop();
                Throws += 1;
                Points += (int) T.Points;
                if (T.WhoThrow == "Player1")
                {
                    Player1Throws += 1;
                    Player1Points += (int) T.Points;
                }
                else
                {
                    Player2Throws += 1;
                    Player2Points += (int) T.Points;
                }

                if (T.IsFault)
                {
                    FaultThrows += 1;
                    if (T.WhoThrow == "Player1")
                    {
                        Player1FaultThrows += 1;
                    }
                    else
                    {
                        Player2FaultThrows += 1;
                    }
                }

                if (T.IsLegWon)
                {
                    LegsPlayed += 1;
                    if (T.WhoThrow == "Player1")
                    {
                        Player1LegsWon += 1;
                    }
                    else
                    {
                        Player2LegsWon += 1;
                    }
                }

                if (T.IsSetWon)
                {
                    SetsPlayed += 1;
                    if (T.WhoThrow == "Player1")
                    {
                        Player1SetsWon += 1;
                    }
                    else
                    {
                        Player2SetsWon += 1;
                    }
                }

                switch (T.Multiplier)
                {
                    case "Zero":
                        ZeroThrows += 1;
                        if (T.WhoThrow == "Player1")
                        {
                            Player1ZeroThrows += 1;
                        }
                        else
                        {
                            Player2ZeroThrows += 1;
                        }

                        break;
                    case "Single":
                        SingleThrows += 1;
                        if (T.WhoThrow == "Player1")
                        {
                            Player1SingleThrows += 1;
                        }
                        else
                        {
                            Player2SingleThrows += 1;
                        }

                        break;
                    case "Double":
                        DoubleThrows += 1;
                        if (T.WhoThrow == "Player1")
                        {
                            Player1DoubleThrows += 1;
                        }
                        else
                        {
                            Player2DoubleThrows += 1;
                        }

                        break;
                    case "Tremble":
                        TrembleThrows += 1;
                        if (T.WhoThrow == "Player1")
                        {
                            Player1TrembleThrows += 1;
                        }
                        else
                        {
                            Player2TrembleThrows += 1;
                        }

                        break;
                    case "Bull_25":
                        _25Throws += 1;
                        if (T.WhoThrow == "Player1")
                        {
                            Player1_25Throws += 1;
                        }
                        else
                        {
                            Player2_25Throws += 1;
                        }

                        break;
                    case "Bull_Eye":
                        BulleyeThrows += 1;
                        if (T.WhoThrow == "Player1")
                        {
                            Player1BulleyeThrows += 1;
                        }
                        else
                        {
                            Player2BulleyeThrows += 1;
                        }

                        break;
                }
            }

            AveragePlayer1Points = Player1Throws == 0
                                       ? 0
                                       : Math.Round((double) Player1Points / Player1Throws, 2); //  Средние очки игрока 1
            AveragePlayer2Points = Player2Throws == 0
                                       ? 0
                                       : Math.Round((double) Player2Points / Player2Throws, 2); //  Средние очки игрока 2
            //  Передаем данные в окно статистики
            //  Winner label
            Canvas.SetLeft(statisticWindow.WinnerLabel,
                           winner.Tag == "Player1"
                               ? 541
                               : 608);

            //  Name box
            statisticWindow.Player1Name.Content = player1.Name;
            statisticWindow.Player2Name.Content = player2.Name;
            //  Throws box
            statisticWindow.NumberOfThrows.Content = Throws;
            statisticWindow.Player1Throws.Content = Player1Throws;
            statisticWindow.Player2Throws.Content = Player2Throws;
            statisticWindow.PlayersThrows.Fill = Brush(Player1Throws, Throws);
            //  Points box
            statisticWindow.NumberOfPoints.Content = Points;
            statisticWindow.Player1Points.Content = Player1Points;
            statisticWindow.Player2Points.Content = Player2Points;
            statisticWindow.PlayersPoints.Fill = Brush(Player1Points, Points);
            //  Avarage points box
            AveragePoints = Math.Round((double) Points / Throws, 2);
            statisticWindow.AveragePoints.Content = AveragePoints;
            statisticWindow.Player1AveragePoints.Content = AveragePlayer1Points;
            statisticWindow.Player2AveragePoints.Content = AveragePlayer2Points;
            statisticWindow.PlayersAveragePoints.Fill = Brush(AveragePlayer1Points, AveragePlayer1Points + AveragePlayer2Points);
            //  Sets played box
            statisticWindow.SetsPlayed.Content = SetsPlayed;
            statisticWindow.Player1SetsWon.Content = Player1SetsWon;
            statisticWindow.Player2SetsWon.Content = Player2SetsWon;
            statisticWindow.PlayersSetsPlayed.Fill = Brush(Player1SetsWon, SetsPlayed);
            //  Legs played box
            statisticWindow.LegsPlayed.Content = LegsPlayed;
            statisticWindow.Player1LegsWon.Content = Player1LegsWon;
            statisticWindow.Player2LegsWon.Content = Player2LegsWon;
            statisticWindow.PlayersLegsPlayed.Fill = Brush(Player1LegsWon, LegsPlayed);
            //  180 box
            Player1_180 = player1._180;
            Player2_180 = player2._180;
            _180 = Player1_180 + Player2_180;
            statisticWindow._180.Content = _180;
            statisticWindow.Player1_180.Content = Player1_180;
            statisticWindow.Player2_180.Content = Player2_180;
            statisticWindow.Players_180.Fill = _180 == 0
                                                   ? Brush(0, 0)
                                                   : Brush(Player1_180, _180);

            //  Tremble throws box
            statisticWindow.TrembleThrows.Content = TrembleThrows;
            statisticWindow.Player1TrembleThrow.Content = Player1TrembleThrows;
            statisticWindow.Player2TrembleThrow.Content = Player2TrembleThrows;
            statisticWindow.PlayersTrembleThrows.Fill = TrembleThrows == 0
                                                            ? Brush(0, 0)
                                                            : Brush(Player1TrembleThrows, TrembleThrows);

            //  Bulleye throws box
            statisticWindow.BulleyeThrows.Content = BulleyeThrows;
            statisticWindow.Player1BulleyeThrow.Content = Player1BulleyeThrows;
            statisticWindow.Player2BulleyeThrow.Content = Player2BulleyeThrows;
            statisticWindow.PlayersBulleyeThrows.Fill = BulleyeThrows == 0
                                                            ? Brush(0, 0)
                                                            : Brush(Player1BulleyeThrows, BulleyeThrows);

            //  Double throws box
            statisticWindow.DoubleThrows.Content = DoubleThrows;
            statisticWindow.Player1DoubleThrow.Content = Player1DoubleThrows;
            statisticWindow.Player2DoubleThrow.Content = Player2DoubleThrows;
            statisticWindow.PlayersDoubleThrows.Fill = DoubleThrows == 0
                                                           ? Brush(0, 0)
                                                           : Brush(Player1DoubleThrows, DoubleThrows);

            //  25 throws box
            statisticWindow._25Throws.Content = _25Throws;
            statisticWindow.Player1_25Throw.Content = Player1_25Throws;
            statisticWindow.Player2_25Throw.Content = Player2_25Throws;
            statisticWindow.Players_25Throws.Fill = _25Throws == 0
                                                        ? Brush(0, 0)
                                                        : Brush(Player1_25Throws, _25Throws);

            //  Single throws box
            statisticWindow.SingleThrows.Content = SingleThrows;
            statisticWindow.Player1SingleThrow.Content = Player1SingleThrows;
            statisticWindow.Player2SingleThrow.Content = Player2SingleThrows;
            statisticWindow.PlayersSingleThrows.Fill = SingleThrows == 0
                                                           ? Brush(0, 0)
                                                           : Brush(Player1SingleThrows, SingleThrows);

            //  Zero throws box
            statisticWindow.ZeroThrows.Content = ZeroThrows;
            statisticWindow.Player1ZeroThrow.Content = Player1ZeroThrows;
            statisticWindow.Player2ZeroThrow.Content = Player2ZeroThrows;
            statisticWindow.PlayersZeroThrows.Fill = ZeroThrows == 0
                                                         ? Brush(0, 0)
                                                         : Brush(ZeroThrows - Player1ZeroThrows, ZeroThrows);

            //  Fault throws box
            statisticWindow.FaultThrows.Content = FaultThrows;
            statisticWindow.Player1FaultThrows.Content = Player1FaultThrows;
            statisticWindow.Player2FaultThrows.Content = Player2FaultThrows;
            statisticWindow.PlayersFaultThrows.Fill = FaultThrows == 0
                                                          ? Brush(0, 0)
                                                          : Brush(FaultThrows - Player1FaultThrows, FaultThrows);
        }

        public void ShowMatchStatistics() //  Показываем окно статистики 
        {
            statisticWindow.Owner = mainWindow; //  Прописываем владельца
            statisticWindow.ShowDialog();
        }

        private Brush Brush(double d, double D) //  Кисть
        {
            double point;
            if (D == 0 && d == 0)
            {
                point = 0.5;
            }
            else
            {
                point = d / D;
            }

            point = Math.Round(point, 3);
            var brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FF4B0131"), 0));
            brush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FFC300CD"), point - 0.005));
            brush.GradientStops.Add(new GradientStop(Colors.Black, point));
            brush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FF0095EA"), point + 0.005));
            brush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FF023042"), 1));
            brush.RelativeTransform = new RotateTransform(-0.5);
            return brush;
        }

        public void ClearCollection() //  Очистка коллекции
        {
            Throws = 0;
            Player1Throws = 0;
            Player2Throws = 0;
            Points = 0;
            Player1Points = 0;
            Player2Points = 0;
            AveragePoints = 0;
            AveragePlayer1Points = 0;
            AveragePlayer2Points = 0;
            LegsPlayed = 0;
            Player1LegsWon = 0;
            Player2LegsWon = 0;
            SetsPlayed = 0;
            Player1SetsWon = 0;
            Player2SetsWon = 0;
            FaultThrows = 0;
            Player1FaultThrows = 0;
            Player2FaultThrows = 0;
            _180 = 0;
            Player1_180 = 0;
            Player2_180 = 0;
            SingleThrows = 0;
            Player1SingleThrows = 0;
            Player2SingleThrows = 0;
            DoubleThrows = 0;
            Player1DoubleThrows = 0;
            Player2DoubleThrows = 0;
            TrembleThrows = 0;
            Player1TrembleThrows = 0;
            Player2TrembleThrows = 0;
            _25Throws = 0;
            Player1_25Throws = 0;
            Player2_25Throws = 0;
            BulleyeThrows = 0;
            Player1BulleyeThrows = 0;
            Player2BulleyeThrows = 0;
            ZeroThrows = 0;
            Player1ZeroThrows = 0;
            Player2ZeroThrows = 0;
            Player1Name = "";
            Player2Name = "";
            WinnerName = "";
            LooserName = "";
            Player1Id = 0;
            Player2Id = 0;
            WinnerId = 0;
            LooserId = 0;
            Player1IsmrZ = false;
            Player1Is3Bull = false;
            Player2IsmrZ = false;
            Player2Is3Bull = false;
        }
    }
}