#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.ScoreBoard
{
    public class ScoreBoardService
    {
        private ScoreBoardWindow scoreBoardWindow;
        private ScoreBoardType scoreBoardType;

        #region Open/Close

        public void OpenScoreBoard(GameTypeUi type,
                                   List<Player> players,
                                   string gameTypeString,
                                   int legPoints = 0)
        {
            scoreBoardWindow = new ScoreBoardWindow();

            switch (type)
            {
                case GameTypeUi.FreeThrowsSingle:
                    scoreBoardType = ScoreBoardType.FreeThrowsSingle;
                    PreSetupForFreeThrowsSingle(players.First(), gameTypeString, legPoints);
                    break;
                case GameTypeUi.FreeThrowsDouble:
                    scoreBoardType = ScoreBoardType.FreeThrowsDouble;
                    PreSetupForFreeThrowsDouble(players, gameTypeString, legPoints);
                    break;
                case GameTypeUi.Classic:
                    scoreBoardType = ScoreBoardType.Classic;
                    PreSetupForClassics(players, gameTypeString, legPoints);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            scoreBoardWindow.Show();
        }

        public void CloseScoreBoard()
        {
            scoreBoardWindow?.Dispatcher.Invoke(() => { scoreBoardWindow?.Kill(); });
        }

        #endregion

        #region PreSetup

        private void PreSetupForFreeThrowsSingle(Player player, string gameTypeString, int legPoints)
        {
            scoreBoardWindow.ScoreBoardFreeThrowsSingleGrid.Visibility = Visibility.Visible;
            TextLabelContentChange(scoreBoardWindow.SingleGameTypeLabel, gameTypeString);
            scoreBoardWindow.SinglePlayerImage.Source = player.Avatar;

            TextLabelContentChange(scoreBoardWindow.SinglePlayerName, $"{player.Name} {player.NickName}");

            SetPointsToSinglePlayer(legPoints);
        }

        private void PreSetupForFreeThrowsDouble(List<Player> players, string gameTypeString, int legPoints)
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

            SetPointsToClassic(legPoints, players.ElementAt(0));
            SetPointsToClassic(legPoints, players.ElementAt(1));

            OnThrowPointerMoveRight();
        }

        private void PreSetupForClassics(List<Player> players, string gameTypeString, int legPoints)
        {
            TextLabelContentChange(scoreBoardWindow.ClassicsGameTypeLabel, gameTypeString);

            scoreBoardWindow.ClassicsPlayer1Image.Source = players.ElementAt(0).Avatar;
            scoreBoardWindow.ClassicsPlayer2Image.Source = players.ElementAt(1).Avatar;

            TextLabelContentChange(scoreBoardWindow.ClassicsPlayer1Name, $"{players.ElementAt(0).Name} {players.ElementAt(0).NickName}");
            TextLabelContentChange(scoreBoardWindow.ClassicsPlayer2Name, $"{players.ElementAt(1).Name} {players.ElementAt(1).NickName}");

            scoreBoardWindow.ScoreBoardClassicGrid.Visibility = Visibility.Visible;

            SetPointsToClassic(legPoints, players.ElementAt(0));
            SetPointsToClassic(legPoints, players.ElementAt(1));

            SetLegsWonToClassic(0, players.ElementAt(0));
            SetLegsWonToClassic(0, players.ElementAt(1));

            SetSetsWonToClassic(0, players.ElementAt(0));
            SetSetsWonToClassic(0, players.ElementAt(1));

            OnThrowPointerMoveRight();
        }

        #endregion

        #region Points

        public void SetPointsToSinglePlayer(int pointsToSet)
        {
            AddSetPointsToSinglePlayerInternal(pointsToSet, true);
        }

        public void AddPointsToSinglePlayer(int pointsToAdd)
        {
            AddSetPointsToSinglePlayerInternal(pointsToAdd);
        }

        public void SetPointsToClassic(int pointsToSet, Player player)
        {
            AddSetPointsToClassicInternal(pointsToSet, player, true);
        }

        public void AddPointsToClassic(int pointsToAdd, Player player)
        {
            AddSetPointsToClassicInternal(pointsToAdd, player);
        }

        private void AddSetPointsToSinglePlayerInternal(int points, bool set = false)
        {
            scoreBoardWindow.Dispatcher.Invoke(() => { DigitLabelContentSet(scoreBoardWindow.SinglePoints, points, set); });
        }

        private void AddSetPointsToClassicInternal(int points, Player player, bool set = false)
        {
            scoreBoardWindow.Dispatcher.Invoke(() =>
                                               {
                                                   if (scoreBoardWindow.ClassicsPlayer1Name.Content.ToString() == $"{player.Name} {player.NickName}")
                                                   {
                                                       DigitLabelContentSet(scoreBoardWindow.ClassicsPlayer1Points, points, set);
                                                   }
                                                   else if (scoreBoardWindow.ClassicsPlayer2Name.Content.ToString() == $"{player.Name} {player.NickName}")
                                                   {
                                                       DigitLabelContentSet(scoreBoardWindow.ClassicsPlayer2Points, points, set);
                                                   }
                                               });
        }

        #endregion

        #region Legs

        private OnPlayer legPointOnPlayer = OnPlayer._1;

        public void LegPointSetOn(Player player)
        {
            scoreBoardWindow.Dispatcher.Invoke(() =>
                                               {
                                                   if (IsForPlayerOne(player))
                                                   {
                                                       LegPointMoveTo(OnPlayer._1);
                                                   }
                                                   else if (IsForPlayerTwo(player))
                                                   {
                                                       LegPointMoveTo(OnPlayer._2);
                                                   }
                                               });
        }

        private void LegPointMoveTo(OnPlayer toPlayer)
        {
            if (legPointOnPlayer == toPlayer)
            {
                return;
            }

            var verticalValue = toPlayer == OnPlayer._1
                                    ? -57
                                    : 57;

            var sb = new Storyboard();

            var fadeOut = new DoubleAnimation
                          {
                              From = 1,
                              To = 0,
                              Duration = fadeTime
                          };
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTarget(fadeOut, scoreBoardWindow.ClassicWhoOnLegPoint);

            var vertical = new Thickness()
                           {
                               Left = scoreBoardWindow.ClassicWhoOnLegPoint.Margin.Left,
                               Top = scoreBoardWindow.ClassicWhoOnLegPoint.Margin.Top + verticalValue,
                               Right = scoreBoardWindow.ClassicWhoOnLegPoint.Margin.Right,
                               Bottom = scoreBoardWindow.ClassicWhoOnLegPoint.Margin.Bottom - verticalValue
                           };
            var slideVertical = new ThicknessAnimation()
                                {
                                    From = scoreBoardWindow.ClassicWhoOnLegPoint.Margin,
                                    To = vertical,
                                    Duration = slideTime,
                                    BeginTime = fadeTime
                                };
            Storyboard.SetTarget(slideVertical, scoreBoardWindow.ClassicWhoOnLegPoint);
            Storyboard.SetTargetProperty(slideVertical, new PropertyPath(FrameworkElement.MarginProperty));

            var fadeIn = new DoubleAnimation
                         {
                             From = 0, To = 1,
                             Duration = fadeTime,
                             BeginTime = fadeTime + slideTime
                         };
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTarget(fadeIn, scoreBoardWindow.ClassicWhoOnLegPoint);

            sb.Children.Add(fadeOut);
            sb.Children.Add(slideVertical);
            sb.Children.Add(fadeIn);

            sb.Begin();

            legPointOnPlayer = toPlayer;
        }

        public void AddLegsWonToClassic(Player player)
        {
            AddSetLegsWonToClassicInternal(1, player);
        }

        public void SetLegsWonToClassic(int legsToSet, Player player)
        {
            AddSetLegsWonToClassicInternal(legsToSet, player, true);
        }

        private void AddSetLegsWonToClassicInternal(int points, Player player, bool set = false)
        {
            scoreBoardWindow.Dispatcher.Invoke(() =>
                                               {
                                                   if (scoreBoardWindow.ClassicsPlayer1Name.Content.ToString() == $"{player.Name} {player.NickName}")
                                                   {
                                                       DigitLabelContentSet(scoreBoardWindow.ClassicsPlayer1Legs, points, set);
                                                   }
                                                   else if (scoreBoardWindow.ClassicsPlayer2Name.Content.ToString() == $"{player.Name} {player.NickName}")
                                                   {
                                                       DigitLabelContentSet(scoreBoardWindow.ClassicsPlayer2Legs, points, set);
                                                   }
                                               });
        }

        #endregion

        #region Sets

        public void AddSetsWonToClassic(Player player)
        {
            AddSetSetsWonToClassicInternal(1, player);
        }

        public void SetSetsWonToClassic(int setsToSet, Player player)
        {
            AddSetSetsWonToClassicInternal(setsToSet, player, true);
        }

        private void AddSetSetsWonToClassicInternal(int points, Player player, bool set = false)
        {
            scoreBoardWindow.Dispatcher.Invoke(() =>
                                               {
                                                   if (scoreBoardWindow.ClassicsPlayer1Name.Content.ToString() == $"{player.Name} {player.NickName}")
                                                   {
                                                       DigitLabelContentSet(scoreBoardWindow.ClassicsPlayer1Sets, points, set);
                                                   }
                                                   else if (scoreBoardWindow.ClassicsPlayer2Name.Content.ToString() == $"{player.Name} {player.NickName}")
                                                   {
                                                       DigitLabelContentSet(scoreBoardWindow.ClassicsPlayer2Sets, points, set);
                                                   }
                                               });
        }

        #endregion

        #region PointsHint

        private bool pointsHintSingleShown;
        private bool pointsHintClassicsPlayer1Shown;
        private bool pointsHintClassicsPlayer2Shown;
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
                                                           if (!pointsHintClassicsPlayer1Shown)
                                                           {
                                                               PointsHintSlideInternal(scoreBoardWindow.PointsHintClassicsPlayer1, -214);
                                                               pointsHintClassicsPlayer1Shown = !pointsHintClassicsPlayer1Shown;
                                                           }

                                                           TextLabelContentChange(scoreBoardWindow.PointsHintClassicsPlayer1Label, hint);
                                                       }
                                                       else if (IsForPlayerTwo(player))
                                                       {
                                                           if (!pointsHintClassicsPlayer2Shown)
                                                           {
                                                               PointsHintSlideInternal(scoreBoardWindow.PointsHintClassicsPlayer2, -214);
                                                               pointsHintClassicsPlayer2Shown = !pointsHintClassicsPlayer2Shown;
                                                           }

                                                           TextLabelContentChange(scoreBoardWindow.PointsHintClassicsPlayer2Label, hint);
                                                       }
                                                   }
                                                   else if (scoreBoardType == ScoreBoardType.FreeThrowsSingle)
                                                   {
                                                       if (!pointsHintSingleShown)
                                                       {
                                                           PointsHintSlideInternal(scoreBoardWindow.PointsHintSingle, -214);
                                                           pointsHintSingleShown = !pointsHintSingleShown;
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
                                                       if (IsForPlayerOne(player) && pointsHintClassicsPlayer1Shown)
                                                       {
                                                           PointsHintSlideInternal(scoreBoardWindow.PointsHintClassicsPlayer1, 214);
                                                           pointsHintClassicsPlayer1Shown = !pointsHintClassicsPlayer1Shown;
                                                       }
                                                       else if (IsForPlayerTwo(player) && pointsHintClassicsPlayer2Shown)
                                                       {
                                                           PointsHintSlideInternal(scoreBoardWindow.PointsHintClassicsPlayer2, 214);
                                                           pointsHintClassicsPlayer2Shown = !pointsHintClassicsPlayer2Shown;
                                                       }
                                                   }
                                                   else if (scoreBoardType == ScoreBoardType.FreeThrowsSingle)
                                                   {
                                                       if (pointsHintSingleShown)
                                                       {
                                                           PointsHintSlideInternal(scoreBoardWindow.PointsHintSingle, 214);
                                                           pointsHintSingleShown = !pointsHintSingleShown;
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

        #region OnThrowPointer

        public void SetThrowNumber(int throwNumber)
        {
            var str = DartSymbol;

            switch (throwNumber)
            {
                case 1:
                    str = $"{DartSymbol}{DartSymbol}{DartSymbol}";
                    break;
                case 2:
                    str = $"{DartSymbol}{DartSymbol}";
                    break;
                case 3:
                    str = $"{DartSymbol}";
                    break;
            }

            scoreBoardWindow.Dispatcher.Invoke(() =>
                                               {
                                                   if (scoreBoardType == ScoreBoardType.FreeThrowsDouble || scoreBoardType == ScoreBoardType.Classic)
                                                   {
                                                       TextLabelContentChange(scoreBoardWindow.ThrowNumberClassicLabel, str);
                                                   }
                                                   else if (scoreBoardType == ScoreBoardType.FreeThrowsSingle)
                                                   {
                                                       TextLabelContentChange(scoreBoardWindow.ThrowNumberSingleLabel, str);
                                                   }
                                               });
        }

        private OnPlayer onThrowPointerOnPlayer = OnPlayer._1;

        public void OnThrowPointerSetOn(Player player)
        {
            scoreBoardWindow.Dispatcher.Invoke(() =>
                                               {
                                                   if (IsForPlayerOne(player))
                                                   {
                                                       OnThrowPointerMoveTo(OnPlayer._1);
                                                   }
                                                   else if (IsForPlayerTwo(player))
                                                   {
                                                       OnThrowPointerMoveTo(OnPlayer._2);
                                                   }
                                               });
        }

        private void OnThrowPointerMoveRight()
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

        private void OnThrowPointerMoveTo(OnPlayer toPlayer)
        {
            if (onThrowPointerOnPlayer == toPlayer)
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

            onThrowPointerOnPlayer = toPlayer;
        }

        private const string DartSymbol = "⬇";

        public void SetDetectionStatus(DetectionServiceStatus status)
        {
            scoreBoardWindow.Dispatcher.Invoke(() =>
                                               {
                                                   SolidColorBrush color;
                                                   switch (status)
                                                   {
                                                       case DetectionServiceStatus.WaitingThrow:
                                                           color = new SolidColorBrush(Colors.ForestGreen);
                                                           break;
                                                       case DetectionServiceStatus.ProcessingThrow:
                                                           color = new SolidColorBrush(Colors.Red);
                                                           break;
                                                       case DetectionServiceStatus.DartsExtraction:
                                                           color = new SolidColorBrush(Colors.Yellow);
                                                           break;
                                                       default:
                                                           throw new ArgumentOutOfRangeException(nameof(status), status, null);
                                                   }

                                                   if (scoreBoardType == ScoreBoardType.Classic || scoreBoardType == ScoreBoardType.FreeThrowsDouble)
                                                   {
                                                       scoreBoardWindow.DetectionStatusClassic.Fill = color;
                                                   }
                                                   else if (scoreBoardType == ScoreBoardType.FreeThrowsSingle)
                                                   {
                                                       scoreBoardWindow.DetectionStatusSingle.Fill = color;
                                                   }
                                               });
        }

        #endregion

        private readonly TimeSpan fadeTime = TimeSpan.FromSeconds(0.5);

        private void DigitLabelContentSet(ContentControl label, int number, bool set = false)
        {
            var sb = new Storyboard();
            var fadeIn = new DoubleAnimation {From = 0, To = 1, Duration = fadeTime};
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTarget(fadeIn, label);
            sb.Children.Add(fadeIn);
            sb.Begin();

            if (set)
            {
                label.Content = number;
            }
            else
            {
                label.Content = int.Parse(label.Content.ToString())
                                + number;
            }
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