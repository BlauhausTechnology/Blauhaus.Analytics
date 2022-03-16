using System;
using System.Collections.Generic;

namespace Blauhaus.Analytics.Abstractions;

public interface IAnalyticsContext
{
    void SetValue(string key, object value);
    bool TryGetValue(string key, out object value);
    Dictionary<string, object> GetAllValues();

    IDisposable BeginScope<T>(Dictionary<string, object> extraProperties);
}