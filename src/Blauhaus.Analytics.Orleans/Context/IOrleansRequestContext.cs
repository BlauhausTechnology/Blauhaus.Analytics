namespace Blauhaus.Analytics.Orleans.Context
{
    public interface IOrleansRequestContext
    {
        void Set(string key, object value);
        object Get(string key);
    }
}