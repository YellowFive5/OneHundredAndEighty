using System.Windows;

namespace OneHundredAndEighty.Windows
{
    /// <summary>
    /// Логика взаимодействия для AbortWindowConfirm.xaml
    /// </summary>
    public partial class AbortWindowConfirm
    {
        public bool Result { get; private set; } //  Результат выбора

        public AbortWindowConfirm()
        {
            InitializeComponent();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }

        private void AbortButton_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            Close();
        }
    }
}