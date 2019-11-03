#region Usings

using System.Collections.Generic;
using System.Text;
using OneHundredAndEighty.Windows;

#endregion

namespace OneHundredAndEighty
{
    public static class WinnerWindowLogic //  Класс логики окна победителя
    {
        private static readonly MainWindow MainWindow = (MainWindow) System.Windows.Application.Current.MainWindow; //  Cсылка на главное окно

        public static void ShowWinner(Player winner, Player player1, Player player2, Stack<Throw> allMatchThrows)
        {
            var winnerWindow = new WinnerWindow();
            MainWindow.FadeIn();
            winnerWindow.Owner = MainWindow; //  Прописываем владельца

            winnerWindow.WinnerName.Content = new StringBuilder().Append("[ ").Append(winner.Name).Append(" ]").ToString();
            winnerWindow.ShowDialog(); //  Показываем диалоговое окно
            MainWindow.game.statisticsWindowLogic.CountMatchStatistics(winner, player1, player2, allMatchThrows); //  Считаем статистику матча
            if (winnerWindow.StatsShow) //  Если нажата кнопка показать статистику матча
            {
                MainWindow.game.statisticsWindowLogic.ShowMatchStatistics(); //  Показываем статистику матча
            }

            MainWindow.FadeOut();
        }
    }
}