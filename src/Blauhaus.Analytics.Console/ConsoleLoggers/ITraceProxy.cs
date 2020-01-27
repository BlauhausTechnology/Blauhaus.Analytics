using System;

namespace Blauhaus.Analytics.Console.ConsoleLoggers
{
    public interface ITraceProxy
    {
        void SetColour(ConsoleColor colour);
        void Write(string message);
    }
}
