#region Usings

using System.Collections.Generic;
using System.Text;

#endregion

namespace OneHundredAndEighty
{
    public class Game
    {
        private readonly MainWindow mainWindow = (MainWindow) System.Windows.Application.Current.MainWindow; //  Ссылка на главное окно для доступа к элементам
        private readonly InfoPanelLogic infoPanelLogic = new InfoPanelLogic(); //  Инфо-панель
        private readonly SettingsPanelLogic settingsPanelLogic = new SettingsPanelLogic(); //  Панель настроек матча
        public readonly StatisticsWindowLogic statisticsWindowLogic = new StatisticsWindowLogic(); //  Окно статистики
        public bool IsOn { get; private set; } //  Флаг работы матча
        private Player player1; //  Игрок 1
        private Player player2; //  Игрок 2
        private Player playerOnThrow; //  Чей подход
        private Player playerOnLeg; //  Кто начинает лег
        private readonly Stack<Throw> allMatchThrows = new Stack<Throw>(); //  Коллекция бросков матча
        private readonly Stack<SavePoint> savePoints = new Stack<SavePoint>(); //  Коллекция точек сохранения игры
        private int pointsToGo; //  Очков в леге
        private int legsToGo; //  Легов в сете
        private int setsToGo; //  Сетов в матче

        public void StartGame() //  Начало нового матча
        {
            IsOn = true; //  Флаг матча
            //  Панели
            mainWindow.PlayerTab.IsEnabled = false;
            settingsPanelLogic.PanelHide(); //  Прячем панель настроек
            infoPanelLogic.PanelShow(); //  Показываем инфо-панель
            BoardPanelLogic.PanelShow(); //  Показываем панель секторов
            PlayerOverview.ClearPanel(); //  Очищаем панель данных игроков
            //  Настройка матча
            pointsToGo = settingsPanelLogic.PointsToGo(); //  Получаем количество очков лега
            setsToGo = settingsPanelLogic.SetsToGo(); //  Получаем количество легов сета
            legsToGo = settingsPanelLogic.LegsToGo(); //  Получаем количество сетов матча
            //  Игроки
            player1 = new Player("Player1", (int) mainWindow.Player1NameCombobox.SelectedValue, settingsPanelLogic.Player1Name(), mainWindow.Player1Help, mainWindow.Player1PointsHelp, mainWindow.Player1SetsWon, mainWindow.Player1LegsWon, mainWindow.Player1Points, pointsToGo); //  Игрок 1
            player2 = new Player("Player2", (int) mainWindow.Player2NameCombobox.SelectedValue, settingsPanelLogic.Player2Name(), mainWindow.Player2Help, mainWindow.Player2PointsHelp, mainWindow.Player2SetsWon, mainWindow.Player2LegsWon, mainWindow.Player2Points, pointsToGo); //  Игрок 2
            playerOnThrow = settingsPanelLogic.WhoThrowFirst(player1, player2); //  Кто первый бросает
            playerOnLeg = playerOnThrow; //  Чей первый лег
            //  Инфо-панель
            infoPanelLogic.PanelNewGame(pointsToGo, legsToGo.ToString(), setsToGo.ToString(), player1, player2, playerOnThrow); //  Новая инфопанель
            infoPanelLogic.HelpCheck(player1); //  Проверка помощи
            infoPanelLogic.HelpCheck(player2); //  Проверка помощи
            //  Текстовая панель
            infoPanelLogic.TextLogAdd(new StringBuilder()
                                      .Append("First to ")
                                      .Append(setsToGo)
                                      .Append(" sets in ")
                                      .Append(legsToGo)
                                      .Append(" legs with ")
                                      .Append(pointsToGo)
                                      .Append(" points").ToString());
            infoPanelLogic.TextLogAdd("Game on");
            infoPanelLogic.TextLogAdd(new StringBuilder().Append(playerOnThrow.Name).Append(" on throw:").ToString());
        }

        private void EndGame() //  Конец матча
        {
            IsOn = false; //  Флаг матча
            //  Сообщение
            WinnerWindowLogic.ShowWinner(playerOnThrow, player1, player2, allMatchThrows); //  Показываем окно победителя и статистику
            //  Панели
            mainWindow.PlayerTab.IsEnabled = true;
            infoPanelLogic.PanelHide(); //  Прячем инфопанель
            BoardPanelLogic.PanelHide(); //  Прячем панель секторов
            settingsPanelLogic.PanelShow(); //  Показываем панель настроек
            //  Сохранение в БД
            DBwork.AfterMatchSave(statisticsWindowLogic);
            DBwork.UpdateAchieves(statisticsWindowLogic);
            //  Обнуление коллекций
            ClearCollections(); //  Зануляем коллекции бросков
        }

        public void AbortGame() //  Отмена текущего матча
        {
            IsOn = false; //  Флаг матча
            ClearCollections(); //  Зануляем коллекции бросков
            //  Панели
            mainWindow.PlayerTab.IsEnabled = true;
            infoPanelLogic.PanelHide(); //  Прячем инфопанель
            BoardPanelLogic.PanelHide(); //  Прячем панель секторов
            settingsPanelLogic.PanelShow(); //  Показываем панель настроек
            infoPanelLogic.TextLogClear(); //  Очищаем текстовую панель
            //  Окно
            var window = new Windows.AbortWindow {Owner = mainWindow};
            window.ShowDialog(); //  Показываем окно отмены матча
        }

        private void SetPlayerOnThrow(Player player) //  Установка игрока на подходе
        {
            infoPanelLogic.HelpCheck(playerOnThrow); //  Проверка помощи
            playerOnThrow = player; //  Устанавливаем игрока на броске
            infoPanelLogic.WhoThrowSliderSet(playerOnThrow); //  Устанавливаем стайдер инфо-панели
            infoPanelLogic.HelpCheck(playerOnThrow); //  Проверка помощи
            infoPanelLogic.TextLogAdd(new StringBuilder().Append(playerOnThrow.Name).Append(" on throw:").ToString()); //  Пишем в текстовую панель
        }

        private void TogglePlayerOnThrow() //  Смена игрока на подходе
        {
            switch (playerOnThrow.Tag)
            {
                //  Меняем игрока
                case "Player1":
                    SetPlayerOnThrow(player2);
                    break;
                case "Player2":
                    SetPlayerOnThrow(player1);
                    break;
            }
        }

        private void TogglePlayerOnLeg() //  Смена игрока на начало лега
        {
            switch (playerOnLeg.Tag)
            {
                //  Меняем игрока
                case "Player1":
                    playerOnLeg = player2;
                    break;
                case "Player2":
                    playerOnLeg = player1;
                    break;
            }

            SetPlayerOnThrow(playerOnLeg); //  Меняем игрока на подходе

            infoPanelLogic.DotSet(playerOnThrow); //  Перемещаем точку
        }

        private void ClearHands() //  Очистка бросков игроков
        {
            player1.ClearHand();
            player2.ClearHand();
        }

        private void ClearCollections() //  Зануляем коллекции бросков
        {
            statisticsWindowLogic.ClearCollection(); //  Зануляем коллекцию статистики
            infoPanelLogic.TextLogClear(); //  Очищаем текстовую панель
            infoPanelLogic.UndoThrowButtonOff(); //  Выключаем кнопку отмены броска
            allMatchThrows.Clear(); //  Зануляем коллекцию бросков матча
            savePoints.Clear(); //  Зануляем точки сохнанения
        }

        public void NextThrow(Throw thrw) //  Очередной бросок
        {
            SavePoint(); //  Сохраняем точку игры перед броском
            infoPanelLogic.UndoThrowButtonOn(); //  Включаем кнопку отмены броска

            infoPanelLogic.TextLogAdd(new StringBuilder().Append("    > ").Append(playerOnThrow.Name).Append(" throws ").Append(thrw.Points).ToString()); //  Пишем в текстовую панель
            thrw.WhoThrow = playerOnThrow.Tag; //  Записываем в бросок кто его бросил

            allMatchThrows.Push(thrw); //  Записываем в последный бросок в коллекцию матча

            if (playerOnThrow.throw1 == null) //  Если это был первый бросок игрока на подходе
            {
                thrw.HandNumber = 1; //  Записываем броску номер в подходе
                playerOnThrow.throw1 = thrw; //  Записываем первый бросок игрока на подходе
            }
            else if (playerOnThrow.throw2 == null) //  Если это был второй бросок игрока на подходе
            {
                thrw.HandNumber = 2; //  Записываем броску номер в подходе
                playerOnThrow.throw2 = thrw; //  Записываем второй бросок игрока на подходе
            }
            else if (playerOnThrow.throw3 == null) //  Если это был третий бросок игрока на подходе
            {
                thrw.HandNumber = 3; //  Записываем броску номер в подходе
                playerOnThrow.throw3 = thrw; //  Записываем третий бросок игрока на подходе
            }

            playerOnThrow.pointsToOut -= (int) thrw.Points; //  Вычитаем набраные за бросок очки игрока из общего результата лега
            playerOnThrow.handPoints += (int) thrw.Points; //  Плюсуем набраные за подход очки игрока
            if (playerOnThrow.handPoints == 180) //  Если в подходе набрано 180
            {
                playerOnThrow._180 += 1; //  Записываем
            }

            if (!playerOnThrow.is3Bull) //  Проверяем на ачивку 3Bull
            {
                if (playerOnThrow.handPoints == 150)
                {
                    if (playerOnThrow.throw1.Points == 50 && playerOnThrow.throw2.Points == 50 && playerOnThrow.throw3.Points == 50)
                    {
                        playerOnThrow.is3Bull = true;
                    }
                }
            }

            if (!playerOnThrow.ismrZ) //  Проверяем на ачивку mrZ
            {
                if (playerOnThrow.handPoints == 0)
                {
                    playerOnThrow.ismrZ = true;
                }
            }

            infoPanelLogic.PointsSet(playerOnThrow); //  Обновляем инфопанель
            infoPanelLogic.HelpCheck(playerOnThrow); //  Проверяем помощь

            IsLegIsOver(); // Проверяем законечен ли лег
        }

        public void UndoThrow() //  Отмена последнего броска
        {
            if (allMatchThrows.Count != 0) //  Проверяем возможность отмены броска, если коллекция бросков матча не пуста
            {
                infoPanelLogic.UndoThrowButtonOff(); //  Выключаем кнопку отмены броска

                player1.is3Bull = savePoints.Peek().Player1Is3Bull;
                player2.is3Bull = savePoints.Peek().Player2Is3Bull;
                player1.ismrZ = savePoints.Peek().Player1IsmrZ;
                player2.ismrZ = savePoints.Peek().Player2IsmrZ;
                player1.setsWon = savePoints.Peek().Player1SetsWon; //  Восстанавливаем Игроку 1 выигранные сеты
                player1.legsWon = savePoints.Peek().Player1LegsWon; //  Восстанавливаем Игроку 1 выигранные леги
                player1.pointsToOut = savePoints.Peek().Player1PointsToOut; //  Восстанавливаем Игроку 1 очки на завершение лега
                player2.setsWon = savePoints.Peek().Player2SetsWon; //  Восстанавливаем Игроку 2 выигранные сеты
                player2.legsWon = savePoints.Peek().Player2LegsWon; //  Восстанавливаем Игроку 2 выигранные леги
                player2.pointsToOut = savePoints.Peek().Player2PointsToOut; //  Восстанавливаем Игроку 2 очки на завершение лега
                player1._180 = savePoints.Peek().Player1_180; //  Восстанавливаем Игроку 1 180
                player2._180 = savePoints.Peek().Player2_180; //  Восстанавливаем Игроку 2 180
                infoPanelLogic.SetsSet(player1); //  Восстанавливаем в инфо-панели очки выигранных сетов Игрока 1
                infoPanelLogic.LegsSet(player1); //  Восстанавливаем в инфо-панели очки выигранных легов Игрока 1
                infoPanelLogic.PointsSet(player1); //  Восстанавливаем в инфо-панели очки на завершение лега Игрока 1
                infoPanelLogic.SetsSet(player2); //  Восстанавливаем в инфо-панели очки выигранных сетов Игрока 2
                infoPanelLogic.LegsSet(player2); //  Восстанавливаем в инфо-панели очки выигранных легов Игрока 2
                infoPanelLogic.PointsSet(player2); //  Восстанавливаем в инфо-панели очки на завершение лега Игрока 2
                playerOnThrow = savePoints.Peek().PlayerOnThrow; //  Восстанавливаем игрока на броске
                playerOnLeg = savePoints.Peek().PlayerOnLeg; //  Восстанавливаем игрока на начало лега
                infoPanelLogic.TextLogUndo(); //  Удаяем строку текстовой панели

                if (allMatchThrows.Peek().IsLegWon || allMatchThrows.Peek().IsFault || allMatchThrows.Peek().HandNumber == 3) //  Если последний бросок был переходным
                {
                    infoPanelLogic.TextLogUndo(); //  Удаяем строку текстовой панели
                    if (allMatchThrows.Peek().IsLegWon) // Если отменяемым броском выигран лег
                    {
                        infoPanelLogic.DotSet(playerOnThrow); //  Перемещаем точку начала лега
                        infoPanelLogic.TextLogUndo(); //  Удаяем строку текстовой панели
                    }

                    if (allMatchThrows.Peek().IsSetWon) // Если отменяемым броском выигран и сет
                    {
                        infoPanelLogic.TextLogUndo(); //  Удаяем строку текстовой панели
                    }

                    if (allMatchThrows.Peek().IsFault) //  Если отменяемый бросок был штрафным
                    {
                        infoPanelLogic.TextLogUndo(); //  Удаяем строку текстовой панели
                    }
                }

                SetPlayerOnThrow(playerOnThrow); //  Восстанавливаем игрока на подходе
                infoPanelLogic.TextLogUndo(); //  Удаяем строку текстовой панели
                playerOnThrow.throw1 = savePoints.Peek().FirstThrow; //  Восстанавливаем игроку на броске первый бросок
                playerOnThrow.throw2 = savePoints.Peek().SecondThrow; //  Восстанавливаем игроку на броске второй бросок
                playerOnThrow.throw3 = savePoints.Peek().ThirdThrow; //  Восстанавливаем игроку на броске третий бросок
                playerOnThrow.handPoints = savePoints.Peek().PlayerOnThrowHand; //  Восстанавливаем игроку на броске очки подхода
                infoPanelLogic.HelpCheck(playerOnThrow); //  Проверяем помощь

                allMatchThrows.Pop(); //  Удалаяем последний бросок из коллекции матча
                savePoints.Pop(); //  Удаляем последнюю точку сохранения

                infoPanelLogic.UndoThrowButtonOn(); //  Включаем кнопку отмены броска

                if (allMatchThrows.Count == 0) //  Если бросков для отмены больше нет
                {
                    infoPanelLogic.UndoThrowButtonOff(); //  Выключаем кнопку отмены броска
                }
            }
        }

        private void SavePoint()
        {
            savePoints.Push(new SavePoint(player1, player2, playerOnThrow, playerOnLeg));
        }

        private void IsLegIsOver() //  Или штраф, или правильное окончание лега, или это был последний бросок в подходе 
        {
            //  Проверяем окончен ли лег
            //  НЕПРАВИЛЬНОЕ окончание лега
            if (playerOnThrow.pointsToOut < 0 || playerOnThrow.pointsToOut == 1 || playerOnThrow.pointsToOut == 0 && allMatchThrows.Peek().Multiplier != "Double" && allMatchThrows.Peek().Multiplier != "Bull_Eye") //  Если игрок ушел в минус или оставил единицу, или закрыл лег не корректно (не через удвоение или Bulleye)
            {
                infoPanelLogic.TextLogAdd(new StringBuilder().Append("    > ").Append(playerOnThrow.Name).Append(" FAULT").ToString()); //  Пишем в текстовую панель
                playerOnThrow.pointsToOut += playerOnThrow.handPoints; //  Отменяем подход игрока
                if (playerOnThrow.handPoints == 180) //  Если в штрафном подходе было набрано 180 очков
                {
                    playerOnThrow._180 -= 1; //  Отменяем игроку 180
                }

                infoPanelLogic.PointsSet(playerOnThrow); //  Обновляем инфопанель
                ClearHands(); //  Очищаем броски
                TogglePlayerOnThrow(); //  Меняем игрока на броске
                allMatchThrows.Peek().IsFault = true; //  Помечаем бросок как штрафной
                //  Выходим
            }
            //  ПРАВИЛЬНОЕ окончание лега
            else if (playerOnThrow.pointsToOut == 0) //  Игрок правильно закрыл лег
            {
                infoPanelLogic.TextLogAdd(new StringBuilder().Append("Leg goes to ").Append(playerOnThrow.Name).ToString()); //  Пишем в текстовую панель
                playerOnThrow.legsWon += 1; //  Плюсуем выиграный лег
                infoPanelLogic.LegsSet(playerOnThrow); //  Обновляем инфопанель
                ClearHands(); //  Очищаем броски
                allMatchThrows.Peek().IsLegWon = true; //  Помечаем бросок как выигравший лег
                IsSetIsOver(); //  Проверяем не закончен ли сет
                if (IsOn) //  Если игра продолжается
                {
                    TogglePlayerOnLeg(); //  Смена игрока на начало лега 
                    player1.pointsToOut = pointsToGo; //  Обновляем очки нового лега игрока 1
                    player2.pointsToOut = pointsToGo; //  Обновляем очки нового лега игрока 2
                    infoPanelLogic.PointsClear(pointsToGo); //  Обновляем инфопанель
                    infoPanelLogic.HelpCheck(player1); //  Проверка помощи
                    infoPanelLogic.HelpCheck(player2); //  Проверка помощи
                }

                //  Выходим
            }
            //  Лег не окончен и это был последний бросок игрока в подходе
            else if (playerOnThrow.throw3 != null) //  Если это был последний бросок игрока
            {
                ClearHands(); //  Очищаем броски
                TogglePlayerOnThrow(); //  Меняем игрока на броске
            }
        }

        private void IsSetIsOver() //  Проверка закончен ли сет
        {
            if (playerOnThrow.legsWon == legsToGo) //  Если игрок выиграл требуемое количество легов для окончания сета
            {
                infoPanelLogic.TextLogAdd(new StringBuilder().Append("Set goes to ").Append(playerOnThrow.Name).ToString()); //  Пишем в текстовую панель
                playerOnThrow.setsWon += 1; //  Добавляем игроку выигранный сет
                infoPanelLogic.SetsSet(playerOnThrow); //  Обновляем инфопанель
                player1.legsWon = 0; //  Обнуляем леги игроков
                player2.legsWon = 0; //  Обнуляем леги игроков
                infoPanelLogic.LegsClear(); //  Обновляем инфопанель
                allMatchThrows.Peek().IsSetWon = true; //  Помечаем бросок как выигравший сет
                IsGameIsOver(); //  Проверяем не закончен ли матч
            }
        }

        private void IsGameIsOver() //  Проверка закончен ли матч
        {
            if (playerOnThrow.setsWon == setsToGo) //  Если игрок выиграл требуемое количество сетов для завершения матча
            {
                allMatchThrows.Peek().IsMatchWon = true; //  Помечаем бросок как выигравший матч
                EndGame(); //  Матч окончен
            }
        }
    }
}