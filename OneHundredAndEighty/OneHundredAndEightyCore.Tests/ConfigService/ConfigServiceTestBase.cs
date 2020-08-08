#region Usings

#endregion

namespace OneHundredAndEightyCore.Tests.ConfigService
{
    public class ConfigServiceTestBase : TestBase
    {
        protected Common.ConfigService configService;

        protected override void Setup()
        {
            base.Setup();
            configService = new Common.ConfigService(LoggerMock.Object,
                                                     DbServiceMock.Object);
        }
    }
}