using System;
using Blauhaus.AppInsights.Abstractions.Operation;
using Blauhaus.AppInsights.Client.Service;
using Blauhaus.AppInsights.Server.Service;
using Blauhaus.AppInsights.Tests.Tests._Base;
using Blauhaus.AppInsights.Tests.Tests.AppInsightsServiceTests._BaseTests;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.AppInsights.Tests.Tests.AppInsightsServiceTests.AppInsightsServerServiceTests
{
    public class StartRequestOperationTests : BaseAppInsightsTest<AppInsightsServerService>
    {
        protected override AppInsightsServerService ConstructSut()
        {
            return new AppInsightsServerService(
                MockConfig.Object,
                MockConsoleLogger.Object,
                MockTelemetryClient.Object);
        }

        [Test]
        public void SHOULD_set_and_return_CurrentOperation()
        {
            //Arrange
            var operationId = Guid.NewGuid().ToString();
            var sessionId = Guid.NewGuid().ToString();

            //Act
            var operation = Sut.StartRequestOperation("RequestName", operationId, "MyOperation", sessionId);

            //Assert
            Assert.That(operation.Name, Is.EqualTo("MyOperation"));
            Assert.That(operation.Id, Is.EqualTo(operationId));
            Assert.That(Sut.CurrentOperation.Name, Is.EqualTo("MyOperation"));
            Assert.That(Sut.CurrentOperation.Id, Is.EqualTo(operationId));
        }

        [Test]
        public void WHEN_Operation_is_disposed_SHOULD_track_dependency()
        {
            //Arrange
            //Arrange
            var operationId = Guid.NewGuid().ToString();
            var sessionId = Guid.NewGuid().ToString();
            var operation = Sut.StartRequestOperation("RequestName", operationId, "MyOperation", sessionId);
            MockTelemetryClient.Mock.Verify(x => x.TrackRequest(It.IsAny<RequestTelemetry>()), Times.Never);

            //Act
            operation.Dispose();
            
            //Assert
            MockTelemetryClient.Mock.Verify(x => x.UpdateOperation(It.Is<IAnalyticsOperation>(y => 
                y.Id == operation.Id &&
                y.Name == "MyOperation"), Sut.CurrentSessionId));
            MockTelemetryClient.Mock.Verify(x => x.TrackRequest(It.Is<RequestTelemetry>(y => 
                y.Name == "RequestName")));
        }
    }
}