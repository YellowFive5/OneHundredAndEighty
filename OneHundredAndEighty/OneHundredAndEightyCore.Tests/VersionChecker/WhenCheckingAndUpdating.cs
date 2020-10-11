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

            FileSystemServiceMock.Verify(fss => fss.CheckFileExists(DbService.DatabaseName), Times.Once);
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
        public void ErrorShowsWhenAppVersionIsGreaterThatDbVersionAndNoConfirm()
        {
            var appVersion = new Version(3, 1);
            var currentDbVersion = "2.0";
            CreateVersionChecker(appVersion);
            DbServiceMock.Setup(ds => ds.SettingsGetValue(SettingsType.DBVersion)).Returns(currentDbVersion);
            MessageBoxServiceMock.Setup(mbs => mbs.AskWarningQuestion(It.IsAny<string>(),
                                                                      It.IsAny<object>(),
                                                                      It.IsAny<object>())).Returns(false);

            InvokeAndSwallowException(() => VersionChecker.CheckAndUpdate());

            MessageBoxServiceMock.Verify(mbs => mbs.ShowError(Resources.Resources.VersionsMismatchErrorText,
                                                              appVersion.ToString(),
                                                              currentDbVersion),
                                         Times.Once);
        }

        [Test]
        public void ErrorThrowsWhenAppVersionIsGreaterThatDbVersionAndNoConfirm()
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
        }

        [Test]
        public void ErrorShowsWhenDbVersionIsGreaterThatAppVersion()
        {
            var appVersion = new Version(2, 0);
            var currentDbVersion = "3.1";
            CreateVersionChecker(appVersion);
            DbServiceMock.Setup(ds => ds.SettingsGetValue(SettingsType.DBVersion)).Returns(currentDbVersion);

            InvokeAndSwallowException(() => VersionChecker.CheckAndUpdate());

            MessageBoxServiceMock.Verify(mbs => mbs.ShowError(Resources.Resources.VersionsMismatchDbVersionGreaterErrorText,
                                                              currentDbVersion,
                                                              appVersion.ToString()),
                                         Times.Once);
        }

        [Test]
        public void ErrorThrowsWhenDbVersionIsGreaterThatAppVersion()
        {
            var appVersion = new Version(2, 0);
            var currentDbVersion = "3.1";
            CreateVersionChecker(appVersion);
            DbServiceMock.Setup(ds => ds.SettingsGetValue(SettingsType.DBVersion)).Returns(currentDbVersion);

            Action act = () => { VersionChecker.CheckAndUpdate(); };

            act.Should().Throw<Exception>();
        }

        [Test]
        public void CopyOfOldDbCreatesWhenDoingMigrations()
        {
            var appVersion = new Version(2, 1);
            var currentDbVersion = "2.0";
            CreateVersionChecker(appVersion);
            DbServiceMock.Setup(ds => ds.SettingsGetValue(SettingsType.DBVersion)).Returns(currentDbVersion);
            MessageBoxServiceMock.Setup(mbs => mbs.AskWarningQuestion(It.IsAny<string>(),
                                                                      It.IsAny<object>(),
                                                                      It.IsAny<object>())).Returns(true);

            VersionChecker.CheckAndUpdate();

            FileSystemServiceMock.Verify(fss => fss.CreateFileCopy(DbService.DatabaseName,
                                                                   DbService.DatabaseCopyName,
                                                                   true),
                                         Times.Once);
        }

        [Test]
        public void CopyOfOldDbDeletesWhenMigrationsSuccessfulDone()
        {
            var appVersion = new Version(2, 1);
            var currentDbVersion = "2.0";
            CreateVersionChecker(appVersion);
            DbServiceMock.Setup(ds => ds.SettingsGetValue(SettingsType.DBVersion)).Returns(currentDbVersion);
            MessageBoxServiceMock.Setup(mbs => mbs.AskWarningQuestion(It.IsAny<string>(),
                                                                      It.IsAny<object>(),
                                                                      It.IsAny<object>())).Returns(true);

            VersionChecker.CheckAndUpdate();

            FileSystemServiceMock.Verify(fss => fss.DeleteFile(DbService.DatabaseCopyName),
                                         Times.Once);
        }

        [Test]
        public void InfoShowsWhenMigrationsSuccessfulDone()
        {
            var appVersion = new Version(2, 1);
            var currentDbVersion = "2.0";
            CreateVersionChecker(appVersion);
            DbServiceMock.Setup(ds => ds.SettingsGetValue(SettingsType.DBVersion)).Returns(currentDbVersion);
            MessageBoxServiceMock.Setup(mbs => mbs.AskWarningQuestion(It.IsAny<string>(),
                                                                      It.IsAny<object>(),
                                                                      It.IsAny<object>())).Returns(true);

            VersionChecker.CheckAndUpdate();

            MessageBoxServiceMock.Verify(mbs => mbs.ShowInfo(Resources.Resources.SuccessDbMigrationText,
                                                             currentDbVersion,
                                                             appVersion.ToString()),
                                         Times.Once);
        }

        [Test]
        public void DbRevertedOnMigrationError()
        {
            var appVersion = new Version(2, 1);
            var currentDbVersion = "2.0";
            CreateVersionChecker(appVersion);
            DbServiceMock.Setup(ds => ds.SettingsGetValue(SettingsType.DBVersion)).Returns(currentDbVersion);
            DbServiceMock.Setup(ds => ds.MigrateFrom2_0to2_1()).Throws<Exception>();
            MessageBoxServiceMock.Setup(mbs => mbs.AskWarningQuestion(It.IsAny<string>(),
                                                                      It.IsAny<object>(),
                                                                      It.IsAny<object>())).Returns(true);

            InvokeAndSwallowException(() => VersionChecker.CheckAndUpdate());

            FileSystemServiceMock.Verify(fss => fss.CreateFileCopy(DbService.DatabaseCopyName,
                                                                   DbService.DatabaseName,
                                                                   true),
                                         Times.Once);
            FileSystemServiceMock.Verify(fss => fss.DeleteFile(DbService.DatabaseCopyName),
                                         Times.Once);
        }

        [Test]
        public void ErrorShowsOnMigrationError()
        {
            var appVersion = new Version(2, 1);
            var currentDbVersion = "2.0";
            CreateVersionChecker(appVersion);
            DbServiceMock.Setup(ds => ds.SettingsGetValue(SettingsType.DBVersion)).Returns(currentDbVersion);
            DbServiceMock.Setup(ds => ds.MigrateFrom2_0to2_1()).Throws<Exception>();
            MessageBoxServiceMock.Setup(mbs => mbs.AskWarningQuestion(It.IsAny<string>(),
                                                                      It.IsAny<object>(),
                                                                      It.IsAny<object>())).Returns(true);

            InvokeAndSwallowException(() => VersionChecker.CheckAndUpdate());

            MessageBoxServiceMock.Verify(mbs => mbs.ShowError(Resources.Resources.ErrorDbMigrationText,
                                                              currentDbVersion,
                                                              appVersion.ToString()),
                                         Times.Once);
        }

        [Test]
        public void ErrorThrowsOnMigrationError()
        {
            var appVersion = new Version(2, 1);
            var currentDbVersion = "2.0";
            CreateVersionChecker(appVersion);
            DbServiceMock.Setup(ds => ds.SettingsGetValue(SettingsType.DBVersion)).Returns(currentDbVersion);
            DbServiceMock.Setup(ds => ds.MigrateFrom2_0to2_1()).Throws<Exception>();
            MessageBoxServiceMock.Setup(mbs => mbs.AskWarningQuestion(It.IsAny<string>(),
                                                                      It.IsAny<object>(),
                                                                      It.IsAny<object>())).Returns(true);

            Action act = () => { VersionChecker.CheckAndUpdate(); };

            act.Should().Throw<Exception>();
        }
    }
}