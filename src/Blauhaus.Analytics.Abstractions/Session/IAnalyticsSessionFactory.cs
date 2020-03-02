namespace Blauhaus.Analytics.Abstractions.Session
{
    public interface IAnalyticsSessionFactory
    {
        IAnalyticsSession CreateSession();
    }
}