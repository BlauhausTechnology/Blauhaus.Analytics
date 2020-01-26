using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Blauhaus.AppInsights.Abstractions.Service
{
    public interface IAppInsightsService
    {
        
        

        Dictionary<string, string> CurrentOperationProperties { get; }

        AnalyticsOperation StartOperation(string operationName);
        AnalyticsOperation StartOrContinueOperation(string operationName);


        void Trace(string message, SeverityLevel severityLevel = 0, Dictionary<string, string> properties = null);
        void LogEvent(string eventName, Dictionary<string, string> properties = null, Dictionary<string, double> metrics = null);

    }
}