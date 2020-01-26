using System;
using System.Collections.Generic;
using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.ConsoleLoggers;
using Blauhaus.AppInsights.Tests.Tests._Base;
using Blauhaus.Common.TestHelpers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Moq;
using NUnit.Framework;

namespace Blauhaus.AppInsights.Tests.Tests.ConsoleLoggerTests
{
    public class LogEventTests : BaseUnitTest<AppInsightsLogger>
    {
        protected MockBuilder<ITraceProxy> MockTraceProxy;
        protected IBuildConfig CurrentBuildConfig;
        protected MockBuilder<IApplicationInsightsConfig> MockConfig;

        [SetUp]
        public virtual void Setup()
        {
            Cleanup();
            MockTraceProxy = new MockBuilder<ITraceProxy>();
            MockConfig = new MockBuilder<IApplicationInsightsConfig>();
            CurrentBuildConfig = BuildConfig.Debug;
        }

        protected override AppInsightsLogger ConstructSut()
        {
            return new AppInsightsLogger(MockConfig.Object, MockTraceProxy.Object, CurrentBuildConfig);
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


        [Test]
        public void IF_metrics_are_specified_SHOULD_write_them()
        {
            //Act
            Sut.LogEvent("EventName", null, new Dictionary<string, double>
            {
                {"EventMetric1", 1 },
                {"EventMetric2", 2 }
            });

            //Assert
            MockTraceProxy.Mock.Verify(x => x.Write(" + EventMetric1: 1"));
            MockTraceProxy.Mock.Verify(x => x.Write(" + EventMetric2: 2"));
        }


    }
}
