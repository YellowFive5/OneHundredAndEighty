using System.Windows;

namespace OneHundredAndEighty.Windows
{
    /// <summary>
    /// Логика взаимодействия для StatisticWindow.xaml
    /// </summary>
    public partial class StatisticWindow
    {
        public StatisticWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}