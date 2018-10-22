using System.Collections.Generic;
using System.Windows.Controls;

namespace OneHundredAndEighty
{
    public class Player //  Класс игрока
    {
        public string Name { get; private set; }    //  Имя игрока
        public string Tag { get; private set; } //  Тэг
        //Матч
        public int SetsWon; //  Количество выигранных сетов
        public int LegsWon; //  Количество выигранных легов в сете
        public int PointsToOut; //  Количество очков на завершение лега
        public Stack<Throw> AllPlayerThrows = new Stack<Throw>();   //  Коллекция бросков игрока в матче
        public int HandPoints;  //  Набранное количестов очков в подходе
        public int _180;    //  Количество 180 в матче
        public Throw Throw1 = null; //  Первый бросок
        public Throw Throw2 = null; //  Второй бросок
        public Throw Throw3 = null; //  Третий бросок
        public void ClearHand() //  Обнуление очередного подхода
        {
            this.HandPoints = 0;
            this.Throw1 = null;
            this.Throw2 = null;
            this.Throw3 = null;
        }
        //  Инфо-панель
        public Canvas HelpPanel;    //  Панель помощи
        public Label HelpLabel; //  Лейбл помощи
        public Label SetsWonLabel;  //  Лейбл выиграных сетов
        public Label LegsWonLabel;  //  Лейбл выиграных легов
        public Label PointsLabel;   //  Лейбл набраных очнов

        public Player(string Tag, string Name, Canvas HelpPanel, Label HelpLabel, Label SetsWonLabel, Label LegsWonLabel, Label PointsLabel, int PointsToOut) //  Конструктор нового игрока
        {
            this.Tag = Tag;
            this.Name = Name;
            this.HelpPanel = HelpPanel;
            this.HelpLabel = HelpLabel;
            this.SetsWonLabel = SetsWonLabel;
            this.LegsWonLabel = LegsWonLabel;
            this.PointsLabel = PointsLabel;
            this.PointsToOut = PointsToOut;
        }
    }
}
