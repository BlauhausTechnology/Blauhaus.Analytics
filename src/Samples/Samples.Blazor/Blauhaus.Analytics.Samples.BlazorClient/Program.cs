using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Hosting;

namespace Blauhaus.Analytics.Samples.BlazorClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await WebAssemblyHostBuilder
                .CreateDefault(args)
                .ConfigureServices()
                .AddComponents()
                .Build()
                .RunAsync();
        }

    }
}
