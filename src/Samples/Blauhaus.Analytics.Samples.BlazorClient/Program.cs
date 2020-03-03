using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;

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
