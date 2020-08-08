#region Usings

using System;
using OneHundredAndEightyCore.Windows.Main.Tabs.Shared;

#endregion

namespace OneHundredAndEightyCore.Windows.Main.Tabs.Player
{
    public class SaveNewPlayerCommand : CommandBase
    {
        public SaveNewPlayerCommand(Action execute)
            : base(execute)
        {
        }
    }
}