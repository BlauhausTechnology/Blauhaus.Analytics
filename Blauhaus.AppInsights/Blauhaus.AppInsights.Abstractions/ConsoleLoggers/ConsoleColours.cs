using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;

namespace Blauhaus.AppInsights.Abstractions.ConsoleLoggers
{
    public static class ConsoleColours
    {
        public static Dictionary<SeverityLevel, ConsoleColor> TraceColours { get; } = new Dictionary<SeverityLevel, ConsoleColor>
        {
            {SeverityLevel.Verbose, ConsoleColor.Cyan },
            {SeverityLevel.Information, ConsoleColor.Green },
            {SeverityLevel.Warning, ConsoleColor.Yellow },
            {SeverityLevel.Error, ConsoleColor.Red },
            {SeverityLevel.Critical, ConsoleColor.Red },
        };

        public static ConsoleColor EventColour { get; } = ConsoleColor.Magenta;
        public static ConsoleColor ExceptionColour { get; } = ConsoleColor.Red;

    }
}
