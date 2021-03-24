using System.Collections.Generic;
using Blauhaus.Analytics.Console.Service;
using Blauhaus.Analytics.Tests.Tests._Base;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.ConsoleLoggerServiceTests
{
    public class LogEventTests : BaseAnalyticsTest<ConsoleLoggerService>
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
            Sut.LogEvent(this, "event name", properties);

            //Assert
            MockConsoleLogger.Mock.Verify(x => x.LogEvent("event name", It.IsAny<Dictionary<string, string>>()));
        }

    }
}