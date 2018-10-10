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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OneHundredAndEighty
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PointsShow(object sender, MouseEventArgs e)
        {
            Shape O = sender as Shape;
            Points.Content = O.Tag.ToString();
        }

        private void GameOn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(PointsBox.Text + " " + SetBox.Text + " " + LegBox.Text);
        }
    }
}
