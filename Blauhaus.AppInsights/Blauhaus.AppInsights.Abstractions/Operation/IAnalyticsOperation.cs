using System;
namespace Blauhaus.AppInsights.Abstractions.Operation
{
    public interface IAnalyticsOperation : IDisposable
    {
        public string Id { get; }
        public string Name { get; }
    }
}
