using System.Windows;
using System.Windows.Input;

namespace OneHundredAndEighty.Windows
{
    /// <summary>
    /// Логика взаимодействия для WinnerWindow.xaml
    /// </summary>
    public partial class WinnerWindow
    {
        public bool StatsShow { get; private set; } //  Показ статистики

        public WinnerWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowStatsButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StatsShow = true;
            Close();
        }
    }
}