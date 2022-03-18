﻿using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Orleans;
using Blauhaus.Analytics.Orleans.Session;
using Blauhaus.Analytics.Serilog.Ioc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Blauhaus.Analytics.Serilog.Orleans.Ioc
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddOrleansSerilogAnalyticsService(this IServiceCollection services, Action<LoggerConfiguration> config)
        {
            
            services.AddScoped<IAnalyticsContext, OrleansAnalyticsContext>();
            services.AddTransient(typeof(IAnalyticsLogger<>), typeof(AnalyticsLogger<>));

            return services;
        }
    }
}