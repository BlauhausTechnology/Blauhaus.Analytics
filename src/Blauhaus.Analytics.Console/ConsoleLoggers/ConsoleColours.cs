using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Service;

namespace Blauhaus.Analytics.Console.ConsoleLoggers
{
    public static class ConsoleColours
    {
        public static Dictionary<LogSeverity, ConsoleColor> TraceColours { get; } = new Dictionary<LogSeverity, ConsoleColor>
        {
            {LogSeverity.Debug, ConsoleColor.DarkCyan },
            {LogSeverity.Verbose, ConsoleColor.Cyan },
            {LogSeverity.Information, ConsoleColor.Green },
            {LogSeverity.Warning, ConsoleColor.Yellow },
            {LogSeverity.Error, ConsoleColor.Red },
            {LogSeverity.Critical, ConsoleColor.Red },
        };


        public static ConsoleColor EventColour { get; } = ConsoleColor.Magenta;
        public static ConsoleColor ExceptionColour { get; } = ConsoleColor.Red;
        public static ConsoleColor OperationColour { get; } = ConsoleColor.DarkYellow;


    }
}
