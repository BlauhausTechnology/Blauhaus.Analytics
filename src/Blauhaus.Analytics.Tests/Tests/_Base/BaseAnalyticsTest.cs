using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Config;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Console.ConsoleLoggers;
using Blauhaus.Analytics.Tests.MockBuilders;
using Blauhaus.Common.TestHelpers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.DeviceServices.Abstractions.Application;
using Blauhaus.DeviceServices.Abstractions.DeviceInfo;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests._Base
{
    public abstract class BaseAnalyticsTest<TSut> : BaseUnitTest<TSut> where TSut : class
    {
        
        protected MockBuilder<ITraceProxy> MockTraceProxy;
        protected MockBuilder<IApplicationInsightsConfig> MockConfig;
        protected MockBuilder<IConsoleLogger> MockConsoleLogger;
        protected TelemetryClientMockBuilder MockTelemetryClient;
        protected IBuildConfig CurrentBuildConfig;
        protected MockBuilder<IDeviceInfoService> MockDeviceInfoService;
        protected MockBuilder<IApplicationInfoService> MockApplicationInfoService;
        protected TelemetryDecoratorMockBuilder MockTelemetryDecorator;
        protected SessionFactoryMockBuilder MockSessionFactory;

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

            MockDeviceInfoService = new MockBuilder<IDeviceInfoService>();
            MockApplicationInfoService = new MockBuilder<IApplicationInfoService>();
            MockTelemetryDecorator = new TelemetryDecoratorMockBuilder();
            MockSessionFactory = new SessionFactoryMockBuilder();
        }

    }
}