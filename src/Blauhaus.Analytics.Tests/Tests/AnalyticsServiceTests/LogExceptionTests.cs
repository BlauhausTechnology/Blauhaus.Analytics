﻿using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Tests.Tests.Base;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AnalyticsServiceTests
{
    public class LogExceptionTests : BaseAnalyticsServiceTest
    {
        [Test]
        public void SHOULD_decorate_telemetry_and_log_to_console_and_server()
        {
            //Arrange
            var properties = new Dictionary<string, object>{{"Property", "value"}};
            var exception = new ArgumentException("oops");
            MockTelemetryDecorator.Where_Decorate_returns(new ExceptionTelemetry(exception));

            //Act
            Sut.LogException(this, exception, properties);

            //Assert
            MockTelemetryDecorator.Mock.Verify(x => x.DecorateTelemetry(
                It.Is<ExceptionTelemetry>(y => y.Exception == exception),
                It.IsAny<string>(),
                It.IsAny<string>(),
                Sut.CurrentOperation, Sut.CurrentSession, It.Is<Dictionary<string, object>>(y => 
                    (string) y["Property"] == "value" && (string) y["ExceptionType"] == "System.ArgumentException")));
            MockTelemetryClient.Mock.Verify(x => x.TrackException(It.Is<ExceptionTelemetry>(y => y.Exception== exception)));
            MockConsoleLogger.Mock.Verify(x => x.LogException(exception, It.Is<Dictionary<string, string>>(y => y["Property"] == "value")));
        }

    }
}