#region Usings

using System;
using System.Text.RegularExpressions;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public static class Validator
    {
        public static bool ValidateNewPlayerNameAndNickName(string newPlayerName, string newPlayerNickName)
        {
            return !string.IsNullOrWhiteSpace(newPlayerName) && !string.IsNullOrWhiteSpace(newPlayerNickName);
        }

        public static bool ValidateNewPlayerAvatar(int imagePixelHeight, int imagePixelWidth)
        {
            return imagePixelHeight <= 1000 && imagePixelWidth <= 1000;
        }

        public static bool ValidateStartNewGamePlayersSelected(GameType newGameType,
                                                               Player selectedPlayer1,
                                                               Player selectedPlayer2)
        {
            bool valid;
            switch (newGameType)
            {
                case GameType.FreeThrowsSingle:
                    valid = selectedPlayer1 != null;
                    break;
                case GameType.FreeThrowsDouble:
                case GameType.Classic:
                    valid = selectedPlayer1 != null && selectedPlayer2 != null &&
                            selectedPlayer1 != selectedPlayer2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return valid;
        }

        public static bool ValidateImplementedGameTypes(GameType newGameType)
        {
            switch (newGameType)
            {
                case GameType.FreeThrowsSingle:
                case GameType.FreeThrowsDouble:
                case GameType.Classic:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool ValidateStartNewClassicGame(GameType newGameType,
                                                       GamePoints newGamePoints)
        {
            switch (newGameType)
            {
                case GameType.Classic:
                    return newGamePoints != GamePoints.Free;
                default:
                    return true;
            }
        }

        public static bool DoubleValidation(string text)
        {
            var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            return !regex.IsMatch(text);
        }

        public static bool IntValidation(string text)
        {
            var regex = new Regex("[^0-9]+");
            return regex.IsMatch(text);
        }
    }
}