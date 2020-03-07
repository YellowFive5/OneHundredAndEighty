#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using OneHundredAndEightyCore.Game;

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
    }
}