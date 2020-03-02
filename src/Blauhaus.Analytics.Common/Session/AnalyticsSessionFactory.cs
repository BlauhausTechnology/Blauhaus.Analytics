using Blauhaus.Analytics.Abstractions.Session;

namespace Blauhaus.Analytics.Common.Session
{
    public class AnalyticsSessionFactory : IAnalyticsSessionFactory
    {
        public IAnalyticsSession CreateSession()
        {
            return AnalyticsSession.New;
        }
    }
}