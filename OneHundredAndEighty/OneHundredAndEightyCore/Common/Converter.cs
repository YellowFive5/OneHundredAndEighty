﻿#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;
using Emgu.CV;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Enums;

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
                var player = new Player(playerRow[$"{Column.Name}"].ToString(),
                                        playerRow[$"{Column.NickName}"].ToString(),
                                        Base64ToBitmapImage(playerRow[$"{Column.Avatar}"].ToString()),
                                        Convert.ToInt32(playerRow[$"{Column.Id}"]));
                playersList.Add(player);
            }

            return playersList;
        }

        public static Player PlayerFromTable(DataSet playerData)
        {
            return null; // todo
        }

        public static string PlayerStatisticsFromTable(DataTable playerStatisticsDataTable) // todo add Statistics object maybe instead of string
        {
            var dataRow = playerStatisticsDataTable.Rows[0];
            return $"Name: {dataRow[$"{Column.Name}"]} \n" +
                   $"Nickname: {dataRow[$"{Column.NickName}"]} \n" +
                   $"Registration date: {DateTime.Parse(dataRow[$"{Column.RegistrationDateTime}"].ToString()):dd.MM.yyyy} \n" +
                   $"Games played: {dataRow["GamesPlayed"]} \n" +
                   $"Games won: {dataRow["GamesWon"]} \n" +
                   $"FreeThrowsSingle games played: {dataRow["FreeThrowsSingleGamesPlayed"]} \n" +
                   $"FreeThrowsDouble games played: {dataRow["FreeThrowsDoubleGamesPlayed"]} \n" +
                   $"FreeThrowsDouble games won: {dataRow["FreeThrowsDoubleGamesWon"]} \n" +
                   $"Classic games played: {dataRow["ClassicGamesPlayed"]} \n" +
                   $"Classic games won: {dataRow["ClassicGamesGamesWon"]} \n" +
                   $"Legs played: {dataRow["LegsPlayed"]} \n" +
                   $"Legs won: {dataRow["LegsWon"]} \n" +
                   $"Sets played: {dataRow["SetsPlayed"]} \n" +
                   $"Sets won: {dataRow["SetsWon"]} \n" +
                   $"Total throws: {dataRow["TotalThrows"]} \n" +
                   $"Total points: {dataRow["TotalPoints"]} \n" +
                   $"AVG points per throw: {dataRow["AvgPointsPerThrow"]} \n" +
                   $"OneHundredAndEighty's: {dataRow["_180"]} \n" +
                   $"Single throws: {dataRow["SingleThrows"]} \n" +
                   $"Double throws: {dataRow["DoubleThrows"]} \n" +
                   $"Tremble throws: {dataRow["TrembleThrows"]} \n" +
                   $"Bull throws: {dataRow["BullThrows"]} \n" +
                   $"25 throws: {dataRow["_25Throws"]} \n" +
                   $"Zero throws: {dataRow["ZeroThrows"]} \n" +
                   $"Achieves obtained: {dataRow["TotalAchieves"]}/18 \n";
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

        public static BitmapImage Base64ToBitmapImage(string base64String)
        {
            var image = (Bitmap) Base64ToImage(base64String);
            return BitmapToBitmapImage(image);
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

        public static BitmapImage EmguImageToBitmapImage(IImage image)
        {
            var imageToSave = new BitmapImage();

            using (var stream = new MemoryStream())
            {
                image.Bitmap.Save(stream, ImageFormat.Bmp);
                imageToSave.BeginInit();
                imageToSave.StreamSource = new MemoryStream(stream.ToArray());
                imageToSave.EndInit();
            }

            return imageToSave;
        }

        public static int CamSetupSectorSettingValueToComboboxSelectedIndex(string camSetupSectorSettingValue)
        {
            switch (camSetupSectorSettingValue)
            {
                case "11":
                    return 0;
                case "11/14":
                    return 1;
                case "14":
                    return 2;
                case "14/9":
                    return 3;
                case "9":
                    return 4;
                case "9/12":
                    return 5;
                case "12":
                    return 6;
                case "12/5":
                    return 7;
                case "5":
                    return 8;
                case "5/20":
                    return 9;
                case "20":
                    return 10;
                case "20/1":
                    return 11;
                case "1":
                    return 12;
                case "1/18":
                    return 13;
                case "18":
                    return 14;
                case "18/4":
                    return 15;
                case "4":
                    return 16;
                case "4/13":
                    return 17;
                case "13":
                    return 18;
                case "13/6":
                    return 19;
                case "6":
                    return 20;
                case "6/10":
                    return 21;
                case "10":
                    return 22;
                case "10/15":
                    return 23;
                case "15":
                    return 24;
                case "15/2":
                    return 25;
                case "2":
                    return 26;
                case "2/17":
                    return 27;
                case "17":
                    return 28;
                case "17/3":
                    return 29;
                case "3":
                    return 30;
                case "3/19":
                    return 31;
                case "19":
                    return 32;
                case "19/7":
                    return 33;
                case "7":
                    return 34;
                case "7/16":
                    return 35;
                case "16":
                    return 36;
                case "16/8":
                    return 37;
                case "8":
                    return 38;
                case "8/11":
                    return 39;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string ToString(object value)
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        public static double ToDouble(string value)
        {
            return Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        public static decimal ToDecimal(string value)
        {
            return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
        }

        public static float ToFloat(string value)
        {
            return (float) ToDecimal(value);
        }

        public static int ToInt(string value)
        {
            return Convert.ToInt32(value);
        }

        public static bool ToBool(string value)
        {
            return Convert.ToBoolean(value);
        }

        public static Version ToVersion(string value)
        {
            return new Version(value);
        }

        public static CamNumber GridNameToCamNumber(string gridName)
        {
            switch (gridName)
            {
                case "Cam1Grid":
                    return CamNumber._1;
                case "Cam2Grid":
                    return CamNumber._2;
                case "Cam3Grid":
                    return CamNumber._3;
                case "Cam4Grid":
                    return CamNumber._4;
                default:
                    throw new ArgumentOutOfRangeException(nameof(GridNameToCamNumber), gridName, null);
            }
        }

        public static int GamePointsToInt(GamePoints points)
        {
            switch (points)
            {
                case GamePoints.Free:
                    return 0;
                case GamePoints._301:
                    return 301;
                case GamePoints._501:
                    return 501;
                case GamePoints._701:
                    return 701;
                case GamePoints._1001:
                    return 1001;
                default:
                    throw new ArgumentOutOfRangeException(nameof(points), points, null);
            }
        }
    }
}