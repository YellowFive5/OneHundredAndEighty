#region Usings

using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Windows.CamsDetection;
using OneHundredAndEightyCore.Windows.Main.Tabs.Shared;
using OneHundredAndEightyCore.Windows.MessageBox;

#endregion

namespace OneHundredAndEightyCore.Windows.Main.Tabs.About
{
    public class AboutTabViewModel : TabViewModelBase
    {
        public AboutTabViewModel()
        {
        }

        public AboutTabViewModel(DataContext dataContext,
                                 IDbService dbService,
                                 ILogger logger,
                                 IConfigService configService,
                                 DrawService drawService,
                                 IMessageBoxService messageBoxService,
                                 CamsDetectionBoard camsDetectionBoard,
                                 IDetectionService detectionService)
            : base(dataContext, dbService, logger, configService, drawService, messageBoxService, camsDetectionBoard, detectionService)
        {
        }
    }
}