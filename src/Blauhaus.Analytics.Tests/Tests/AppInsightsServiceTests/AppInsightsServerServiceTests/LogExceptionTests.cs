using Blauhaus.Analytics.Client.Service;
using Blauhaus.Analytics.Server.Service;
using Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests._BaseTests;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.AppInsightsServerServiceTests
{
    [TestFixture]
    public class LogExceptionTests : BaseLogExceptionTests<AnalyticsServerServerService>
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