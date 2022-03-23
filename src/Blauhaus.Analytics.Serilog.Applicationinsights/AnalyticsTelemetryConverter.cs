using Microsoft.ApplicationInsights.DataContracts;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;
using System;

public class AnalyticsTelemetryConverter : TraceTelemetryConverter
{ 
    
    public static AnalyticsTelemetryConverter Instance = new();

    public override void ForwardPropertiesToTelemetryProperties(LogEvent logEvent, ISupportProperties telemetryProperties, IFormatProvider formatProvider)
    {
        ForwardPropertiesToTelemetryProperties(logEvent, telemetryProperties, formatProvider,
            includeLogLevel: true,
            includeRenderedMessage: true,
            includeMessageTemplate: false);
    }
}