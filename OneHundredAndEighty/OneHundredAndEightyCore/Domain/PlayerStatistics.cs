#region Usings

using System.Collections.Generic;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Domain
{
    public class PlayerStatistics
    {
        public List<Achieve> AchievesObtained;

        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int GamesLoose { get; set; }

        public int FreeThrowsSingleGamesPlayed { get; set; }

        public int FreeThrowsDoubleGamesPlayed { get; set; }
        public int FreeThrowsDoubleGamesWon { get; set; }

        public int ClassicGamesPlayed { get; set; }
        public int ClassicGamesGamesWon { get; set; }

        public int LegsPlayed { get; set; }
        public int LegsWon { get; set; }
        public int SetsPlayed { get; set; }
        public int SetsWon { get; set; }
    }
}