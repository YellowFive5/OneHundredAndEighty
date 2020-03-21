#region Usings

using System.Collections.Generic;
using OneHundredAndEightyCore.ScoreBoard;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public interface IGameProcessor
    {
        void OnThrow(int thrwTotalPoints, List<Player> players, Player playerOnThrow, ScoreBoardService scoreBoard);
    }
}