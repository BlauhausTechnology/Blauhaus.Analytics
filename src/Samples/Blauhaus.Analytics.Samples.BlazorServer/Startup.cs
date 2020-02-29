using System.Diagnostics;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.AspNetCore._Ioc;
using Blauhaus.Analytics.Samples.BlazorServer._Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Blauhaus.Analytics.Samples.BlazorServer.Data;
using Blauhaus.Analytics.Server.AspNetCore.Middleware;
using Blauhaus.Common.ValueObjects.BuildConfigs;

namespace Blauhaus.Analytics.Samples.BlazorServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            RegisterBuildConfig(services);

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<WeatherForecastService>();

            
            //analytics
            services.AddHttpContextAccessor();
            services.RegisterAspNetCoreAnalyticsService<AnalyticsConfig>(new ConsoleTraceListener());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAnalyticsService analyticsService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            analyticsService.Trace(this, "Server Started");
        }

        private static void RegisterBuildConfig(IServiceCollection services)
        {
            #if DEBUG
            services.AddSingleton<IBuildConfig>(BuildConfig.Debug);
            #else
            services.AddSingleton<IBuildConfig>(BuildConfig.Release);
            #endif
        }

    }
}
