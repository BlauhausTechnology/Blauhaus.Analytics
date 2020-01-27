using System;
using System.Diagnostics;

namespace Blauhaus.Analytics.Console.ConsoleLoggers
{
    public class TraceProxy : ITraceProxy
    {

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
