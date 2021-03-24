using Orleans.Runtime;

namespace Blauhaus.Analytics.Orleans.Context
{
    public class OrleansRequestContext : IOrleansRequestContext
    {
        public void Set(string key, object value)
        {
            RequestContext.Set(key, value);
        }

        public object Get(string key)
        {
            return RequestContext.Get(key);
        }
    }
}