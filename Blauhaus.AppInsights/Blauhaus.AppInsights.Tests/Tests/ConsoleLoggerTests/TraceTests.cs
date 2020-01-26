
using System;
using System.Collections.Generic;
using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.ConsoleLoggers;
using Blauhaus.AppInsights.Tests.Tests._Base;
using Blauhaus.Common.TestHelpers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.AppInsights.Tests.Tests.ConsoleLoggerTests
{
    public class TraceTests : BaseUnitTest<ConsoleLogger>
    {
        protected MockBuilder<ITraceProxy> MockTraceProxy;
        protected IBuildConfig CurrentBuildConfig;
        protected MockBuilder<IApplicationInsightsConfig> MockConfig;

        [SetUp]
        public virtual void Setup()
        {
            Cleanup();
            MockTraceProxy = new MockBuilder<ITraceProxy>();
            CurrentBuildConfig = BuildConfig.Debug;
            MockConfig = new MockBuilder<IApplicationInsightsConfig>();
        }

        protected override ConsoleLogger ConstructSut()
        {
            return new ConsoleLogger(MockConfig.Object, MockTraceProxy.Object, CurrentBuildConfig);
        }

        [Test]
        public void IF_CurrentBuildConfig_is_release_SHOULD_not_trace_event()
        {
            //Arrange
            CurrentBuildConfig = BuildConfig.Release;

            //Act
            Sut.Trace("message", SeverityLevel.Error);

            //Assert
            MockTraceProxy.Mock.Verify(x => x.Write(It.IsAny<string>()), Times.Never);
        }


        [Test]
        public void SHOULD_trace_message_in_nice_colour()
        {
            //Act
            Sut.Trace("message", SeverityLevel.Error);

            //Assert
            MockTraceProxy.Mock.Verify(x => x.SetColour(ConsoleColours.TraceColours[SeverityLevel.Error]));
            MockTraceProxy.Mock.Verify(x => x.Write("TRACE: message"));
        }


        [Test]
        public void IF_properties_are_specified_SHOULD_write_them()
        {
            //Act
            Sut.Trace("message", SeverityLevel.Error, new Dictionary<string, string>
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
