using System;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.TestHelpers.MockBuilders;

namespace Blauhaus.Analytics.TestHelpers.MockBuilders
{
    public class AnalyticsOperationMockBuilder : BaseMockBuilder<AnalyticsOperationMockBuilder, IAnalyticsOperation>
    {
        public AnalyticsOperationMockBuilder()
        {
            With(x => x.Id, Guid.NewGuid().ToString());
            With(x => x.Name, $"Operation {Guid.NewGuid()}");
        }
    }
}