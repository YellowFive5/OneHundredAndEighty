using System.Collections.Generic;
using System.Text;

namespace OneHundredAndEighty
{
    public class Game
    {
        MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам
        public InfoPanelLogic InfoPanelLogic = new InfoPanelLogic();    //  Инфо-панель
        public SettingsPanelLogic SettingsPanelLogic = new SettingsPanelLogic();    //  Панель настроек матча
        public StatisticsWindowLogic StatisticsWindowLogic = new StatisticsWindowLogic();   //  Окно статистики
        public bool IsOn { get; private set; }  //  Флаг работы матча
        Player Player1 = null;  //  Игрок 1
        Player Player2 = null;  //  Игрок 2
        Player PlayerOnThrow = null;    //  Чей подход
        Player PlayerOnLeg = null;  //  Кто начинает лег
        Stack<Throw> AllMatchThrows = new Stack<Throw>();    //  Коллекция бросков матча
        Stack<SavePoint> SavePoints = new Stack<SavePoint>();   //  Коллекция точек сохранения игры
        int PointsToGo; //  Очков в леге
        int LegsToGo;   //  Легов в сете
        int SetsToGo;   //  Сетов в матче

        public void StartGame() //  Начало нового матча
        {

            IsOn = true;    //  Флаг матча
            //  Панели
            SettingsPanelLogic.PanelHide(); //  Прячем панель настроек
            InfoPanelLogic.PanelShow(); //  Показываем инфо-панель
            BoardPanelLogic.PanelShow(); //  Показываем панель секторов
            //  Настройка матча
            PointsToGo = SettingsPanelLogic.PointsToGo();   //  Получаем количество очков лега
            SetsToGo = SettingsPanelLogic.SetsToGo();   //  Получаем количество легов сета
            LegsToGo = SettingsPanelLogic.LegsToGo();   //  Получаем количество сетов матча
            //  Игроки
            Player1 = new Player("Player1", (int)MainWindow.Player1NameCombobox.SelectedValue, SettingsPanelLogic.Player1Name(), MainWindow.Player1Help, MainWindow.Player1PointsHelp, MainWindow.Player1SetsWon, MainWindow.Player1LegsWon, MainWindow.Player1Points, PointsToGo);  //  Игрок 1
            Player2 = new Player("Player2", (int)MainWindow.Player2NameCombobox.SelectedValue, SettingsPanelLogic.Player2Name(), MainWindow.Player2Help, MainWindow.Player2PointsHelp, MainWindow.Player2SetsWon, MainWindow.Player2LegsWon, MainWindow.Player2Points, PointsToGo);  //  Игрок 2
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
                .Append(" legs with ")
                .Append(PointsToGo)
                .Append(" points").ToString());
            InfoPanelLogic.TextLogAdd("Game on");
            InfoPanelLogic.TextLogAdd(new StringBuilder().Append(PlayerOnThrow.Name).Append(" on throw:").ToString());
        }
        void EndGame()   //  Конец матча
        {
            IsOn = false;   //  Флаг матча
            //  Сообщение
            WinnerWindowLogic.ShowWinner(PlayerOnThrow, Player1, Player2, AllMatchThrows);    //  Показываем окно победителя и статистику
            //  Панели
            InfoPanelLogic.PanelHide(); //  Прячем инфопанель
            BoardPanelLogic.PanelHide();    //  Прячем панель секторов
            SettingsPanelLogic.PanelShow(); //  Показываем панель настроек
            //  Сохранение в БД
            DBwork.AftermatchSave(StatisticsWindowLogic);
            //  Обнуление коллекций
            ClearCollections();   //  Зануляем коллекции бросков
        }
        public void AbortGame() //  Отмена текущего матча
        {
            IsOn = false;   //  Флаг матча
            ClearCollections();   //  Зануляем коллекции бросков
            //  Панели
            InfoPanelLogic.PanelHide(); //  Прячем инфопанель
            BoardPanelLogic.PanelHide();    //  Прячем панель секторов
            SettingsPanelLogic.PanelShow(); //  Показываем панель настроек
            InfoPanelLogic.TextLogClear();  //  Очищаем текстовую панель
            //  Окно
            Windows.AbortWindow window = new Windows.AbortWindow();
            window.Owner = MainWindow;
            window.ShowDialog();    //  Показываем окно отмены матча
        }

        void SetPlayerOnThrow(Player p)  //  Установка игрока на подходе
        {
            InfoPanelLogic.HelpCheck(PlayerOnThrow);  //  Проверка помощи
            PlayerOnThrow = p;  //  Устанавливаем игрока на броске
            InfoPanelLogic.WhoThrowSliderSet(PlayerOnThrow);    //  Устанавливаем стайдер инфо-панели
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
        void ClearCollections()  //  Зануляем коллекции бросков
        {
            StatisticsWindowLogic.ClearColection(); //  Зануляем коллекцию статистики
            InfoPanelLogic.TextLogClear();  //  Очищаем текстовую панель
            InfoPanelLogic.UndoThrowButtonOff();    //  Выключаем кнопку отмены броска
            AllMatchThrows.Clear(); //  Зануляем коллекцию бросков матча
            SavePoints.Clear(); //  Зануляем точки сохнанения
            Player1.AllPlayerThrows.Clear();    //  Зануляем коллекцию бросков игрока
            Player2.AllPlayerThrows.Clear();    //  Зануляем коллекцию бросков игрока
        }

        public void NextThrow(Throw T)  //  Очередной бросок
        {
            SavePoint();    //  Сохраняем точку игры перед броском
            InfoPanelLogic.UndoThrowButtonOn();    //  Включаем кнопку отмены броска

            InfoPanelLogic.TextLogAdd(new StringBuilder().Append("    > ").Append(PlayerOnThrow.Name).Append(" throws ").Append(T.Points).ToString());  //  Пишем в текстовую панель
            T.WhoThrow = PlayerOnThrow.Tag; //  Записываем в бросок кто его бросил

            AllMatchThrows.Push(T);  //  Записываем в последный бросок в коллекцию матча
            PlayerOnThrow.AllPlayerThrows.Push(T);  //  Записываем последний бросок бросившему игроку

            if (PlayerOnThrow.Throw1 == null) //  Если это был первый бросок игрока на подходе
            {
                T.HandNumber = 1;   //  Записываем броску номер в подходе
                PlayerOnThrow.Throw1 = T;   //  Записываем первый бросок игрока на подходе
            }
            else if (PlayerOnThrow.Throw2 == null)  //  Если это был второй бросок игрока на подходе
            {
                T.HandNumber = 2;   //  Записываем броску номер в подходе
                PlayerOnThrow.Throw2 = T;   //  Записываем второй бросок игрока на подходе
            }
            else if (PlayerOnThrow.Throw3 == null)  //  Если это был третий бросок игрока на подходе
            {
                T.HandNumber = 3;   //  Записываем броску номер в подходе
                PlayerOnThrow.Throw3 = T;   //  Записываем третий бросок игрока на подходе
            }

            PlayerOnThrow.PointsToOut -= (int)T.Points; //  Вычитаем набраные за бросок очки игрока из общего результата лега
            PlayerOnThrow.HandPoints += (int)T.Points;  //  Плюсуем набраные за подход очки игрока
            if (PlayerOnThrow.HandPoints == 180)    //  Если в подходе набрано 180
                PlayerOnThrow._180 += 1;    //  Записываем
            InfoPanelLogic.PointsSet(PlayerOnThrow);    //  Обновляем инфопанель
            InfoPanelLogic.HelpCheck(PlayerOnThrow);    //  Проверяем помощь

            IsLegIsOver();  // Проверяем законечен ли лег
        }

        public void UndoThrow() //  Отмена последнего броска
        {
            if (AllMatchThrows.Count != 0)  //  Проверяем возможность отмены броска, если коллекция бросков матча не пуста
            {
                InfoPanelLogic.UndoThrowButtonOff();    //  Выключаем кнопку отмены броска

                Player1.SetsWon = SavePoints.Peek().Player1SetsWon; //  Восстанавливаем Игроку 1 выигранные сеты
                Player1.LegsWon = SavePoints.Peek().Player1LegsWon; //  Восстанавливаем Игроку 1 выигранные леги
                Player1.PointsToOut = SavePoints.Peek().Player1PointsToOut; //  Восстанавливаем Игроку 1 очки на завершение лега
                Player2.SetsWon = SavePoints.Peek().Player2SetsWon; //  Восстанавливаем Игроку 2 выигранные сеты
                Player2.LegsWon = SavePoints.Peek().Player2LegsWon; //  Восстанавливаем Игроку 2 выигранные леги
                Player2.PointsToOut = SavePoints.Peek().Player2PointsToOut; //  Восстанавливаем Игроку 2 очки на завершение лега
                Player1._180 = SavePoints.Peek().Player1_180;   //  Восстанавливаем Игроку 1 180
                Player2._180 = SavePoints.Peek().Player2_180;   //  Восстанавливаем Игроку 2 180
                InfoPanelLogic.SetsSet(Player1);    //  Восстанавливаем в инфо-панели очки выигранных сетов Игрока 1
                InfoPanelLogic.LegsSet(Player1);    //  Восстанавливаем в инфо-панели очки выигранных легов Игрока 1
                InfoPanelLogic.PointsSet(Player1);  //  Восстанавливаем в инфо-панели очки на завершение лега Игрока 1
                InfoPanelLogic.SetsSet(Player2);    //  Восстанавливаем в инфо-панели очки выигранных сетов Игрока 2
                InfoPanelLogic.LegsSet(Player2);    //  Восстанавливаем в инфо-панели очки выигранных легов Игрока 2
                InfoPanelLogic.PointsSet(Player2);  //  Восстанавливаем в инфо-панели очки на завершение лега Игрока 2
                PlayerOnThrow = SavePoints.Peek().PlayerOnThrow;    //  Восстанавливаем игрока на броске
                PlayerOnLeg = SavePoints.Peek().PlayerOnLeg;    //  Восстанавливаем игрока на начало лега
                InfoPanelLogic.TextLogUndo();   //  Удаяем строку текстовой панели

                if (AllMatchThrows.Peek().IsLegWon || AllMatchThrows.Peek().IsFault || AllMatchThrows.Peek().HandNumber == 3)   //  Если последний бросок был переходным
                {
                    InfoPanelLogic.TextLogUndo();   //  Удаяем строку текстовой панели
                    if (AllMatchThrows.Peek().IsLegWon) // Если отменяемым броском выигран лег
                    {
                        InfoPanelLogic.DotSet(PlayerOnThrow); //  Перемещаем точку начала лега
                        InfoPanelLogic.TextLogUndo();   //  Удаяем строку текстовой панели
                    }
                    if (AllMatchThrows.Peek().IsSetWon) // Если отменяемым броском выигран и сет
                    {
                        InfoPanelLogic.TextLogUndo();   //  Удаяем строку текстовой панели
                    }
                    if (AllMatchThrows.Peek().IsFault)  //  Если отменяемый бросок был штрафным
                    {
                        InfoPanelLogic.TextLogUndo();   //  Удаяем строку текстовой панели
                    }
                }

                SetPlayerOnThrow(PlayerOnThrow);    //  Восстанавливаем игрока на подходе
                InfoPanelLogic.TextLogUndo();   //  Удаяем строку текстовой панели
                PlayerOnThrow.Throw1 = SavePoints.Peek().FirstThrow;    //  Восстанавливаем игроку на броске первый бросок
                PlayerOnThrow.Throw2 = SavePoints.Peek().SecondThrow;   //  Восстанавливаем игроку на броске второй бросок
                PlayerOnThrow.Throw3 = SavePoints.Peek().ThirdThrow;    //  Восстанавливаем игроку на броске третий бросок
                PlayerOnThrow.HandPoints = SavePoints.Peek().PlayerOnThrowHand; //  Восстанавливаем игроку на броске очки подхода
                InfoPanelLogic.HelpCheck(PlayerOnThrow);    //  Проверяем помощь

                AllMatchThrows.Pop();   //  Удалаяем последний бросок из коллекции матча
                PlayerOnThrow.AllPlayerThrows.Pop();    //  Удаляем последний бросок из коллекции игрока
                SavePoints.Pop();   //  Удаляем последнюю точку сохранения

                InfoPanelLogic.UndoThrowButtonOn();    //  Включаем кнопку отмены броска

                if (AllMatchThrows.Count == 0)  //  Если бросков для отмены больше нет
                    InfoPanelLogic.UndoThrowButtonOff();    //  Выключаем кнопку отмены броска
            }
        }

        public void SavePoint()
        {
            SavePoints.Push(new SavePoint(Player1, Player2, PlayerOnThrow, PlayerOnLeg));
        }

        void IsLegIsOver()    //  Или штраф, или правильное окончание лега, или это был последний бросок в подходе 
        {
            //  Проверяем окончен ли лег
            //  НЕПРАВИЛЬНОЕ окончание лега
            if (PlayerOnThrow.PointsToOut < 0 || PlayerOnThrow.PointsToOut == 1 || (PlayerOnThrow.PointsToOut == 0 && (AllMatchThrows.Peek().Multiplier != "Double" && AllMatchThrows.Peek().Multiplier != "Bull_Eye")))    //  Если игрок ушел в минус или оставил единицу, или закрыл лег не корректно (не через удвоение или Bulleye)
            {
                InfoPanelLogic.TextLogAdd(new StringBuilder().Append("    > ").Append(PlayerOnThrow.Name).Append(" FAULT").ToString());  //  Пишем в текстовую панель
                PlayerOnThrow.PointsToOut += PlayerOnThrow.HandPoints;  //  Отменяем подход игрока
                if (PlayerOnThrow.HandPoints == 180)    //  Если в штрафном подходе было набрано 180 очков
                    PlayerOnThrow._180 -= 1;    //  Отменяем игроку 180
                InfoPanelLogic.PointsSet(PlayerOnThrow);    //  Обновляем инфопанель
                ClearHands();   //  Очищаем броски
                TogglePlayerOnThrow();  //  Меняем игрока на броске
                AllMatchThrows.Peek().IsFault = true;   //  Помечаем бросок как штрафной
                //  Выходим
            }
            //  ПРАВИЛЬНОЕ окончание лега
            else if (PlayerOnThrow.PointsToOut == 0)    //  Игрок правильно закрыл лег
            {
                InfoPanelLogic.TextLogAdd(new StringBuilder().Append("Leg goes to ").Append(PlayerOnThrow.Name).ToString());  //  Пишем в текстовую панель
                PlayerOnThrow.LegsWon += 1; //  Плюсуем выиграный лег
                InfoPanelLogic.LegsSet(PlayerOnThrow); //  Обновляем инфопанель
                ClearHands();   //  Очищаем броски
                AllMatchThrows.Peek().IsLegWon = true;  //  Помечаем бросок как выигравший лег
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
            else if (PlayerOnThrow.Throw3 != null) //  Если это был последний бросок игрока
            {
                ClearHands();   //  Очищаем броски
                TogglePlayerOnThrow();  //  Меняем игрока на броске
            }
        }
        void IsSetIsOver()    //  Проверка закончен ли сет
        {
            if (PlayerOnThrow.LegsWon == LegsToGo)  //  Если игрок выиграл требуемое количество легов для окончания сета
            {
                InfoPanelLogic.TextLogAdd(new StringBuilder().Append("Set goes to ").Append(PlayerOnThrow.Name).ToString());  //  Пишем в текстовую панель
                PlayerOnThrow.SetsWon += 1; //  Добавляем игроку выигранный сет
                InfoPanelLogic.SetsSet(PlayerOnThrow); //  Обновляем инфопанель
                Player1.LegsWon = 0;    //  Обнуляем леги игроков
                Player2.LegsWon = 0;    //  Обнуляем леги игроков
                InfoPanelLogic.LegsClear(); //  Обновляем инфопанель
                AllMatchThrows.Peek().IsSetWon = true;  //  Помечаем бросок как выигравший сет
                IsGameIsOver(); //  Проверяем не закончен ли матч
            }
        }
        void IsGameIsOver()   //  Проверка закончен ли матч
        {
            if (PlayerOnThrow.SetsWon == SetsToGo)  //  Если игрок выиграл требуемое количество сетов для завершения матча
            {
                AllMatchThrows.Peek().IsMatchWon = true;  //  Помечаем бросок как выигравший матч
                EndGame();  //  Матч окончен
            }
        }
    }
}
