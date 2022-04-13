using System;
using System.Diagnostics;
using Blauhaus.Analytics.Abstractions.Config;

namespace Blauhaus.Analytics.Console.ConsoleLoggers
{
    public class TraceProxy : ITraceProxy
    {

        public TraceProxy()
        {
        }

        public void SetColour(ConsoleColor colour)
        {
            System.Console.ForegroundColor = colour;
        }

        public void Write(string message)
        {
            Trace.WriteLine(message);
        }
    }
}
