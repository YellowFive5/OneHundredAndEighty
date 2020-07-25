#region Usings

using System.Windows;

#endregion

namespace OneHundredAndEightyCore.Windows.MessageBox
{
    public partial class MessageWindow
    {
        private readonly MessageBoxService messageBoxService;

        public MessageWindow(MessageBoxService messageBoxService)
        {
            this.messageBoxService = messageBoxService;
            InitializeComponent();
            DataContext = messageBoxService;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NoButtonClick(object sender, RoutedEventArgs e)
        {
            messageBoxService.SetQuestionResult(false);
            Close();
        }

        private void YesButtonClick(object sender, RoutedEventArgs e)
        {
            messageBoxService.SetQuestionResult(true);
            Close();
        }

        private void CopyToClipboardButtonClick(object sender, RoutedEventArgs e)
        {
            messageBoxService.CopyToClipboard();
        }
    }
}