using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Ioc.Abstractions;
using Microsoft.Extensions.Logging;

namespace Blauhaus.Analytics.Serilog;

public class AnalyticsLogger<T> : IAnalyticsLogger<T>
{
    private readonly ILogger<T> _logger;
    private readonly IAnalyticsContext _analyticsContext;

    public AnalyticsLogger(ILogger<T> logger, IAnalyticsContext analyticsContext)
    {
        _logger = logger;
        _analyticsContext = analyticsContext;
    }


    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _logger.Log(logLevel, eventId, state, exception, formatter);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(logLevel);
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return _logger.BeginScope(state);
    }

    public IAnalyticsLogger SetValue(string key, object value)
    {
        _analyticsContext.SetValue(key, value);
        return this;
    }

    public IAnalyticsLogger SetValues(Dictionary<string, object> newProperties)
    {
        _analyticsContext.SetValues(newProperties);
        return this;
    }

    public IDisposable BeginScope()
    {
        return _logger.BeginScope(_analyticsContext.GetAllValues());
    }

    public IDisposable BeginTimedScope(LogLevel logLevel, string messageTemplate, params object[] args)
    {
        var scope = BeginScope();
        
        var newArgs = new object[args.Length+1];
        for (var i = 0; i < args.Length; i++)
        {
            newArgs[i] = args[i];
        }
        return new LoggerTimer(duration =>
        {
            newArgs[newArgs.Length - 1] = duration;
            messageTemplate += " in {Duration}";
            _logger.Log(logLevel, messageTemplate, newArgs);
            scope.Dispose();
        });
    }
}