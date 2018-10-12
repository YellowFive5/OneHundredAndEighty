using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHundredAndEighty
{
    class BoardPanelLogic    //  Класс логики панели секторов
    {
        MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам
        public void PanelShow()
        {
            MainWindow.BoardPanel.Visibility = System.Windows.Visibility.Visible;
        }   //  Показать панель секторов 
        public void PanelHide()
        {
            MainWindow.BoardPanel.Visibility = System.Windows.Visibility.Hidden;
        }   //  Спрятать панель секторов

    }
}
