using Blauhaus.Analytics.Abstractions.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Blauhaus.Analytics.Abstractions;

public interface IAnalyticsLogger : ILogger
{
    IAnalyticsLogger SetValue(string key, object value);
    IAnalyticsLogger SetValues(Dictionary<string, object> newProperties);

    IDisposable BeginScope();

    [MessageFormatMethod("messageTemplate")]
    IDisposable BeginTimedScope(LogLevel logLevel, string messageTemplate, params object[] args);
}

public interface IAnalyticsLogger<out T> : IAnalyticsLogger, ILogger<T>
{
  
}