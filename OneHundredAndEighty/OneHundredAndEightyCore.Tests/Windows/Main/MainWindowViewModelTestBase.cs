#region Usings

using OneHundredAndEightyCore.Windows.Main;

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main
{
    public class MainWindowViewModelTestBase : WindowsTestBase
    {
        protected MainWindowViewModel viewModel;

        protected override void Setup()
        {
            base.Setup();

            viewModel = new MainWindowViewModel(logger.Object,
                                                null,
                                                dbService.Object,
                                                versionChecker.Object,
                                                null,
                                                null,
                                                null,
                                                detectionService.Object,
                                                null,
                                                null,
                                                configService.Object,
                                                null);
        }
    }
}