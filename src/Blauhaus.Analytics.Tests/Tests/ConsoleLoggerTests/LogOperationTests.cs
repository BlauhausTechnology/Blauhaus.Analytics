using System;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Analytics.Tests.Tests._Base;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.ConsoleLoggerTests
{
    public class LogOperationTests : BaseAnalyticsTest<ConsoleLogger>
    {

        protected override ConsoleLogger ConstructSut()
        {
            return new ConsoleLogger(MockTraceProxy.Object, CurrentBuildConfig);
        }

        [Test]
        public void IF_CurrentBuildConfig_is_release_SHOULD_not_log_event()
        {
            //Arrange
            CurrentBuildConfig = BuildConfig.Release;

            //Act
            Sut.LogOperation("operation gigolo", TimeSpan.FromTicks(11121221212211));

            //Assert
            MockTraceProxy.Mock.Verify(x => x.Write(It.IsAny<string>()), Times.Never);
        }


        [Test]
        public void SHOULD_trace_message_in_nice_colour()
        {
            //Act
            Sut.LogOperation("operation gigolo", TimeSpan.FromTicks(11121221212211));

            //Assert
            MockTraceProxy.Mock.Verify(x => x.SetColour(ConsoleColours.OperationColour));
            MockTraceProxy.Mock.Verify(x => x.Write("OPERATION: operation gigolo completed in 121 ms"));
        }


    }
}
