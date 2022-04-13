using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions;
using Serilog.Context;

namespace Blauhaus.Analytics.Serilog;

public class InMemoryAnalyticsContext : IAnalyticsContext
{

    private static readonly object Locker = new();
    private readonly Dictionary<string, object> _values = new();

    public Dictionary<string, object> SetValue(string key, object value)
    {
        lock (Locker)
        {
            _values[key] = value;
            foreach (var property in _values)
            {
                LogContext.PushProperty(property.Key, property.Value);
            }
            return _values;  
        }
    }

    public Dictionary<string, object> SetValues(Dictionary<string, object> newProperties)
    {
        lock (Locker)
        {
            foreach (var property in newProperties)
            {
                _values[property.Key] = property.Value;   
            }
            foreach (var property in _values)
            {
                LogContext.PushProperty(property.Key, property.Value);
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