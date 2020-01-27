﻿using Blauhaus.Analytics.Client.Service;
using Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests._BaseTests;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.AppInsightsClientServiceTests
{
    [TestFixture]
    public class StartOperationTests : BaseStartOperationTests<AppInsightsClientService>
    {
        protected override AppInsightsClientService ConstructSut()
        {
            return new AppInsightsClientService(
                MockConfig.Object,
                MockConsoleLogger.Object,
                MockTelemetryClient.Object,
                CurrentBuildConfig);
        }
    }
}