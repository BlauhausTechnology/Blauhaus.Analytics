using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.AppInsights.Abstractions.ConsoleLoggers
{
    public interface IConsoleLogger
    {

        void Trace(string message, SeverityLevel severityLevel = 0, Dictionary<string, string>? properties = null);
        void LogEvent(string eventName, Dictionary<string, string>? properties = null, Dictionary<string, double>? metrics = null);

    }
}
