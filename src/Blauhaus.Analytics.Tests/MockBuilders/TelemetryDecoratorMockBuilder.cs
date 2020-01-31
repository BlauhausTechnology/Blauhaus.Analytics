using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Common.TelemetryClients;
using Blauhaus.Common.TestHelpers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Moq;

namespace Blauhaus.Analytics.Tests.MockBuilders
{
    public class TelemetryDecoratorMockBuilder : BaseMockBuilder<TelemetryDecoratorMockBuilder, ITelemetryDecorator>
    {



        public TelemetryDecoratorMockBuilder Where_Decorate_returns<TTelemetry>(TTelemetry telemetry) where TTelemetry : ITelemetry, ISupportProperties
        {
            Mock.Setup(x => x.DecorateTelemetry(It.IsAny<TTelemetry>(), It.IsAny<IAnalyticsOperation>(), It.IsAny<IAnalyticsSession>(), It.IsAny<Dictionary<string, string>>()));
            return this;
        }
    }
}