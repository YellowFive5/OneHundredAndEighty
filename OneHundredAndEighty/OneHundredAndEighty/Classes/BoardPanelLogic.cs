namespace OneHundredAndEighty
{
    public static class BoardPanelLogic    //  Класс логики панели секторов
    {
        static MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам

        public static void PanelShow()
        {
            //MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам
            MainWindow.BoardPanel.Visibility = System.Windows.Visibility.Visible;
        }   //  Показать панель секторов 
        public static void PanelHide()
        {
            //MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам
            MainWindow.BoardPanel.Visibility = System.Windows.Visibility.Hidden;
        }   //  Спрятать панель секторов
    }
}
