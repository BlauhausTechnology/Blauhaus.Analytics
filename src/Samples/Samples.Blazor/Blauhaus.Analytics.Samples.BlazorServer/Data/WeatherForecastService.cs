using System;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;

namespace Blauhaus.Analytics.Samples.BlazorServer.Data
{
    public class WeatherForecastService
    {
        private readonly IAnalyticsService _analyticsService;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastService(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
            Id = Guid.NewGuid().ToString();
            ConstructionCount++;
        }
        public string Id { get; }
        public static int ConstructionCount;

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            var rng = new Random();
            var result = Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());
            
            _analyticsService.LogEvent(this, "Weather generated!");

            return result;
        }
    }
}
