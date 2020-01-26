using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.AppInsights.Abstractions.Service
{
    public interface IAppInsightsClientService : IAppInsightsService
    {
        AnalyticsOperation StartPageView(string viewName);

      
    }
}