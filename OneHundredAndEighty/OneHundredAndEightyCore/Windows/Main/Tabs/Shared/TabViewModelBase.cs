#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Enums;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Windows.CamsDetection;
using OneHundredAndEightyCore.Windows.MessageBox;

#endregion

namespace OneHundredAndEightyCore.Windows.Main.Tabs.Shared
{
    public abstract class TabViewModelBase : INotifyPropertyChanged
    {
        protected TabViewModelBase()
        {
        }

        protected readonly IDbService dbService;
        protected readonly ILogger logger;
        protected readonly IConfigService configService;
        protected readonly DrawService drawService;
        protected readonly IMessageBoxService messageBoxService;
        protected readonly CamsDetectionBoard camsDetectionBoard;
        protected readonly IDetectionService detectionService;
        public DataContext DataContext { get; }
        public HyperLinkNavigateCommand HyperLinkNavigateCommand { get; }

        protected TabViewModelBase(DataContext dataContext,
                                   IDbService dbService,
                                   ILogger logger,
                                   IConfigService configService,
                                   DrawService drawService,
                                   IMessageBoxService messageBoxService,
                                   CamsDetectionBoard camsDetectionBoard,
                                   IDetectionService detectionService)
        {
            DataContext = dataContext;
            this.dbService = dbService;
            this.logger = logger;
            this.configService = configService;
            this.drawService = drawService;
            this.messageBoxService = messageBoxService;
            this.camsDetectionBoard = camsDetectionBoard;
            this.detectionService = detectionService;

            HyperLinkNavigateCommand = new HyperLinkNavigateCommand(OnHyperlinkNavigate);
        }

        protected void LoadPlayers()
        {
            DataContext.Players = Converter.PlayersFromTable(dbService.PlayersLoadAll());
        }

        protected List<CamService> CreateCamsServices()
        {
            var cams = new List<CamService>();
            var cam1Active = configService.Cam1Enabled && !App.NoCams;
            var cam2Active = configService.Cam2Enabled && !App.NoCams;
            var cam3Active = configService.Cam3Enabled && !App.NoCams;
            var cam4Active = configService.Cam4Enabled && !App.NoCams;

            if (cam1Active)
            {
                cams.Add(new CamService(CamNumber._1,
                                        logger,
                                        drawService,
                                        configService));
            }

            if (cam2Active)
            {
                cams.Add(new CamService(CamNumber._2,
                                        logger,
                                        drawService,
                                        configService));
            }

            if (cam3Active)
            {
                cams.Add(new CamService(CamNumber._3,
                                        logger,
                                        drawService,
                                        configService));
            }

            if (cam4Active)
            {
                cams.Add(new CamService(CamNumber._4,
                                        logger,
                                        drawService,
                                        configService));
            }

            return cams;
        }

        private void OnHyperlinkNavigate(Uri uri)
        {
            // so ugly because of https://github.com/dotnet/runtime/issues/28005

            var psi = new ProcessStartInfo
                      {
                          FileName = "cmd",
                          WindowStyle = ProcessWindowStyle.Hidden,
                          UseShellExecute = false,
                          CreateNoWindow = true,
                          Arguments = $"/c start {uri}"
                      };

            Process.Start(psi);
        }

        #region PropertyChangingFire

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}