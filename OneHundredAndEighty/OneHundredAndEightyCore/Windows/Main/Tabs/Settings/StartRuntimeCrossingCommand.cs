#region Usings

using System;
using OneHundredAndEightyCore.Windows.Shared;

#endregion

namespace OneHundredAndEightyCore.Windows.Main.Tabs.Settings
{
    public class StartRuntimeCrossingCommand : CommandBase
    {
        public StartRuntimeCrossingCommand(Action execute)
            : base(execute)
        {
        }
    }
}