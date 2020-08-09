#region Usings

using System;
using OneHundredAndEightyCore.Windows.Shared;

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public class CorrectThrowCommand : CommandBase
    {
        public CorrectThrowCommand(Action execute) : base(execute)
        {
        }
    }
}