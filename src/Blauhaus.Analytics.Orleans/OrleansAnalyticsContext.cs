using Blauhaus.Analytics.Abstractions;
using Orleans.Runtime;

namespace Blauhaus.Analytics.Orleans;

public class OrleansAnalyticsContext : IAnalyticsContext
{
    public void Set(string key, object value)
    {
        RequestContext.Set(key, value);
    }

    public bool TryGet(string key, out object value)
    {
        value = RequestContext.Get(key);
        return value != null;
    }
}