#region Usings

using System;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Enums;
using OneHundredAndEightyCore.Windows.Main.Tabs.Shared;

#endregion

namespace OneHundredAndEightyCore.Windows.Main.Tabs.Settings
{
    public class StartCamSetupCapturingCommand : CommandBase
    {
        public StartCamSetupCapturingCommand(Action<CamNumber> execute)
            : base(execute)
        {
        }

        public override void Execute(object parameter)
        {
            executeWithCamNumberParameter.Invoke(Converter.GridNameToCamNumber(parameter.ToString()));
        }
    }
}