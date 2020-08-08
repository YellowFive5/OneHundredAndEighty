#region Usings

using OneHundredAndEightyCore.Windows.Main.Tabs.Settings;

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main.Tabs.Settings
{
    public class SettingsTabViewModelTestBase : WindowsTestBase
    {
        protected SettingsTabViewModel SettingsTabViewModel { get; private set; }

        protected override void Setup()
        {
            base.Setup();
            SettingsTabViewModel = new SettingsTabViewModel(null,
                                                            DbServiceMock.Object,
                                                            LoggerMock.Object,
                                                            ConfigServiceMock.Object,
                                                            null,
                                                            MessageBoxServiceMock.Object,
                                                            null,
                                                            DetectionServiceMock.Object);
        }
    }
}