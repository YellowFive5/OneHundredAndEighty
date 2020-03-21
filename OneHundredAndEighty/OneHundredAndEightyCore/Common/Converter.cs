#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using OneHundredAndEightyCore.Game;
using Image = System.Drawing.Image;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public static class Converter
    {
        public static List<Player> PlayersFromTable(DataTable playersTable)
        {
            var playersList = new List<Player>();
            foreach (DataRow playerRow in playersTable.Rows)
            {
                playersList.Add(new Player(playerRow[$"{Column.Name}"].ToString(),
                                           playerRow[$"{Column.NickName}"].ToString(),
                                           Convert.ToInt32(playerRow[$"{Column.Id}"]),
                                           Base64ToBitmapImage(playerRow[$"{Column.Avatar}"].ToString())));
            }

            return playersList;
        }

        public static DateTime DateTimeFromString(string dateTimeStringFromDb)
        {
            return DateTime.Parse(dateTimeStringFromDb);
        }

        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            var image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        public static string BitmapImageToBase64(BitmapImage bitmapImage)
        {
            var image = BitmapImageToBitmap(bitmapImage);
            return ImageToBase64(image, ImageFormat.Bmp);
        }

        private static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            if (bitmapImage.StreamSource != null)
            {
                return new Bitmap(bitmapImage.StreamSource);
            }

            if (bitmapImage.UriSource != null)
            {
                return new Bitmap(bitmapImage.UriSource.OriginalString);
            }

            return null; // todo
        }

        private static BitmapImage Base64ToBitmapImage(string base64String)
        {
            var image = (Bitmap) Base64ToImage(base64String);
            return BitmapToBitmapImage(image);
        }

        private static string ImageToBase64(Image image, ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, format);
                var imageBytes = ms.ToArray();
                var base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        private static Image Base64ToImage(string base64String)
        {
            var imageBytes = Convert.FromBase64String(base64String);
            var ms = new MemoryStream(imageBytes,
                                      0,
                                      imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            var image = Image.FromStream(ms, true);
            return image;
        }

        public static Visibility ControlVisibilityToggle(Visibility visibility)
        {
            return visibility == Visibility.Visible
                       ? Visibility.Hidden
                       : Visibility.Visible;
        }

        public static GameTypeUi NewGameControlsToGameTypeUi(Grid mainWindowNewGameControls)
        {
            var selectedGameType = Enum.Parse<GameTypeUi>((((ComboBox) mainWindowNewGameControls
                                                                       .Children.OfType<FrameworkElement>()
                                                                       .Single(e => e.Name == "NewGameTypeComboBox")).SelectedItem as ComboBoxItem)
                                                          ?.Content.ToString());

            switch (selectedGameType)
            {
                case GameTypeUi.FreeThrowsSingle:
                    return GameTypeUi.FreeThrowsSingle;
                case GameTypeUi.FreeThrowsDouble:
                    return GameTypeUi.FreeThrowsDouble;
                case GameTypeUi.Classic:
                    return GameTypeUi.Classic;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static GameType NewGameControlsToGameTypeGameService(Grid mainWindowNewGameControls)
        {
            var selectedGameType = NewGameControlsToGameTypeUi(mainWindowNewGameControls);

            var selectedGamePoints = (((ComboBox) mainWindowNewGameControls
                                                  .Children.OfType<FrameworkElement>()
                                                  .Single(e => e.Name == "NewGamePointsComboBox")).SelectedItem as ComboBoxItem)
                                     ?.Content.ToString();

            switch (selectedGameType)
            {
                case GameTypeUi.FreeThrowsSingle:
                    switch (selectedGamePoints)
                    {
                        case "Free":
                            return GameType.FreeThrowsSingleFreePoints;
                        case "301":
                            return GameType.FreeThrowsSingle301Points;
                        case "501":
                            return GameType.FreeThrowsSingle501Points;
                        case "701":
                            return GameType.FreeThrowsSingle701Points;
                        case "1001":
                            return GameType.FreeThrowsSingle1001Points;
                    }

                    break;

                case GameTypeUi.FreeThrowsDouble:
                    switch (selectedGamePoints)
                    {
                        case "Free":
                            return GameType.FreeThrowsDoubleFreePoints;
                        case "301":
                            return GameType.FreeThrowsDouble301Points;
                        case "501":
                            return GameType.FreeThrowsDouble501Points;
                        case "701":
                            return GameType.FreeThrowsDouble701Points;
                        case "1001":
                            return GameType.FreeThrowsDouble1001Points;
                    }

                    break;

                case GameTypeUi.Classic:
                    switch (selectedGamePoints)
                    {
                        case "301":
                            return GameType.Classic301Points;
                        case "501":
                            return GameType.Classic501Points;
                        case "701":
                            return GameType.Classic701Points;
                        case "1001":
                            return GameType.Classic1001Points;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return GameType.FreeThrowsSingleFreePoints; // todo cant get here
        }
    }
}