using System.Collections.Generic;
using System.Text;

namespace OneHundredAndEighty
{
    public static class WinnerWindowLogic  //  Класс логики окна победителя
    {
        public static void ShowWinner(Player winner, Player p1, Player p2, Stack<Throw> AllMatchThrows)
        {
            MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Cсылка на главное окно
            Windows.WinnerWindow WinnerWindow = null;
            WinnerWindow = new Windows.WinnerWindow(); //  Ссылка на окно победителя
            MainWindow.FadeIn();
            WinnerWindow.Owner = MainWindow;    //  Прописываем владельца

            WinnerWindow.WinnerName.Content = new StringBuilder().Append("[ ").Append(winner.Name).Append(" ]").ToString();
            WinnerWindow.ShowDialog();  //  Показываем диалоговое окно
            MainWindow.G.StatisticsWindowLogic.CountMatchStatistics(winner, p1, p2, AllMatchThrows);    //  Считаем статистику матча
            if (WinnerWindow.StatsShow) //  Если нажата кнопка показать статистику матча
                MainWindow.G.StatisticsWindowLogic.ShowMatchStatistics();   //  Показываем статистику матча
            MainWindow.FadeOut();
        }
    }
}
