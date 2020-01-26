using System.Threading.Tasks;
using Blauhaus.AppInsights.Abstractions.Service;

namespace Blauhaus.AppInsights.Test.Tests._Base
{
    public abstract class BaseTest
    {

        protected abstract IAppInsightsService GetAppInsightsService();

        public async Task Run()
        {
            var appInsightsService = GetAppInsightsService();

            var client = appInsightsService.GetClient();
            client.TrackTrace("TestTrace");
            client.Flush();
            
        }
    }
}