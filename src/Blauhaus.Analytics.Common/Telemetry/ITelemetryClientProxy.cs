﻿using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.Analytics.Common.Telemetry
{
    public interface ITelemetryClientProxy 
    {
        void TrackTrace(TraceTelemetry traceTelemetry);
        void TrackEvent(EventTelemetry eventTelemetry);
        void TrackException(ExceptionTelemetry exceptionTelemetry);
        void TrackDependency(DependencyTelemetry dependencyTelemetry);
        void TrackRequest(RequestTelemetry requestTelemetry);
        void TrackPageView(PageViewTelemetry pageViewTelemetry);
    }
}