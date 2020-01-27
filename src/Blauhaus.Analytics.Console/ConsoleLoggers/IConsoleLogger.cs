using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Service;

namespace Blauhaus.Analytics.Console.ConsoleLoggers
{
    public interface IConsoleLogger
    {
        
        void TrackTrace(string message, LogSeverity severityLevel, Dictionary<string, object> properties);
        void TrackEvent(string eventName, Dictionary<string, object> properties, Dictionary<string, double> metrics);

    }
}
