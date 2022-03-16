namespace Blauhaus.Analytics.Abstractions;

public interface IAnalyticsContext
{
    void Set(string key, object value);
    bool TryGet(string key, out object value);
}