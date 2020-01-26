using Microsoft.ApplicationInsights;

namespace Blauhaus.AppInsights.Abstractions.Service
{
    public interface IAppInsightsServerService : IAppInsightsService
    {
        
        AnalyticsOperation StartRequest(string requestName, string operationId, string operationName, string sessionId);
    }
}