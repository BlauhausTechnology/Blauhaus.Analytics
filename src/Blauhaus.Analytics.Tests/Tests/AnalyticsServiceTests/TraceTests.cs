using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Tests.Tests.Base;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AnalyticsServiceTests
{
    public class TraceTests : BaseAnalyticsServiceTest
    {
        [Test]
        public void SHOULD_decorate_telemetry_and_log_to_console_and_server()
        {
            //Arrange
            var properties = new Dictionary<string, object>{{"Property", "value"}};
            MockTelemetryDecorator.Where_Decorate_returns(new TraceTelemetry("Decorated"));

            //Act
            Sut.Trace(this, "Trace message", LogSeverity.Critical, properties);

            //Assert
            MockTelemetryDecorator.Mock.Verify(x => x.DecorateTelemetry(
                It.Is<TraceTelemetry>(y => 
                    y.Message == "Trace message" && 
                    y.SeverityLevel == SeverityLevel.Critical),
                It.IsAny<string>(),
                It.IsAny<string>(),
                Sut.CurrentOperation, Sut.CurrentSession, It.Is<Dictionary<string, object>>(y => 
                    (string) y["Property"] ==  "value")));
            MockTelemetryClient.Mock.Verify(x => x.TrackTrace(It.Is<TraceTelemetry>(y => y.Message == "Decorated")));
            MockConsoleLogger.Mock.Verify(x => x.LogTrace("Trace message", LogSeverity.Critical, It.Is<Dictionary<string, string>>(y => y["Property"] == "value")));
        }

        [TestCase(LogSeverity.Verbose, SeverityLevel.Verbose)]
        [TestCase(LogSeverity.Information, SeverityLevel.Information)]
        [TestCase(LogSeverity.Warning, SeverityLevel.Warning)]
        [TestCase(LogSeverity.Critical, SeverityLevel.Critical)]
        [TestCase(LogSeverity.Error, SeverityLevel.Error)]
        public void SHOULD_convert_log_severity(LogSeverity logSeverity, SeverityLevel severityLevel)
        { 
            //Act
            Sut.Trace(this, "Trace message", logSeverity);

            //Assert
            MockTelemetryDecorator.Mock.Verify(x => x.DecorateTelemetry(
                It.Is<TraceTelemetry>(y => 
                    y.Message == "Trace message" && 
                    y.SeverityLevel == severityLevel),
                It.IsAny<string>(),
                It.IsAny<string>(),
                Sut.CurrentOperation, Sut.CurrentSession, It.IsAny<Dictionary<string, object>>()));
        }

        [Test]
        public void IF_properties_is_null_SHOULD_log_empty()
        {
            //Arrange
            MockTelemetryDecorator.Where_Decorate_returns(new TraceTelemetry("Decorated"));

            //Act
            Sut.Trace(this, "Trace message", LogSeverity.Verbose, null);

            //Assert
            MockTelemetryDecorator.Mock.Verify<TraceTelemetry>(x => x.DecorateTelemetry(
                It.Is<TraceTelemetry>(y => y.Message == "Trace message"),
                It.IsAny<string>(),
                It.IsAny<string>(),
                null, Sut.CurrentSession, It.Is<Dictionary<string, object>>(y => y.Count == 0)));
            MockTelemetryClient.Mock.Verify(x => x.TrackTrace(It.Is<TraceTelemetry>(y => y.Message == "Decorated")));
            MockConsoleLogger.Mock.Verify(x => x.LogTrace("Trace message", LogSeverity.Verbose, It.Is<Dictionary<string, string>>(y => y.Count == 0)));
        }


        [Test]
        public void SHOULD_work_with_extension_string()
        {
            //Arrange
            MockTelemetryDecorator.Where_Decorate_returns(new TraceTelemetry("Decorated"));

            //Act
            Sut.TraceVerbose(this, "Trace message", "Property", "value");

            //Assert
            MockTelemetryDecorator.Mock.Verify(x => x.DecorateTelemetry(
                It.Is<TraceTelemetry>(y => y.Message == "Trace message"),
                It.IsAny<string>(),
                It.IsAny<string>(),
                Sut.CurrentOperation, Sut.CurrentSession, It.Is<Dictionary<string, object>>(y => 
                    (string) y["Property"] ==  "value")));
            MockTelemetryClient.Mock.Verify(x => x.TrackTrace(It.Is<TraceTelemetry>(y => y.Message == "Decorated")));
            MockConsoleLogger.Mock.Verify(x => x.LogTrace("Trace message", LogSeverity.Verbose, It.Is<Dictionary<string, string>>(y => y["Property"] == "value")));
        }
        private class TestObject{}

        [Test]
        public void SHOULD_work_with_extension_object()
        {
            //Arrange
            MockTelemetryDecorator.Where_Decorate_returns(new TraceTelemetry("Decorated"));
            var property = new TestObject();

            //Act
            Sut.TraceVerbose(this, "Trace message", property.ToObjectDictionary());

            //Assert
            MockTelemetryDecorator.Mock.Verify(x => x.DecorateTelemetry(
                It.Is<TraceTelemetry>(y => y.Message == "Trace message"),
                It.IsAny<string>(),
                It.IsAny<string>(),
                Sut.CurrentOperation, Sut.CurrentSession, It.Is<Dictionary<string, object>>(y => 
                    (TestObject) y["TestObject"] ==  property)));
            MockTelemetryClient.Mock.Verify(x => x.TrackTrace(It.Is<TraceTelemetry>(y => y.Message == "Decorated")));
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