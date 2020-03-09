#region Usings

using System;
using System.Collections.Generic;
using System.Windows;
using OneHundredAndEightyCore.Game;

#endregion

namespace OneHundredAndEightyCore.ScoreBoard
{
    public class ScoreBoardService
    {
        private ScoreBoardWindow scoreBoardWindow;
        private ScoreBoardType scoreBoardType;

        #region Open/Close

        public void OpenScoreBoard(Game.Game game, List<Player> players)
        {
            if (game.Type != GameType.FreeThrows)
            {
                scoreBoardType = ScoreBoardType.Classic;
            }
            else
            {
                scoreBoardType = players.Count == 1
                                     ? ScoreBoardType.FreeThrowsSingle
                                     : ScoreBoardType.FreeThrowsDouble;
            }

            scoreBoardWindow = new ScoreBoardWindow();

            PreSetupWindow();

            scoreBoardWindow.Show();
        }

        public void CloseScoreBoard()
        {
            scoreBoardWindow?.Kill();
        }

        #endregion

        #region PreSetup

        private void PreSetupWindow()
        {
            switch (scoreBoardType)
            {
                case ScoreBoardType.FreeThrowsSingle:
                    PreSetupForFreeThrowsSingle();
                    break;
                case ScoreBoardType.Classic:
                    PreSetupForClassics();
                    break;
                case ScoreBoardType.FreeThrowsDouble:
                    PreSetupForFreeThrowsDouble();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scoreBoardType), scoreBoardType, null);
            }
        }

        private void PreSetupForFreeThrowsSingle()
        {
            scoreBoardWindow.ScoreBoardFreeThrowsSingleGrid.Visibility = Visibility.Visible;
        }

        private void PreSetupForFreeThrowsDouble()
        {
            scoreBoardWindow.ScoreBoardClassicGrid.Visibility = Visibility.Visible;
        }

        private void PreSetupForClassics()
        {
            scoreBoardWindow.ScoreBoardClassicGrid.Visibility = Visibility.Visible;
        }

        #endregion
    }
}