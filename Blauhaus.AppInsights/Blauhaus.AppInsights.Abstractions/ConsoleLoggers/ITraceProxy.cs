using System;
namespace Blauhaus.AppInsights.Abstractions.ConsoleLoggers
{
    public interface ITraceProxy
    {
        void SetColour(ConsoleColor colour);
        void Write(string message);
    }
}
