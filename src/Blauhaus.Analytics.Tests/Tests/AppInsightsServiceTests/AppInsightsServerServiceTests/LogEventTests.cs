using Blauhaus.Analytics.Client.Service;
using Blauhaus.Analytics.Server.Service;
using Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests._BaseTests;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.AppInsightsServerServiceTests
{
    [TestFixture]
    public class LogEventTests : BaseLogEventTests<AppInsightsServerService>
    {
        protected override AppInsightsServerService ConstructSut()
        {
            return new AppInsightsServerService(
                MockConfig.Object,
                MockConsoleLogger.Object,
                MockTelemetryClient.Object,
                CurrentBuildConfig);
        }
    }
}