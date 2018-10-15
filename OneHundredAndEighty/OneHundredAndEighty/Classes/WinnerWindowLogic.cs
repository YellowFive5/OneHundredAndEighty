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
    public class WinnerWindowLogic
    {
        MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам
        Windows.WinnerWindow WinnerWindow = new Windows.WinnerWindow();

        public void ShowWinner(Player p)
        {
            WinnerWindow.Owner = MainWindow;


            WinnerWindow.ShowDialog();
        }
    }
}
