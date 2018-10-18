using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OneHundredAndEighty.Windows
{
    /// <summary>
    /// Логика взаимодействия для ExitWindow.xaml
    /// </summary>
    public partial class ExitWindow : Window
    {
        public bool result { get; private set; }    //  Результат выбора

        public ExitWindow()
        {
            InitializeComponent();
        }

        private void StayButton_Click(object sender, RoutedEventArgs e) //  Остаёмся в приложении
        {
            this.result = true;
            this.Close();
        }

        private void LeaveButton_Click(object sender, RoutedEventArgs e)    //  Выходим из приложения
        {
            this.result = false;
            this.Close();
        }
    }
}
