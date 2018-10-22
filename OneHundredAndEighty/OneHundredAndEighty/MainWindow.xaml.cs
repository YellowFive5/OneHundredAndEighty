using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace OneHundredAndEighty
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Game G = null;

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
            DBwork.LoadPlayers();
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
            Windows.AbortWindowConfirm window = new Windows.AbortWindowConfirm();
            window.Owner = this;
            window.ShowDialog();
            if (window.result)
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
            G.UndoThrow();
        }
        private void NewPlayer_Click(object sender, RoutedEventArgs e)  //  Кнопка нового игрока
        {
            this.FadeIn();
            NewPlayer.ShowNewPlayerRegisterWindow();
            this.FadeOut();
        }
    }
}
