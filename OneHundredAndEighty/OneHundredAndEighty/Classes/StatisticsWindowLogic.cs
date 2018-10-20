using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace OneHundredAndEighty
{

    public class StatisticsWindowLogic  //  Статистика матча
    {
        MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Cсылка на главное окно
        Windows.StatisticWindow StatisticWindow = null;
        public int Throws { get; private set; } //  Всего бросков
        public int Player1Throws { get; private set; }  //  Бросков у игрока 1
        public int Player2Throws { get; private set; }  //  Бросков у игрока 2
        public int Points { get; private set; } //  Всего очков
        public int Player1Points { get; private set; }  //  Очков у игрока 1
        public int Player2Points { get; private set; }  //  Очков у игрока 2
        public double AveragePoints { get; private set; }   //  Средние очки за бросок
        public double AveragePlayer1Points { get; private set; }    //  Средние очки за бросок игрока 1
        public double AveragePlayer2Points { get; private set; }    //  Средние очки за бросок игрока 2
        public int SetsPlayed { get; private set; } //  Сыграно сетов
        public int Player1SetsWon { get; private set; } //  Выиграно сетов игроком 1
        public int Player2SetsWon { get; private set; } //  Выиграно сетов игроком 2
        public int LegsPlayed { get; private set; } //  Сыграно легов
        public int Player1LegsWon { get; private set; } //  Выиграно легов игроком 1
        public int Player2LegsWon { get; private set; } //  Выиграно легов игроком 2
        public int FaultThrows { get; private set; }    //  Штрафных бросков
        public int Player1FaultThrows { get; private set; } //  Штрафных бросков игрока 1
        public int Player2FaultThrows { get; private set; } //  Штрафных бросков игрока 2
        public int _180 { get; private set; }   //  Количество набора 180 очков
        public int Player1_180 { get; private set; }    //  180 у игрока 1
        public int Player2_180 { get; private set; }    //  180 у игрока 2
        public int SingleThrows { get; private set; }   //  Бросков в одинарный сектор
        public int Player1SingleThrows { get; private set; }    //  Бросков в одинарный сектор игрока 1
        public int Player2SingleThrows { get; private set; }    //  Бросков в одинарный сектор игрока 2
        public int DoubleThrows { get; private set; }   //  Бросков в удвоение
        public int Player1DoubleThrows { get; private set; }    //  Бросков в удвоение игрока 1
        public int Player2DoubleThrows { get; private set; }    //  Бросков в удвоение игрока 2
        public int TrembleThrows { get; private set; }  //  Бросков в утроение
        public int Player1TrembleThrows { get; private set; }   //  Бросков в утроение игрока 1
        public int Player2TrembleThrows { get; private set; }   //  Бросков в утроение игрока 2
        public int _25Throws { get; private set; }  //  Бросков в 25
        public int Player1_25Throws { get; private set; }   //  Бросков в 25 игрока 1
        public int Player2_25Throws { get; private set; }   //  Бросков в 25 игрока 2
        public int BulleyeThrows { get; private set; }  //  Бросков в булл
        public int Player1BulleyeThrows { get; private set; }   //  Бросков в булл игрока 1
        public int Player2BulleyeThrows { get; private set; }   //  Бросков в булл игрока 2
        public int ZeroThrows { get; private set; } //  Бросков в 0
        public int Player1ZeroThrows { get; private set; }  //  Бросков в 0 игрока 1
        public int Player2ZeroThrows { get; private set; }  //  Бросков в 0 игрока 2

        public void ShowMatchStatistics(Player winner, Player p1, Player p2, Stack<Throw> AllMatchThrows)   //  Показать статистику матча
        {
            StatisticWindow = new Windows.StatisticWindow();    //  Ссылка на окно статистики матча
            StatisticWindow.Owner = MainWindow;    //  Прописываем владельца
            while (AllMatchThrows.Count != 0)   //  Разбираем бросок на запчасти
            {
                Throw T = AllMatchThrows.Pop();
                Throws += 1;
                Points += (int)T.Points;
                if (T.WhoThrow == "Player1")
                {
                    Player1Throws += 1;
                    Player1Points += (int)T.Points;
                }
                else
                {
                    Player2Throws += 1;
                    Player2Points += (int)T.Points;
                }
                if (T.IsFault)
                {
                    FaultThrows += 1;
                    if (T.WhoThrow == "Player1")
                        Player1FaultThrows += 1;
                    else
                        Player2FaultThrows += 1;
                }
                if (T.IsLegWon)
                {
                    LegsPlayed += 1;
                    if (T.WhoThrow == "Player1")
                        Player1LegsWon += 1;
                    else
                        Player2LegsWon += 1;
                }
                if (T.IsSetWon)
                {
                    SetsPlayed += 1;
                    if (T.WhoThrow == "Player1")
                        Player1SetsWon += 1;
                    else
                        Player2SetsWon += 1;
                }
                switch (T.Multiplier)
                {
                    case "Zero":
                        ZeroThrows += 1;
                        if (T.WhoThrow == "Player1")
                            Player1ZeroThrows += 1;
                        else
                            Player2ZeroThrows += 1;
                        break;
                    case "Single":
                        SingleThrows += 1;
                        if (T.WhoThrow == "Player1")
                            Player1SingleThrows += 1;
                        else
                            Player2SingleThrows += 1;
                        break;
                    case "Double":
                        DoubleThrows += 1;
                        if (T.WhoThrow == "Player1")
                            Player1DoubleThrows += 1;
                        else
                            Player2DoubleThrows += 1;
                        break;
                    case "Tremble":
                        TrembleThrows += 1;
                        if (T.WhoThrow == "Player1")
                            Player1TrembleThrows += 1;
                        else
                            Player2TrembleThrows += 1;
                        break;
                    case "Bull_25":
                        _25Throws += 1;
                        if (T.WhoThrow == "Player1")
                            Player1_25Throws += 1;
                        else
                            Player2_25Throws += 1;
                        break;
                    case "Bull_Eye":
                        BulleyeThrows += 1;
                        if (T.WhoThrow == "Player1")
                            Player1BulleyeThrows += 1;
                        else
                            Player2BulleyeThrows += 1;
                        break;
                    default:
                        break;
                }
            }
            AveragePlayer1Points = (Player1Throws == 0) ? 0 : Math.Round((double)Player1Points / Player1Throws, 2); //  Средние очки игрока 1
            AveragePlayer2Points = (Player2Throws == 0) ? 0 : Math.Round((double)Player2Points / Player2Throws, 2); //  Средние очки игрока 2
            //  Передаем данные в окно статистики
            //  Winner label
            if (winner.Tag == "Player1")
                Canvas.SetLeft(StatisticWindow.WinnerLabel, 541);
            else
                Canvas.SetLeft(StatisticWindow.WinnerLabel, 608);
            //  Name box
            StatisticWindow.Player1Name.Content = p1.Name;
            StatisticWindow.Player2Name.Content = p2.Name;
            //  Throws box
            StatisticWindow.NumberOfThrows.Content = Throws;
            StatisticWindow.Player1Throws.Content = Player1Throws;
            StatisticWindow.Player2Throws.Content = Player2Throws;
            StatisticWindow.PlayersThrows.Fill = Brush(Player1Throws, Throws); ;
            //  Points box
            StatisticWindow.NumberOfPoints.Content = Points;
            StatisticWindow.Player1Points.Content = Player1Points;
            StatisticWindow.Player2Points.Content = Player2Points;
            StatisticWindow.PlayersPoints.Fill = Brush(Player1Points, Points); ;
            //  Avarage points box
            AveragePoints = Math.Round((double)Points / Throws, 2);
            StatisticWindow.AvaragePoints.Content = AveragePoints;
            StatisticWindow.Player1AvaragePoints.Content = AveragePlayer1Points;
            StatisticWindow.Player2AvaragePoints.Content = AveragePlayer2Points;
            StatisticWindow.PlayersAvaragePoints.Fill = Brush(AveragePlayer1Points, AveragePlayer1Points + AveragePlayer2Points);
            //  Sets played box
            StatisticWindow.SetsPlayed.Content = SetsPlayed;
            StatisticWindow.Player1SetsWon.Content = Player1SetsWon;
            StatisticWindow.Player2SetsWon.Content = Player2SetsWon;
            StatisticWindow.PlayersSetsPlayed.Fill = Brush(Player1SetsWon, SetsPlayed);
            //  Legs played box
            StatisticWindow.LegsPlayed.Content = LegsPlayed;
            StatisticWindow.Player1LegsWon.Content = Player1LegsWon;
            StatisticWindow.Player2LegsWon.Content = Player2LegsWon;
            StatisticWindow.PlayersLegsPlayed.Fill = Brush(Player1LegsWon, LegsPlayed);
            //  180 box
            Player1_180 = p1._180;
            Player2_180 = p2._180;
            _180 = Player1_180 + Player2_180;
            StatisticWindow._180.Content = _180;
            StatisticWindow.Player1_180.Content = Player1_180;
            StatisticWindow.Player2_180.Content = Player2_180;
            if (_180 == 0)
                StatisticWindow.Players_180.Fill = Brush(0, 0);
            else
                StatisticWindow.Players_180.Fill = Brush(Player1_180, _180);
            //  Tremble throws box
            StatisticWindow.TrembleThrows.Content = TrembleThrows;
            StatisticWindow.Player1TrembleThrow.Content = Player1TrembleThrows;
            StatisticWindow.Player2TrembleThrow.Content = Player2TrembleThrows;
            if (TrembleThrows == 0)
                StatisticWindow.PlayersTrembleThrows.Fill = Brush(0, 0);
            else
                StatisticWindow.PlayersTrembleThrows.Fill = Brush(Player1TrembleThrows, TrembleThrows);
            //  Bulleye throws box
            StatisticWindow.BulleyeThrows.Content = BulleyeThrows;
            StatisticWindow.Player1BulleyeThrow.Content = Player1BulleyeThrows;
            StatisticWindow.Player2BulleyeThrow.Content = Player2BulleyeThrows;
            if (BulleyeThrows == 0)
                StatisticWindow.PlayersBulleyeThrows.Fill = Brush(0, 0);
            else
                StatisticWindow.PlayersBulleyeThrows.Fill = Brush(Player1BulleyeThrows, BulleyeThrows);
            //  Double throws box
            StatisticWindow.DoubleThrows.Content = DoubleThrows;
            StatisticWindow.Player1DoubleThrow.Content = Player1DoubleThrows;
            StatisticWindow.Player2DoubleThrow.Content = Player2DoubleThrows;
            if (DoubleThrows == 0)
                StatisticWindow.PlayersDoubleThrows.Fill = Brush(0, 0);
            else
                StatisticWindow.PlayersDoubleThrows.Fill = Brush(Player1DoubleThrows, DoubleThrows);
            //  25 throws box
            StatisticWindow._25Throws.Content = _25Throws;
            StatisticWindow.Player1_25Throw.Content = Player1_25Throws;
            StatisticWindow.Player2_25Throw.Content = Player2_25Throws;
            if (_25Throws == 0)
                StatisticWindow.Players_25Throws.Fill = Brush(0, 0);
            else
                StatisticWindow.Players_25Throws.Fill = Brush(Player1_25Throws, _25Throws);
            //  Single throws box
            StatisticWindow.SingleThrows.Content = SingleThrows;
            StatisticWindow.Player1SingleThrow.Content = Player1SingleThrows;
            StatisticWindow.Player2SingleThrow.Content = Player2SingleThrows;
            if (SingleThrows == 0)
                StatisticWindow.PlayersSingleThrows.Fill = Brush(0, 0);
            else
                StatisticWindow.PlayersSingleThrows.Fill = Brush(Player1SingleThrows, SingleThrows);
            //  Zero throws box
            StatisticWindow.ZeroThrows.Content = ZeroThrows;
            StatisticWindow.Player1ZeroThrow.Content = Player1ZeroThrows;
            StatisticWindow.Player2ZeroThrow.Content = Player2ZeroThrows;
            if (ZeroThrows == 0)
                StatisticWindow.PlayersZeroThrows.Fill = Brush(0, 0);
            else
                StatisticWindow.PlayersZeroThrows.Fill = Brush(ZeroThrows - Player1ZeroThrows, ZeroThrows);
            //  Fault throws box
            StatisticWindow.FaultThrows.Content = FaultThrows;
            StatisticWindow.Player1FaultThrows.Content = Player1FaultThrows;
            StatisticWindow.Player2FaultThrows.Content = Player2FaultThrows;
            if (FaultThrows == 0)
                StatisticWindow.PlayersFaultThrows.Fill = Brush(0, 0);
            else
                StatisticWindow.PlayersFaultThrows.Fill = Brush(FaultThrows - Player1FaultThrows, FaultThrows);
            //
            ClearColection();
            StatisticWindow.ShowDialog();   //  Показываем окно статистики
        }
        Brush Brush(double d, double D)   //  Кисть
        {
            double point;
            if (D == 0 && d == 0)
                point = 0.5;
            else
                point = d / D;
            point = Math.Round(point, 3);
            LinearGradientBrush B = new LinearGradientBrush();
            B.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF4B0131"), 0));
            B.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFC300CD"), point - 0.005));
            B.GradientStops.Add(new GradientStop(Colors.Black, point));
            B.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF0095EA"), point + 0.005));
            B.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF023042"), 1));
            B.RelativeTransform = new RotateTransform(-0.5);
            return B;
        }
        void ClearColection()   //  Очистка коллекции
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
        }
    }
}
