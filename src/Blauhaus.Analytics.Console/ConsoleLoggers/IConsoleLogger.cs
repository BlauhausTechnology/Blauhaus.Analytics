using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Service;

namespace Blauhaus.Analytics.Console.ConsoleLoggers
{
    public interface IConsoleLogger
    {
        
        void LogTrace(string message, LogSeverity severityLevel, Dictionary<string, object>? properties);
        void LogEvent(string eventName, Dictionary<string, object>? properties, Dictionary<string, double>? metrics);
        void LogException(Exception exception, Dictionary<string, object>? properties, Dictionary<string, double>? metrics);
        void LogOperation(string operationName, TimeSpan duration);
    }
}
