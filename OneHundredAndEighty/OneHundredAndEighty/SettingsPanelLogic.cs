using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHundredAndEighty
{
    public class SettingsPanelLogic    //  Класс логики панели настроек
    {
        MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам
        public void PanelShow()
        {
            MainWindow.SettingsPanel.Visibility = System.Windows.Visibility.Visible;
        }   //  Показать панель настроек
        public void PanelHide()
        {
            MainWindow.SettingsPanel.Visibility = System.Windows.Visibility.Hidden;
        }   //  Спрятать панель настроек
        public Player WhoThrowFirst(Player p1,Player p2)
        {
            if (MainWindow.Player1Radiobutton.IsChecked == true)
                return p1;
            else
                return p2;
        }   //  Кто бросает первым
        public int LegsToGo()
        {
            return Int32.Parse(MainWindow.LegBox.Text);
        }
        public int SetsToGo()
        {
            return Int32.Parse(MainWindow.SetBox.Text);
        }
        public int PointsToGo()
        {
            return Int32.Parse(MainWindow.PointsBox.Text);
        }
        public string PlayerName(string s)
        {
            if (s == "Player1")
                return MainWindow.Player1NameBox.Text;
            else
                return MainWindow.Player2NameBox.Text;
        }

    }
}
