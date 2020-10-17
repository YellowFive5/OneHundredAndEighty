#region Usings

using System;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Windows.CamsDetection;
using OneHundredAndEightyCore.Windows.Main.Tabs.Shared;
using OneHundredAndEightyCore.Windows.MessageBox;

#endregion

namespace OneHundredAndEightyCore.Windows.Main.Tabs.Player
{
    public class PlayerTabViewModel : TabViewModelBase
    {
        public PlayerTabViewModel()
        {
        }

        public ChooseNewPlayerAvatarCommand ChooseNewPlayerAvatarCommand { get; }
        public SaveNewPlayerCommand SaveNewPlayerCommand { get; }
        public PlayerStatisticsLoadCommand PlayerStatisticsLoadCommand { get; }

        public PlayerTabViewModel(DataContext dataContext,
                                  ILogger logger,
                                  IConfigService configService,
                                  DrawService drawService,
                                  IDbService dbService,
                                  IMessageBoxService messageBoxService,
                                  CamsDetectionBoard camsDetectionBoard,
                                  IDetectionService detectionService)
            : base(dataContext, dbService, logger, configService, drawService, messageBoxService, camsDetectionBoard, detectionService)
        {
            ChooseNewPlayerAvatarCommand = new ChooseNewPlayerAvatarCommand(SelectAvatarImage);
            SaveNewPlayerCommand = new SaveNewPlayerCommand(SaveNewPlayer);
            PlayerStatisticsLoadCommand = new PlayerStatisticsLoadCommand(PlayerStatisticsLoad);
        }

        #region Bindable props

        private string newPlayerNameText;

        public string NewPlayerNameText
        {
            get => newPlayerNameText;
            set
            {
                newPlayerNameText = value;
                OnPropertyChanged(nameof(NewPlayerNameText));
            }
        }

        private string newPlayerNickNameText;

        public string NewPlayerNickNameText
        {
            get => newPlayerNickNameText;
            set
            {
                newPlayerNickNameText = value;
                OnPropertyChanged(nameof(NewPlayerNickNameText));
            }
        }

        private BitmapImage newPlayerAvatar;

        public BitmapImage NewPlayerAvatar
        {
            get => newPlayerAvatar;
            set
            {
                newPlayerAvatar = value;
                OnPropertyChanged(nameof(NewPlayerAvatar));
            }
        }

        public Domain.Player PlayerForStatisticsBrowse { get; set; }

        private BitmapImage playerForStatisticsAvatar;

        public BitmapImage PlayerForStatisticsAvatar
        {
            get => playerForStatisticsAvatar;
            set
            {
                playerForStatisticsAvatar = value;
                OnPropertyChanged(nameof(PlayerForStatisticsAvatar));
            }
        }

        private string playerForStatisticsBrowseText;

        public string PlayerForStatisticsBrowseText
        {
            get => playerForStatisticsBrowseText;
            set
            {
                playerForStatisticsBrowseText = value;
                OnPropertyChanged(nameof(PlayerForStatisticsBrowseText));
            }
        }

        #endregion

        private void SaveNewPlayer()
        {
            if (!Validator.ValidateNewPlayerNameAndNickName(NewPlayerNameText, NewPlayerNickNameText))
            {
                messageBoxService.ShowWarning(Resources.Resources.NewPlayerEmptyDataErrorText);
                return;
            }

            var newPlayer = new Domain.Player(NewPlayerNameText,
                                              NewPlayerNickNameText,
                                              newPlayerAvatar);
            try
            {
                dbService.PlayerSaveNew(newPlayer);
            }
            catch (Exception e)
            {
                messageBoxService.ShowError(e.Message); // todo need to explain error
                return;
            }

            messageBoxService.ShowInfo(Resources.Resources.NewPlayerSuccessfullySavedText, newPlayer);

            NewPlayerNameText = string.Empty;
            NewPlayerNickNameText = string.Empty;
            NewPlayerAvatar = Converter.BitmapToBitmapImage(Resources.Resources.EmptyUserIcon);

            LoadPlayers();
        }

        private void SelectAvatarImage()
        {
            var ofd = new OpenFileDialog
                      {
                          Title = $"{Resources.Resources.ChoosePlayerAvatarText}",
                          Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif"
                      };
            if (ofd.ShowDialog() == true)
            {
                var image = new BitmapImage(new Uri(ofd.FileName));
                if (Validator.ValidateNewPlayerAvatar(image.PixelHeight,
                                                      image.PixelWidth))
                {
                    NewPlayerAvatar = image;
                }
                else
                {
                    messageBoxService.ShowWarning(Resources.Resources.PlayerAvatarTooBigErrorText);
                }
            }
        }

        public void LoadSettings()
        {
            NewPlayerAvatar = Converter.BitmapToBitmapImage(Resources.Resources.EmptyUserIcon);
            
            PlayerForStatisticsAvatar = Converter.BitmapToBitmapImage(Resources.Resources.EmptyUserIcon);
            PlayerForStatisticsBrowseText = "Choose player for statistics browse";
        }

        private void PlayerStatisticsLoad()
        {
            if (PlayerForStatisticsBrowse != null)
            {
                PlayerForStatisticsAvatar = PlayerForStatisticsBrowse.Avatar;
                PlayerForStatisticsBrowseText = Converter.PlayerStatisticsFromTable(dbService.StatisticsGetForPlayer(PlayerForStatisticsBrowse.Id));
            }
            else
            {
                PlayerForStatisticsAvatar = Converter.BitmapToBitmapImage(Resources.Resources.EmptyUserIcon);
                PlayerForStatisticsBrowseText = "Choose player for statistics browse";
            }
        }
        
        
    }
}