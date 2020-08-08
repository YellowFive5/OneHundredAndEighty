namespace OneHundredAndEightyCore.Tests.Windows.Main.DataContext
{
    public class DataContextTestBase : WindowsTestBase
    {
        protected OneHundredAndEightyCore.Windows.Main.DataContext DataContext { get; set; }

        protected override void Setup()
        {
            base.Setup();

            DataContext = new OneHundredAndEightyCore.Windows.Main.DataContext();
        }
    }
}