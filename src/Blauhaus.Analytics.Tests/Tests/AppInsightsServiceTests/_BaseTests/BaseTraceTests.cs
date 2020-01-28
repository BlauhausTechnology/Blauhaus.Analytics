using System.Collections.Generic;
using System.Globalization;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Tests.Tests._Base;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests._BaseTests
{
    public abstract class BaseTraceTests<TSut> : BaseAnalyticsServiceTest<TSut> where TSut : class, IAnalyticsService
    {
        [Test]
        public void SHOULD_log_to_console_and_server()
        {
            //Arrange
            var properties = new Dictionary<string, object>();

            //Act
            Sut.Trace("Trace message", LogSeverity.Verbose, properties);

            //Assert
            MockConsoleLogger.Mock.Verify(x => x.LogTrace("Trace message", LogSeverity.Verbose, properties));
            MockTelemetryClient.Mock.Verify(x => x.TrackTrace("Trace message", SeverityLevel.Verbose, It.IsAny<Dictionary<string, string>>()));
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

            //Act
            Sut.Trace("Trace message", LogSeverity.Verbose, properties);

            //Assert
            MockTelemetryClient.Mock.Verify(x => x.TrackTrace("Trace message", SeverityLevel.Verbose, It.Is<Dictionary<string, string>>(y =>
                y["Integer"] == "1" &&
                y["Double"] == 1.2d.ToString(CultureInfo.InvariantCulture) &&
                y["Decimal"] == 1.2m.ToString(CultureInfo.InvariantCulture) &&
                y.ContainsKey("String") &&
                y["String"] == "\"stringValue\"")));
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

            //Act
            Sut.Trace("Trace message", LogSeverity.Verbose, properties);

            //Assert
            MockTelemetryClient.Mock.Verify(x => x.TrackTrace("Trace message", SeverityLevel.Verbose, It.Is<Dictionary<string, string>>(y =>
                y["MyObject"] == JsonConvert.SerializeObject(myObject))));
        }


        [Test]
        public void IF_configured_min_logSeverity_is_higher_than_message_SHOULD_not_trace_to_server()
        {
            //Arrange
            CurrentBuildConfig = BuildConfig.Debug;
            var properties = new Dictionary<string, object>();
            MockConfig.With(x => x.MinimumLogToServerSeverity, new Dictionary<IBuildConfig, LogSeverity>
            {
                {BuildConfig.Debug, LogSeverity.Information }
            });

            //Act
            Sut.Trace("Trace message", LogSeverity.Verbose, properties);

            //Assert
            MockConsoleLogger.Mock.Verify(x => x.LogTrace("Trace message", LogSeverity.Verbose, properties));
            MockTelemetryClient.Mock.Verify(x => x.TrackTrace("Trace message", SeverityLevel.Verbose, It.IsAny<Dictionary<string, string>>()), Times.Never);
        }
    }
}