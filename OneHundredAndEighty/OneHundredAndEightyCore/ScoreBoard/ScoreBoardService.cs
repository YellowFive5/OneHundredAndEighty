#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using OneHundredAndEightyCore.Game;

#endregion

namespace OneHundredAndEightyCore.ScoreBoard
{
    public class ScoreBoardService
    {
        private ScoreBoardWindow scoreBoardWindow;
        private ScoreBoardType scoreBoardType;

        #region Open/Close

        public void OpenScoreBoard(GameTypeUi type, List<Player> players, string gameTypeString)
        {
            scoreBoardWindow = new ScoreBoardWindow();

            switch (type)
            {
                case GameTypeUi.FreeThrowsSingle:
                    scoreBoardType = ScoreBoardType.FreeThrowsSingle;
                    PreSetupForFreeThrowsSingle(players.First(), gameTypeString);
                    break;
                case GameTypeUi.FreeThrowsDouble:
                    scoreBoardType = ScoreBoardType.FreeThrowsDouble;
                    PreSetupForFreeThrowsDouble(players, gameTypeString);
                    break;
                case GameTypeUi.Classic:
                    scoreBoardType = ScoreBoardType.Classic;
                    PreSetupForClassics();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            scoreBoardWindow.Show();
        }

        public void CloseScoreBoard()
        {
            scoreBoardWindow?.Kill();
        }

        #endregion

        #region PreSetup

        private void PreSetupForFreeThrowsSingle(Player player, string gameTypeString)
        {
            scoreBoardWindow.ScoreBoardFreeThrowsSingleGrid.Visibility = Visibility.Visible;
            TextLabelContentChange(scoreBoardWindow.SingleGameTypeLabel, gameTypeString);
            scoreBoardWindow.SinglePlayerImage.Source = player.Avatar;
            TextLabelContentChange(scoreBoardWindow.SinglePlayerName, $"{player.Name} {player.NickName}");
        }

        private void PreSetupForFreeThrowsDouble(List<Player> players, string gameTypeString)
        {
            TextLabelContentChange(scoreBoardWindow.ClassicsGameTypeLabel, gameTypeString);

            scoreBoardWindow.ClassicsPlayer1Image.Source = players.ElementAt(0).Avatar;
            scoreBoardWindow.ClassicsPlayer2Image.Source = players.ElementAt(1).Avatar;

            TextLabelContentChange(scoreBoardWindow.ClassicsPlayer1Name, $"{players.ElementAt(0).Name} {players.ElementAt(0).NickName}");
            TextLabelContentChange(scoreBoardWindow.ClassicsPlayer2Name, $"{players.ElementAt(1).Name} {players.ElementAt(1).NickName}");

            scoreBoardWindow.ScoreBoardClassicGrid.Visibility = Visibility.Visible;
            scoreBoardWindow.ClassicsSetsLabel.Visibility = Visibility.Hidden;
            scoreBoardWindow.ClassicsLegsLabel.Visibility = Visibility.Hidden;
            scoreBoardWindow.ClassicsPlayer1Legs.Visibility = Visibility.Hidden;
            scoreBoardWindow.ClassicsPlayer1Sets.Visibility = Visibility.Hidden;
            scoreBoardWindow.ClassicsPlayer2Legs.Visibility = Visibility.Hidden;
            scoreBoardWindow.ClassicsPlayer2Sets.Visibility = Visibility.Hidden;
            scoreBoardWindow.ClassicWhoOnLegPoint.Visibility = Visibility.Hidden;
        }

        private void PreSetupForClassics()
        {
            scoreBoardWindow.ScoreBoardClassicGrid.Visibility = Visibility.Visible;
        }

        #endregion

        #region Points

        public void AddPointsToSinglePlayer(int pointsToAdd)
        {
            scoreBoardWindow.Dispatcher.Invoke(() => { DigitLabelContentAdd(scoreBoardWindow.SinglePoints, pointsToAdd); });
        }

        public void AddPointsToClassic(int pointsToAdd, Player player)
        {
            scoreBoardWindow.Dispatcher.Invoke(() =>
                                               {
                                                   if (scoreBoardWindow.ClassicsPlayer1Name.Content.ToString() == $"{player.Name} {player.NickName}")
                                                   {
                                                       DigitLabelContentAdd(scoreBoardWindow.ClassicsPlayer1Points, pointsToAdd);
                                                   }
                                                   else if (scoreBoardWindow.ClassicsPlayer2Name.Content.ToString() == $"{player.Name} {player.NickName}")
                                                   {
                                                       DigitLabelContentAdd(scoreBoardWindow.ClassicsPlayer2Points, pointsToAdd);
                                                   }
                                               });
        }

        #endregion

        private readonly TimeSpan fadeTime = TimeSpan.FromSeconds(0.5);

        private void DigitLabelContentAdd(ContentControl label, int pointsToAdd)
        {
            var sb = new Storyboard();
            var fadeIn = new DoubleAnimation {From = 0, To = 1, Duration = fadeTime};
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTarget(fadeIn, label);
            sb.Children.Add(fadeIn);
            sb.Begin();

            label.Content = int.Parse(label.Content.ToString())
                            + pointsToAdd;
        }

        private void TextLabelContentChange(ContentControl label, string text)
        {
            var sb = new Storyboard();
            var fadeIn = new DoubleAnimation {From = 0, To = 1, Duration = fadeTime};
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTarget(fadeIn, label);
            sb.Children.Add(fadeIn);
            sb.Begin();

            label.Content = text;
        }

        #region PointsHint

        private bool PointsHintSingleShown;
        private bool PointsHintClassicsPlayer1Shown;
        private bool PointsHintClassicsPlayer2Shown;
        private readonly TimeSpan slideTime = TimeSpan.FromSeconds(0.25);

        public void CheckPointsHintShow(FrameworkElement grid)
        {
            if (grid == scoreBoardWindow.PointsHintClassicsPlayer1 &&
                !PointsHintClassicsPlayer1Shown)
            {
                CheckPointsHintSlideInternal(grid, -214);
                PointsHintClassicsPlayer1Shown = !PointsHintClassicsPlayer1Shown;
            }
            else if (grid == scoreBoardWindow.PointsHintClassicsPlayer2 &&
                     !PointsHintClassicsPlayer2Shown)
            {
                CheckPointsHintSlideInternal(grid, -214);
                PointsHintClassicsPlayer2Shown = !PointsHintClassicsPlayer2Shown;
            }
        }

        public void CheckPointsHintHide(FrameworkElement grid)
        {
            if (grid == scoreBoardWindow.PointsHintClassicsPlayer1 &&
                PointsHintClassicsPlayer1Shown)
            {
                CheckPointsHintSlideInternal(grid, 214);
                PointsHintClassicsPlayer1Shown = !PointsHintClassicsPlayer1Shown;
            }
            else if (grid == scoreBoardWindow.PointsHintClassicsPlayer2 &&
                     PointsHintClassicsPlayer2Shown)
            {
                CheckPointsHintSlideInternal(grid, 214);
                PointsHintClassicsPlayer2Shown = !PointsHintClassicsPlayer2Shown;
            }
        }

        private void CheckPointsHintSlideInternal(FrameworkElement grid, int value)
        {
            var sb = new Storyboard();

            var newPosition = new Thickness()
                              {
                                  Left = grid.Margin.Left + value,
                                  Top = grid.Margin.Top,
                                  Right = grid.Margin.Right - value,
                                  Bottom = grid.Margin.Bottom
                              };
            var slide = new ThicknessAnimation()
                        {
                            From = grid.Margin,
                            To = newPosition,
                            Duration = slideTime
                        };

            Storyboard.SetTarget(slide, grid);
            Storyboard.SetTargetProperty(slide, new PropertyPath(FrameworkElement.MarginProperty));

            sb.Children.Add(slide);
            sb.Begin();
        }

        #endregion
    }
}