using System.Collections.Generic;
using Blauhaus.Analytics.Tests.Tests._Base;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AnalyticsServiceTests
{
    public class LogEventTests : BaseAnalyticsServiceTest
    {
        [Test]
        public void SHOULD_decorate_telemetry_and_log_to_console_and_server()
        {
            //Arrange
            var properties = new Dictionary<string, object>{{"Property", "value"}};
            MockTelemetryDecorator.Where_Decorate_returns(new EventTelemetry("Decorated"));

            //Act
            Sut.LogEvent(this, "Event Name", properties);

            //Assert
            MockTelemetryDecorator.Mock.Verify<EventTelemetry>(x => x.DecorateTelemetry(
                It.Is<EventTelemetry>(y => y.Name == "Event Name"),
                It.IsAny<string>(),
                It.IsAny<string>(),
                Sut.CurrentOperation, Sut.CurrentSession, It.Is<Dictionary<string, object>>(y => 
                    (string) y["Property"] ==  "value")));
            MockTelemetryClient.Mock.Verify(x => x.TrackEvent(It.Is<EventTelemetry>(y => y.Name == "Decorated")));
            MockConsoleLogger.Mock.Verify(x => x.LogEvent("Event Name", It.Is<Dictionary<string, string>>(y => y["Property"] == "\"value\"")));
        }

        
    }
}