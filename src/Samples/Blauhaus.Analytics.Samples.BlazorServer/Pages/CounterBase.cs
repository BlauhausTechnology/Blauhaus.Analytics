using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blauhaus.Analytics.Samples.BlazorServer.Pages
{
    public class CounterBase : ComponentBase
    {
        protected int CurrentCount = 0;
        
        [Inject]
        public IAnalyticsService AnalyticsService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            using (var _ = AnalyticsService.StartPageViewOperation(this, "Counter"))
            {
            }
        }
        protected void IncrementCount()
        {
            CurrentCount++;
        }
    }
}