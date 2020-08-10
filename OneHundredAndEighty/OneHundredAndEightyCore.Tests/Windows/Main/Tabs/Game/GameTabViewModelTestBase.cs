#region Usings

using OneHundredAndEightyCore.Windows.Main.Tabs.Game;

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main.Tabs.Game
{
    public class GameTabViewModelTestBase : WindowsTestBase
    {
        protected GameTabViewModel GameTabViewModel { get; private set; }

        protected override void Setup()
        {
            base.Setup();
            GameTabViewModel = new GameTabViewModel(null,
                                                    DbServiceMock.Object,
                                                    LoggerMock.Object,
                                                    ConfigServiceMock.Object,
                                                    null,
                                                    MessageBoxServiceMock.Object,
                                                    null,
                                                    DetectionServiceMock.Object,
                                                    null,
                                                    null,
                                                    null);
        }
    }
}