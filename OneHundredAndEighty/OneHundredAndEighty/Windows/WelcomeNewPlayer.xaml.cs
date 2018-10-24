using System.Windows;

namespace OneHundredAndEighty.Windows
{
    /// <summary>
    /// Логика взаимодействия для WelcomeNewPlayer.xaml
    /// </summary>
    public partial class WelcomeNewPlayer : Window
    {
        public WelcomeNewPlayer()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
