using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Console.Service;
using Blauhaus.Analytics.Tests.Tests._Base;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.ConsoleLoggerServiceTests
{
    public class LogEventTests : BaseAnalyticsServiceTest<ConsoleLoggerService>
    {
        protected override ConsoleLoggerService ConstructSut()
        {
            return new ConsoleLoggerService(
                MockConsoleLogger.Object);
        }

        [Test]
        public void SHOULD_log()
        {
            //Arrange
            var properties = new Dictionary<string, object>();
            var metrics = new Dictionary<string, double>();

            //Act
            Sut.LogEvent("event name", properties, metrics);

            //Assert
            MockConsoleLogger.Mock.Verify(x => x.LogEvent("event name", properties, metrics));
        }

    }
}