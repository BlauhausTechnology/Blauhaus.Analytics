using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Client.Service;
using Blauhaus.Analytics.Tests.Tests._Base;
using Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests._BaseTests;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.AppInsightsClientServiceTests
{
    [TestFixture]
    public class AddAnalyticsHeadersTests : BaseAnalyticsServiceTest<AnalyticsClientService>
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
            var headers = new HttpRequestMessage().Headers;
            
            //Act
            var result = Sut.AddAnalyticsHeaders(headers);

            //Assert
            result.TryGetValues(AnalyticsHeaders.OperationId, out var operationId);
            Assert.That(operationId.First(), Is.EqualTo(currentOperation.Id));
            result.TryGetValues(AnalyticsHeaders.OperationName, out var operationName);
            Assert.That(operationName.First(), Is.EqualTo("Operation"));
            result.TryGetValues(AnalyticsHeaders.SessionId, out var sessionId);
            Assert.That(sessionId.First(), Is.EqualTo(Sut.CurrentSessionId));
        }
    }
}