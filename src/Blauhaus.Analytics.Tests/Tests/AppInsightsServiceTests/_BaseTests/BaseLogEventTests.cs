using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Tests.Tests._Base;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests._BaseTests
{
    public abstract class BaseLogEventTests<TSut> : BaseAnalyticsServiceTest<TSut> where TSut : class, IAnalyticsService
    {
        [Test]
        public void SHOULD_log_to_console_and_server()
        {
            //Arrange
            var properties = new Dictionary<string, object>();
            var metrics = new Dictionary<string, double>{{"Metric", 12}};
            MockTelemetryDecorator.Where_Decorate_returns(new EventTelemetry("Decorated"));

            //Act
            Sut.LogEvent("Event Name", properties, metrics);

            //TODO 

            //Assert
            MockConsoleLogger.Mock.Verify(x => x.LogEvent("Event Name", properties, metrics));
            MockTelemetryClient.Mock.Verify(x => x.TrackEvent(It.IsAny<EventTelemetry>()));
            //MockTelemetryClient.Mock.Verify(x => x.TrackEvent(It.Is<EventTelemetry>(y => y.Name == "Decorated")));
            MockTelemetryDecorator.Mock.Verify(x => x.DecorateTelemetry(
                It.Is<EventTelemetry>(y => y.Name == "EventName"), 
                It.IsAny<IAnalyticsOperation>(), Sut.CurrentSession, It.IsAny<Dictionary<string, string>>()));
        }

        [Test]
        public void WHEN_logging_to_server_SHOULD_convert_string_and_scalar_properties_toString()
        {
            //Arrange
            var properties = new Dictionary<string, object>
            {
                {"Integer", 1 },
                {"Double", 1.2d },
                {"Decimal", 1.2m },
                {"String", "stringValue" },
            };
            var metrics = new Dictionary<string, double>();

            //Act
            Sut.LogEvent("Event Name", properties, metrics);

            //Assert
            MockTelemetryClient.Mock.Verify(x => x.TrackEvent("Event Name", It.Is<Dictionary<string, string>>(y =>
                y["Integer"] == "1" &&
                y["Double"] == 1.2d.ToString(CultureInfo.InvariantCulture) &&
                y["Decimal"] == 1.2m.ToString(CultureInfo.InvariantCulture) &&
                y.ContainsKey("String") &&
                y["String"] == "\"stringValue\""), metrics));
        }

        [Test]
        public void WHEN_logging_to_server_SHOULD_convert_object_properties_to_json()
        {
            //Arrange
            var myObject = new
            {
                Name = "Adrian",
                Age = 211
            };
            var properties = new Dictionary<string, object>
            {
                {"MyObject", myObject }
            };
            var metrics = new Dictionary<string, double>();

            //Act
            Sut.LogEvent("Event Name", properties, metrics);

            //Assert
            MockTelemetryClient.Mock.Verify(x => x.TrackEvent("Event Name", It.Is<Dictionary<string, string>>(y =>
                y["MyObject"] == JsonConvert.SerializeObject(myObject)), metrics));
        }

        
    }
}