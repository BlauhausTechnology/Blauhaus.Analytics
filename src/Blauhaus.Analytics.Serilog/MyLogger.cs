using Microsoft.Extensions.Logging;

namespace Blauhaus.Analytics.Serilog;

public class MyLogger<T> : Logger<T>
{
    public MyLogger(ILoggerFactory factory) : base(factory)
    {
    }
    
}