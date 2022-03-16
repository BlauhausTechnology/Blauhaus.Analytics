using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

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
                        stringifiedProperties[property.Key] = JsonConvert.SerializeObject(property.Value, Formatting.Indented, new JsonSerializerSettings
                        {
                            Culture = CultureInfo.InvariantCulture
                        });
                    }
                }
            }

            return stringifiedProperties;
        }
    }
}