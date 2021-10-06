using System.Collections.Generic;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Analytics.Tests.Tests.Base;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.ConsoleLoggerTests
{
    public class LogEventTests : BaseAnalyticsTest<ConsoleLogger>
    {


        protected override ConsoleLogger ConstructSut()
        {
            return new ConsoleLogger(MockTraceProxy.Object, CurrentBuildConfig);
        }

        [Test]
        public void IF_CurrentBuildConfig_is_null_SHOULD_not_trace_event()
        {
            //Arrange
            CurrentBuildConfig = null;

            //Act
            Sut.LogEvent("EventName");

            //Assert
            MockTraceProxy.Mock.Verify(x => x.Write(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void IF_CurrentBuildConfig_is_release_SHOULD_not_trace_event()
        {
            //Arrange
            CurrentBuildConfig = BuildConfig.Release;

            //Act
            Sut.LogEvent("EventName");

            //Assert
            MockTraceProxy.Mock.Verify(x => x.Write(It.IsAny<string>()), Times.Never);
        }


        [Test]
        public void SHOULD_trace_event_name_in_nice_colour()
        {
            //Act
            Sut.LogEvent("EventName");

            //Assert
            MockTraceProxy.Mock.Verify(x => x.SetColour(ConsoleColours.EventColour));
            MockTraceProxy.Mock.Verify(x => x.Write("EVENT: EventName"));
        }


        [Test]
        public void IF_properties_are_specified_SHOULD_write_them()
        {
            //Act
            Sut.LogEvent("EventName", new Dictionary<string, string>
            {
                {"EventProperty1", "EventValue1" },
                {"EventProperty2", "EventValue2" },
            });

            //Assert
            MockTraceProxy.Mock.Verify(x => x.Write(" * EventProperty1: EventValue1"));
            MockTraceProxy.Mock.Verify(x => x.Write(" * EventProperty2: EventValue2"));
        }

    }
}
