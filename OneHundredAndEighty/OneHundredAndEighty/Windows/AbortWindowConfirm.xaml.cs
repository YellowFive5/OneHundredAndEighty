using System.Windows;

namespace OneHundredAndEighty.Windows
{
    /// <summary>
    /// Логика взаимодействия для AbortWindowConfirm.xaml
    /// </summary>
    public partial class AbortWindowConfirm : Window
    {
        public bool result { get; private set; }    //  Результат выбора
        public AbortWindowConfirm()
        {
            InitializeComponent();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            this.result = false;
            this.Close();
        }

        private void AbortButton_Click(object sender, RoutedEventArgs e)
        {
            this.result = true;
            this.Close();
        }
    }
}
