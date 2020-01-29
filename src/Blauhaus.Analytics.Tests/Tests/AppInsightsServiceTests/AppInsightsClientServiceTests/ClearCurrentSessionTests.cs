using Blauhaus.Analytics.Client.Service;
using Blauhaus.Analytics.Tests.Tests._Base;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.AppInsightsClientServiceTests
{
    [TestFixture]
    public class ClearCurrentSessionTests : BaseAnalyticsServiceTest<AnalyticsClientService>
    {
        protected override AnalyticsClientService ConstructSut()
        {
            return new AnalyticsClientService(
                MockConfig.Object,
                MockConsoleLogger.Object,
                MockTelemetryClient.Object,
                CurrentBuildConfig,
                MockDeviceInfoService.Object,
                MockApplicationInfoService.Object);
        }

        [Test]
        public void SHOULD_nullify_CurrentOperation()
        {
            //Arrange
            Sut.StartPageViewOperation("MyOperation");

            //Act
            Sut.ClearCurrentSession();

            //Assert
            Assert.That(Sut.CurrentOperation, Is.Null);
        }

        [Test]
        public void SHOULD_clear_Current_session_but_keep_same_id()
        {
            //Arrange
            Sut.StartOperation("Operation");
            Sut.CurrentSession.UserId = "userId";
            Sut.CurrentSession.AccountId = "accountId";
            Sut.CurrentSession.DeviceId = "deviceId";
            Sut.CurrentSession.AppVersion = "1.0.2";
            Sut.CurrentSession.SetProperty("FavouriteColour", "Red");
            Sut.CurrentSession.SetProperty("LuckyNumber", "2");
            var originalSessionId = Sut.CurrentSession.Id;

            //Act
            Sut.ClearCurrentSession();
            
            //Assert
            Assert.That(Sut.CurrentSession.Id, Is.EqualTo(originalSessionId));
            Assert.That(Sut.CurrentSession.UserId, Is.Null);
            Assert.That(Sut.CurrentSession.AccountId, Is.Null);
            Assert.That(Sut.CurrentSession.DeviceId, Is.Null);
            Assert.That(Sut.CurrentSession.AppVersion, Is.Null);
            Assert.That(Sut.CurrentSession.Properties.Count, Is.EqualTo(0));
        }

    }
}