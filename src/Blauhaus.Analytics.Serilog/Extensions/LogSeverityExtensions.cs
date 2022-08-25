using System;
using Blauhaus.Analytics.Abstractions.Service;
using Serilog.Events;

namespace Blauhaus.Analytics.Serilog;

public static class LogSeverityExtensions
{
    public static LogEventLevel ToLogEventLevel(this LogSeverity logSeverity)
    {
        switch (logSeverity)
        {
            case LogSeverity.Verbose:
                return LogEventLevel.Verbose;
            case LogSeverity.Debug:
                return LogEventLevel.Debug;
            case LogSeverity.Information:
                return LogEventLevel.Information;
            case LogSeverity.Warning:
                return LogEventLevel.Warning;
            case LogSeverity.Error:
                return LogEventLevel.Error;
            case LogSeverity.Critical:
                return LogEventLevel.Fatal;
            default:
                throw new ArgumentOutOfRangeException(nameof(logSeverity), logSeverity, null);
        }
    }
}