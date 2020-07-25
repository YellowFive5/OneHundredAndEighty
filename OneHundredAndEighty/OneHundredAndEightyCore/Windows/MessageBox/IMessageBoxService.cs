namespace OneHundredAndEightyCore.Windows.MessageBox
{
    public interface IMessageBoxService
    {
        void ShowInfo(string infoText, params object[] args);
        bool AskInfoQuestion(string questionText, params object[] args);
        void ShowWarning(string warningText, params object[] args);
        bool AskWarningQuestion(string questionText, params object[] args);
        void ShowError(string errorText, params object[] args);
        bool AskErrorQuestion(string questionText, params object[] args);
    }
}