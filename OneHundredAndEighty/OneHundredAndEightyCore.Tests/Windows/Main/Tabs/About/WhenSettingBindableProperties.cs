namespace OneHundredAndEightyCore.Tests.Windows.Main.Tabs.About
{
    public class WhenSettingBindableProperties : AboutTabViewModelTestBase
    {
        private NotifyPropertyChangedTester tester;

        protected override void Setup()
        {
            base.Setup();
            tester = new NotifyPropertyChangedTester(AboutTabViewModel);
        }
    }
}