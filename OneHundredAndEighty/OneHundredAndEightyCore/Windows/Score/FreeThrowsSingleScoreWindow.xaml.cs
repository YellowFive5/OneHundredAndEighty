#region Usings

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public partial class FreeThrowsSingleScoreWindow : IScoreWindow
    {
        private bool ForceClose { get; set; }

        private bool pointsHintSingleShown;
        private bool pointsHintClassicsPlayer1Shown;
        private bool pointsHintClassicsPlayer2Shown;
        private readonly TimeSpan slideTime = TimeSpan.FromSeconds(0.25);
        private readonly TimeSpan fadeTime = TimeSpan.FromSeconds(0.5);

        public FreeThrowsSingleScoreWindow(WindowSettings settings,
                                           Player player,
                                           string gameTypeString,
                                           int legPoints)
        {
            InitializeComponent();
            Left = settings.PositionLeft;
            Top = settings.PositionTop;
            Height = settings.Height;
            Width = settings.Width;
            PlayerAvatar.Source = player.Avatar;
            PlayerNameText.Text = $"{player.Name} {player.NickName}";
            GameTypeText.Text = gameTypeString;
            PointsText.Text = Converter.ToString(legPoints);
        }

        #region IScoreWindow

        public void SetSemaphore(DetectionServiceStatus status)
        {
            Dispatcher.Invoke(() =>
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

                                  DetectionStatusSemaphore.Fill = color;
                              });
        }

        public void SetThrowNumber(ThrowNumber number)
        {
            var fillBrush = new SolidColorBrush() {Color = Colors.Black};
            var transparentBrush = new SolidColorBrush() {Opacity = 0};

            switch (number)
            {
                case ThrowNumber.FirstThrow:
                    Throw1Rectangle.Fill = fillBrush;
                    Throw2Rectangle.Fill = fillBrush;
                    Throw3Rectangle.Fill = fillBrush;
                    break;
                case ThrowNumber.SecondThrow:
                    Throw1Rectangle.Fill = transparentBrush;
                    Throw2Rectangle.Fill = fillBrush;
                    Throw3Rectangle.Fill = fillBrush;
                    break;
                case ThrowNumber.ThirdThrow:
                    Throw1Rectangle.Fill = transparentBrush;
                    Throw2Rectangle.Fill = transparentBrush;
                    Throw3Rectangle.Fill = fillBrush;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(number), number, null);
            }
        }

        public new void Close()
        {
            ForceClose = true;
            base.Close();
        }

        public void AddPointsTo(int pointsToAdd, Player player)
        {
            var sb = new Storyboard();
            var fadeIn = new DoubleAnimation {From = 0, To = 1, Duration = fadeTime};
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(OpacityProperty));
            Storyboard.SetTarget(fadeIn, PointsText);
            sb.Children.Add(fadeIn);
            sb.Begin();

            PointsText.Text = Converter.ToString(int.Parse(PointsText.Text) + pointsToAdd);
        }

        #endregion

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (!ForceClose)
            {
                e.Cancel = true;
            }
        }
    }
}