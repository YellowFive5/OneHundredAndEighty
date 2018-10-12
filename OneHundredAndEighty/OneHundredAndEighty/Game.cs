using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OneHundredAndEighty
{
    public class Game
    {
        MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам
        public InfoPanelLogic InfoPanelLogic = new InfoPanelLogic();
        public SettingsPanelLogic SettingsPanelLogic = new SettingsPanelLogic();
        public BoardPanelLogic BoardPanelLogic = new BoardPanelLogic();

        Player Player1 = null;
        Player Player2 = null;
        Player WhoThrow = null;
        Player WhosSet = null;
        int LegsToGo;
        int SetsToGo;
        int PointsToGo;

        public void StartGame()
        {
            SettingsPanelLogic.PanelHide();
            InfoPanelLogic.PanelShow();
            BoardPanelLogic.PanelShow();
            LegsToGo = SettingsPanelLogic.LegsToGo();
            SetsToGo = SettingsPanelLogic.SetsToGo();
            PointsToGo = SettingsPanelLogic.PointsToGo();
            Player1 = new Player("Player1", SettingsPanelLogic.PlayerName("Player1"), MainWindow.Player1Help, MainWindow.Player1PointsHelp, MainWindow.Player1SetsWon, MainWindow.Player1LegsWon, MainWindow.Player1Points, MainWindow.Player1SetDot, PointsToGo);
            Player2 = new Player("Player2", SettingsPanelLogic.PlayerName("Player2"), MainWindow.Player2Help, MainWindow.Player2PointsHelp, MainWindow.Player2SetsWon, MainWindow.Player2LegsWon, MainWindow.Player2Points, MainWindow.Player2SetDot, PointsToGo);
            WhoThrow = SettingsPanelLogic.WhoThrowFirst(Player1, Player2);
            WhoThrow.OnThrow = true;
            WhoThrow.Legstart = true;
            WhosSet = WhoThrow;
            InfoPanelLogic.PanelNewGame(PointsToGo, LegsToGo.ToString(), SetsToGo.ToString(), Player1, Player2, WhoThrow);
            InfoPanelLogic.HelpCheck(Player1);
            InfoPanelLogic.HelpCheck(Player2);
        }
        public void EndGame()
        {
            InfoPanelLogic.PanelHide();
            BoardPanelLogic.PanelHide();
            SettingsPanelLogic.PanelShow();
            //Player1 = null;
            //Player2 = null;
            //Player WhoThrow = null;
            //Player WhosSet = null;
            int LegsToGo = 0;
            int SetsToGo = 0;
            int PointsToGo = 0;
            MessageBox.Show("Победитель матча: " + WhoThrow.Name);
        }

        public void SetPlayerOnThrow(Player p)
        {
            Player1.OnThrow = false;
            Player2.OnThrow = false;
            WhoThrow.OnThrow = true;
            WhoThrow = p;
        }
        public void TogglePlayerOnThrow()
        {
            if (WhoThrow.Tag == "Player1")
            {
                Player1.OnThrow = false;
                Player2.OnThrow = true;
                WhoThrow = Player2;
            }
            else
            {
                Player1.OnThrow = true;
                Player2.OnThrow = false;
                WhoThrow = Player1;
            }
        }
        public void ToggleLegStart()
        {
            if (WhosSet.Tag == "Player1")
            {
                Player1.Legstart = false;
                Player2.Legstart = true;
                WhosSet = Player2;
            }
            else if (WhosSet.Tag == "Player2")
            {
                Player1.Legstart = true;
                Player2.Legstart = false;
                WhosSet = Player1;
            }
        }

        public void AnotherThrow(int points)
        {
            if (WhoThrow.FirstThrow == null)
            {
                WhoThrow.FirstThrow = 0; ;
                WhoThrow.FirstThrow = points;
            }
            else if (WhoThrow.SecondThrow == null)
            {
                WhoThrow.SecondThrow = 0; ;
                WhoThrow.SecondThrow = points;
            }
            else if (WhoThrow.ThirdThrow == null)
            {
                WhoThrow.ThirdThrow = 0; ;
                WhoThrow.ThirdThrow = points;
            }

            AfterEachThrow(points);
        }
        public void AfterEachThrow(int points)
        {
            WhoThrow.PointsToOut -= points;
            WhoThrow.HandPoints += points;
            InfoPanelLogic.PointsSet(WhoThrow);
            InfoPanelLogic.HelpCheck(WhoThrow);

            if (WhoThrow.PointsToOut < 0 || WhoThrow.PointsToOut == 1)
                Fault();
            else if (WhoThrow.PointsToOut == 0)
                SetEnd();
            else if (WhoThrow.ThirdThrow != null)
                TurnHand();
        }
        public void TurnHand()
        {
            InfoPanelLogic.HelpCheck(WhoThrow);
            ClearHands();
            InfoPanelLogic.WhoThrowSliderToggle();
            TogglePlayerOnThrow();
            InfoPanelLogic.HelpCheck(WhoThrow);
        }
        public void Fault()
        {
            WhoThrow.PointsToOut += WhoThrow.HandPoints;
            InfoPanelLogic.PointsSet(WhoThrow);
            TurnHand();
        }
        public void SetEnd()
        {
            WhoThrow.SetsWon += 1;
            InfoPanelLogic.SetIncrement(WhoThrow);
            CheckLegIsOver(WhoThrow);
            SetStart();
        }
        public void SetStart()
        {
            ToggleLegStart();
            SetPlayerOnThrow(WhosSet);
            InfoPanelLogic.WhoThrowSliderSet(WhoThrow);
            InfoPanelLogic.DotToggle();
            ClearHands();
            Player1.PointsToOut = PointsToGo;
            Player2.PointsToOut = PointsToGo;
            InfoPanelLogic.PointsClear(PointsToGo);
            InfoPanelLogic.HelpCheck(Player1);
            InfoPanelLogic.HelpCheck(Player2);
        }
        public void ClearHands()
        {
            Player1.HandPoints = 0;
            Player1.FirstThrow = null;
            Player1.SecondThrow = null;
            Player1.ThirdThrow = null;
            Player2.HandPoints = 0;
            Player2.FirstThrow = null;
            Player2.SecondThrow = null;
            Player2.ThirdThrow = null;
        }
        public void CheckLegIsOver(Player p)
        {
            if (p.SetsWon == SetsToGo)
            {
                p.LegsWon += 1;
                Player1.SetsWon = 0;
                Player2.SetsWon = 0;
                InfoPanelLogic.LegIncrement(p);
                InfoPanelLogic.SetsClear();
            }
            CheckGameIsOver(p);
        }
        public void CheckGameIsOver(Player p)
        {
            if (p.LegsWon == LegsToGo)
                EndGame();
        }
    }


}
