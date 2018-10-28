using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace OneHundredAndEighty
{
    public class SettingsPanelLogic    //  Класс логики панели настроек
    {
        MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам
        TimeSpan PanelFadeTime = TimeSpan.FromSeconds(0.5); //  Время анимации фейда панели

        public void PanelShow()
        {
            MainWindow.SettingsPanel.Visibility = System.Windows.Visibility.Visible;
            DoubleAnimation animation = new DoubleAnimation(0, 1, PanelFadeTime);
            MainWindow.SettingsPanel.BeginAnimation(UIElement.OpacityProperty, animation);
        }   //  Показать панель настроек
        public void PanelHide()
        {
            DoubleAnimation animation = new DoubleAnimation(1, 0, PanelFadeTime);
            MainWindow.InfoPanel.BeginAnimation(UIElement.OpacityProperty, animation);
            MainWindow.SettingsPanel.Visibility = System.Windows.Visibility.Hidden;
        }   //  Спрятать панель настроек
        public Player WhoThrowFirst(Player p1,Player p2)
        {
            if (MainWindow.Player1Radiobutton.IsChecked == true)
                return p1;
            else
                return p2;
        }   //  Кто бросает первым
        public int PointsToGo()
        {
            return Int32.Parse(MainWindow.PointsBox.Text);
        }   //  Сколько очков в леге
        public int LegsToGo()
        {
            return Int32.Parse(MainWindow.LegBox.Text);
        }   //  Сколько играем легов в сете
        public int SetsToGo()
        {
            return Int32.Parse(MainWindow.SetBox.Text);
        }   //  Сколько легов в метче
        public string Player1Name()
        {
                return MainWindow.Player1NameCombobox.Text;
        }   //  Имя 1 игрока
        public string Player2Name()
        {
                return MainWindow.Player2NameCombobox.Text;
        }   //  Имя 2 игрока
    }
}
