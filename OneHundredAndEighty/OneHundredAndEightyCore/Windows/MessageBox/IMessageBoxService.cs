namespace OneHundredAndEightyCore.Windows.MessageBox
{
    public interface IMessageBoxService
    {
        void ShowInfo(string infoText, params object[] args);
        bool AskQuestion(string questionText);
        void ShowError(string errorText, params object[] args);
        bool AskWarningQuestion(string questionText, params object[] args);
    }
}