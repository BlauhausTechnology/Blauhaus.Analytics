using Blauhaus.AppInsights.Abstractions.Operation;

namespace Blauhaus.AppInsights.Abstractions.Service
{
    public interface IAppInsightsServerService : IAppInsightsService
    {

        IAnalyticsOperation StartRequestOperation(string requestName, string operationId, string operationName, string sessionId);
    }
}