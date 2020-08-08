#region Usings

using OneHundredAndEightyCore.Windows.Main.Tabs.About;

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main.Tabs.About
{
    public class AboutTabViewModelTestBase : WindowsTestBase
    {
        protected AboutTabViewModel AboutTabViewModel { get; private set; }

        protected override void Setup()
        {
            base.Setup();
            AboutTabViewModel = new AboutTabViewModel(null,
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