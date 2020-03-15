#region Usings

using System;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
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
            var selectedGameType = Converter.NewGameControlsToGameTypeUi(mainWindowNewGameControls);
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
                case GameTypeUi.FreeThrowsSingle:
                    valid = selectedPlayer1 != null;
                    break;
                case GameTypeUi.FreeThrowsDouble:
                case GameTypeUi.Classic:
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
            var selectedGameType = Converter.NewGameControlsToGameTypeUi(mainWindowNewGameControls);
            switch (selectedGameType)
            {
                case GameTypeUi.FreeThrowsSingle:
                    return true;
                case GameTypeUi.FreeThrowsDouble:
                case GameTypeUi.Classic:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool ValidateStartNewClassicGamePoints(Grid mainWindowNewGameControls)
        {
            var selectedGameType = Converter.NewGameControlsToGameTypeUi(mainWindowNewGameControls);
            var selectedGamePoints = (((ComboBox) mainWindowNewGameControls
                                                  .Children.OfType<FrameworkElement>()
                                                  .Single(e => e.Name == "NewGamePointsComboBox")).SelectedItem as ComboBoxItem)
                                     ?.Content.ToString();
            switch (selectedGameType)
            {
                case GameTypeUi.FreeThrowsSingle:
                case GameTypeUi.FreeThrowsDouble:
                    return true;
                case GameTypeUi.Classic:
                    return selectedGamePoints != "Free";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}