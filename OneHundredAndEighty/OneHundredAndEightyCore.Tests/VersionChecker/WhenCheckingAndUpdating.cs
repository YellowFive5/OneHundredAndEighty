#region Usings

using Moq;
using NUnit.Framework;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Tests.VersionChecker
{
    public class WhenCheckingAndUpdating : VersionCheckerTestBase
    {
        [Test]
        public void DbExistsChecks()
        {
            CreateVersionChecker();
            FileSystemServiceMock.Setup(fss => fss.CheckFileExists(It.IsAny<string>())).Returns(true);

            VersionChecker.CheckAndUpdate();

            FileSystemServiceMock.Verify(fss => fss.CheckFileExists(DBService.DatabaseName), Times.Once);
        }
    }
}