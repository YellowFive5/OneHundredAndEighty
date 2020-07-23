#region Usings

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

#endregion

namespace OneHundredAndEightyCore.Windows.MessageBox
{
    public class MessageBoxService : IMessageBoxService, INotifyPropertyChanged
    {
        private bool QuestionResult { get; set; }

        private string messageText;

        public string MessageText
        {
            get => messageText;
            set
            {
                messageText = value;
                OnPropertyChanged(nameof(MessageText));
            }
        }

        public void SetQuestionResult(bool result)
        {
            QuestionResult = result;
        }

        public bool AskQuestion(string questionText)
        {
            var w = PrepareQuestionWindow();
            MessageText = questionText;
            w.ShowDialog();
            Application.Current.MainWindow.Opacity = 1;
            return QuestionResult;
        }

        private Window PrepareQuestionWindow()
        {
            var window = new QuestionMessageWindow(this)
                         {
                             Owner = Application.Current.MainWindow,
                             Width = Application.Current.MainWindow.Width,
                             Height = Application.Current.MainWindow.Height / 3
                         };
            Application.Current.MainWindow.Opacity = 0.6;
            return window;
        }

        public void ShowInfo(string infoText, params object[] args)
        {
            var w = PrepareInfoWindow();
            MessageText = string.Format(infoText, args);
            w.ShowDialog();
            Application.Current.MainWindow.Opacity = 1;
        }

        private Window PrepareInfoWindow()
        {
            var window = new MessageWindow(this)
                         {
                             Owner = Application.Current.MainWindow,
                             Width = Application.Current.MainWindow.Width,
                             Height = Application.Current.MainWindow.Height / 3
                         };
            Application.Current.MainWindow.Opacity = 0.6;
            return window;
        }

        public void ShowError(string errorText, params object[] args)
        {
            System.Windows.MessageBox.Show(string.Format(errorText, args), "Error", MessageBoxButton.OK);
        }

        public bool AskWarningQuestion(string questionText, params object[] args)
        {
            return System.Windows.MessageBox.Show(string.Format(questionText, args), "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        #region PropertyChangingFire

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}