#region Usings

using System;

#endregion

namespace OneHundredAndEightyCore.Windows.Main.Tabs.Shared
{
    public class HyperLinkNavigateCommand : CommandBase
    {
        public HyperLinkNavigateCommand(Action<Uri> execute) :
            base(execute)
        {
        }

        public override void Execute(object parameter)
        {
            executeWithUriParameter.Invoke(new Uri(parameter.ToString()));
        }
    }
}