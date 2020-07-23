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
    }
}