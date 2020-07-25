#region Usings

using System;
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

        public void CopyToClipboard()
        {
            Clipboard.SetText(MessageText);
        }

        public void ShowInfo(string infoText, params object[] args)
        {
            var window = PrepareWindow(MessageType.InfoOk);
            MessageText = string.Format(infoText, args);
            window.ShowDialog();
            Application.Current.MainWindow.Opacity = 1;
        }

        public bool AskInfoQuestion(string questionText, params object[] args)
        {
            var window = PrepareWindow(MessageType.InfoQuestion);
            MessageText = string.Format(questionText, args);
            window.ShowDialog();
            Application.Current.MainWindow.Opacity = 1;
            return QuestionResult;
        }

        public void ShowWarning(string warningText, params object[] args)
        {
            var window = PrepareWindow(MessageType.WarningOk);
            MessageText = string.Format(warningText, args);
            window.ShowDialog();
            Application.Current.MainWindow.Opacity = 1;
        }

        public bool AskWarningQuestion(string questionText, params object[] args)
        {
            var window = PrepareWindow(MessageType.WarningQuestion);
            MessageText = string.Format(questionText, args);
            window.ShowDialog();
            Application.Current.MainWindow.Opacity = 1;
            return QuestionResult;
        }

        public void ShowError(string errorText, params object[] args)
        {
            var window = PrepareWindow(MessageType.ErrorOk);
            MessageText = string.Format(errorText, args);
            window.ShowDialog();
            Application.Current.MainWindow.Opacity = 1;
        }

        public bool AskErrorQuestion(string questionText, params object[] args)
        {
            var window = PrepareWindow(MessageType.ErrorQuestion);
            MessageText = string.Format(questionText, args);
            window.ShowDialog();
            Application.Current.MainWindow.Opacity = 1;
            return QuestionResult;
        }

        private Window PrepareWindow(MessageType type)
        {
            var window = new MessageWindow(this)
                         {
                             Owner = Application.Current.MainWindow,
                             Width = Application.Current.MainWindow.Width,
                             Height = Application.Current.MainWindow.Height / 3
                         };
            switch (type)
            {
                case MessageType.InfoOk:
                    window.InfoOkGrid.Visibility = Visibility.Visible;
                    break;
                case MessageType.InfoQuestion:
                    window.InfoQuestionGrid.Visibility = Visibility.Visible;
                    break;
                case MessageType.WarningOk:
                    window.WarningOkGrid.Visibility = Visibility.Visible;
                    break;
                case MessageType.WarningQuestion:
                    window.WarningQuestionGrid.Visibility = Visibility.Visible;
                    break;
                case MessageType.ErrorOk:
                    window.ErrorOkGrid.Visibility = Visibility.Visible;
                    break;
                case MessageType.ErrorQuestion:
                    window.ErrorQuestionGrid.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            Application.Current.MainWindow.Opacity = 0.6;
            return window;
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