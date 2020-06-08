#region Usings

using Moq;
using NLog;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Tests.ConfigService
{
    public class ConfigServiceTestBase : TestBase
    {
        protected Mock<IDBService> dbService;
        protected Mock<Logger> logger;
        protected Common.ConfigService configService;

        protected override void Setup()
        {
            base.Setup();

            logger = new Mock<Logger>();
            logger.SetupAllProperties();
        }
    }
}