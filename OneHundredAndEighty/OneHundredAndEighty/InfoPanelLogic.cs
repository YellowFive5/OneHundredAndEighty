using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace OneHundredAndEighty
{
    public class InfoPanelLogic //  Класс логики инфо-панели
    {
        MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам
        SortedList<int, string> CheckoutTable = new SortedList<int, string>()
        {
            [2] = "D1",
            [3] = "1 D1",
            [4] = "D2",
            [5] = "1 D2",
            [6] = "D3",
            [7] = "3 D2",
            [8] = "D4",
            [9] = "1 D4",
            [10] = "D5",
            [11] = "3 D4",
            [12] = "D6",
            [13] = "5 D4",
            [14] = "D7",
            [15] = "7 D4",
            [16] = "D8",
            [17] = "9 D4",
            [18] = "D9",
            [19] = "3 D8",
            [20] = "D10",
            [21] = "5 D8",
            [22] = "D11",
            [23] = "7 D8",
            [24] = "D12",
            [25] = "1 D12",
            [26] = "D13",
            [27] = "3 D12",
            [28] = "D14",
            [29] = "5 D12",
            [30] = "D15",
            [31] = "7 D12",
            [32] = "D16",
            [33] = "1 D16",
            [34] = "D17",
            [35] = "3 D16",
            [36] = "D18",
            [37] = "5 D16",
            [38] = "D19",
            [39] = "7 D6",
            [40] = "D20",
            [41] = "9 D16",
            [42] = "10 D16",
            [43] = "3 D20",
            [44] = "4 D20",
            [45] = "13 D16",
            [46] = "6 D20",
            [47] = "7 D20",
            [48] = "16 D16",
            [49] = "17 D16",
            [50] = "18 D16",
            [51] = "19 D16",
            [52] = "20 D16",
            [53] = "13 D20",
            [54] = "14 D20",
            [55] = "15 D20",
            [56] = "16 D20",
            [57] = "17 D20",
            [58] = "18 D20",
            [59] = "19 D20",
            [60] = "20 D20",
            [61] = "T15 D8",
            [62] = "T10 D16",
            [63] = "T13 D12",
            [64] = "T16 D8",
            [65] = "T19 D4",
            [66] = "T14 D12",
            [67] = "T17 D8",
            [68] = "T20 D4",
            [69] = "T19 D6",
            [70] = "T18 D8",
            [71] = "T13 16",
            [72] = "T16 D12",
            [73] = "T19 D8",
            [74] = "T14 D16",
            [75] = "T17 D12",
            [76] = "T20 D8",
            [77] = "T19 D10",
            [78] = "T18 D12",
            [79] = "T19 D11",
            [80] = "T20 D10",
            [81] = "T19 D12",
            [82] = "Bull D16",
            [83] = "T17 D16",
            [84] = "T20 D12",
            [85] = "T15 D20",
            [86] = "T18 D18",
            [87] = "T17 D18",
            [88] = "T20 D14",
            [89] = "T19 D16",
            [90] = "T20 D15",
            [91] = "T17 D20",
            [92] = "T20 D16",
            [93] = "T19 D18",
            [94] = "T18 D20",
            [95] = "T19 D19",
            [96] = "T20 D18",
            [97] = "T19 D20",
            [98] = "T20 D19",
            [99] = "T19 10 D16",
            [100] = "T20 D20",
            [101] = "T20 9 D16",
            [102] = "T16 14 D20",
            [103] = "T19 6 D20",
            [104] = "T16 16 D20",
            [105] = "T20 13 D16",
            [106] = "T20 6 D20",
            [107] = "T19 10 D20",
            [108] = "T20 16 D16",
            [109] = "T20 17 D16",
            [110] = "T20 10 D20",
            [111] = "T19 14 D20",
            [112] = "T20 20 D16",
            [113] = "T19 16 D20",
            [114] = "T20 14 D20",
            [115] = "T20 15 D20",
            [116] = "T20 16 D20",
            [117] = "T20 17 D20",
            [118] = "T20 18 D20",
            [119] = "T19 12 Bull",
            [120] = "T20 20 D20",
            [121] = "T20 11 Bull",
            [122] = "T18 18 Bull",
            [123] = "T19 16 Bull",
            [124] = "T20 14 Bull",
            [125] = "25 T20 D20",
            [126] = "T19 19 Bull",
            [127] = "T20 17 Bull",
            [128] = "18 T20 Bull",
            [129] = "19 T20 Bull",
            [130] = "T20 20 Bull",
            [131] = "T20 T13 D16",
            [132] = "25 T19 Bull",
            [133] = "T20 T19 D8",
            [134] = "T20 T14 D16",
            [135] = "25 T20 Bull",
            [136] = "T20 T20 D8",
            [137] = "T20 T19 D10",
            [138] = "T20 T18 D12",
            [139] = "T19 T14 D20",
            [140] = "T20 T20 D10",
            [141] = "T20 T19 D12",
            [142] = "T20 T14 D20",
            [143] = "T20 T17 D16",
            [144] = "T20 T20 D12",
            [145] = "T20 T15 D20",
            [146] = "T20 T18 D16",
            [147] = "T20 T17 D18",
            [148] = "T20 T20 D14",
            [149] = "T20 T19 D16",
            [150] = "T20 T18 D18",
            [151] = "T20 T17 D20",
            [152] = "T20 T20 D16",
            [153] = "T20 T19 D18",
            [154] = "T20 T18 D20",
            [155] = "T20 T19 D19",
            [156] = "T20 T20 D18",
            [157] = "T20 T19 D20",
            [158] = "T20 T20 D19",
            [160] = "T20 T20 D20",
            [161] = "T20 T17 Bull",
            [164] = "T20 T18 Bull",
            [167] = "T20 T19 Bull",
            [170] = "T20 T20 Bull",
        };  //  Коллекция закрытия сета
        TimeSpan Time = TimeSpan.FromSeconds(0.23);  //  Hide/show animation duration

        public void PanelShow()
        {
            MainWindow.InfoPanel.Visibility = System.Windows.Visibility.Visible;
        }   //  Спрятать инфо-панель
        public void PanelHide()
        {
            MainWindow.InfoPanel.Visibility = System.Windows.Visibility.Hidden;
        }   //  Показать инфо-панель
        public void PanelNewGame(int points, string legs, string sets, Player p1, Player p2, Player first)
        {

            MainWindow.MainBoxSummary.Content = (new StringBuilder().Append("First to ").Append(legs).Append(" legs in ").Append(sets).Append(" sets")).ToString();
            MainWindow.Player1Name.Content = p1.Name;
            MainWindow.Player2Name.Content = p2.Name;
            p1.SetsWonLabel.Content = 0;
            p2.SetsWonLabel.Content = 0;
            p1.LegsWonLabel.Content = 0;
            p2.LegsWonLabel.Content = 0;
            PointsClear(points);
            DotSet(first);
            WhoThrowSliderSet(first);
        }   //  Установка в 0 в начале игры
        public void DotSet(Player p)
        {
            p.DotPoint.Opacity = 1;

            if (p.Tag == "Player1")
                MainWindow.Player2SetDot.Opacity = 0;
            if (p.Tag == "Player2")
                MainWindow.Player1SetDot.Opacity = 0;
        }   //  Установка точки начала сета
        public void DotToggle()
        {
            Storyboard SB = new Storyboard();
            DoubleAnimation fadein = new DoubleAnimation(0, 1, Time);
            DoubleAnimation fadeout = new DoubleAnimation(1, 0, Time);
            SB.Children.Add(fadein);
            SB.Children.Add(fadeout);

            if (MainWindow.Player1SetDot.Opacity != 0)
            {
                Storyboard.SetTarget(fadeout, MainWindow.Player1SetDot);
                Storyboard.SetTargetProperty(fadeout, new System.Windows.PropertyPath(UIElement.OpacityProperty));
                Storyboard.SetTarget(fadein, MainWindow.Player2SetDot);
                Storyboard.SetTargetProperty(fadein, new System.Windows.PropertyPath(UIElement.OpacityProperty));
            }
            else
            {
                Storyboard.SetTarget(fadeout, MainWindow.Player2SetDot);
                Storyboard.SetTargetProperty(fadeout, new System.Windows.PropertyPath(UIElement.OpacityProperty));
                Storyboard.SetTarget(fadein, MainWindow.Player1SetDot);
                Storyboard.SetTargetProperty(fadein, new System.Windows.PropertyPath(UIElement.OpacityProperty));
            }
            SB.Begin();
        }  //  Переключатель точки начала сета
        public void WhoThrowSliderSet(Player p)
        {
            if (p.Tag == "Player1")
            {
                Canvas.SetTop(MainWindow.WhoThrowSlider, 21);
                MainWindow.Player1HelpBackground.Opacity = 1;
                MainWindow.Player2HelpBackground.Opacity = 0;
            }
            if (p.Tag == "Player2")
            {
                Canvas.SetTop(MainWindow.WhoThrowSlider, 52);
                MainWindow.Player1HelpBackground.Opacity = 0;
                MainWindow.Player2HelpBackground.Opacity = 1;
            }

        }   //  Установка слайдера текущего броска
        public void WhoThrowSliderToggle()
        {
            Storyboard Slider = new Storyboard();

            DoubleAnimation fadein = new DoubleAnimation(0, 1, Time);
            DoubleAnimation fadeout = new DoubleAnimation(1, 0, Time);
            DoubleAnimation hide = new DoubleAnimation() { From = 316, To = 292, Duration = Time };

            Storyboard.SetTarget(hide, MainWindow.WhoThrowSlider);
            Storyboard.SetTargetProperty(hide, new System.Windows.PropertyPath(Canvas.LeftProperty));
            Slider.Children.Add(hide);

            if (Canvas.GetTop(MainWindow.WhoThrowSlider) == 52)
            {
                DoubleAnimation toggle = new DoubleAnimation() { From = 52, To = 21, Duration = TimeSpan.FromSeconds(0), BeginTime = Time };
                Storyboard.SetTarget(toggle, MainWindow.WhoThrowSlider);
                Storyboard.SetTargetProperty(toggle, new System.Windows.PropertyPath(Canvas.TopProperty));
                Slider.Children.Add(toggle);

                Storyboard.SetTarget(fadeout, MainWindow.Player2HelpBackground);
                Storyboard.SetTargetProperty(fadeout, new System.Windows.PropertyPath(UIElement.OpacityProperty));
                Storyboard.SetTarget(fadein, MainWindow.Player1HelpBackground);
                Storyboard.SetTargetProperty(fadein, new System.Windows.PropertyPath(UIElement.OpacityProperty));
            }
            else
            {
                DoubleAnimation toggle = new DoubleAnimation() { From = 21, To = 52, Duration = TimeSpan.FromSeconds(0), BeginTime = Time };
                Storyboard.SetTarget(toggle, MainWindow.WhoThrowSlider);
                Storyboard.SetTargetProperty(toggle, new System.Windows.PropertyPath(Canvas.TopProperty));
                Slider.Children.Add(toggle);

                Storyboard.SetTarget(fadeout, MainWindow.Player1HelpBackground);
                Storyboard.SetTargetProperty(fadeout, new System.Windows.PropertyPath(UIElement.OpacityProperty));
                Storyboard.SetTarget(fadein, MainWindow.Player2HelpBackground);
                Storyboard.SetTargetProperty(fadein, new System.Windows.PropertyPath(UIElement.OpacityProperty));
            }
            Slider.Children.Add(fadein);
            Slider.Children.Add(fadeout);

            DoubleAnimation show = new DoubleAnimation() { From = 292, To = 316, Duration = Time, BeginTime = Time };
            Storyboard.SetTarget(show, MainWindow.WhoThrowSlider);
            Storyboard.SetTargetProperty(show, new System.Windows.PropertyPath(Canvas.LeftProperty));
            Slider.Children.Add(show);

            Slider.Begin();
        }   // Переключатель слайдера текущего броска
        public void HelpCheck(Player p)
        {
            if (CheckoutTable.ContainsKey(p.PointsToOut) == true)
            {
                p.HelpLabel.Content = CheckoutTable[p.PointsToOut];
                HelpShow(p);
            }
            else
            {
                p.HelpLabel.Content = "";
                HelpHide(p);
            }
        }  //  Установка помощи очков на закрытие сета

        public void HelpShow(Player p)
        {
            if (p.HelpPanel.Tag.ToString() == "OFF")
            {
                DoubleAnimation animation = new DoubleAnimation(77, -1, Time);
                p.HelpPanel.BeginAnimation(Canvas.LeftProperty, animation);
                p.HelpPanel.Tag = "ON";
            }

        }  //  Показ бокса помощи
        public void HelpHide(Player p)
        {
            if (p.HelpPanel.Tag.ToString() == "ON")
            {
                DoubleAnimation animation = new DoubleAnimation(-1, 77, Time);
                p.HelpPanel.BeginAnimation(Canvas.LeftProperty, animation);
                p.HelpPanel.Tag = "OFF";
            }
        }  //  Скрытие бокса помощи
        public void PointsSet(Player p)
        {
            p.PointsLabel.Content = p.PointsToOut;
        }  //  Установка текущих очков
        public void PointsClear(int p)
        {
            MainWindow.Player1Points.Content = p;
            MainWindow.Player2Points.Content = p;
        }    //  Установка очков в начале сета
        public void SetsClear()
        {
            MainWindow.Player1SetsWon.Content = 0;
            MainWindow.Player2SetsWon.Content = 0;
        }
        public void SetIncrement(Player p)
        {
            p.SetsWonLabel.Content = Int32.Parse((p.SetsWonLabel.Content).ToString()) + 1;
        }  //  +1 к сету
        public void LegIncrement(Player p)
        {
            p.LegsWonLabel.Content = Int32.Parse((p.LegsWonLabel.Content).ToString()) + 1;
        }  //  +1 к легу
    }
}
