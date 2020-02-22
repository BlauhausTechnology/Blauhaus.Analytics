using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blauhaus.Analytics.Abstractions.Operation;

namespace Blauhaus.Analytics.Abstractions.Service
{
    public interface IAnalyticsClientService : IAnalyticsService
    {
        IAnalyticsOperation StartPageViewOperation(string viewName, [CallerMemberName] string callingClass = "");
        IDictionary<string, string> AnalyticsOperationHeaders { get; }
        void ClearCurrentSession();
    }
}