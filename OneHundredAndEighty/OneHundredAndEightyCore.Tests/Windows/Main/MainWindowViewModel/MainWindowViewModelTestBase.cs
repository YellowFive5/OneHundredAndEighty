#region Usings

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main.MainWindowViewModel
{
    public class MainWindowViewModelTestBase : WindowsTestBase
    {
        protected OneHundredAndEightyCore.Windows.Main.MainWindowViewModel MainWindowViewModel { get; set; }

        protected override void Setup()
        {
            base.Setup();

            MainWindowViewModel = new OneHundredAndEightyCore.Windows.Main.MainWindowViewModel(LoggerMock.Object,
                                                                                               null,
                                                                                               DbServiceMock.Object,
                                                                                               VersionCheckerMock.Object,
                                                                                               null,
                                                                                               null,
                                                                                               null,
                                                                                               DetectionServiceMock.Object,
                                                                                               null,
                                                                                               null,
                                                                                               ConfigServiceMock.Object);
        }
    }
}