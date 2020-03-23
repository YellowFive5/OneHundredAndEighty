#region Usings

using System.Collections.Generic;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.ScoreBoard;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public interface IGameProcessor
    {
        void OnThrow(DetectedThrow thrw,
                     List<Player> players,
                     Player playerOnThrow,
                     Game game,
                     ScoreBoardService scoreBoard,
                     DBService dbService);
    }
}