using System.Windows;

namespace OneHundredAndEighty.Windows
{
    /// <summary>
    /// Логика взаимодействия для AbortWindow.xaml
    /// </summary>
    public partial class AbortWindow
    {
        public AbortWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}