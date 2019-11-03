#region Usings

using System;
using System.Windows;
using System.Windows.Media.Animation;

#endregion

namespace OneHundredAndEighty
{
    public class SettingsPanelLogic //  Класс логики панели настроек
    {
        private readonly MainWindow mainWindow = (MainWindow) Application.Current.MainWindow; //  Ссылка на главное окно для доступа к элементам
        private readonly TimeSpan panelFadeTime = TimeSpan.FromSeconds(0.5); //  Время анимации фейда панели

        public void PanelShow()
        {
            mainWindow.SettingsPanel.Visibility = Visibility.Visible;
            var animation = new DoubleAnimation(0, 1, panelFadeTime);
            mainWindow.SettingsPanel.BeginAnimation(UIElement.OpacityProperty, animation);
        } //  Показать панель настроек

        public void PanelHide()
        {
            var animation = new DoubleAnimation(1, 0, panelFadeTime);
            mainWindow.InfoPanel.BeginAnimation(UIElement.OpacityProperty, animation);
            mainWindow.SettingsPanel.Visibility = Visibility.Hidden;
        } //  Спрятать панель настроек

        public Player WhoThrowFirst(Player p1, Player p2)
        {
            if (mainWindow.Player1Radiobutton.IsChecked == true)
            {
                return p1;
            }
            else
            {
                return p2;
            }
        } //  Кто бросает первым

        public int PointsToGo()
        {
            return int.Parse(mainWindow.PointsBox.Text);
        } //  Сколько очков в леге

        public int LegsToGo()
        {
            return int.Parse(mainWindow.LegBox.Text);
        } //  Сколько играем легов в сете

        public int SetsToGo()
        {
            return int.Parse(mainWindow.SetBox.Text);
        } //  Сколько легов в метче

        public string Player1Name()
        {
            return mainWindow.Player1NameCombobox.Text;
        } //  Имя 1 игрока

        public string Player2Name()
        {
            return mainWindow.Player2NameCombobox.Text;
        } //  Имя 2 игрока
    }
}