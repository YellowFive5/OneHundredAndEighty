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
