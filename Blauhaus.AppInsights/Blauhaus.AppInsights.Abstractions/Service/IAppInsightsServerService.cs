using Microsoft.ApplicationInsights;
using Blauhaus.AppInsights.Abstractions.Operation;

namespace Blauhaus.AppInsights.Abstractions.Service
{
    public interface IAppInsightsServerService : IAppInsightsService
    {
        
        IAnalyticsOperation StartRequest(string requestName, string operationId, string operationName, string sessionId);
    }
}