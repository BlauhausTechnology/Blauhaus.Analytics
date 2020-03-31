using Microsoft.AspNetCore.Blazor.Hosting;

namespace Blauhaus.Analytics.Samples.BlazorClient
{
    public static class Startup
    {
        //when last attempted this throws 
        //Severity	Code	Description	Project	File	Line	Suppression State
        //Error		Unhandled exception. Mono.Linker.MarkException: Error processing method: 'System.IDisposable Microsoft.Extensions.Logging.Logger::BeginScope(TState)' in assembly: 'Microsoft.Extensions.Logging.dll'	Blauhaus.Analytics.Samples.BlazorClient	C:\Users\Jedi\.nuget\packages\microsoft.aspnetcore.blazor.build\3.2.0-preview1.20073.1\targets\Blazor.MonoRuntime.targets	258	
        //https://github.com/mono/mono/issues/16063

        public static WebAssemblyHostBuilder ConfigureServices(this WebAssemblyHostBuilder builder)
        {
            var services = builder.Services;

           // services.RegisterAspNetCoreWebAnalyticsService<AnalyticsConfig>(new DefaultTraceListener());

            return builder;
        }

        public static WebAssemblyHostBuilder AddComponents(this WebAssemblyHostBuilder builder)
        {
            builder.RootComponents.Add<App>("app");
            return builder;
        }
    }
}