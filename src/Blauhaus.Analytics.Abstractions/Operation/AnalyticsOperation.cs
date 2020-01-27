using System;
using System.Diagnostics;

namespace Blauhaus.Analytics.Abstractions.Operation
{
    public class AnalyticsOperation : IAnalyticsOperation
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

        public AnalyticsOperation(string operationName, Action<TimeSpan> onStopAction)
        {
            _onStopAction = onStopAction;
            Id = Guid.NewGuid().ToString();
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