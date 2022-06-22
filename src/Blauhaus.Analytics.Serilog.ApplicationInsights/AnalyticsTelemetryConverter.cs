using System;
using Microsoft.ApplicationInsights.DataContracts;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;

namespace Blauhaus.Analytics.Serilog.ApplicationInsights;

public class AnalyticsTelemetryConverter : TraceTelemetryConverter
{

    public static AnalyticsTelemetryConverter Instance = new();

    public override void ForwardPropertiesToTelemetryProperties(LogEvent logEvent, ISupportProperties telemetryProperties, IFormatProvider formatProvider)
    {
        base.ForwardPropertiesToTelemetryProperties(logEvent, telemetryProperties, formatProvider,
            includeLogLevel: true,
            includeRenderedMessage: true,
            includeMessageTemplate: false);
    }
}