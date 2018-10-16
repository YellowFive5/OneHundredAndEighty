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
        public WinnerWindowLogic WinnerWindowLogic = new WinnerWindowLogic();
        public bool IsOn { get; private set; }  //  Флаг работы матча
        Player Player1 = null;  //  Игрок 1
        Player Player2 = null;  //  Игрок 2
        Player PlayerOnThrow = null;    //  Чей подход
        Player PlayerOnLeg = null;  //  Кто начинает лег
        Stack<Throw> AllMatchThrows = new Stack<Throw>();    //  Коллекция бросков матча
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
            ClearThrowsCollections();
            //  Панели
            InfoPanelLogic.PanelHide(); //  Прячем инфопанель
            BoardPanelLogic.PanelHide();    //  Прячем панель секторов
            SettingsPanelLogic.PanelShow(); //  Показываем панель настроек
            InfoPanelLogic.TextLogClear();  //  Очищаем текстовую панель
            //  Сообщение
            WinnerWindowLogic.ShowWinner(PlayerOnThrow);
        }
        public void AbortGame() //  Отмена текущего матча
        {
            IsOn = false;
            ClearThrowsCollections();
            //  Панели
            InfoPanelLogic.PanelHide(); //  Прячем инфопанель
            BoardPanelLogic.PanelHide();    //  Прячем панель секторов
            SettingsPanelLogic.PanelShow(); //  Показываем панель настроек
            InfoPanelLogic.TextLogClear();  //  Очищаем текстовую панель
            //  Окно
            Windows.AbortWindow window = new Windows.AbortWindow();
            window.Owner = MainWindow;
            window.ShowDialog();
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

            AllMatchThrows.Push(T);  //  Записываем в последный бросок в коллекцию матча
            PlayerOnThrow.AllPlayerThrows.Push(T);  //  Записываем последний бросок игроку

            if (PlayerOnThrow.Throw1 == null) //  Первый бросок
            {
                T.HandNumber = 1;   //  Записываем броску номер в подходе
                PlayerOnThrow.Throw1 = T;   //  Записываем первый бросок игрока на подходе
            }
            else if (PlayerOnThrow.Throw2 == null)    //  Второй бросок
            {
                T.HandNumber = 2;   //  Записываем броску номер в подходе
                PlayerOnThrow.Throw2 = T;   //  Записываем второй бросок игрока на подходе
            }
            else if (PlayerOnThrow.Throw3 == null)    //  Третий бросок
            {
                T.HandNumber = 3;   //  Записываем броску номер в подходе
                PlayerOnThrow.Throw3 = T;   //  Записываем третий бросок игрока на подходе
            }

            PlayerOnThrow.PointsToOut -= (int)T.Points; //  Вычитаем набраные за бросок очки игрока из общего результата лега
            PlayerOnThrow.HandPoints += (int)T.Points;  //  Плюсуем набраные за подход очки игрока
            InfoPanelLogic.PointsSet(PlayerOnThrow);    //  Обновляем инфопанель
            InfoPanelLogic.HelpCheck(PlayerOnThrow);

            IsLegIsOver();  // Проверяем законечен ли лег

        }
        void IsLegIsOver()    //  Или штраф, или правильное окончание лега, или это был последний бросок в подходе 
        {
            //  Проверяем окончен ли лег
            //  НЕПРАВИЛЬНОЕ окончание лега
            if (PlayerOnThrow.PointsToOut < 0 || PlayerOnThrow.PointsToOut == 1 || (PlayerOnThrow.PointsToOut == 0 && (AllMatchThrows.Peek().Multiplier != "Double" && AllMatchThrows.Peek().Multiplier != "Bull_Eye")))    //  Если игрок ушел в минус или оставил единицу, или закрыл лег не корректно (не через удвоение или Bulleye)
            {
                InfoPanelLogic.TextLogAdd(new StringBuilder().Append("    > ").Append(PlayerOnThrow.Name).Append(" FAULT").ToString());  //  Пишем в текстовую панель
                PlayerOnThrow.PointsToOut += PlayerOnThrow.HandPoints;  //  Отменяем подход игрока
                InfoPanelLogic.PointsSet(PlayerOnThrow);    //  Обновляем инфопанель
                ClearHands();   //  Очищаем броски
                TogglePlayerOnThrow();  //  Меняем игрока на броске
                AllMatchThrows.Peek().IsFault = true;   //  Помечаем бросок как штрафной
                //  Выходим
            }
            //  ПРАВИЛЬНОЕ окончание лега
            else if (PlayerOnThrow.PointsToOut == 0)    //  Игрок правильно закрылся
            {
                InfoPanelLogic.TextLogAdd(new StringBuilder().Append("Leg goes to ").Append(PlayerOnThrow.Name).ToString());  //  Пишем в текстовую панель
                PlayerOnThrow.LegsWon += 1; //  Плюсуем выиграный лег
                InfoPanelLogic.LegIncrement(PlayerOnThrow); //  Обновляем инфопанель


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
            if (PlayerOnThrow.LegsWon == LegsToGo)  //  Если игрок выиграл требуемое количество легов
            {
                InfoPanelLogic.TextLogAdd(new StringBuilder().Append("Set goes to ").Append(PlayerOnThrow.Name).ToString());  //  Пишем в текстовую панель
                PlayerOnThrow.SetsWon += 1; //  Добавляем игроку выигранный сет
                Player1.LegsWon = 0;    //  Обнуляем леги игроков
                Player2.LegsWon = 0;    //  Обнуляем леги игроков
                InfoPanelLogic.SetIncrement(PlayerOnThrow); //  Обновляем инфопанель
                InfoPanelLogic.LegsClear(); //  Обновляем инфопанель
                AllMatchThrows.Peek().IsSetWon = true;  //  Помечаем бросок как выигравший сет
                IsGameIsOver(); //  Проверяем не закончен ли матч
            }
        }
        void IsGameIsOver()   //  Проверка закончен ли матч
        {
            if (PlayerOnThrow.SetsWon == SetsToGo)  //  Если игрок выиграл требуемое количество сетов
            {
                AllMatchThrows.Peek().IsSetWon = true;  //  Помечаем бросок как выигравший матч
                EndGame();  //  Матч окончен
            }
        }
        void ClearThrowsCollections()  //  Зануляем коллекции бросков
        {
            AllMatchThrows.Clear(); //  Зануляем коллекцию бросков матча
            Player1.AllPlayerThrows.Clear();    //  Зануляем коллекцию бросков игроков
            Player2.AllPlayerThrows.Clear();
        }

        public void UndoThrow() //  Отмена последнего броска
        {
            if (PlayerOnThrow.Throw1 != null && PlayerOnThrow.Throw2 == null)
            {
                PlayerOnThrow.AllPlayerThrows.Pop();    //  Удаляем последний бросок игрока из коллекции бросков игрока
                PlayerOnThrow.PointsToOut += (int)PlayerOnThrow.Throw1.Points;  //  Возвращаем игроку очки для выхода
                PlayerOnThrow.HandPoints -= (int)PlayerOnThrow.Throw1.Points;   //  Возвращаем игроку очки подхода
                PlayerOnThrow.Throw1 = null;    //  Очищаем первый бросок
            }   //  Первый бросок брошен
            else if (PlayerOnThrow.Throw2 != null && PlayerOnThrow.Throw3 == null)
            {
                PlayerOnThrow.AllPlayerThrows.Pop();    //  Удаляем последний бросок игрока из коллекции бросков игрока
                PlayerOnThrow.PointsToOut += (int)PlayerOnThrow.Throw2.Points;  //  Возвращаем игроку очки для выхода
                PlayerOnThrow.HandPoints -= (int)PlayerOnThrow.Throw2.Points;   //  Возвращаем игроку очки подхода
                PlayerOnThrow.Throw2 = null;    //  Очищаем второй бросок
            }   //  Второй бросок брошен
            else if (PlayerOnThrow.Throw1 == null && AllMatchThrows.Count != 0)
            {
                InfoPanelLogic.TextLogUndo();   //  Очищаем строку текстовой панели
                InfoPanelLogic.TextLogUndo();   //  Очищаем строку текстовой панели

                if (AllMatchThrows.Peek().IsLegWon) //  Если последним броском выигран лег
                {
                    if (AllMatchThrows.Peek().IsSetWon) //  Если последним броском выигран и сет
                    {
                        InfoPanelLogic.SetDecrement(PlayerOnThrow); //  Уменьшаем сет игрока
                        InfoPanelLogic.PointsSet(PlayerOnThrow);    //  Обновляем инфопанель
                        InfoPanelLogic.HelpCheck(PlayerOnThrow);    //  Обновляем помощь
                    }

                    TogglePlayerOnLeg();  //  Возвращаем бросавшего игрока
                    PlayerOnThrow.LegsWon -= 1; //  Удаляем игроку выиграный лег
                    InfoPanelLogic.LegDecrement(PlayerOnThrow); //  Обновляем инфо-панель
                    PlayerOnThrow.PointsToOut = 0;  //  Зануляем очки для победы в леге
                    PlayerOnThrow.PointsToOut += (int)PlayerOnThrow.AllPlayerThrows.Pop().Points;  //  Возвращаем игроку очки последнего результативного броска и удаляем его из коллекции

                    switch (AllMatchThrows.Peek().HandNumber)   //  Смотрим на каком броске закончил подход предыдущий игрок
                    {
                        case 2:
                            PlayerOnThrow.Throw1 = PlayerOnThrow.AllPlayerThrows.Peek(); //  Восстанавливаем первый бросок игрока в подходе
                            PlayerOnThrow.HandPoints += (int)PlayerOnThrow.Throw1.Points;   //  Возвращаем игроку набранные очки подхода
                            break;
                        case 3:
                            PlayerOnThrow.Throw2 = PlayerOnThrow.AllPlayerThrows.Pop(); //  Восстанавливаем второй бросок игрока в подходе
                            PlayerOnThrow.Throw1 = PlayerOnThrow.AllPlayerThrows.Peek(); //  Восстанавливаем первый бросок игрока в подходе
                            PlayerOnThrow.AllPlayerThrows.Push(PlayerOnThrow.Throw2);   //  Возвращаем бросок его в коллекцию игрока
                            PlayerOnThrow.HandPoints += (int)PlayerOnThrow.Throw1.Points;   //  Возвращаем игроку набранные очки подхода 
                            PlayerOnThrow.HandPoints += (int)PlayerOnThrow.Throw2.Points;   //  Возвращаем игроку набранные очки подхода 
                            break;
                        default:
                            break;
                    }
                    InfoPanelLogic.TextLogUndo();   //  Очищаем строку текстовой панели
                }
                else if (AllMatchThrows.Peek().IsFault) //  Бросок был штрафным
                {
                    TogglePlayerOnThrow();  //  Возвращаем бросавшего игрока
                    PlayerOnThrow.AllPlayerThrows.Pop();    //  Удаляем последний бросок игрока из коллекции бросков игрока
                    switch (AllMatchThrows.Peek().HandNumber)   //  Смотрим какой бросок подхода был штрафным
                    {
                        case 2:
                            PlayerOnThrow.Throw1 = PlayerOnThrow.AllPlayerThrows.Peek(); //  Восстанавливаем первый бросок игрока в подходе
                            PlayerOnThrow.HandPoints += (int)PlayerOnThrow.Throw1.Points;   //  Возвращаем игроку набранные очки подхода 
                            PlayerOnThrow.PointsToOut -= PlayerOnThrow.HandPoints;  //  Возвращаем игроку очки для победы в леге
                            break;
                        case 3:
                            PlayerOnThrow.Throw2 = PlayerOnThrow.AllPlayerThrows.Pop(); //  Восстанавливаем второй бросок игрока в подходе
                            PlayerOnThrow.Throw1 = PlayerOnThrow.AllPlayerThrows.Peek(); //  Восстанавливаем первый бросок игрока в подходе
                            PlayerOnThrow.AllPlayerThrows.Push(PlayerOnThrow.Throw2);   //  Возвращаем бросок его в коллекцию игрока
                            PlayerOnThrow.HandPoints += (int)PlayerOnThrow.Throw1.Points;   //  Возвращаем игроку набранные очки подхода 
                            PlayerOnThrow.HandPoints += (int)PlayerOnThrow.Throw2.Points;   //  Возвращаем игроку набранные очки подхода 
                            PlayerOnThrow.PointsToOut -= PlayerOnThrow.HandPoints;  //  Возвращаем игроку очки для победы в леге
                            break;
                        default:
                            break;
                    }
                    InfoPanelLogic.TextLogUndo();   //  Очищаем строку текстовой панели
                }
                else    //  Простой бросок с переходом хода
                {
                    TogglePlayerOnThrow();  //  Возвращаем бросавшего игрока
                    PlayerOnThrow.PointsToOut += (int)PlayerOnThrow.AllPlayerThrows.Pop().Points;   //  Возвращаем игроку очки отмененного броска
                    PlayerOnThrow.Throw2 = PlayerOnThrow.AllPlayerThrows.Pop(); //  Восстанавливаем второй бросок игрока в подходе
                    PlayerOnThrow.Throw1 = PlayerOnThrow.AllPlayerThrows.Pop(); //  Восстанавливаем первый бросок игрока в подходе
                    PlayerOnThrow.AllPlayerThrows.Push(PlayerOnThrow.Throw1);   //  Возвращаем бросок его в коллекцию игрока
                    PlayerOnThrow.AllPlayerThrows.Push(PlayerOnThrow.Throw2);   //  Возвращаем бросок его в коллекцию игрока
                    PlayerOnThrow.HandPoints += (int)PlayerOnThrow.Throw1.Points;   //  Возвращаем игроку набранные очки подхода 
                    PlayerOnThrow.HandPoints += (int)PlayerOnThrow.Throw2.Points;   //  Возвращаем игроку набранные очки подхода 
                }
            }   //  В коллекции бросков матча есть броски. Первый не брошен, значит ход перешел к другому игроку

            if (AllMatchThrows.Count != 0)  //  После каждого нажания кнопки, если броски были
            {
                AllMatchThrows.Pop();   //  Удаляем последний бросок из коллекции бросков матча
                InfoPanelLogic.PointsSet(PlayerOnThrow);    //  Обновляем инфопанель
                InfoPanelLogic.HelpCheck(PlayerOnThrow);    //  Обновляем помощь
                InfoPanelLogic.TextLogUndo();   //  Очищаем строку текстовой панели
            }
        }
        public void SavePoint()
        {

        }
    }



}
