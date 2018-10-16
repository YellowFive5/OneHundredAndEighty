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
    public class WinnerWindowLogic  //  Класс логики окна победителя
    {
        MainWindow MainWindow = null;    //  Ссылка на главное окно, зануляем
        Windows.WinnerWindow WinnerWindow = null;    //  Ссылка на окно победителя, зануляем
        public void ShowWinner(Player p)
        {
            MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Обновляем ссылку на главное окно
            MainWindow.FadeIn();
            Windows.WinnerWindow WinnerWindow = new Windows.WinnerWindow(); //  Обновляем ссылку на окно победителя
            WinnerWindow.Owner = MainWindow;    //  Прописываем владельца

            WinnerWindow.WinnerName.Content = new StringBuilder().Append("[ ").Append(p.Name).Append(" ]").ToString() ;
            WinnerWindow.ShowDialog();  //  Показываем диалоговое окно
            MainWindow.FadeOut();
        }
    }
}
