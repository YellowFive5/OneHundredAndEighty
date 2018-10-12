using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHundredAndEighty
{
    class SettingsPanelLogic    //  Класс логики панели настроек
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
        public string WhoThrowFirst()
        {
            if (MainWindow.Player1Radiobutton.IsChecked == true)
                return "1";
            else
                return "2";
        }   //  Кто бросает первым

    }
}
