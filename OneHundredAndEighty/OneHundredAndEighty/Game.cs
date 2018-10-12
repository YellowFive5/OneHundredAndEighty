using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OneHundredAndEighty
{
    class Game
    {
        MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам
        InfoPanelLogic InfoPanelLogic = new InfoPanelLogic();
        SettingsPanelLogic SettingsPanelLogic = new SettingsPanelLogic();
        BoardPanelLogic BoardPanelLogic = new BoardPanelLogic();

        Player Player1 = null;
        Player Player2 = null;
        int LegsToGo;
        int SetsToGo;
        int PointsToGo;
        string WhoThrowFirst;

        public Game()
        {
            Player1 = new Player(MainWindow.Player1NameBox.Text);
            Player2 = new Player(MainWindow.Player2NameBox.Text);

            LegsToGo = Int32.Parse(MainWindow.LegBox.Text);
            SetsToGo = Int32.Parse(MainWindow.SetBox.Text);
            PointsToGo = Int32.Parse(MainWindow.PointsBox.Text);
            WhoThrowFirst = SettingsPanelLogic.WhoThrowFirst();

            SettingsPanelLogic.PanelHide();

            InfoPanelLogic.PanelNewGame(PointsToGo, LegsToGo.ToString(), SetsToGo.ToString(), WhoThrowFirst);
            InfoPanelLogic.ShowPanel();

            BoardPanelLogic.PanelShow();

        }
        public void NewGame()
        {
        }
    }
}
