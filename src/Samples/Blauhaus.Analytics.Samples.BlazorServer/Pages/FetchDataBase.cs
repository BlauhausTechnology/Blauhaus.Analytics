using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Samples.BlazorServer.Data;
using Microsoft.AspNetCore.Components;

namespace Blauhaus.Analytics.Samples.BlazorServer.Pages
{
    public class FetchDataBase : ComponentBase
    {
        public WeatherForecast[] Forecasts;
        private IAnalyticsOperation _pageViewOperation;

        [Inject] 
        public WeatherForecastService ForecastService { get; set; }

        [Inject]
        public IAnalyticsService AnalyticsService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _pageViewOperation = AnalyticsService.StartPageViewOperation(this, "Fetch Data");
            Forecasts = await ForecastService.GetForecastAsync(DateTime.Now);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                //just a hack to show analytics info on screen
                StateHasChanged();
                _pageViewOperation.Dispose();
            }

        }
    }
}