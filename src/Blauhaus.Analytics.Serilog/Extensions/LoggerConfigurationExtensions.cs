using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Blauhaus.Analytics.Serilog.Extensions;

public static class LoggerConfigurationExtensions
{
    public static LoggerConfiguration ApplyLoggingConfiguration(this LoggerConfiguration logging, IConfiguration configuration, Dictionary<string, LogEventLevel>? sourceContexts = null)
    {
        sourceContexts ??= new Dictionary<string, LogEventLevel>();
            
        var loggingSection = configuration.GetSection("Logging");
        var logLevel = loggingSection.GetValue<LogEventLevel>("level");
        logging.MinimumLevel.Is(logLevel);

        foreach (var child in loggingSection.GetChildren())
        {
            var context = child.GetValue<string>("SourceContext");
            if (!string.IsNullOrEmpty(context))
            {
                sourceContexts[context] = child.GetValue<LogEventLevel>("Level");
            }
        }

        foreach (var defaultContext in sourceContexts)
        {
            logging.MinimumLevel.Override(defaultContext.Key, defaultContext.Value);
        }

        return logging;
    }
}