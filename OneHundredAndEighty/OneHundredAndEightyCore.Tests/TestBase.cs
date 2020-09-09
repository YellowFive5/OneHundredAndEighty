#region Usings

using System.Data;
using Moq;
using NLog;
using NUnit.Framework;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Enums;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.Windows.MessageBox;

#endregion

namespace OneHundredAndEightyCore.Tests
{
    public class TestBase
    {
        protected DataTable PlayersDataTableFromDb { get; private set; }
        protected Mock<ILogger> LoggerMock { get; private set; }
        protected Mock<IMessageBoxService> MessageBoxServiceMock { get; private set; }
        protected Mock<IDbService> DbServiceMock { get; private set; }
        protected Mock<IVersionChecker> VersionCheckerMock { get; private set; }
        protected Mock<IConfigService> ConfigServiceMock { get; private set; }
        protected Mock<IDetectionService> DetectionServiceMock { get; private set; }
        protected Mock<IFileSystemService> FileSystemServiceMock { get; private set; }

        [SetUp]
        protected virtual void Setup()
        {
            LoggerMock = new Mock<ILogger>();
            LoggerMock.SetupAllProperties();

            MessageBoxServiceMock = new Mock<IMessageBoxService>();
            MessageBoxServiceMock.SetupAllProperties();

            DbServiceMock = new Mock<IDbService>();
            DbServiceMock.Setup(x => x.SettingsSetValue(It.IsAny<SettingsType>(),
                                                        It.IsAny<string>()));
            VersionCheckerMock = new Mock<IVersionChecker>();
            VersionCheckerMock.SetupAllProperties();

            ConfigServiceMock = new Mock<IConfigService>();
            ConfigServiceMock.SetupAllProperties();

            DetectionServiceMock = new Mock<IDetectionService>();
            DetectionServiceMock.SetupAllProperties();

            FileSystemServiceMock = new Mock<IFileSystemService>();
            FileSystemServiceMock.SetupAllProperties();

            PlayersDataTableFromDb = new DataTable();
            PlayersDataTableFromDb.Columns.Add(Column.Id.ToString(), typeof(int));
            PlayersDataTableFromDb.Columns.Add(Column.Name.ToString(), typeof(string));
            PlayersDataTableFromDb.Columns.Add(Column.NickName.ToString(), typeof(string));
            PlayersDataTableFromDb.Columns.Add(Column.Avatar.ToString(), typeof(string));
        }
    }
}