using System.Windows;

namespace OneHundredAndEighty.Windows
{
    /// <summary>
    /// Логика взаимодействия для WelcomeNewPlayer.xaml
    /// </summary>
    public partial class WelcomeNewPlayer
    {
        public WelcomeNewPlayer()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}