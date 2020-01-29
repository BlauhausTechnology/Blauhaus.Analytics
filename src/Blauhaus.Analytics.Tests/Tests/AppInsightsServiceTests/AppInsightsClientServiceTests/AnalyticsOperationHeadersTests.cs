using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Client.Service;
using Blauhaus.Analytics.Tests.Tests._Base;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.AppInsightsClientServiceTests
{
    [TestFixture]
    public class AnalyticsOperationHeadersTests : BaseAnalyticsServiceTest<AnalyticsClientService>
    {
        protected override AnalyticsClientService ConstructSut()
        {
            return new AnalyticsClientService(
                MockConfig.Object,
                MockConsoleLogger.Object,
                MockTelemetryClient.Object,
                CurrentBuildConfig);
        }

        [Test]
        public void SHOULD_add_operation_and_session_details()
        {
            //Arrange
            var currentOperation = Sut.StartOperation("Operation");
            Sut.CurrentSession.UserId = "userId";
            Sut.CurrentSession.AccountId = "accountId";
            Sut.CurrentSession.DeviceId = "deviceId";
            Sut.CurrentSession.AppVersion = "1.0.2";
            Sut.CurrentSession.Properties["FavouriteColour"] = "Red";
            Sut.CurrentSession.Properties["LuckyNumber"] = "2";
            
            //Act
            var result = Sut.AnalyticsOperationHeaders;

            //Assert
            Assert.That(result[AnalyticsHeaders.Operation.Id], Is.EqualTo(currentOperation.Id));
            Assert.That(result[AnalyticsHeaders.Operation.Name], Is.EqualTo("Operation"));
            Assert.That(result[AnalyticsHeaders.Session.Id], Is.EqualTo(Sut.CurrentSession.Id));
            Assert.That(result[AnalyticsHeaders.Session.AccountId], Is.EqualTo("accountId"));
            Assert.That(result[AnalyticsHeaders.Session.UserId], Is.EqualTo("userId"));
            Assert.That(result[AnalyticsHeaders.Session.DeviceId], Is.EqualTo("deviceId"));
            Assert.That(result[AnalyticsHeaders.Session.AppVersion], Is.EqualTo("1.0.2"));
            Assert.That(result[AnalyticsHeaders.Prefix + "FavouriteColour"], Is.EqualTo("Red"));
            Assert.That(result[AnalyticsHeaders.Prefix + "LuckyNumber"], Is.EqualTo("2"));
        }

        [Test]
        public void IF_property_is_null_SHOULD_exclude_it()
        {
            //Arrange
            Sut.CurrentSession.UserId = null;
            
            //Act
            var result = Sut.AnalyticsOperationHeaders;

            //Assert
            Assert.That(result.ContainsKey(AnalyticsHeaders.Session.UserId), Is.False);
        }
    }
}