using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Session;

namespace Blauhaus.Analytics.Abstractions.Service
{
    public interface IAnalyticsServerService : IAnalyticsService
    {
        IAnalyticsOperation StartRequestOperation(object sender, string requestName, IDictionary<string, string> headers, [CallerMemberName] string callingMember = "");
    }
}