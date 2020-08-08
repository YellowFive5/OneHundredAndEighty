#region Usings

using System.Windows.Media.Imaging;
using FluentAssertions;
using NUnit.Framework;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main.Tabs.Player
{
    public class WhenSettingBindableProperties : PlayerTabViewModelTestBase
    {
        private NotifyPropertyChangedTester tester;

        protected override void Setup()
        {
            base.Setup();
            tester = new NotifyPropertyChangedTester(PlayerTabViewModel);
        }

        [Test]
        public void NewPlayerAvatarLoaded()
        {
            PlayerTabViewModel.NewPlayerAvatar = null;

            PlayerTabViewModel.LoadSettings();

            PlayerTabViewModel.NewPlayerAvatar.Should().NotBeNull();
        }

        [Test]
        public void NewPlayerNameTextSetsAndChangeFired()
        {
            var oldValue = PlayerTabViewModel.NewPlayerNameText;

            PlayerTabViewModel.NewPlayerNameText = "SomeName";
            var newValue = PlayerTabViewModel.NewPlayerNameText;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("SomeName");
            tester.AssertOnPropertyChangedInvoke(0, nameof(PlayerTabViewModel.NewPlayerNameText));
        }

        [Test]
        public void NewPlayerNickNameTextSetsAndChangeFired()
        {
            var oldValue = PlayerTabViewModel.NewPlayerNickNameText;

            PlayerTabViewModel.NewPlayerNickNameText = "SomeNickName";
            var newValue = PlayerTabViewModel.NewPlayerNickNameText;

            newValue.Should().NotBe(oldValue);
            newValue.Should().Be("SomeNickName");
            tester.AssertOnPropertyChangedInvoke(0, nameof(PlayerTabViewModel.NewPlayerNickNameText));
        }

        [Test]
        public void NewPlayerAvatarSetsAndChangeFired()
        {
            var oldValue = PlayerTabViewModel.NewPlayerAvatar;

            PlayerTabViewModel.NewPlayerAvatar = new BitmapImage();
            var newValue = PlayerTabViewModel.NewPlayerAvatar;

            newValue.Should().NotBe(oldValue);
            tester.AssertOnPropertyChangedInvoke(0, nameof(PlayerTabViewModel.NewPlayerAvatar));
        }
    }
}