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

            VersionChecker.CheckAndUpdate();

            FileSystemServiceMock.Verify(fss => fss.CheckFileExists(DBService.DatabaseName), Times.Once);
        }
    }
}