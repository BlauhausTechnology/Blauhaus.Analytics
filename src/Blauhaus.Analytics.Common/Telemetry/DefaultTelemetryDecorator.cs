﻿using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Common.Telemetry;

public class DefaultTelemetryDecorator : ITelemetryDecorator
{
    public TTelemetry DecorateTelemetry<TTelemetry>(TTelemetry telemetry, string className, string callerMemberName, IAnalyticsOperation? currentOperation, IAnalyticsSession currentSession, Dictionary<string, object> properties) where TTelemetry : ITelemetry, ISupportProperties
    {
        return telemetry;
    }
}