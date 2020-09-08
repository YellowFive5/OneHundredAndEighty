#region Usings

using System;
using Moq;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Tests.VersionChecker
{
    public class VersionCheckerTestBase : TestBase
    {
        protected Common.VersionChecker VersionChecker { get; private set; }
        private readonly Version DoesNotMatterVersion = new Version(2, 0);

        protected void CreateVersionChecker(Version currentAppVersion = null)
        {
            VersionChecker = new Common.VersionChecker(currentAppVersion ?? DoesNotMatterVersion,
                                                       FileSystemServiceMock.Object,
                                                       DbServiceMock.Object,
                                                       ConfigServiceMock.Object,
                                                       MessageBoxServiceMock.Object);
        }

        protected override void Setup()
        {
            base.Setup();
            
            DbServiceMock.Setup(ds => ds.SettingsGetValue(It.Is<SettingsType>(st => st == SettingsType.DBVersion)))
                         .Returns("2.0");
        }
    }
}