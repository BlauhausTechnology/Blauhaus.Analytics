﻿using Blauhaus.Analytics.Client.Service;
using Blauhaus.Analytics.Server.Service;
using Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests._BaseTests;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.AppInsightsServerServiceTests
{
    [TestFixture]
    public class TraceTests : BaseTraceTests<AnalyticsServerService>
    {
        protected override AnalyticsServerService ConstructSut()
        {
            return new AnalyticsServerService(
                MockConfig.Object,
                MockConsoleLogger.Object,
                MockTelemetryClient.Object,
                MockTelemetryDecorator.Object,
                CurrentBuildConfig);
        }
    }
}