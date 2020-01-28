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
            
            //Act
            var result = Sut.AnalyticsOperationHeaders;

            //Assert
            result.TryGetValue(AnalyticsHeaders.OperationId, out var operationId);
            Assert.That(operationId, Is.EqualTo(currentOperation.Id));
            result.TryGetValue(AnalyticsHeaders.OperationName, out var operationName);
            Assert.That(operationName, Is.EqualTo("Operation"));
            result.TryGetValue(AnalyticsHeaders.SessionId, out var sessionId);
            Assert.That(sessionId, Is.EqualTo(Sut.CurrentSessionId));
        }
    }
}