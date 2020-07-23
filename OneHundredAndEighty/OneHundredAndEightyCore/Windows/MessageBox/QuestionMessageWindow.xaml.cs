#region Usings

using System.Windows;

#endregion

namespace OneHundredAndEightyCore.Windows.MessageBox
{
    public partial class QuestionMessageWindow
    {
        private readonly MessageBoxService messageBoxService;

        public QuestionMessageWindow(MessageBoxService messageBoxService)
        {
            this.messageBoxService = messageBoxService;
            InitializeComponent();
            DataContext = messageBoxService;
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
    }
}