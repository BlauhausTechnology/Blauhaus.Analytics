using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Analytics.Tests.Tests._Base;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.ConsoleLoggerTests
{
    public class LogExceptionTests : BaseAnalyticsTest<ConsoleLogger>
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
            Sut.LogException(new Exception("oops"), new Dictionary<string, string>());

            //Assert
            MockTraceProxy.Mock.Verify(x => x.Write(It.IsAny<string>()), Times.Never);
        }


        [Test]
        public void SHOULD_trace_exception_message_and_stack_trace_in_nice_colour()
        {
            //Arrange
            Exception thrownException;
            try
            {
                throw new Exception("oops");
            }
            catch (Exception exception)
            {
                thrownException = exception;
            }

            //Act
            Sut.LogException(thrownException, new Dictionary<string, string>());

            //Assert
            MockTraceProxy.Mock.Verify(x => x.SetColour(ConsoleColours.ExceptionColour));
            MockTraceProxy.Mock.Verify(x => x.Write("EXCEPTION: oops"));
            MockTraceProxy.Mock.Verify(x => x.Write(It.Is<string>(y => y.Contains(thrownException.StackTrace))));
        }


        [Test]
        public void IF_properties_are_specified_SHOULD_write_them()
        {
            //Act
            Sut.LogException(new Exception("oops"), new Dictionary<string, string>
            {
                {"EventProperty1", "EventValue1" },
                {"EventProperty2", "EventValue2" },
            });

            //Assert
            MockTraceProxy.Mock.Verify(x => x.Write(" !*! EventProperty1: EventValue1"));
            MockTraceProxy.Mock.Verify(x => x.Write(" !*! EventProperty2: EventValue2"));
        }


    }
}
