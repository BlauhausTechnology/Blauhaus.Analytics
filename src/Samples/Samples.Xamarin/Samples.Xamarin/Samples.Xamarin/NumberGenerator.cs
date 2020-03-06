using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;

namespace Samples.Xamarin
{
    public class NumberGenerator
    {
        private readonly IAnalyticsService _analyticsService;

        public NumberGenerator(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        public async Task<int> GenerateAsync()
        {
            using (var _ = _analyticsService.ContinueOperation(this, "Fallback operation name"))
            {
                var random = new Random().Next();
                await Task.Delay(2000);

                _analyticsService.LogEvent(this, "New number generated", new Dictionary<string, object>{{"Number", random}});

                return random;
            }
        }
    }
}