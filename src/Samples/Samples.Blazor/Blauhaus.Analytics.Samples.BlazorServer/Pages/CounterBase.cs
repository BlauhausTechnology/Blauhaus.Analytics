using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Samples.BlazorServer.Data;
using Microsoft.AspNetCore.Components;

namespace Blauhaus.Analytics.Samples.BlazorServer.Pages
{
    public class CounterBase : ComponentBase
    {
        protected int CurrentCount = 0;
        private IAnalyticsOperation _pageViewOperation;

        [Inject]
        public IAnalyticsService AnalyticsService { get; set; }
        
        [Inject] 
        public WeatherForecastService ForecastService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _pageViewOperation = AnalyticsService.StartPageViewOperation(this, "Counter");

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

        protected void IncrementCount()
        {
            CurrentCount++;
        }
    }
}