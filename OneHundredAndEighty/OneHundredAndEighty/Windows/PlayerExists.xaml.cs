using System.Windows;

namespace OneHundredAndEighty.Windows
{
    /// <summary>
    /// Логика взаимодействия для PlayerExists.xaml
    /// </summary>
    public partial class PlayerExists
    {
        public PlayerExists()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e) //  Кнопка ОК
        {
            Close();
        }
    }
}