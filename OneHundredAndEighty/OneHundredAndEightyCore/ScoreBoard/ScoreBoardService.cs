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

        private readonly TimeSpan slideTime = TimeSpan.FromSeconds(0.25);
        private readonly TimeSpan fadeTime = TimeSpan.FromSeconds(0.5);

        private bool IsCheckPointsHintShown;

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
                    PreSetupForFreeThrowsDouble();
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
            scoreBoardWindow.ScoreBoardFreeThrowsSingleGridGameType.Content = gameTypeString;
            scoreBoardWindow.ScoreBoardFreeThrowsSingleGridPlayerImage.Source = player.Avatar;
            scoreBoardWindow.ScoreBoardFreeThrowsSingleGridPlayerName.Content = $"{player.Name} {player.NickName}";
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
                                                   AnimationAdd(scoreBoardWindow.ScoreBoardFreeThrowsSinglePoints, pointsToAdd);
                                                   SlideCheckPointsHint(scoreBoardWindow.PointsHintGrid);
                                               });
        }

        #endregion

        private void AnimationAdd(Label label, int pointsToAdd)
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

        private void SlideCheckPointsHint(FrameworkElement grid)
        {
            var value = IsCheckPointsHintShown
                            ? 214
                            : -214;

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

            IsCheckPointsHintShown = !IsCheckPointsHintShown;
        }
    }
}