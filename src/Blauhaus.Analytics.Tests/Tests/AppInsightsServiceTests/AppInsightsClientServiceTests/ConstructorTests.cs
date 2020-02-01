using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Client.Service;
using Blauhaus.Analytics.Tests.Tests._Base;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.AppInsightsClientServiceTests
{
    public class ConstructorTests : BaseAnalyticsServiceTest<AnalyticsClientService>
    {
        protected override AnalyticsClientService ConstructSut()
        {
            return new AnalyticsClientService(
                MockConfig.Object,
                MockConsoleLogger.Object,
                MockTelemetryClient.Object,
                MockTelemetryDecorator.Object,
                CurrentBuildConfig,
                MockDeviceInfoService.Object,
                MockApplicationInfoService.Object);
        }
        
        [Test]
        public void SHOULD_set_session_properties_from_device()
        {
            //Arrange
            MockDeviceInfoService.With(x => x.DeviceUniqueIdentifier, "deviceId");
            MockApplicationInfoService.With(x => x.CurrentVersion, "12.2");

            //Assert
            Assert.That(Sut.CurrentSession.AppVersion, Is.EqualTo("12.2"));
            Assert.That(Sut.CurrentSession.DeviceId, Is.EqualTo("deviceId"));
        }

    }
}