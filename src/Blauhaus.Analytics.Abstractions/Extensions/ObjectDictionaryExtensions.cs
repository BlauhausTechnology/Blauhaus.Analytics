using System.Collections.Generic;

namespace Blauhaus.Analytics.Abstractions.Extensions
{
    public static class ObjectDictionaryExtensions
    {
        public static Dictionary<string, object> WithValue(this Dictionary<string, object> dictionary, string key, object value)
        {
            dictionary[key] = value;
            return dictionary;
        }

        public static Dictionary<string, object> WithValue(this Dictionary<string, object> dictionary, object value)
        {
            dictionary[value.GetType().Name] = value;
            return dictionary;
        }

        
        public static Dictionary<string, object> WithValues(this Dictionary<string, object> dictionary, Dictionary<string, string> values)
        {
            foreach (var value in values)
            {
                dictionary[value.Key] = value.Value;
            }
            return dictionary;
        }

        
        public static Dictionary<string, object> WithValues(this Dictionary<string, object> dictionary, IEnumerable<object> values)
        {
            foreach (var value in values)
            {
                dictionary[value.GetType().Name] = value;
            }
            return dictionary;
        }
        public static Dictionary<string, object> WithValues(this Dictionary<string, object> dictionary, params object[] values)
        {
            foreach (var value in values)
            {
                dictionary[value.GetType().Name] = value;
            }
            return dictionary;
        }
    }
}