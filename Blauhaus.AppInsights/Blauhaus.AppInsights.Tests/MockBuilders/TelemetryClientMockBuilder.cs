using Blauhaus.AppInsights.Abstractions.Config;
using Blauhaus.AppInsights.Abstractions.Operation;
using Blauhaus.AppInsights.Abstractions.TelemetryClients;
using Blauhaus.Common.TestHelpers;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;

namespace Blauhaus.AppInsights.Tests.MockBuilders
{
    public class TelemetryClientMockBuilder : BaseMockBuilder<TelemetryClientMockBuilder, ITelemetryClientProxy>
    {

    }
}