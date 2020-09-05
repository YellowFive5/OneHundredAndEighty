#region Usings

using FluentAssertions;
using Moq;
using NUnit.Framework;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main.MainWindowViewModel
{
    public class WhenOnMainWindowLoaded : MainWindowViewModelTestBase
    {
        protected override void Setup()
        {
            base.Setup();
            PlayersDataTableFromDb.Rows.Add(1, "doesNotMatter", "doesNotMatter", base64ImageString);
            PlayersDataTableFromDb.Rows.Add(2, "doesNotMatter", "doesNotMatter", base64ImageString);

            DbServiceMock.Setup(x => x.PlayersLoadAll()).Returns(PlayersDataTableFromDb);

            DetectionServiceMock.Setup(x => x.FindConnectedCams()).Returns("[HBV HD CAMERA]-[ID:'8&2f223cfb']");
            DetectionServiceMock.SetupAdd(m => m.OnErrorOccurred += e => { });
        }

        private OneHundredAndEightyCore.Windows.Main.MainWindowViewModel CreateViewModel(IConfigService configService)
        {
            return new OneHundredAndEightyCore.Windows.Main.MainWindowViewModel(LoggerMock.Object,
                                                                                MessageBoxServiceMock.Object,
                                                                                DbServiceMock.Object,
                                                                                VersionCheckerMock.Object,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                DetectionServiceMock.Object,
                                                                                null,
                                                                                null,
                                                                                configService);
        }

        [Test]
        public void VersionIsChecked()
        {
            MainWindowViewModel.OnMainWindowLoaded();

            VersionCheckerMock.Verify(v => v.CheckVersions(), Times.Once);
        }

        [Test]
        public void SettingsLoadedForConfigService()
        {
            MainWindowViewModel.OnMainWindowLoaded();

            ConfigServiceMock.Verify(v => v.LoadSettings(), Times.Once);
        }

        [Test]
        public void MainWindowHeightSets()
        {
            ConfigServiceMock.Object.MainWindowHeight = 1555.55;
            MainWindowViewModel = CreateViewModel(ConfigServiceMock.Object);
            MainWindowViewModel.MainWindowHeight = 0.999;

            MainWindowViewModel.OnMainWindowLoaded();

            MainWindowViewModel.MainWindowHeight.Should().Be(1555.55);
        }

        [Test]
        public void MainWindowWidthSets()
        {
            ConfigServiceMock.Object.MainWindowWidth = 1555.55;
            MainWindowViewModel = CreateViewModel(ConfigServiceMock.Object);
            MainWindowViewModel.MainWindowWidth = 0.999;

            MainWindowViewModel.OnMainWindowLoaded();

            MainWindowViewModel.MainWindowWidth.Should().Be(1555.55);
        }

        [Test]
        public void MainWindowPositionLeftSets()
        {
            ConfigServiceMock.Object.MainWindowPositionLeft = 1555.55;
            MainWindowViewModel = CreateViewModel(ConfigServiceMock.Object);
            MainWindowViewModel.MainWindowPositionLeft = 0.999;

            MainWindowViewModel.OnMainWindowLoaded();

            MainWindowViewModel.MainWindowPositionLeft.Should().Be(1555.55);
        }

        [Test]
        public void MainWindowPositionTopSets()
        {
            ConfigServiceMock.Object.MainWindowPositionTop = 1555.55;
            MainWindowViewModel = CreateViewModel(ConfigServiceMock.Object);
            MainWindowViewModel.MainWindowPositionTop = 0.999;

            MainWindowViewModel.OnMainWindowLoaded();

            MainWindowViewModel.MainWindowPositionTop.Should().Be(1555.55);
        }

        [Test]
        public void PlayersSets()
        {
            MainWindowViewModel.OnMainWindowLoaded();

            MainWindowViewModel.DataContext.Players.Should().NotBeNull();
        }

        [Test]
        public void OnErrorOccurredSubscribed()
        {
            MainWindowViewModel.OnMainWindowLoaded();

            DetectionServiceMock.VerifyAdd(m => m.OnErrorOccurred += It.IsAny<DetectionService.ExceptionOccurredDelegate>(), Times.Once);
        }
    }
}