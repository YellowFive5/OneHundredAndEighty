#region Usings

using Moq;
using NUnit.Framework;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Tests.ConfigService
{
    public class WhenWrite : ConfigServiceTestBase
    {
        protected override void Setup()
        {
            base.Setup();

            dbService = new Mock<IDBService>();
            dbService.Setup(x => x.SettingsSetValue(It.IsAny<SettingsType>(),
                                                    It.IsAny<string>()));
            configService = new Common.ConfigService(logger.Object,
                                                     dbService.Object);
        }

        [Test]
        public void DbServiceMethodCallsWithPassedArguments()
        {
            var settingsType = SettingsType.Cam1Id;
            var value = 5;

            configService.Write(settingsType, value);

            dbService.Verify(m => m.SettingsSetValue(settingsType, value.ToString()));
        }
    }
}