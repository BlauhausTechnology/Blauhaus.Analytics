using System.Collections.Generic;

namespace Blauhaus.Analytics.Abstractions.Extensions
{
    public static class ObjectExtensions
    {
        public static Dictionary<string, object> ToObjectDictionary(this object? value, string key = "")
        {
            if (value == null)
            {
                return string.IsNullOrEmpty(key) 
                    ? new Dictionary<string, object>() 
                    : new Dictionary<string, object>{{key, "NULL"}};
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                key = value.GetType().Name;
            }

            return new Dictionary<string, object>
            {
                {key, value }
            };
        }
    }
}