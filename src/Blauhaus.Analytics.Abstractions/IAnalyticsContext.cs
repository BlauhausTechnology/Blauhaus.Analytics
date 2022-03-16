using System;
using System.Collections.Generic;

namespace Blauhaus.Analytics.Abstractions;

public interface IAnalyticsContext
{
    Dictionary<string, object> SetValue(string key, object value);
    Dictionary<string, object> SetValues(Dictionary<string, object> newProperties);

    bool TryGetValue(string key, out object value);
    Dictionary<string, object> GetAllValues();

    IDisposable BeginScope<T>(Dictionary<string, object> extraProperties);
}