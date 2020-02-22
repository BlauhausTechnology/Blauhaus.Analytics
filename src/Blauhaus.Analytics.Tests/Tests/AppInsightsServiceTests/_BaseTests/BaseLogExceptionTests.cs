using System;
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
    public abstract class BaseLogExceptionTests<TSut> : BaseAnalyticsServiceTest<TSut> where TSut : class, IAnalyticsService
    {
        [Test]
        public void SHOULD_decorate_telemetry_and_log_to_console_and_server()
        {
            //Arrange
            var properties = new Dictionary<string, object>{{"Property", "value"}};
            var metrics = new Dictionary<string, double>{{"Metric", 12}};
            var exception = new Exception("oops");
            MockTelemetryDecorator.Where_Decorate_with_metrics_returns(new ExceptionTelemetry(exception));

            //Act
            Sut.LogException(exception, properties, metrics);

            //Assert
            MockTelemetryDecorator.Mock.Verify(x => x.DecorateTelemetry(
                It.Is<ExceptionTelemetry>(y => y.Exception == exception),
                It.IsAny<string>(),
                Sut.CurrentOperation, Sut.CurrentSession, It.Is<Dictionary<string, object>>(y => 
                    (string) y["Property"] ==  "value"), metrics));
            MockTelemetryClient.Mock.Verify(x => x.TrackException(It.Is<ExceptionTelemetry>(y => y.Exception== exception)));
            MockConsoleLogger.Mock.Verify(x => x.LogException(exception, It.Is<Dictionary<string, string>>(y => y["Property"] == "\"value\""), metrics));
        }

    }
}