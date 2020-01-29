using System;
using System.Collections.Generic;
using System.Net.Http;
using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;
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
                var operation = Sut.StartRequestOperation("RequestName", "MyOperation", operationId, AnalyticsSession.FromExisting(sessionId));

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
                var operation = Sut.StartRequestOperation("RequestName", "MyOperation", operationId, AnalyticsSession.FromExisting(sessionId));
                MockTelemetryClient.Mock.Verify(x => x.TrackRequest(It.IsAny<RequestTelemetry>()), Times.Never);

                //Act
                operation.Dispose();
            
                //Assert
                MockTelemetryClient.Mock.Verify(x => x.UpdateOperation(It.Is<IAnalyticsOperation>(y => 
                    y.Id == operation.Id &&
                    y.Name == "MyOperation"), Sut.CurrentSession));
                MockTelemetryClient.Mock.Verify(x => x.TrackRequest(It.Is<RequestTelemetry>(y => 
                    y.Name == "RequestName")));
                MockConsoleLogger.Mock.Verify(x => x.LogOperation("RequestName", It.IsAny<TimeSpan>()));
            }
        }

        
        public class GivenHttpHeaders : StartRequestOperationTests
        {
            
            [Test]
            public void SHOULD_set_and_return_CurrentOperation()
            {
                //Arrange
                var operationId = Guid.NewGuid().ToString();
                var headers = new Dictionary<string, string>
                {
                    {AnalyticsHeaders.Operation.Name, "MyOperation"}, 
                    {AnalyticsHeaders.Operation.Id, operationId}
                };

                //Act
                var operation = Sut.StartRequestOperation("RequestName", headers);

                //Assert
                Assert.That(operation.Name, Is.EqualTo("MyOperation"));
                Assert.That(operation.Id, Is.EqualTo(operationId));
                Assert.That(Sut.CurrentOperation.Name, Is.EqualTo("MyOperation"));
                Assert.That(Sut.CurrentOperation.Id, Is.EqualTo(operationId));
            }

            [Test]
            public void SHOULD_populate_Session_variables()
            {
                //Arrange
                var headers = new Dictionary<string, string>
                {
                    {AnalyticsHeaders.Session.Id, "sessionId"},
                    {AnalyticsHeaders.Session.UserId, "userId"},
                    {AnalyticsHeaders.Session.AccountId, "accountId"},
                    {AnalyticsHeaders.Session.DeviceId, "deviceId"},
                    {AnalyticsHeaders.Session.AppVersion, "appVersion"},
                    {AnalyticsHeaders.Prefix + "FavouriteColour", "Red" }
                };

                //Act
                Sut.StartRequestOperation("RequestName", headers);

                //Assert
                Assert.That(Sut.CurrentSession.Id, Is.EqualTo("sessionId"));
                Assert.That(Sut.CurrentSession.AppVersion, Is.EqualTo("appVersion"));
                Assert.That(Sut.CurrentSession.AccountId, Is.EqualTo("accountId"));
                Assert.That(Sut.CurrentSession.UserId, Is.EqualTo("userId"));
                Assert.That(Sut.CurrentSession.DeviceId, Is.EqualTo("deviceId"));
                Assert.That(Sut.CurrentSession.Properties["FavouriteColour"], Is.EqualTo("Red"));
            }

            [Test]
            public void IF_Session_values_besides_Id_are_not_provided_SHOULD_make_null()
            {
                //Arrange
                var headers = new Dictionary<string, string>
                {
                    {AnalyticsHeaders.Session.Id, "sessionId"}
                };

                //Act
                Sut.StartRequestOperation("RequestName", headers);

                //Assert
                Assert.That(Sut.CurrentSession.Id, Is.EqualTo("sessionId"));
                Assert.That(Sut.CurrentSession.AppVersion, Is.Null);
                Assert.That(Sut.CurrentSession.AccountId, Is.Null);
                Assert.That(Sut.CurrentSession.UserId, Is.Null);
                Assert.That(Sut.CurrentSession.DeviceId, Is.Null);
            }

            [Test]
            public void IF_Session_Id_is_not_provided_SHOULD_generate_one()
            {
                //Arrange
                var headers = new Dictionary<string, string>();

                //Act
                Sut.StartRequestOperation("RequestName", headers);

                //Assert
                Assert.That(Sut.CurrentSession.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
            }

            
            [Test]
            public void IF_Operation_details_not_provided_WHEN_Operation_is_disposed_SHOULD_log_new_operation()
            {
                //Arrange
                var operation = Sut.StartRequestOperation("RequestName", new Dictionary<string, string>());
                MockTelemetryClient.Mock.Verify(x => x.TrackRequest(It.IsAny<RequestTelemetry>()), Times.Never);

                //Act
                operation.Dispose();
            
                //Assert
                MockTelemetryClient.Mock.Verify(x => x.UpdateOperation(It.Is<IAnalyticsOperation>(y => 
                    y.Id == operation.Id &&
                    y.Name == "NewRequest"), Sut.CurrentSession));
                MockTelemetryClient.Mock.Verify(x => x.TrackRequest(It.Is<RequestTelemetry>(y => 
                    y.Name == "RequestName")));
                MockConsoleLogger.Mock.Verify(x => x.LogOperation("RequestName", It.IsAny<TimeSpan>()));
            }

            [Test]
            public void WHEN_Operation_is_disposed_SHOULD_track_dependency()
            {
                //Arrange
                var headers = new Dictionary<string, string>
                {
                    {AnalyticsHeaders.Operation.Name,  "MyOperation"}
                };
                var operation = Sut.StartRequestOperation("RequestName", headers);
                MockTelemetryClient.Mock.Verify(x => x.TrackRequest(It.IsAny<RequestTelemetry>()), Times.Never);

                //Act
                operation.Dispose();
            
                //Assert
                MockTelemetryClient.Mock.Verify(x => x.UpdateOperation(It.Is<IAnalyticsOperation>(y => 
                    y.Id == operation.Id &&
                    y.Name == "MyOperation"), Sut.CurrentSession));
                MockTelemetryClient.Mock.Verify(x => x.TrackRequest(It.Is<RequestTelemetry>(y => 
                    y.Name == "RequestName")));
                MockConsoleLogger.Mock.Verify(x => x.LogOperation("RequestName", It.IsAny<TimeSpan>()));
            }
        }
    }
}