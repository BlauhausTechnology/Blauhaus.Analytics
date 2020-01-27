using Blauhaus.Analytics.Abstractions.Operation;

namespace Blauhaus.Analytics.Abstractions.Service
{
    public interface IAnalyticsServerService : IAnalyticsService
    {

        IAnalyticsOperation StartRequestOperation(string requestName, string operationId, string operationName, string sessionId);
    }
}