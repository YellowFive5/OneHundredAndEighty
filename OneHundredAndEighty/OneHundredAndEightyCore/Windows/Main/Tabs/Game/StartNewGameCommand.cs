#region Usings

using System;
using OneHundredAndEightyCore.Windows.Main.Tabs.Shared;

#endregion

namespace OneHundredAndEightyCore.Windows.Main.Tabs.Game
{
    public class StartNewGameCommand : CommandBase
    {
        public StartNewGameCommand(Action execute)
            : base(execute)
        {
        }
    }
}