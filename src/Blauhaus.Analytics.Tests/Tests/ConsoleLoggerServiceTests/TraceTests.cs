﻿using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Console.Service;
using Blauhaus.Analytics.Tests.Tests.Base;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.ConsoleLoggerServiceTests
{
    public class TraceTests : BaseAnalyticsTest<ConsoleLoggerService>
    {
        protected override ConsoleLoggerService ConstructSut()
        {
            return new ConsoleLoggerService(
                CurrentBuildConfig,
                MockConsoleLogger.Object);
        }

        [Test]
        public void SHOULD_log()
        {
            //Arrange
            var properties = new Dictionary<string, object>();

            //Act
            Sut.Trace(this, "event name", LogSeverity.Critical, properties);

            //Assert
            MockConsoleLogger.Mock.Verify(x => x.LogTrace("event name", LogSeverity.Critical, It.IsAny<Dictionary<string, string>>()));
        }

    }
}