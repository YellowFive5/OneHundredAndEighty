#region Usings

using OneHundredAndEightyCore.Windows.Main.Tabs.Player;

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main.Tabs.Player
{
    public class PlayerTabViewModelTestBase : WindowsTestBase
    {
        protected PlayerTabViewModel PlayerTabViewModel { get; private set; }

        protected override void Setup()
        {
            base.Setup();
            PlayerTabViewModel = new PlayerTabViewModel(null,
                                                        LoggerMock.Object,
                                                        ConfigServiceMock.Object,
                                                        null,
                                                        DbServiceMock.Object,
                                                        MessageBoxServiceMock.Object,
                                                        null,
                                                        DetectionServiceMock.Object);
        }
    }
}