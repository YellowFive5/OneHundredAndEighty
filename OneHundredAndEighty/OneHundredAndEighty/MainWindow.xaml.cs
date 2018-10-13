using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        Game G = null;

        public MainWindow()
        {
            G = new Game();
            InitializeComponent();
        }
        private void PointsShow(object sender, MouseEventArgs e)    //  Показ очков сектора
        {
            Shape O = sender as Shape;
            HowManyPoints.Content = O.Tag.ToString();
        }
        private void GameOn_Click(object sender, RoutedEventArgs e) //  Кнопка GAMEON
        {
            G.StartGame();
        }
        private void Throw(object sender, RoutedEventArgs e)    //  Бросок
        {
            G.NextThrow(new Throw(sender));
        }

    }
}
