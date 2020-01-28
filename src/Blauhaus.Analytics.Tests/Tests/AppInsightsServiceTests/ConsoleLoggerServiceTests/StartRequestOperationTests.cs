using System;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Console.Service;
using Blauhaus.Analytics.Server.Service;
using Blauhaus.Analytics.Tests.Tests._Base;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.ConsoleLoggerServiceTests
{
    public class StartRequestOperationTests : BaseAnalyticsServiceTest<ConsoleLoggerService>
    {
        protected override ConsoleLoggerService ConstructSut()
        {
            return new ConsoleLoggerService(
                MockConsoleLogger.Object);
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
            var operationId = Guid.NewGuid().ToString();
            var sessionId = Guid.NewGuid().ToString();
            var operation = Sut.StartRequestOperation("RequestName", operationId, "MyOperation", sessionId);
            MockConsoleLogger.Mock.Verify(x => x.LogOperation(It.IsAny<string>(), It.IsAny<TimeSpan>()), Times.Never);

            //Act
            operation.Dispose();
            
            //Assert
            MockConsoleLogger.Mock.Verify(x => x.LogOperation("RequestName", It.IsAny<TimeSpan>()));
        }
    }
}