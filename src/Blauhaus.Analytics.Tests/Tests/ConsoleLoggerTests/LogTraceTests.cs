using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Analytics.Tests.Tests._Base;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.ConsoleLoggerTests
{
    public class LogTraceTests : BaseAnalyticsServiceTest<ConsoleLogger>
    {

        protected override ConsoleLogger ConstructSut()
        {
            return new ConsoleLogger(MockTraceProxy.Object, CurrentBuildConfig);
        }

        [Test]
        public void IF_CurrentBuildConfig_is_release_SHOULD_not_trace_event()
        {
            //Arrange
            CurrentBuildConfig = BuildConfig.Release;

            //Act
            Sut.LogTrace("message", LogSeverity.Error);

            //Assert
            MockTraceProxy.Mock.Verify(x => x.Write(It.IsAny<string>()), Times.Never);
        }


        [Test]
        public void SHOULD_trace_message_in_nice_colour()
        {
            //Act
            Sut.LogTrace("message", LogSeverity.Error);

            //Assert
            MockTraceProxy.Mock.Verify(x => x.SetColour(ConsoleColours.TraceColours[LogSeverity.Error]));
            MockTraceProxy.Mock.Verify(x => x.Write("TRACE: message"));
        }


        [Test]
        public void IF_properties_are_specified_SHOULD_write_them()
        {
            //Act
            Sut.LogTrace("message", LogSeverity.Error, new Dictionary<string, string>
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
