using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;

namespace Blauhaus.Analytics.Abstractions.Service
{
    public interface IAnalyticsServerService : IAnalyticsService
    {

        IAnalyticsOperation StartRequestOperation(string requestName, string operationId, string operationName, IAnalyticsSession session);
        IAnalyticsOperation StartRequestOperation(string requestName, IDictionary<string, string> headers);
    }
}