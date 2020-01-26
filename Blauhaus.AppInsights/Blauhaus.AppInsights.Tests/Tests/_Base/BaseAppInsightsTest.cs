using System;
using System.Collections.Generic;
using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.ConsoleLoggers;
using Blauhaus.AppInsights.Abstractions.TelemetryClients;
using Blauhaus.AppInsights.Tests.MockBuilders;
using Blauhaus.Common.TestHelpers;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Microsoft.ApplicationInsights.DataContracts;
using NUnit.Framework;

namespace Blauhaus.AppInsights.Tests.Tests._Base
{
    public abstract class BaseAppInsightsTest<TSut> : BaseUnitTest<TSut> where TSut : class
    {

        protected MockBuilder<ITraceProxy> MockTraceProxy;
        protected MockBuilder<IApplicationInsightsConfig> MockConfig;
        protected MockBuilder<IConsoleLogger> MockConsoleLogger;
        protected TelemetryClientMockBuilder MockTelemetryClient;
        protected IBuildConfig CurrentBuildConfig;

        [SetUp]
        public virtual void Setup()
        {
            Cleanup();
            CurrentBuildConfig = BuildConfig.Debug;
            MockTraceProxy = new MockBuilder<ITraceProxy>();
            MockConfig = new MockBuilder<IApplicationInsightsConfig>()
                .With(x => x.InstrumentationKey, "TestInstrumentationKey")
                .With(x => x.ClientName, "TestClientName")
                .With(x => x.MinimumLogToServerSeverity, new Dictionary<IBuildConfig, SeverityLevel>
                {
                    {BuildConfig.Debug, SeverityLevel.Information },
                    {BuildConfig.Release, SeverityLevel.Information }
                });

            MockConsoleLogger = new MockBuilder<IConsoleLogger>();
            MockTelemetryClient = new TelemetryClientMockBuilder();
            
        }

    }
}
