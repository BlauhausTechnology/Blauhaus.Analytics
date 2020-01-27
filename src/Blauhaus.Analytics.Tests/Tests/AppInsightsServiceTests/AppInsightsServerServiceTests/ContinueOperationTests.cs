﻿using Blauhaus.Analytics.Server.Service;
using Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests._BaseTests;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.AppInsightsServerServiceTests
{
    public class ContinueOperationTests : BaseContinueOperationTests<AnalyticsServerServerService>
    {
        protected override AnalyticsServerServerService ConstructSut()
        {
            return new AnalyticsServerServerService(
                MockConfig.Object,
                MockConsoleLogger.Object,
                MockTelemetryClient.Object,
                CurrentBuildConfig);
        }
    }
}