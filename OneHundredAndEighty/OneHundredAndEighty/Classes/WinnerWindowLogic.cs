﻿using System;
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
        MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Cсылка на главное окно
        Windows.WinnerWindow WinnerWindow = null;
        public void ShowWinner(Player winner, Player p1, Player p2, Stack<Throw> AllMatchThrows)
        {
            WinnerWindow = new Windows.WinnerWindow(); //  Ссылка на окно победителя
            MainWindow.FadeIn();
            WinnerWindow.Owner = MainWindow;    //  Прописываем владельца

            WinnerWindow.WinnerName.Content = new StringBuilder().Append("[ ").Append(winner.Name).Append(" ]").ToString();
            WinnerWindow.ShowDialog();  //  Показываем диалоговое окно
            if (WinnerWindow.StatsShow) //  Если нажата кнопка показать статистику матча
            {
                MainWindow.G.StatisticsWindowLogic.ShowMatchStatistics(winner, p1, p2, AllMatchThrows);   //  Показываем статистику матча
            }
            MainWindow.FadeOut();
        }
    }
}
