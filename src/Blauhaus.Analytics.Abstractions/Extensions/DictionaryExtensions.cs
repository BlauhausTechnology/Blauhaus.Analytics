using System.Collections.Generic;
using System.Text.Json;

namespace Blauhaus.Analytics.Abstractions.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<string, string> ToDictionaryOfStrings(this Dictionary<string, object>? properties)
        {
            var stringifiedProperties = new Dictionary<string, string>();

            if (properties == null) return stringifiedProperties;
            
            foreach (var property in properties)
            {
                if (property.Value != null)
                {
                    if (property.Value is string stringValue)
                    {
                        stringifiedProperties[property.Key] = stringValue;
                    }
                    else
                    {
                        stringifiedProperties[property.Key] = JsonSerializer.Serialize(property.Value);
                    }
                }
            }

            return stringifiedProperties;
        }
    }
}