#region Usings

using System;

#endregion

namespace OneHundredAndEightyCore
{
    public enum GameType
    {
        FreeThrows = 1,
        Classic1001,
        Classic701,
        Classic501,
        Classic301,
        Classic101,
    }

    public enum GameResultType
    {
        NotDefined = 1,
        Win,
        Loose
    }

    public class Game
    {
        public int Id { get; private set; }
        public GameType Type { get; }
        public DateTime StartTimeStamp { get; }
        public DateTime EndTimeStamp { get; set; }

        public Game(GameType type, int id = -1)
        {
            Id = id;
            Type = type;
            StartTimeStamp = DateTime.Now;
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public void SetEndTimeStamp()
        {
            EndTimeStamp = DateTime.Now;
        }
    }
}