using System;
using System.Collections.Generic;
using System.Net.Http;
using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Server.Service;
using Blauhaus.Analytics.Tests.Tests._Base;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.AppInsightsServerServiceTests
{
    public class StartRequestOperationTests : BaseAnalyticsServiceTest<AnalyticsServerService>
    {
        protected override AnalyticsServerService ConstructSut()
        {
            return new AnalyticsServerService(
                MockConfig.Object,
                MockConsoleLogger.Object,
                MockTelemetryClient.Object,
                CurrentBuildConfig);
        }

        public class GivenValues : StartRequestOperationTests
        {
            
            [Test]
            public void SHOULD_set_and_return_CurrentOperation()
            {
                //Arrange
                var operationId = Guid.NewGuid().ToString();
                var sessionId = Guid.NewGuid().ToString();

                //Act
                var operation = Sut.StartRequestOperation("RequestName", "MyOperation", operationId, sessionId);

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
                var operation = Sut.StartRequestOperation("RequestName", "MyOperation", operationId, sessionId);
                MockTelemetryClient.Mock.Verify(x => x.TrackRequest(It.IsAny<RequestTelemetry>()), Times.Never);

                //Act
                operation.Dispose();
            
                //Assert
                MockTelemetryClient.Mock.Verify(x => x.UpdateOperation(It.Is<IAnalyticsOperation>(y => 
                    y.Id == operation.Id &&
                    y.Name == "MyOperation"), Sut.CurrentSessionId));
                MockTelemetryClient.Mock.Verify(x => x.TrackRequest(It.Is<RequestTelemetry>(y => 
                    y.Name == "RequestName")));
                MockConsoleLogger.Mock.Verify(x => x.LogOperation("RequestName", It.IsAny<TimeSpan>()));
            }
        }

        
        public class GivenHttpHEaders : StartRequestOperationTests
        {
            
            [Test]
            public void SHOULD_set_and_return_CurrentOperation()
            {
                //Arrange
                var operationId = Guid.NewGuid().ToString();
                var sessionId = Guid.NewGuid().ToString();
                var headers = new Dictionary<string, string>();
                headers.Add(AnalyticsHeaders.OperationName, "MyOperation");
                headers.Add(AnalyticsHeaders.OperationId, operationId);
                headers.Add(AnalyticsHeaders.SessionId, sessionId);

                //Act
                var operation = Sut.StartRequestOperation("RequestName", headers);

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
                var operationId = Guid.NewGuid().ToString();
                var sessionId = Guid.NewGuid().ToString();
                var headers = new Dictionary<string, string>();
                headers.Add(AnalyticsHeaders.OperationName, "MyOperation");
                headers.Add(AnalyticsHeaders.OperationId, operationId);
                headers.Add(AnalyticsHeaders.SessionId, sessionId);
                var operation = Sut.StartRequestOperation("RequestName", headers);
                MockTelemetryClient.Mock.Verify(x => x.TrackRequest(It.IsAny<RequestTelemetry>()), Times.Never);

                //Act
                operation.Dispose();
            
                //Assert
                MockTelemetryClient.Mock.Verify(x => x.UpdateOperation(It.Is<IAnalyticsOperation>(y => 
                    y.Id == operation.Id &&
                    y.Name == "MyOperation"), Sut.CurrentSessionId));
                MockTelemetryClient.Mock.Verify(x => x.TrackRequest(It.Is<RequestTelemetry>(y => 
                    y.Name == "RequestName")));
                MockConsoleLogger.Mock.Verify(x => x.LogOperation("RequestName", It.IsAny<TimeSpan>()));
            }
        }
    }
}