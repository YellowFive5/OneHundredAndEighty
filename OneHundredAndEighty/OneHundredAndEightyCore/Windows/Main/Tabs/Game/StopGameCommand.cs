#region Usings

using System;
using OneHundredAndEightyCore.Windows.Main.Tabs.Shared;

#endregion

namespace OneHundredAndEightyCore.Windows.Main.Tabs.Game
{
    public class StopGameCommand : CommandBase
    {
        public StopGameCommand(Action execute)
            : base(execute)
        {
        }
    }
}