using System;
using System.Diagnostics;

namespace Blauhaus.AppInsights.Abstractions.ConsoleLoggers
{
    public class TraceProxy : ITraceProxy
    {

        public void SetColour(ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
        }

        public void Write(string message)
        {
            Trace.WriteLine(message);
        }
    }
}
