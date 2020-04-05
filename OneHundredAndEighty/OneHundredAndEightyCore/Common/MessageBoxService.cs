#region Usings

using System.Windows;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public class MessageBoxService
    {
        public void ShowError(string errorText, params object[] args)
        {
            MessageBox.Show(string.Format(errorText, args), "Error", MessageBoxButton.OK);
        }

        public void ShowInfo(string infoText, params object[] args)
        {
            MessageBox.Show(string.Format(infoText, args), "Info", MessageBoxButton.OK);
        }

        public bool AskWarningQuestion(string questionText, params object[] args)
        {
            return MessageBox.Show(string.Format(questionText, args), "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }
    }
}