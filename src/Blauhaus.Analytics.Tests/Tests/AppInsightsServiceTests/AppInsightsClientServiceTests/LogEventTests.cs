using Blauhaus.Analytics.Client.Service;
using Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests._BaseTests;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AppInsightsServiceTests.AppInsightsClientServiceTests
{
    [TestFixture]
    public class LogEventTests : BaseLogEventTests<AppInsightsClientService>
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