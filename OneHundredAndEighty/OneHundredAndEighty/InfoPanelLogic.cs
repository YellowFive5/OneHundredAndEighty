using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OneHundredAndEighty
{
    public class InfoPanelLogic //  Класс логики инфо-панели
    {
        MainWindow MV = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам
        SortedList<int, string> CheckoutTable = new SortedList<int, string>()
        {
            [41] = "9 D16",
            [42] = "",
            [43] = "",
            [44] = "",
            [45] = "",
            [46] = "",
            [47] = "",
            [48] = "",
            [49] = "",
            [50] = "",
            [51] = "",
            [52] = "",
            [53] = "",
            [54] = "",
            [55] = "",
            [56] = "",
            [57] = "",
            [58] = "",
            [59] = "",
            [60] = "",
            [61] = "",
            [62] = "",
            [63] = "",
            [64] = "",
            [65] = "",
            [66] = "",
            [67] = "",
            [68] = "",
            [69] = "",
            [70] = "",
            [71] = "",
            [72] = "",
            [73] = "",
            [74] = "",
            [75] = "",
            [76] = "",
            [77] = "",
            [78] = "",
            [79] = "",
            [80] = "",
            [81] = "",
            [82] = "",
            [83] = "",
            [84] = "",
            [85] = "",
            [86] = "",
            [87] = "",
            [88] = "",
            [89] = "",
            [90] = "",
            [91] = "",
            [92] = "",
            [93] = "",
            [94] = "",
            [95] = "",
            [96] = "",
            [97] = "",
            [98] = "",
            [99] = "",
            [100] = "",
            [101] = "",
            [102] = "",
            [103] = "",
            [104] = "",
            [105] = "",
            [106] = "",
            [107] = "",
            [108] = "",
            [109] = "",
            [110] = "",
            [111] = "",
            [112] = "",
            [113] = "",
            [114] = "",
            [115] = "",
            [116] = "",
            [117] = "",
            [118] = "",
            [119] = "",
            [120] = "",
            [121] = "",
            [122] = "",
            [123] = "",
            [124] = "",
            [125] = "",
            [126] = "",
            [127] = "",
            [128] = "",
            [129] = "",
            [130] = "",
            [131] = "",
            [132] = "",
            [133] = "",
            [134] = "",
            [135] = "",
            [136] = "",
            [137] = "",
            [138] = "",
            [139] = "",
            [140] = "",
            [141] = "",
            [142] = "",
            [143] = "",
            [144] = "",
            [145] = "",
            [146] = "",
            [147] = "",
            [148] = "",
            [149] = "",
            [150] = "",
            [151] = "",
            [152] = "",
            [153] = "",
            [154] = "",
            [155] = "",
            [156] = "",
            [157] = "",
            [158] = "",
            [160] = "",
            [161] = "",
            [164] = "",
            [167] = "",
            [170] = "",
        };  //  Коллекция закрытия сета
        public void ToggleWhoThrowSlider()   // Переключатель слайдера текущего броска
        {
            if (Canvas.GetTop(MV.WhoThrowSlider) == 52)
                Canvas.SetTop(MV.WhoThrowSlider, 21);
            else
                Canvas.SetTop(MV.WhoThrowSlider, 52);
        }
        public void ToggleSetDot()  //  Переключатель точки начала сета
        {
            if (MV.Player1SetDot.IsVisible)
            {
                MV.Player1SetDot.Visibility = System.Windows.Visibility.Hidden;
                MV.Player2SetDot.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                MV.Player1SetDot.Visibility = System.Windows.Visibility.Visible;
                MV.Player2SetDot.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        public void SetHelp(int p)  //  Установка помощи очков на закрытие сета
        {
            if (CheckoutTable.ContainsKey(p) == true)
                MV.Player1PointsHelp.Content = CheckoutTable[p];
            else
            {
                MV.Player1PointsHelp.Content = "";
                HideHelp("1");
            }

        }
        public void ShowHelp(string s)  //  Показ бокса помощи
        {
            if (s == "1")
                Canvas.SetLeft(MV.Player1Help, -1);
            else
                Canvas.SetLeft(MV.Player2Help, -1);
        }
        public void HideHelp(string s)  //  Скрытие бокса помощи
        {
            if (s == "1")
                Canvas.SetLeft(MV.Player1Help, 77);
            else
                Canvas.SetLeft(MV.Player2Help, 77);
        }
        public void SetZero()   //  Установка в 0 в начале игры
        {
            MV.Player1SetsWon.Content = 0;
            MV.Player2SetsWon.Content = 0;
            MV.Player1LegsWon.Content = 0;
            MV.Player2LegsWon.Content = 0;
            Set180();
        }
        public void Set180()    //  Устоновка 180 в начале сета
        {
            MV.Player1Points.Content = 180;
            MV.Player2Points.Content = 180;
        }
        public void SetPoints(string s, int p)  //  Установка текущих очков
        {
            if (s == "1")
                MV.Player1Points.Content = p.ToString();
            else
                MV.Player2Points.Content = p.ToString();
        }
        public void IncrementSet(string s)  //  +1 к сету
        {
            if (s == "1")
                MV.Player1SetsWon.Content = (Int32.Parse((MV.Player1SetsWon.Content).ToString()) + 1).ToString();
            else
                MV.Player2SetsWon.Content = (Int32.Parse((MV.Player2SetsWon.Content).ToString()) + 1).ToString();
        }
        public void IncrementLeg(string s)  //  +1 к легу
        {
            if (s == "1")
                MV.Player1LegsWon.Content = (Int32.Parse((MV.Player1LegsWon.Content).ToString()) + 1).ToString();
            else
                MV.Player2LegsWon.Content = (Int32.Parse((MV.Player2LegsWon.Content).ToString()) + 1).ToString();
        }
    }
}
