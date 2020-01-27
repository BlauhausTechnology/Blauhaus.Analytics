using Blauhaus.Analytics.Abstractions.Operation;

namespace Blauhaus.Analytics.Abstractions.Service
{
    public interface IAnalyticsClientService : IAnalyticsService
    {
        IAnalyticsOperation StartPageViewOperation(string viewName);
    }
}