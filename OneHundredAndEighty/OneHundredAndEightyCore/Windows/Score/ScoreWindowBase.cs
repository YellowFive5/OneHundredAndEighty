#region Usings

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public class ScoreWindowBase : Window
    {
        protected bool pointsHintClassicsPlayer1Shown;
        protected bool pointsHintSingleShown;
        protected bool pointsHintClassicsPlayer2Shown;
        private readonly TimeSpan slideTime = TimeSpan.FromSeconds(0.25);
        private readonly TimeSpan fadeTime = TimeSpan.FromSeconds(0.5);
        private readonly SolidColorBrush redBrush = new SolidColorBrush(Colors.Red);
        private readonly SolidColorBrush yellowBrush = new SolidColorBrush(Colors.Yellow);
        private readonly SolidColorBrush greenBrush = new SolidColorBrush(Colors.ForestGreen);
        private readonly SolidColorBrush blackBrush = new SolidColorBrush(Colors.Black);
        private readonly SolidColorBrush transparentBrush = new SolidColorBrush() {Opacity = 0};

        protected bool ForceClose { get; set; }

        protected ScoreWindowBase(WindowSettings settings)
        {
            Left = settings.PositionLeft;
            Top = settings.PositionTop;
            Height = settings.Height;
            Width = settings.Width;
        }

        protected void AddPoints(TextBlock control, int pointsToAdd)
        {
            var sb = new Storyboard();
            var fadeIn = new DoubleAnimation {From = 0, To = 1, Duration = fadeTime};
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(OpacityProperty));
            Storyboard.SetTarget(fadeIn, control);
            sb.Children.Add(fadeIn);
            sb.Begin();

            control.Text = Converter.ToString(int.Parse(control.Text) + pointsToAdd);
        }

        protected void SetSemaphore(Ellipse control, DetectionServiceStatus status)
        {
            SolidColorBrush color;
            switch (status)
            {
                case DetectionServiceStatus.WaitingThrow:
                    color = greenBrush;
                    break;
                case DetectionServiceStatus.ProcessingThrow:
                    color = redBrush;
                    break;
                case DetectionServiceStatus.DartsExtraction:
                    color = yellowBrush;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }

            control.Fill = color;
        }

        protected void SetThrowNumber(Rectangle throw1Control,
                                      Rectangle throw2Control,
                                      Rectangle throw3Control,
                                      ThrowNumber number)
        {
            switch (number)
            {
                case ThrowNumber.FirstThrow:
                    throw1Control.Fill = blackBrush;
                    throw2Control.Fill = blackBrush;
                    throw3Control.Fill = blackBrush;
                    break;
                case ThrowNumber.SecondThrow:
                    throw1Control.Fill = transparentBrush;
                    throw2Control.Fill = blackBrush;
                    throw3Control.Fill = blackBrush;
                    break;
                case ThrowNumber.ThirdThrow:
                    throw1Control.Fill = transparentBrush;
                    throw2Control.Fill = transparentBrush;
                    throw3Control.Fill = blackBrush;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(number), number, null);
            }
        }

        protected new void Close()
        {
            ForceClose = true;
            base.Close();
        }

        protected void OnMouseLeftButtonDown()
        {
            DragMove();
        }

        protected void OnClosing(CancelEventArgs e)
        {
            if (!ForceClose)
            {
                e.Cancel = true;
            }
        }
    }
}