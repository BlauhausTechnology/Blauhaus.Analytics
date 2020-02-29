using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Microsoft.AspNetCore.Components;

namespace Blauhaus.Analytics.Samples.BlazorServer.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        public IAnalyticsService AnalyticsService { get; set; }


        //the index page must only log page views after render is called else it logs every time;
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                using (var _ = AnalyticsService.StartPageViewOperation(this, "Index"))
                {
                }
            }
        }
    }
}