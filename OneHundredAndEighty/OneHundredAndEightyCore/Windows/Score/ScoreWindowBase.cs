﻿#region Usings

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
        private readonly TimeSpan fadeTime = TimeSpan.FromSeconds(0.5);
        private readonly SolidColorBrush redBrush = new SolidColorBrush(Colors.Red);
        private readonly SolidColorBrush yellowBrush = new SolidColorBrush(Colors.Yellow);
        private readonly SolidColorBrush greenBrush = new SolidColorBrush(Colors.ForestGreen);
        private readonly SolidColorBrush blackBrush = new SolidColorBrush(Colors.Black);
        private readonly SolidColorBrush transparentBrush = new SolidColorBrush() {Opacity = 0};

        private bool ForceClose { get; set; }

        protected ScoreWindowBase(WindowSettings settings)
        {
            Height = settings.Height;
            Width = settings.Width;
            Left = settings.PositionLeft;
            Top = settings.PositionTop;
        }

        protected void OnMouseLeftButtonDown()
        {
            DragMove();
        }

        protected new void Close()
        {
            ForceClose = true;
            base.Close();
        }

        protected new void OnClosing(CancelEventArgs e)
        {
            if (!ForceClose)
            {
                e.Cancel = true;
            }
        }

        public WindowSettings GetWindowSettings()
        {
            return new WindowSettings(Height, Width, Left, Top);
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

        protected void AddPoints(TextBlock control, int pointsToAdd)
        {
            FadeIn(control);
            control.Text = Converter.ToString(int.Parse(control.Text) + pointsToAdd);
        }

        protected void SetPoints(TextBlock control, int pointsToSet)
        {
            FadeIn(control);
            control.Text = Converter.ToString(pointsToSet);
        }

        protected void CheckoutShow(Grid checkoutGrid,
                                    TextBlock checkoutControl,
                                    string hint)
        {
            FadeIn(checkoutGrid);
            CheckoutUpdate(checkoutControl,
                           hint);
        }

        protected void CheckoutUpdate(TextBlock checkoutControl,
                                      string hint)
        {
            FadeIn(checkoutControl);
            checkoutControl.Text = hint;
        }

        protected void CheckoutHide(Grid checkoutGrid,
                                    TextBlock checkoutControl)
        {
            FadeOut(checkoutGrid);
            CheckoutUpdate(checkoutControl,
                           string.Empty);
        }

        protected void FadeIn(FrameworkElement grid)
        {
            var sb = new Storyboard();
            var fadeIn = new DoubleAnimation {From = 0, To = 1, Duration = fadeTime};
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(OpacityProperty));
            Storyboard.SetTarget(fadeIn, grid);
            sb.Children.Add(fadeIn);
            sb.Begin();
        }

        protected void FadeOut(FrameworkElement grid)
        {
            var sb = new Storyboard();
            var fadeOut = new DoubleAnimation {From = 1, To = 0, Duration = fadeTime};
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath(OpacityProperty));
            Storyboard.SetTarget(fadeOut, grid);
            sb.Children.Add(fadeOut);
            sb.Begin();
        }
    }
}