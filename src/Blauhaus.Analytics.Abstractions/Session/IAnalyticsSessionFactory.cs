using System.Threading.Tasks;

namespace Blauhaus.Analytics.Abstractions.Session
{
    public interface IAnalyticsSessionFactory
    {
        ValueTask<string> GetSessionIdAsync();
    }
}