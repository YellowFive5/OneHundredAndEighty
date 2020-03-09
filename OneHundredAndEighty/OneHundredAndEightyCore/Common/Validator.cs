#region Usings

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using OneHundredAndEightyCore.Game;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public static class Validator
    {
        public static bool ValidateNewPlayerNameAndNickName(string newPlayerName, string newPlayerNickName)
        {
            return !string.IsNullOrWhiteSpace(newPlayerName) && !string.IsNullOrWhiteSpace(newPlayerNickName);
        }

        public static bool ValidateNewPlayerAvatar(BitmapImage image)
        {
            return image.PixelHeight <= 500 && image.PixelWidth <= 500;
        }

        public static bool ValidateStartNewGamePlayersSelected(Grid mainWindowNewGameControls)
        {
            bool valid;
            var selectedGameType = Enum.Parse<GameType>(((ComboBox) mainWindowNewGameControls
                                                                    .Children.OfType<FrameworkElement>()
                                                                    .Single(e => e.Name == "NewGameTypeComboBox"))
                                                        .SelectionBoxItem
                                                        .ToString());
            var selectedPlayer1 = ((ComboBox) mainWindowNewGameControls
                                              .Children.OfType<FrameworkElement>()
                                              .Single(e => e.Name == "NewGamePlayer1ComboBox"))
                                  .SelectedItem as Player;
            var selectedPlayer2 = ((ComboBox) mainWindowNewGameControls
                                              .Children.OfType<FrameworkElement>()
                                              .Single(e => e.Name == "NewGamePlayer2ComboBox"))
                                  .SelectedItem as Player;
            switch (selectedGameType)
            {
                case GameType.FreeThrows:
                    valid = (selectedPlayer1 != null || selectedPlayer2 != null) &&
                            selectedPlayer1 != selectedPlayer2;
                    break;
                case GameType.Classic1001:
                case GameType.Classic701:
                case GameType.Classic501:
                case GameType.Classic301:
                case GameType.Classic101:
                    valid = selectedPlayer1 != null && selectedPlayer2 != null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return valid;
        }

        public static bool ValidateImplementedGameTypes(Grid mainWindowNewGameControls)
        {
            var selectedGameType = Enum.Parse<GameType>(((ComboBox) mainWindowNewGameControls
                                                                    .Children.OfType<FrameworkElement>()
                                                                    .Single(e => e.Name == "NewGameTypeComboBox"))
                                                        .SelectionBoxItem
                                                        .ToString());
            switch (selectedGameType)
            {
                case GameType.FreeThrows:
                    return true;
                case GameType.Classic1001:
                case GameType.Classic701:
                case GameType.Classic501:
                case GameType.Classic301:
                case GameType.Classic101:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}