#region Usings

using System;
using OneHundredAndEightyCore.Windows.Shared;

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