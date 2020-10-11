#region Usings

using System;
using OneHundredAndEightyCore.Windows.Shared;

#endregion

namespace OneHundredAndEightyCore.Windows.Main.Tabs.Player
{
    public class PlayerStatisticsLoadCommand : CommandBase
    {
        public PlayerStatisticsLoadCommand(Action execute)
            : base(execute)
        {
        }
    }
}