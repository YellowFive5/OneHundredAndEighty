namespace OneHundredAndEighty
{
    public static class BoardPanelLogic //  Класс логики панели секторов
    {
        private static readonly MainWindow MainWindow = (MainWindow) System.Windows.Application.Current.MainWindow;

        public static void PanelShow()
        {
            MainWindow.BoardPanel.Visibility = System.Windows.Visibility.Visible;
        }

        public static void PanelHide()
        {
            MainWindow.BoardPanel.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}