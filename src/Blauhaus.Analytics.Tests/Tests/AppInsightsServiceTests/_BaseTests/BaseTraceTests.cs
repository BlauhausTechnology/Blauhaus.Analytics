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
        public void SHOULD_decorate_telemetry_and_log_to_console_and_server()
        {
            //Arrange
            var properties = new Dictionary<string, object>{{"Property", "value"}};
            MockTelemetryDecorator.Where_Decorate_returns(new TraceTelemetry("Decorated"));

            //Act
            Sut.Trace(this, "Trace message", LogSeverity.Verbose, properties);

            //Assert
            MockTelemetryDecorator.Mock.Verify(x => x.DecorateTelemetry(
                It.Is<TraceTelemetry>(y => y.Message == "Trace message"),
                It.IsAny<string>(),
                It.IsAny<string>(),
                Sut.CurrentOperation, Sut.CurrentSession, It.Is<Dictionary<string, object>>(y => 
                    (string) y["Property"] ==  "value")));
            MockTelemetryClient.Mock.Verify(x => x.TrackTrace(It.Is<TraceTelemetry>(y => y.Message == "Decorated")));
            MockConsoleLogger.Mock.Verify(x => x.LogTrace("Trace message", LogSeverity.Verbose, It.Is<Dictionary<string, string>>(y => y["Property"] == "\"value\"")));
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
            Sut.Trace(this, "Trace message", LogSeverity.Verbose, properties);

            //Assert
            MockConsoleLogger.Mock.Verify(x => x.LogTrace("Trace message", LogSeverity.Verbose, It.IsAny<Dictionary<string, string>>()));
            MockTelemetryClient.Mock.Verify(x => x.TrackTrace(It.IsAny<TraceTelemetry>()), Times.Never);
        }
    }
}