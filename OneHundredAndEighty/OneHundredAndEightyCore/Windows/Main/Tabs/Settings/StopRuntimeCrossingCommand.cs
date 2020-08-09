#region Usings

using System;
using OneHundredAndEightyCore.Windows.Shared;

#endregion

namespace OneHundredAndEightyCore.Windows.Main.Tabs.Settings
{
    public class StopRuntimeCrossingCommand : CommandBase
    {
        public StopRuntimeCrossingCommand(Action execute)
            : base(execute)
        {
        }
    }
}