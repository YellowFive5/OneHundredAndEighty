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

        public void OpenScoreBoard(GameTypeUi type, List<Player> players, string gameTypeString, int writeOffPoints = 0)
        {
            scoreBoardWindow = new ScoreBoardWindow();

            switch (type)
            {
                case GameTypeUi.FreeThrowsSingle:
                    scoreBoardType = ScoreBoardType.FreeThrowsSingle;
                    PreSetupForFreeThrowsSingle(players.First(), gameTypeString, writeOffPoints);
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

        private void PreSetupForFreeThrowsSingle(Player player, string gameTypeString, int writeOffPoints)
        {
            scoreBoardWindow.ScoreBoardFreeThrowsSingleGrid.Visibility = Visibility.Visible;
            TextLabelContentChange(scoreBoardWindow.SingleGameTypeLabel, gameTypeString);
            scoreBoardWindow.SinglePlayerImage.Source = player.Avatar;
            TextLabelContentChange(scoreBoardWindow.SinglePlayerName, $"{player.Name} {player.NickName}");
            scoreBoardWindow.SinglePoints.Content = writeOffPoints;
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

            WhoThrowsPointerRight();
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

        public void CheckPointsHintFor(Player player)
        {
            var hint = CheckOut.Get(player.LegPoints, player.ThrowNumber);
            if (hint != null)
            {
                PointsHintShowFor(player, hint);
            }
            else
            {
                PointsHintHideFor(player);
            }
        }

        private void PointsHintShowFor(Player player, string hint)
        {
            scoreBoardWindow.Dispatcher.Invoke(() =>
                                               {
                                                   if (scoreBoardType == ScoreBoardType.Classic || scoreBoardType == ScoreBoardType.FreeThrowsDouble)
                                                   {
                                                       if (IsForPlayerOne(player))
                                                       {
                                                           if (!PointsHintClassicsPlayer1Shown)
                                                           {
                                                               PointsHintSlideInternal(scoreBoardWindow.PointsHintClassicsPlayer1, -214);
                                                               PointsHintClassicsPlayer1Shown = !PointsHintClassicsPlayer1Shown;
                                                           }

                                                           TextLabelContentChange(scoreBoardWindow.PointsHintClassicsPlayer1Label, hint);
                                                       }
                                                       else if (IsForPlayerTwo(player))
                                                       {
                                                           if (!PointsHintClassicsPlayer2Shown)
                                                           {
                                                               PointsHintSlideInternal(scoreBoardWindow.PointsHintClassicsPlayer2, -214);
                                                               PointsHintClassicsPlayer2Shown = !PointsHintClassicsPlayer2Shown;
                                                           }

                                                           TextLabelContentChange(scoreBoardWindow.PointsHintClassicsPlayer2Label, hint);
                                                       }
                                                   }
                                                   else if (scoreBoardType == ScoreBoardType.FreeThrowsSingle)
                                                   {
                                                       if (!PointsHintSingleShown)
                                                       {
                                                           PointsHintSlideInternal(scoreBoardWindow.PointsHintSingle, -214);
                                                           PointsHintSingleShown = !PointsHintSingleShown;
                                                       }

                                                       TextLabelContentChange(scoreBoardWindow.PointsHintSingleLabel, hint);
                                                   }
                                               });
        }

        private void PointsHintHideFor(Player player)
        {
            scoreBoardWindow.Dispatcher.Invoke(() =>
                                               {
                                                   if (scoreBoardType == ScoreBoardType.Classic || scoreBoardType == ScoreBoardType.FreeThrowsDouble)
                                                   {
                                                       if (IsForPlayerOne(player) && PointsHintClassicsPlayer1Shown)
                                                       {
                                                           PointsHintSlideInternal(scoreBoardWindow.PointsHintClassicsPlayer1, 214);
                                                           PointsHintClassicsPlayer1Shown = !PointsHintClassicsPlayer1Shown;
                                                       }
                                                       else if (IsForPlayerTwo(player) && PointsHintClassicsPlayer2Shown)
                                                       {
                                                           PointsHintSlideInternal(scoreBoardWindow.PointsHintClassicsPlayer2, 214);
                                                           PointsHintClassicsPlayer2Shown = !PointsHintClassicsPlayer2Shown;
                                                       }
                                                   }
                                                   else if (scoreBoardType == ScoreBoardType.FreeThrowsSingle)
                                                   {
                                                       if (PointsHintSingleShown)
                                                       {
                                                           PointsHintSlideInternal(scoreBoardWindow.PointsHintSingle, 214);
                                                           PointsHintSingleShown = !PointsHintSingleShown;
                                                       }
                                                   }
                                               });
        }

        private void PointsHintSlideInternal(FrameworkElement grid, int value)
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

        #region WhoThrowsPointer

        private OnPlayer onPlayer = OnPlayer._1;

        public void WhoThrowsPointerSetOn(Player player)
        {
            scoreBoardWindow.Dispatcher.Invoke(() =>
                                               {
                                                   if (IsForPlayerOne(player))
                                                   {
                                                       WhoThrowsPointerMoveTo(OnPlayer._1);
                                                   }
                                                   else if (IsForPlayerTwo(player))
                                                   {
                                                       WhoThrowsPointerMoveTo(OnPlayer._2);
                                                   }
                                               });
        }

        private void WhoThrowsPointerRight()
        {
            var sb = new Storyboard();

            var newPosition = new Thickness()
                              {
                                  Left = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin.Left + 54,
                                  Top = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin.Top,
                                  Right = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin.Right - 54,
                                  Bottom = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin.Bottom
                              };
            var slide = new ThicknessAnimation()
                        {
                            From = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin,
                            To = newPosition,
                            Duration = slideTime
                        };

            Storyboard.SetTarget(slide, scoreBoardWindow.ClassicsWhoThrowsPointer);
            Storyboard.SetTargetProperty(slide, new PropertyPath(FrameworkElement.MarginProperty));

            sb.Children.Add(slide);
            sb.Begin();
        }

        private void WhoThrowsPointerMoveTo(OnPlayer toPlayer)
        {
            if (onPlayer == toPlayer)
            {
                return;
            }

            var verticalValue = toPlayer == OnPlayer._1
                                    ? -57
                                    : 57;

            var sb = new Storyboard();

            var left = new Thickness()
                       {
                           Left = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin.Left - 54,
                           Top = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin.Top,
                           Right = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin.Right + 54,
                           Bottom = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin.Bottom
                       };
            var slideLeft = new ThicknessAnimation()
                            {
                                From = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin,
                                To = left,
                                Duration = slideTime
                            };

            var vertical = new Thickness()
                           {
                               Left = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin.Left - 54,
                               Top = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin.Top + verticalValue,
                               Right = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin.Right + 54,
                               Bottom = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin.Bottom - verticalValue
                           };
            var slideVertical = new ThicknessAnimation()
                                {
                                    From = left,
                                    To = vertical,
                                    Duration = slideTime,
                                    BeginTime = slideTime
                                };

            var right = new Thickness()
                        {
                            Left = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin.Left,
                            Top = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin.Top + verticalValue,
                            Right = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin.Right,
                            Bottom = scoreBoardWindow.ClassicsWhoThrowsPointer.Margin.Bottom - verticalValue
                        };
            var slideRight = new ThicknessAnimation()
                             {
                                 From = vertical,
                                 To = right,
                                 Duration = slideTime,
                                 BeginTime = slideTime * 2
                             };

            Storyboard.SetTarget(slideLeft, scoreBoardWindow.ClassicsWhoThrowsPointer);
            Storyboard.SetTargetProperty(slideLeft, new PropertyPath(FrameworkElement.MarginProperty));
            Storyboard.SetTarget(slideVertical, scoreBoardWindow.ClassicsWhoThrowsPointer);
            Storyboard.SetTargetProperty(slideVertical, new PropertyPath(FrameworkElement.MarginProperty));
            Storyboard.SetTarget(slideRight, scoreBoardWindow.ClassicsWhoThrowsPointer);
            Storyboard.SetTargetProperty(slideRight, new PropertyPath(FrameworkElement.MarginProperty));

            sb.Children.Add(slideLeft);
            sb.Children.Add(slideVertical);
            sb.Children.Add(slideRight);

            sb.Begin();

            onPlayer = toPlayer;
        }

        #endregion

        private bool IsForPlayerOne(Player player)
        {
            return scoreBoardWindow.ClassicsPlayer1Name.Content.ToString() == $"{player.Name} {player.NickName}";
        }

        private bool IsForPlayerTwo(Player player)
        {
            return scoreBoardWindow.ClassicsPlayer2Name.Content.ToString() == $"{player.Name} {player.NickName}";
        }
    }
}