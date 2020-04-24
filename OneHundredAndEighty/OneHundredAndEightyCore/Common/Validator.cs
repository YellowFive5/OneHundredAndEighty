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
            var selectedGameType = Converter.NewGameControlsToGameType(mainWindowNewGameControls);
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

        public static bool ValidateImplementedGameTypes(Grid mainWindowNewGameControls)
        {
            var selectedGameType = Converter.NewGameControlsToGameType(mainWindowNewGameControls);
            switch (selectedGameType)
            {
                case GameType.FreeThrowsSingle:
                case GameType.FreeThrowsDouble:
                case GameType.Classic:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool ValidateStartNewClassicGamePoints(Grid mainWindowNewGameControls)
        {
            var selectedGameType = Converter.NewGameControlsToGameType(mainWindowNewGameControls);
            var selectedGamePoints = (((ComboBox) mainWindowNewGameControls
                                                  .Children.OfType<FrameworkElement>()
                                                  .Single(e => e.Name == "NewGamePointsComboBox")).SelectedItem as ComboBoxItem)
                                     ?.Content.ToString();
            switch (selectedGameType)
            {
                case GameType.FreeThrowsSingle:
                case GameType.FreeThrowsDouble:
                    return true;
                case GameType.Classic:
                    return selectedGamePoints != "Free";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}