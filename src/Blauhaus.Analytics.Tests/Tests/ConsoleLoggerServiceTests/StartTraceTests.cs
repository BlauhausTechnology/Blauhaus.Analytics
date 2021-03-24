using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Console.Service;
using Blauhaus.Analytics.Tests.Tests._Base;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.ConsoleLoggerServiceTests
{
    public class StartTraceTests : BaseAnalyticsTest<ConsoleLoggerService>
    {
        protected override ConsoleLoggerService ConstructSut()
        {
            return new ConsoleLoggerService(
                CurrentBuildConfig,
                MockConsoleLogger.Object);
        }

        [Test]
        public void SHOULD_return_new_operation_but_not_set_current()
        {
            //Act
            var operation = Sut.StartTrace(this, "Trace", LogSeverity.Warning);

            //Assert
            Assert.That(operation.Name, Is.EqualTo("Trace"));
            Assert.That(Sut.CurrentOperation, Is.Null); 
            Assert.That(operation.Id.Length, Is.EqualTo(Guid.NewGuid().ToString().Length));
        }

        [Test]
        public void WHEN_Operation_is_disposed_SHOULD_log_operation_to_console()
        {
            //Arrange
            var operation = Sut.StartTrace(this, "Trace", LogSeverity.Warning);
            MockTelemetryClient.Mock.Verify(x => x.TrackDependency(It.IsAny<DependencyTelemetry>()), Times.Never);

            //Act
            operation.Dispose();
            
            //Assert
            MockConsoleLogger.Mock.Verify(x => x.LogTrace("Trace", LogSeverity.Warning, 
                It.Is<Dictionary<string, string>>(y => y.ContainsKey("Duration"))));
        }
    }
}