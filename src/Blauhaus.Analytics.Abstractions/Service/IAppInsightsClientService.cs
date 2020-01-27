using Blauhaus.Analytics.Abstractions.Operation;

namespace Blauhaus.Analytics.Abstractions.Service
{
    public interface IAppInsightsClientService : IAppInsightsService
    {
        IAnalyticsOperation StartPageViewOperation(string viewName);
    }
}