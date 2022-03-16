using System.Collections.Generic;

namespace Blauhaus.Analytics.Abstractions;

public class AnalyticsContext : IAnalyticsContext
{

    private readonly Dictionary<string, object> _contextProperties = new();
 
    public void Set(string key, object value)
    {
        _contextProperties[key] = value;
    }

    public bool TryGet(string key, out object value)
    {
        return _contextProperties.TryGetValue(key, out value);
    }
     
}