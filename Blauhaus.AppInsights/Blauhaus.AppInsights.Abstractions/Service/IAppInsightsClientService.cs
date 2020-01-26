using Blauhaus.AppInsights.Abstractions.Operation;

namespace Blauhaus.AppInsights.Abstractions.Service
{
    public interface IAppInsightsClientService : IAppInsightsService
    {
        IAnalyticsOperation StartPageView(string viewName);
    }
}