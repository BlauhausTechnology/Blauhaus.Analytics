﻿using Blauhaus.Analytics.Serilog.Ioc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Blauhaus.Analytics.Serilog.AspNetCore.Ioc
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddAspNetCoreSerilogAnalyticsService(this IServiceCollection services, string appName, Action<LoggerConfiguration> config)
        {

            services.AddSerilogAnalyticsService<SerilogAnalyticsService, AspNetCoreSessionFactory, InMemoryAnalyticsContext>(appName, config);
            

            return services;
        }
    }
}