using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions;
using Serilog.Context;

namespace Blauhaus.Analytics.Serilog;

public class ClientAnalyticsContext : IAnalyticsContext
{

    private static readonly object Locker = new();
    private readonly Dictionary<string, object> _values = new();

    public Dictionary<string, object> SetValue(string key, object value)
    {
        lock (Locker)
        {
            LogContext.PushProperty(key, value);
            _values[key] = value; 
            return _values;  
        }
    }

    public Dictionary<string, object> SetValues(Dictionary<string, object> newProperties)
    {
        lock (Locker)
        {
            foreach (var property in newProperties)
            {
                LogContext.PushProperty(property.Key, property.Value);
                _values[property.Key] = property.Value;   
            }
            return _values;  
        }
    }

    public bool TryGetValue(string key, out object value)
    {
        lock (Locker)
        {
            return _values.TryGetValue(key, out value);
        }
    }

    public Dictionary<string, object> GetAllValues()
    {
        lock (Locker)
        {
            return new Dictionary<string, object>(_values);
        }
    }
}