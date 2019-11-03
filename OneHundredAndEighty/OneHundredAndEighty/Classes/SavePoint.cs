namespace OneHundredAndEighty
{
    internal class SavePoint //  Точка сохранения 
    {
        //  Счет
        public int Player1LegsWon { get; } //  Количество выигранных легов Игрока 1
        public int Player1SetsWon { get; } //  Количество выигранных сетов Игрока 1
        public int Player1PointsToOut { get; } //  Количество очков на закрытие лега Игрока 1
        public int Player2LegsWon { get; } //  Количество выигранных легов Игрока 2
        public int Player2SetsWon { get; } //  Количество выигранных сетов Игрока 2
        public int Player2PointsToOut { get; } //  Количество очков на закрытие лега Игрока 2
        public int Player1_180 { get; } //  Количество 180 Игрока 1
        public int Player2_180 { get; } //  Количество 180 Игрока 1
        public Player PlayerOnThrow { get; } //  Игрок на подходе
        public Player PlayerOnLeg { get; } //  Игрок на начало лега
        public Throw FirstThrow { get; } //  Первый бросок игрока на подходе
        public Throw SecondThrow { get; } //  Второй бросок игрока на подходе
        public Throw ThirdThrow { get; } //  Третий бросок игрока на подходе
        public int PlayerOnThrowHand { get; } //  Очки подхода игрока на подходе
        public bool Player1Is3Bull { get; } //  Спец-ачивки игроков
        public bool Player2Is3Bull { get; }
        public bool Player1IsmrZ { get; }
        public bool Player2IsmrZ { get; }

        public SavePoint(Player player1, Player player2, Player playerOnThrow, Player playerOnLeg) //  Конструктор точки сохранения
        {
            Player1LegsWon = player1.legsWon;
            Player2LegsWon = player2.legsWon;
            Player1SetsWon = player1.setsWon;
            Player2SetsWon = player2.setsWon;
            Player1PointsToOut = player1.pointsToOut;
            Player2PointsToOut = player2.pointsToOut;
            Player1_180 = player1._180;
            Player2_180 = player2._180;
            this.PlayerOnThrow = playerOnThrow;
            this.PlayerOnLeg = playerOnLeg;
            FirstThrow = playerOnThrow.throw1;
            SecondThrow = playerOnThrow.throw2;
            ThirdThrow = playerOnThrow.throw3;
            PlayerOnThrowHand = playerOnThrow.handPoints;
            Player1Is3Bull = player1.is3Bull;
            Player2Is3Bull = player2.is3Bull;
            Player1IsmrZ = player1.ismrZ;
            Player2IsmrZ = player2.ismrZ;
        }
    }
}