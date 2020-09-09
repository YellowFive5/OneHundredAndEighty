#region Usings

using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Tests.VersionChecker
{
    public class WhenCheckingAndUpdating : VersionCheckerTestBase
    {
        protected override void Setup()
        {
            base.Setup();
            FileSystemServiceMock.Setup(fss => fss.CheckFileExists(It.IsAny<string>())).Returns(true);
        }

        [Test]
        public void DbExistsChecks()
        {
            CreateVersionChecker();

            VersionChecker.CheckAndUpdate();

            FileSystemServiceMock.Verify(fss => fss.CheckFileExists(DBService.DatabaseName), Times.Once);
        }

        [Test]
        public void WarningQuestionAskedWhenAppVersionIsGreaterThatDbVersion()
        {
            var appVersion = new Version(3, 1);
            var currentDbVersion = "2.0";
            CreateVersionChecker(appVersion);
            DbServiceMock.Setup(ds => ds.SettingsGetValue(SettingsType.DBVersion)).Returns(currentDbVersion);
            MessageBoxServiceMock.Setup(mbs => mbs.AskWarningQuestion(It.IsAny<string>(),
                                                                      It.IsAny<object>(),
                                                                      It.IsAny<object>())).Returns(true);

            VersionChecker.CheckAndUpdate();

            MessageBoxServiceMock.Verify(mbs => mbs.AskWarningQuestion(Resources.Resources.VersionsMismatchWarningQuestionText,
                                                                       currentDbVersion,
                                                                       appVersion.ToString()),
                                         Times.Once);
        }

        [Test]
        public void ErrorShowsAndThrowsWhenAppVersionIsGreaterThatDbVersionAndNoConfirm()
        {
            var appVersion = new Version(3, 1);
            var currentDbVersion = "2.0";
            CreateVersionChecker(appVersion);
            DbServiceMock.Setup(ds => ds.SettingsGetValue(SettingsType.DBVersion)).Returns(currentDbVersion);
            MessageBoxServiceMock.Setup(mbs => mbs.AskWarningQuestion(It.IsAny<string>(),
                                                                      It.IsAny<object>(),
                                                                      It.IsAny<object>())).Returns(false);

            Action act = () => { VersionChecker.CheckAndUpdate(); };
            act.Should().Throw<Exception>();
            MessageBoxServiceMock.Verify(mbs => mbs.ShowError(Resources.Resources.VersionsMismatchErrorText,
                                                              appVersion.ToString(),
                                                              currentDbVersion),
                                         Times.Once);
        }

        [Test]
        public void ErrorShowsAndThrowsWhenDbVersionIsGreaterThatAppVersion()
        {
            var appVersion = new Version(2, 0);
            var currentDbVersion = "3.1";
            CreateVersionChecker(appVersion);
            DbServiceMock.Setup(ds => ds.SettingsGetValue(SettingsType.DBVersion)).Returns(currentDbVersion);

            Action act = () => { VersionChecker.CheckAndUpdate(); };

            act.Should().Throw<Exception>();
            MessageBoxServiceMock.Verify(mbs => mbs.ShowError(Resources.Resources.VersionsMismatchDbVersionGreaterErrorText,
                                                              currentDbVersion,
                                                              appVersion.ToString()),
                                         Times.Once);
        }
    }
}