#region Usings

using System;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Domain
{
    public class Game
    {
        public int Id { get; private set; }
        public GameType Type { get; }
        public DateTime StartTimeStamp { get; }

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
    }
}