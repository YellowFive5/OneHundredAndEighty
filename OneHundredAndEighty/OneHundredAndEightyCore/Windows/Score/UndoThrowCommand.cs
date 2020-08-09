#region Usings

using System;
using OneHundredAndEightyCore.Windows.Shared;

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public class UndoThrowCommand : CommandBase
    {
        public UndoThrowCommand(Action execute)
            : base(execute)
        {
        }
    }
}