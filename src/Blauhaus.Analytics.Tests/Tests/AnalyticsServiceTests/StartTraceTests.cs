using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Tests.Tests.Base;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AnalyticsServiceTests
{
    public class StartTraceTests : BaseAnalyticsServiceTest
    {
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
        public void WHEN_Operation_is_disposed_SHOULD_track_dependency()
        {
            //Arrange
            var operation = Sut.StartTrace(this, "Trace", LogSeverity.Warning, new Dictionary<string, object>
            {
                {"key", "1" }
            });
            MockTelemetryDecorator.Where_Decorate_returns(new TraceTelemetry("Decorated"));
            MockTelemetryClient.Mock.Verify(x => x.TrackTrace(It.IsAny<TraceTelemetry>()), Times.Never);
            Sut.StartOperation(this, "overall");

            //Act
            operation.Dispose();
            
            //Assert
            MockTelemetryDecorator.Mock.Verify(x => x.DecorateTelemetry(
                It.Is<TraceTelemetry>(y => 
                    y.Message == "Trace" && 
                    y.SeverityLevel == SeverityLevel.Warning),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.Is<IAnalyticsOperation>(y => 
                    y.Name == "overall"), 
                Sut.CurrentSession,
                It.Is<Dictionary<string, object>>(y => 
                    ((TimeSpan)y["Duration"]).Ticks > 0 &&
                    (string) y["key"] == "1")));

            MockTelemetryClient.Mock.Verify(x => x.TrackTrace(It.Is<TraceTelemetry>(y => y.Message == "Decorated")));
            
            MockConsoleLogger.Mock.Verify(x => x.LogTrace("Trace", LogSeverity.Warning, It.Is<Dictionary<string, string>>(y => y["key"] == "1")));
        }

    }
}