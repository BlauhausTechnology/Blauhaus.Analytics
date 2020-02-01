using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Service;

namespace Blauhaus.Analytics.Console.ConsoleLoggers
{
    public interface IConsoleLogger
    {
        
        void LogTrace(string message, LogSeverity severityLevel, Dictionary<string, string>? properties);
        void LogEvent(string eventName, Dictionary<string, string>? properties, Dictionary<string, double>? metrics);
        void LogException(Exception exception, Dictionary<string, string>? properties, Dictionary<string, double>? metrics);
        void LogOperation(string operationName, TimeSpan duration);
    }
}
