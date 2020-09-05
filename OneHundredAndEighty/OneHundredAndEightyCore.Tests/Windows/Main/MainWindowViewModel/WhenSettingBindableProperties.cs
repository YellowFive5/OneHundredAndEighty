#region Usings

using FluentAssertions;
using NUnit.Framework;

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main.MainWindowViewModel
{
    public class WhenSettingBindableProperties : MainWindowViewModelTestBase
    {
        private NotifyPropertyChangedTester tester;

        protected override void Setup()
        {
            base.Setup();
            tester = new NotifyPropertyChangedTester(MainWindowViewModel);
        }

        [Test]
        public void MainWindowPositionLeftSetsAndChangeFired()
        {
            var oldValue = MainWindowViewModel.MainWindowPositionLeft;

            MainWindowViewModel.MainWindowPositionLeft = 555.05;
            var newValue = MainWindowViewModel.MainWindowPositionLeft;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(MainWindowViewModel.MainWindowPositionLeft));
        }

        [Test]
        public void MainWindowPositionTopSetsAndChangeFired()
        {
            var oldValue = MainWindowViewModel.MainWindowPositionTop;

            MainWindowViewModel.MainWindowPositionTop = 555.05;
            var newValue = MainWindowViewModel.MainWindowPositionTop;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(MainWindowViewModel.MainWindowPositionTop));
        }

        [Test]
        public void MainWindowHeightSetsAndChangeFired()
        {
            var oldValue = MainWindowViewModel.MainWindowHeight;

            MainWindowViewModel.MainWindowHeight = 555.05;
            var newValue = MainWindowViewModel.MainWindowHeight;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(MainWindowViewModel.MainWindowHeight));
        }

        [Test]
        public void MainWindowWidthSetsAndChangeFired()
        {
            var oldValue = MainWindowViewModel.MainWindowWidth;

            MainWindowViewModel.MainWindowWidth = 555.05;
            var newValue = MainWindowViewModel.MainWindowWidth;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be(555.05);
            tester.AssertOnPropertyChangedInvoke(0, nameof(MainWindowViewModel.MainWindowWidth));
        }
        
    }
}