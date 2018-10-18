using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHundredAndEighty
{
    class SavePoint //  Точка сохранения 
    {
        //  Счет
        public int Player1LegsWon { get; private set; } //  Количество выигранных легов Игрока 1
        public int Player1SetsWon { get; private set; } //  Количество выигранных сетов Игрока 1
        public int Player1PointsToOut { get; private set; } //  Количество очков на закрытие лега Игрока 1
        public int Player2LegsWon{ get; private set; }  //  Количество выигранных легов Игрока 2
        public int Player2SetsWon { get; private set; } //  Количество выигранных сетов Игрока 2
        public int Player2PointsToOut { get; private set; } //  Количество очков на закрытие лега Игрока 2
        public Player PlayerOnThrow { get; private set; }   //  Игрок на подходе
        public Player PlayerOnLeg { get; private set; } //  Игрок на начало лега
        public Throw FirstThrow { get; private set; }   //  Первый бросок игрока на подходе
        public Throw SecondThrow { get; private set; }  //  Второй бросок игрока на подходе
        public Throw ThirdThrow { get; private set; }   //  Третий бросок игрока на подходе
        public int PlayerOnThrowHand { get; private set; }  //  Очки подхода игрока на подходе

        public SavePoint(Player Player1,Player Player2,Player PlayerOnThrow,Player PlayerOnLeg) //  Конструктор точки сохранения
        {
            this.Player1LegsWon = Player1.LegsWon;
            this.Player2LegsWon = Player2.LegsWon;
            this.Player1SetsWon = Player1.SetsWon;
            this.Player2SetsWon = Player2.SetsWon;
            this.Player1PointsToOut = Player1.PointsToOut;
            this.Player2PointsToOut = Player2.PointsToOut;
            this.PlayerOnThrow = PlayerOnThrow;
            this.PlayerOnLeg = PlayerOnLeg;
            this.FirstThrow = PlayerOnThrow.Throw1;
            this.SecondThrow = PlayerOnThrow.Throw2;
            this.ThirdThrow = PlayerOnThrow.Throw3;
            this.PlayerOnThrowHand = PlayerOnThrow.HandPoints;
        }
    }
}
