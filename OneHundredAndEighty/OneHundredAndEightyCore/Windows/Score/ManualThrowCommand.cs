#region Usings

using System;
using OneHundredAndEightyCore.Windows.Shared;

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public class ManualThrowCommand : CommandBase
    {
        public ManualThrowCommand(Action execute) : base(execute)
        {
        }
    }
}