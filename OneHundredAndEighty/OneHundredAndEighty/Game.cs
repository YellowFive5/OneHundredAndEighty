using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OneHundredAndEighty
{
    public class Game
    {
        MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам
        public InfoPanelLogic InfoPanelLogic = new InfoPanelLogic();    //  Инфо-панель
        public SettingsPanelLogic SettingsPanelLogic = new SettingsPanelLogic();    //  Панель настроек матча
        public BoardPanelLogic BoardPanelLogic = new BoardPanelLogic(); //  Панель секторов
        public bool IsOn { get; private set; }  //  Флаг работы матча
        Player Player1 = null;  //  Игрок 1
        Player Player2 = null;  //  Игрок 2
        Player PlayerOnThrow = null;    //  Чей подход
        Player PlayerOnLeg = null;  //  Кто начинает лег
        int PointsToGo; //  Очков в леге
        int LegsToGo;   //  Легов в сете
        int SetsToGo;   //  Сетов в матче

        public void StartGame() //  Начало нового матча
        {
            IsOn = true;
            //  Панели
            SettingsPanelLogic.PanelHide(); //  Прячем панель настроек
            InfoPanelLogic.PanelShow(); //  Показываем инфо-панель
            BoardPanelLogic.PanelShow(); //  Показываем панель секторов
            //  Настройка матча
            PointsToGo = SettingsPanelLogic.PointsToGo();   //  Получаем количество очков лега
            SetsToGo = SettingsPanelLogic.SetsToGo();   //  Получаем количество легов сета
            LegsToGo = SettingsPanelLogic.LegsToGo();   //  Получаем количество сетов матча
            //  Игроки
            Player1 = new Player("Player1", SettingsPanelLogic.Player1Name(), MainWindow.Player1Help, MainWindow.Player1PointsHelp, MainWindow.Player1SetsWon, MainWindow.Player1LegsWon, MainWindow.Player1Points, PointsToGo);  //  Игрок 1
            Player2 = new Player("Player2", SettingsPanelLogic.Player2Name(), MainWindow.Player2Help, MainWindow.Player2PointsHelp, MainWindow.Player2SetsWon, MainWindow.Player2LegsWon, MainWindow.Player2Points, PointsToGo);  //  Игрок 2
            PlayerOnThrow = SettingsPanelLogic.WhoThrowFirst(Player1, Player2); //  Кто первый бросает
            PlayerOnLeg = PlayerOnThrow;    //  Чей первый лег
            //  Инфо-панель
            InfoPanelLogic.PanelNewGame(PointsToGo, LegsToGo.ToString(), SetsToGo.ToString(), Player1, Player2, PlayerOnThrow); //  Новая инфопанель
            InfoPanelLogic.HelpCheck(Player1);  //  Проверка помощи
            InfoPanelLogic.HelpCheck(Player2);  //  Проверка помощи
            //  Текстовая панель
            InfoPanelLogic.TextLogAdd(new StringBuilder()
                .Append("First to ")
                .Append(SetsToGo)
                .Append(" sets in ")
                .Append(LegsToGo)
                .Append(" legs in ")
                .Append(PointsToGo)
                .Append(" points").ToString());
            InfoPanelLogic.TextLogAdd("Game on");
            InfoPanelLogic.TextLogAdd(new StringBuilder().Append(PlayerOnThrow.Name).Append(" on throw:").ToString());
        }
        void EndGame()   //  Конец матча
        {
            IsOn = false;
            //  Панели
            InfoPanelLogic.PanelHide(); //  Прячем инфопанель
            BoardPanelLogic.PanelHide();    //  Прячем панель секторов
            SettingsPanelLogic.PanelShow(); //  Показываем панель настроек
            InfoPanelLogic.TextLogClear();  //  Очищаем текстовую панель
            //  Сообщение
            MessageBox.Show("Победитель матча: " + PlayerOnThrow.Name);
        }
        public void AbortGame()
        {
            IsOn = false;
            //  Панели
            InfoPanelLogic.PanelHide(); //  Прячем инфопанель
            BoardPanelLogic.PanelHide();    //  Прячем панель секторов
            SettingsPanelLogic.PanelShow(); //  Показываем панель настроек
            InfoPanelLogic.TextLogClear();  //  Очищаем текстовую панель
            //  Сообщение
            MessageBox.Show("Матч отменен");
        }

        void SetPlayerOnThrow(Player p)  //  Установка игрока на подходе
        {
            InfoPanelLogic.HelpCheck(PlayerOnThrow);  //  Проверка помощи
            PlayerOnThrow = p;
            InfoPanelLogic.WhoThrowSliderSet(PlayerOnThrow);
            InfoPanelLogic.HelpCheck(PlayerOnThrow);  //  Проверка помощи
            InfoPanelLogic.TextLogAdd(new StringBuilder().Append(PlayerOnThrow.Name).Append(" on throw:").ToString());  //  Пишем в текстовую панель
        }
        void TogglePlayerOnThrow()   //  Смена игрока на подходе
        {
            if (PlayerOnThrow.Tag == "Player1") //  Меняем игрока
                SetPlayerOnThrow(Player2);
            else if (PlayerOnThrow.Tag == "Player2")
                SetPlayerOnThrow(Player1);
        }
        void TogglePlayerOnLeg() //  Смена игрока на начало лега
        {
            if (PlayerOnLeg.Tag == "Player1") //  Меняем игрока
                PlayerOnLeg = Player2;
            else if (PlayerOnLeg.Tag == "Player2")
                PlayerOnLeg = Player1;
            SetPlayerOnThrow(PlayerOnLeg); //  Меняем игрока на подходе

            InfoPanelLogic.DotSet(PlayerOnThrow); //  Перемещаем точку
        }
        void ClearHands()    //  Очистка бросков игроков
        {
            Player1.ClearHand();
            Player2.ClearHand();
        }

        public void NextThrow(Throw T)  //  Очередной бросок
        {
            InfoPanelLogic.TextLogAdd(new StringBuilder().Append("    > ").Append(PlayerOnThrow.Name).Append(" throws ").Append(T.Points).ToString());

            if (!PlayerOnThrow.Throw1.IsThrown) //  Первый бросок
                PlayerOnThrow.Throw1 = T;   //  Записываем первый бросок игрока на подходе
            else if (!PlayerOnThrow.Throw2.IsThrown)    //  Второй бросок
                PlayerOnThrow.Throw2 = T;   //  Записываем второй бросок игрока на подходе
            else if (!PlayerOnThrow.Throw3.IsThrown)    //  Третий бросок
                PlayerOnThrow.Throw3 = T;   //  Записываем третий бросок игрока на подходе

            PlayerOnThrow.PointsToOut -= (int)T.Points; //  Вычитаем набраные за бросок очки игрока из общего результата лега
            PlayerOnThrow.HandPoints += (int)T.Points;  //  Плюсуем набраные за подход очки игрока
            InfoPanelLogic.PointsSet(PlayerOnThrow);    //  Обновляем инфопанель
            InfoPanelLogic.HelpCheck(PlayerOnThrow);

            IsLegIsOver(T);  // Проверяем законечен ли лег
        }
        void IsLegIsOver(Throw T)    //  Или штраф, или правильное окончание лега, или это был последний бросок в подходе 
        {
            //  Проверяем окончен ли лег
            //  НЕПРАВИЛЬНОЕ окончание лега
            if (PlayerOnThrow.PointsToOut < 0 || PlayerOnThrow.PointsToOut == 1 || (PlayerOnThrow.PointsToOut == 0 && (T.Multiplier != "Double" && T.Multiplier != "Bull_Eye")))    //  Если игрок ушел в минус или оставил единицу, или закрыл лег не корректно (не через удвоение или Bulleye)
            {
                InfoPanelLogic.TextLogAdd(new StringBuilder().Append("    > ").Append(PlayerOnThrow.Name).Append(" FAULT").ToString());  //  Пишем в текстовую панель
                PlayerOnThrow.PointsToOut += PlayerOnThrow.HandPoints;  //  Отменяем подход игрока
                InfoPanelLogic.PointsSet(PlayerOnThrow);    //  Обновляем инфопанель
                ClearHands();   //  Очищаем броски
                TogglePlayerOnThrow();  //  Меняем игрока на броске
                //  Выходим
            }
            //  ПРАВИЛЬНОЕ окончание лега
            else if (PlayerOnThrow.PointsToOut == 0)    //  Игрок правильно закрылся
            {
                InfoPanelLogic.TextLogAdd(new StringBuilder().Append("Leg goes to ").Append(PlayerOnThrow.Name).ToString());  //  Пишем в текстовую панель
                PlayerOnThrow.LegsWon += 1; //  Плюсуем выиграный лег
                InfoPanelLogic.LegIncrement(PlayerOnThrow); //  Обновляем инфопанель
                ClearHands();   //  Очищаем броски
                IsSetIsOver();  //  Проверяем не закончен ли сет
                if (IsOn)   //  Если игра продолжается
                {
                    TogglePlayerOnLeg();    //  Смена игрока на начало лега 
                    Player1.PointsToOut = PointsToGo;   //  Обновляем очки нового лега игрока 1
                    Player2.PointsToOut = PointsToGo;   //  Обновляем очки нового лега игрока 2
                    InfoPanelLogic.PointsClear(PointsToGo);    //  Обновляем инфопанель
                    InfoPanelLogic.HelpCheck(Player1);  //  Проверка помощи
                    InfoPanelLogic.HelpCheck(Player2);  //  Проверка помощи
                }
                //  Выходим
            }
            //  Лег не окончен и это был последний бросок игрока в подходе
            else if (PlayerOnThrow.Throw3.IsThrown) //  Если это был последний бросок игрока
            {
                ClearHands();   //  Очищаем броски
                TogglePlayerOnThrow();  //  Меняем игрока на броске
            }
        }
        void IsSetIsOver()    //  Проверка закончен ли сет
        {
            if (PlayerOnThrow.LegsWon == LegsToGo)  //  Если игрок выиграл требуемое количество легов
            {
                InfoPanelLogic.TextLogAdd(new StringBuilder().Append("Set goes to ").Append(PlayerOnThrow.Name).ToString());  //  Пишем в текстовую панель
                PlayerOnThrow.SetsWon += 1; //  Добавляем игроку выигранный сет
                Player1.LegsWon = 0;    //  Обнуляем леги игроков
                Player2.LegsWon = 0;    //  Обнуляем леги игроков
                InfoPanelLogic.SetIncrement(PlayerOnThrow); //  Обновляем инфопанель
                InfoPanelLogic.LegsClear(); //  Обновляем инфопанель
                IsGameIsOver(); //  Проверяем не закончен ли матч
            }
        }
        void IsGameIsOver()   //  Проверка закончен ли матч
        {
            if (PlayerOnThrow.SetsWon == SetsToGo)  //  Если игрок выиграл требуемое количество сетов
                EndGame();  //  Матч окончен
        }
    }


}
