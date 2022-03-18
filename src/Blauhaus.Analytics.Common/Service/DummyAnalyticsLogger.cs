using System;
using System.Collections.Generic;
using System.Text;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Utils.Disposables;
using Microsoft.Extensions.Logging;

namespace Blauhaus.Analytics.Common.Service
{
    public class DummyAnalyticsLogger<T> : IAnalyticsLogger<T>
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly IAnalyticsContext _analyticsContext;

        public DummyAnalyticsLogger(IAnalyticsService analyticsService, IAnalyticsContext analyticsContext)
        {
            _analyticsService = analyticsService;
            _analyticsContext = analyticsContext;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _analyticsService.Trace(this, state!.ToString());
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new ActionDisposable(() => { });
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
            return new ActionDisposable(() => { });
        }

        public IDisposable BeginTimedScope(LogLevel logLevel, string messageTemplate, params object[] args)
        {
            return new ActionDisposable(() => { });
        }
    }
}
