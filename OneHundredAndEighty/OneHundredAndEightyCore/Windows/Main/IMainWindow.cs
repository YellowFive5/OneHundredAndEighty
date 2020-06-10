#region Usings

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Autofac;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Windows.Main
{
    public interface IMainWindow
    {
        Dispatcher Dispatcher { get; }
        IContainer ServiceContainer { get; }
        void ToggleMainTabItemsEnabled();
        void ToggleMatchControlsEnabled();

        void SetCalibratedCamsControls(PointF calibratedCam1SetupPoint,
                                       PointF calibratedCam2SetupPoint,
                                       PointF calibratedCam3SetupPoint,
                                       PointF calibratedCam4SetupPoint);

        void SetSelectedAvatar(BitmapImage image);
        void ToggleCamSetupGridControlsEnabled(CamNumber camNumber);
        void ToggleSetupTabItemsEnabled();
        void ToggleCrossingButtonsEnabled();
        void SetConnectedCamsText(string text);
        void ToggleConnectedCamsControls();
        void ClearNewPlayerControls();

        void SetCamImages(CamNumber number,
                          BitmapImage image,
                          BitmapImage roiImage);

        void SavePosition(ConfigService configService);
        void SaveAllData(ConfigService configService);
        void LoadAllData(ConfigService configService);
        List<double> GetCamsSetupSlidersData(CamNumber camNumber);
    }
}