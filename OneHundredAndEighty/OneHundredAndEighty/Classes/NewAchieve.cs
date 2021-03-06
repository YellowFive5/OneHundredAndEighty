﻿#region Usings

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#endregion

namespace OneHundredAndEighty
{
    public static class NewAchieve //  Заработана новая ачивка
    {
        private static readonly MainWindow MainWindow = (MainWindow) Application.Current.MainWindow; //  Cсылка на главное окно

        public static void ShowNewAchieveWindow(string name, string achieve) //  Показываем окно новой ачивки
        {
            var window = new Windows.NewAchieve();
            switch (achieve) //  В зависимости от названия ачивки вибираем сообщение, картинку и кисть
            {
                case "A10matchespalyed":
                    window.AchieveName.Content = "\"10 games played\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/10MatchesPlayed.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "A100MatchesPalyed":
                    window.AchieveName.Content = "\"100 games played\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/100MatchesPlayed.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "A1000MatchesPalyed":
                    window.AchieveName.Content = "\"1000 games played\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/1000MatchesPlayed.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "A10MatchesWon":
                    window.AchieveName.Content = "\"10 matches won\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/10MatchesWon.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "A100MatchesWon":
                    window.AchieveName.Content = "\"100 matches won\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/100MatchesWon.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "A1000MatchesWon":
                    window.AchieveName.Content = "\"1000 matches won\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/1000MatchesWon.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "A1000Throws":
                    window.AchieveName.Content = "\"1000 throws made\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/1000Throws.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "A10000Throws":
                    window.AchieveName.Content = "\"10000 throws made\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/10000Throws.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "A100000Throws":
                    window.AchieveName.Content = "\"100000 throws made\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/100000Throws.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "A10000Points":
                    window.AchieveName.Content = "\"10000 points collected\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/10000Points.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "A100000Points":
                    window.AchieveName.Content = "\"100000 points collected\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/100000Points.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "A1000000Points":
                    window.AchieveName.Content = "\"1000000 points collected\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/1000000Points.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "A180x10":
                    window.AchieveName.Content = "\"10x180 hand committed\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/180x10.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "A180x100":
                    window.AchieveName.Content = "\"100x180 hand committed\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/180x100.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "A180x1000":
                    window.AchieveName.Content = "\"1000x180 hand committed\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/180x1000.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "AFirst180":
                    window.AchieveName.Content = "\"It's your first 180 !\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/First180.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "A3Bull":
                    window.AchieveName.Content = "\"3-eyed bull\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/3Bull.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
                case "AmrZ":
                    window.AchieveName.Content = "\"Absolute zero\"";
                    window.AchieveImage.Source = new BitmapImage(new Uri("/OneHundredAndEighty;component/Images/Achieves/mr.Z.png", UriKind.Relative));
                    window.AchieveLight.Fill = Brush(achieve);
                    break;
            }

            window.PlayerName.Content = name;
            window.Owner = MainWindow;
            MainWindow.FadeIn();
            window.ShowDialog();
            MainWindow.FadeOut();

            Brush Brush(string achieveName) //  Выбираем кисть
            {
                var brush = new LinearGradientBrush();
                switch (achieveName)
                {
                    case "A10matchespalyed":
                    case "A10MatchesWon":
                    case "A1000Throws":
                    case "A10000Points":
                    case "A180x10":
                        brush.StartPoint = new Point(0.5, 1);
                        brush.EndPoint = new Point(0.5, 0);
                        brush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FF00420F"), 0));
                        brush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FF2DC700"), 1));
                        brush.RelativeTransform = new RotateTransform(225);
                        break;
                    case "A100MatchesPalyed":
                    case "A100MatchesWon":
                    case "A10000Throws":
                    case "A100000Points":
                    case "A180x100":
                        brush.StartPoint = new Point(0.5, 1);
                        brush.EndPoint = new Point(0.5, 0);
                        brush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FF050078"), 0));
                        brush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FF00B1D8"), 1));
                        brush.RelativeTransform = new RotateTransform(225);
                        break;
                    case "A1000MatchesPalyed":
                    case "A1000MatchesWon":
                    case "A100000Throws":
                    case "A1000000Points":
                    case "A180x1000":
                        brush.StartPoint = new Point(0.5, 1);
                        brush.EndPoint = new Point(0.5, 0);
                        brush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FF610000"), 0));
                        brush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FF81009E"), 1));
                        brush.RelativeTransform = new RotateTransform(225);
                        break;
                    case "AFirst180":
                        brush.StartPoint = new Point(0.5, 0);
                        brush.EndPoint = new Point(0.5, 1);
                        brush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FFFF4600"), 0));
                        brush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FF34CF07"), 1));
                        brush.RelativeTransform = new RotateTransform(250);
                        break;
                    case "AmrZ":
                        brush.StartPoint = new Point(0.5, 0);
                        brush.EndPoint = new Point(0.5, 1);
                        brush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FF5B5B5B"), 0));
                        brush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FFF7F7F7"), 1));
                        brush.RelativeTransform = new RotateTransform(225);
                        break;
                    case "A3Bull":
                        var gradientBrush = new RadialGradientBrush();
                        gradientBrush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FFC70900"), 0));
                        gradientBrush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FF00420F"), 0.577));
                        gradientBrush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#FF360146"), 1));
                        return gradientBrush;
                }

                return brush;
            }
        }
    }
}