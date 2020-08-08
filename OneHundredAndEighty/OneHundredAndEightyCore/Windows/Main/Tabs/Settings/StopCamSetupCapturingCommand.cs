#region Usings

using System;
using OneHundredAndEightyCore.Windows.Main.Tabs.Shared;

#endregion

namespace OneHundredAndEightyCore.Windows.Main.Tabs.Settings
{
    public class StopCamSetupCapturingCommand : CommandBase
    {
        public StopCamSetupCapturingCommand(Action execute)
            : base(execute)
        {
        }
    }
}