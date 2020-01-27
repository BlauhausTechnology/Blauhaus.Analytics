using System;
using System.Diagnostics;

namespace Blauhaus.Analytics.Abstractions.Operation
{
    public class AnalyticsOperation : IAnalyticsOperation
    {
        private readonly Action<TimeSpan> _onStopAction;

        private readonly Stopwatch _stopwatch = new Stopwatch();

        public AnalyticsOperation(string operationName, string operationId, Action<TimeSpan> onStopAction)
        {
            Id = operationId;
            Name = operationName;
            _onStopAction = onStopAction;
            _stopwatch.Start();
        }
        public AnalyticsOperation(IAnalyticsOperation existingOperation, Action<TimeSpan> onStopAction)
        {
            Id = existingOperation.Id;
            Name = existingOperation.Name;
            _onStopAction = onStopAction;
            _stopwatch.Start();
        }

        public AnalyticsOperation(string operationName, Action<TimeSpan> onStopAction)
        {
            Id = Guid.NewGuid().ToString();
            Name = operationName;
            _onStopAction = onStopAction;
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