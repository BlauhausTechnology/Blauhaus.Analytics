using System;
namespace Blauhaus.AppInsights.Abstractions.Operation
{
    public interface IAnalyticsOperation : IDisposable
    {
         string Id { get; }
         string Name { get; }
    }
}
