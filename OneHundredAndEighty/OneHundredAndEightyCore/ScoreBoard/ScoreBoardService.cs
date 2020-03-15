#region Usings

using System;
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

        public void OpenScoreBoard(GameTypeDb type)
        {
            switch (type)
            {
                case GameTypeDb.FreeThrowsSingle:
                    scoreBoardType = ScoreBoardType.FreeThrowsSingle;
                    break;
                case GameTypeDb.FreeThrowsDouble:
                    scoreBoardType = ScoreBoardType.FreeThrowsDouble;
                    break;
                case GameTypeDb.Classic1001:
                case GameTypeDb.Classic701:
                case GameTypeDb.Classic501:
                case GameTypeDb.Classic301:
                case GameTypeDb.Classic101:
                    scoreBoardType = ScoreBoardType.Classic;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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

        #region Points

        public void AddPoints(int pointsToAdd)
        {
            scoreBoardWindow.Dispatcher.Invoke(() =>
                                               {
                                                   scoreBoardWindow.ScoreBoardFreeThrowsSinglePoints.Content = int.Parse(scoreBoardWindow.ScoreBoardFreeThrowsSinglePoints.Content.ToString()) 
                                                                                                               + pointsToAdd;
                                               });
        }

        #endregion
    }
}