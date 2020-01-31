using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Analytics.Tests.MockBuilders;
using Blauhaus.Common.TestHelpers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.DeviceServices.Abstractions.Application;
using Blauhaus.DeviceServices.Abstractions.DeviceInfo;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests._Base
{
    public abstract class BaseAnalyticsServiceTest<TSut> : BaseUnitTest<TSut> where TSut : class
    {

        protected MockBuilder<ITraceProxy> MockTraceProxy;
        protected MockBuilder<IApplicationInsightsConfig> MockConfig;
        protected MockBuilder<IConsoleLogger> MockConsoleLogger;
        protected TelemetryClientMockBuilder MockTelemetryClient;
        protected IBuildConfig CurrentBuildConfig;
        protected MockBuilder<IDeviceInfoService> MockDeviceInfoService;
        protected MockBuilder<IApplicationInfoService> MockApplicationInfoService;

        [SetUp]
        public virtual void Setup()
        {
            Cleanup();
            CurrentBuildConfig = BuildConfig.Debug;
            MockTraceProxy = new MockBuilder<ITraceProxy>();
            MockConfig = new MockBuilder<IApplicationInsightsConfig>()
                .With(x => x.InstrumentationKey, "TestInstrumentationKey")
                .With(x => x.RoleName, "TestClientName")
                .With(x => x.MinimumLogToServerSeverity, new Dictionary<IBuildConfig, LogSeverity>
                {
                    {BuildConfig.Debug, LogSeverity.Verbose },
                    {BuildConfig.Release, LogSeverity.Verbose }
                });

            MockConsoleLogger = new MockBuilder<IConsoleLogger>();
            MockTelemetryClient = new TelemetryClientMockBuilder();
            MockTelemetryClient.Mock.Setup(x => x.UpdateOperation(It.IsAny<IAnalyticsOperation>(), It.IsAny<AnalyticsSession>()))
                .Returns(MockTelemetryClient.Object);

            MockDeviceInfoService = new MockBuilder<IDeviceInfoService>();
            MockApplicationInfoService = new MockBuilder<IApplicationInfoService>();
        }

    }
}
