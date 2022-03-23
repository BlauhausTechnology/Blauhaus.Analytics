using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;
using System.Collections.Generic;
using System;
using System.Linq;

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