using System;

namespace Blauhaus.Analytics.Abstractions.Operation
{
    public interface IAnalyticsOperation : IDisposable
    {
         string Id { get; }
         string Name { get; }

    }
}
