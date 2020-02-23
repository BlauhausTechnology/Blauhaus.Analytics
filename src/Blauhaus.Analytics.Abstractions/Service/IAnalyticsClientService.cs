using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blauhaus.Analytics.Abstractions.Operation;

namespace Blauhaus.Analytics.Abstractions.Service
{
    public interface IAnalyticsClientService : IAnalyticsService
    {
        IDictionary<string, string> AnalyticsOperationHeaders { get; }
        void ClearCurrentSession();

        //todo void ResetCurrentSession(string sessionId);
    }
}