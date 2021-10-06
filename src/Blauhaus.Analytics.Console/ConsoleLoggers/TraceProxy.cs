using System;
using System.Diagnostics;
using Blauhaus.Analytics.Abstractions.Config;

namespace Blauhaus.Analytics.Console.ConsoleLoggers
{
    public class TraceProxy : ITraceProxy
    {
        private readonly IApplicationInsightsConfig _config;

        public TraceProxy(IApplicationInsightsConfig config)
        {
            _config = config;
        }

        public void SetColour(ConsoleColor colour)
        {
            System.Console.ForegroundColor = colour;
        }

        public void Write(string message)
        {
            if (_config.ConsoleOutput == ConsoleOutput.TraceWriter)
            {
                Trace.WriteLine(message);
            }
            else
            {
                System.Console.Out.WriteLine(message);
            }
        }
    }
}
