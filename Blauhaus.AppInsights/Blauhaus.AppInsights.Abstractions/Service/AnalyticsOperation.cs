using System;
using System.Diagnostics;
using System.Threading;

namespace Blauhaus.AppInsights.Abstractions.Service
{
    public class AnalyticsOperation : IDisposable
    {
        private readonly Action<TimeSpan> _onStopAction;

        private readonly Stopwatch _stopwatch = new Stopwatch();

        public AnalyticsOperation(string id, string operationName, Action<TimeSpan> onStopAction)
        {
            _onStopAction = onStopAction;
            Id = id;
            Name = operationName;
            _stopwatch.Start();
        }

        public string Id { get; }
        public string Name { get; }



        public void Dispose()
        {
            _stopwatch.Stop();
            GC.SuppressFinalize(this);
            _onStopAction.Invoke(_stopwatch.Elapsed);
        }
    }
}