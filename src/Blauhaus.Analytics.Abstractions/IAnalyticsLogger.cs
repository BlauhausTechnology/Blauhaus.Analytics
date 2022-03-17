using Blauhaus.Analytics.Abstractions.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Blauhaus.Analytics.Abstractions;

public interface IAnalyticsLogger<out T> : ILogger<T>
{
    IAnalyticsLogger<T> SetValue(string key, object value);
    IAnalyticsLogger<T> SetValues(Dictionary<string, object> newProperties);

    IDisposable BeginScope();

    [MessageFormatMethod("messageTemplate")]
    IDisposable BeginTimedScope(LogLevel logLevel, string messageTemplate, params object[] args);
}