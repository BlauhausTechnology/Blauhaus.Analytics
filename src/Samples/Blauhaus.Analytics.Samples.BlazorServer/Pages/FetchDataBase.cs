using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Samples.BlazorServer.Data;
using Microsoft.AspNetCore.Components;

namespace Blauhaus.Analytics.Samples.BlazorServer.Pages
{
    public class FetchDataBase : ComponentBase
    {
        public WeatherForecast[] Forecasts;

        [Inject] 
        public WeatherForecastService ForecastService { get; set; }

        [Inject]
        public IAnalyticsService AnalyticsService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            using (var _ = AnalyticsService.StartPageViewOperation(this, "Fetch Data"))
            {
                Forecasts = await ForecastService.GetForecastAsync(DateTime.Now);
            }
        }
    }
}