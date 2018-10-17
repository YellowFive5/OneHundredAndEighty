using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public void FadeIn()    //  Затухание главного окна
        {
            this.Opacity = 0.4;
        }
        public void FadeOut()   //  Появление главного окна
        {
            this.Opacity = 1;
        }

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
            G.BoardPanelLogic.PanelHide();    //  Скрываем панель секторов
            G.NextThrow(new Throw(sender));
            if (G.IsOn)   //  Если игра продолжается
                G.BoardPanelLogic.PanelShow();    //  Показываем панель секторов и бросаем дальше
        }
        private void EndMatchButton_Click(object sender, RoutedEventArgs e) //  Кнопка отмены матча
        {
            this.FadeIn();
            G.AbortGame();
            this.FadeOut();
        }
        private void MainWindow_Closing(object sender, CancelEventArgs e)   //  Выход из приложения
        {
            this.FadeIn();
            Windows.ExitWindow window = new Windows.ExitWindow();
            window.Owner = this;
            window.ShowDialog();
            if (window.result)
                e.Cancel = true;
            this.FadeOut();
        }

        private void UndoThrow_Click(object sender, RoutedEventArgs e)  //  Кнопка отмены броска
        {
            //UndoThrow.IsEnabled = false;
            G.UndoThrow();
            //UndoThrow.IsEnabled = true;
        }
    }
}
