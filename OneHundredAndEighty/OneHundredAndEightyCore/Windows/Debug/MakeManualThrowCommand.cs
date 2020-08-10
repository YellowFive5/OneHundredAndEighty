#region Usings

using System;
using OneHundredAndEightyCore.Windows.Shared;

#endregion

namespace OneHundredAndEightyCore.Windows.Debug
{
    public class MakeManualThrowCommand : CommandBase
    {
        public MakeManualThrowCommand(Action<string> executeWithStringParameter)
            : base(executeWithStringParameter)
        {
        }

        public override void Execute(object parameter)
        {
            executeWithStringParameter.Invoke(parameter.ToString());
        }
    }
}