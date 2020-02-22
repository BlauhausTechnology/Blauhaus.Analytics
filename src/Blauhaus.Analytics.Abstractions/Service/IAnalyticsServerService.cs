using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;

namespace Blauhaus.Analytics.Abstractions.Service
{
    public interface IAnalyticsServerService : IAnalyticsService
    {
        IAnalyticsOperation StartRequestOperation(string requestName, IDictionary<string, string> headers);
    }
}