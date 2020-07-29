using System;
using Blauhaus.Analytics.Abstractions.Service;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Common.Extensions
{
    public static class LogSeverityExtensions
    {
        public static SeverityLevel ToSeverityLevel(this LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Verbose:
                    return SeverityLevel.Verbose;
                case LogSeverity.Information:
                    return SeverityLevel.Information;
                case LogSeverity.Warning:
                    return SeverityLevel.Warning;
                case LogSeverity.Error:
                    return SeverityLevel.Error;
                case LogSeverity.Critical:
                    return SeverityLevel.Critical;
                default:
                    throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
            }
        }
    }
}