using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Blauhaus.AppInsights.Abstractions.Operation;

namespace Blauhaus.AppInsights.Abstractions.Service
{
    public interface IAppInsightsClientService : IAppInsightsService
    {
        IAnalyticsOperation StartPageView(string viewName);
    }
}