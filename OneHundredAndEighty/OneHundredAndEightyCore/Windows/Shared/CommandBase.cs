#region Usings

using System;
using System.Windows.Input;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Windows.Shared
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly Action execute;
        protected readonly Action<Uri> executeWithUriParameter;
        protected readonly Action<CamNumber> executeWithCamNumberParameter;
        protected readonly Action<string> executeWithStringParameter;

        protected CommandBase(Action execute)
        {
            this.execute = execute;
        }

        protected CommandBase(Action<Uri> executeWithUriParameter)
        {
            this.executeWithUriParameter = executeWithUriParameter;
        }

        protected CommandBase(Action<CamNumber> executeWithCamNumberParameter)
        {
            this.executeWithCamNumberParameter = executeWithCamNumberParameter;
        }

        protected CommandBase(Action<string> executeWithStringParameter)
        {
            this.executeWithStringParameter = executeWithStringParameter;
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
            execute.Invoke();
        }
    }
}