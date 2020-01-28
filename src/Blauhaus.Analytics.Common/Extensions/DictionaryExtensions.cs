using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace Blauhaus.Analytics.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<string, string> ToDictionaryOfStrings(this Dictionary<string, object> properties)
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

                    if (double.TryParse(property.Value.ToString(), out var numericValue))
                    {
                        stringifiedProperties[property.Key] = numericValue.ToString(CultureInfo.InvariantCulture);
                    }

                    else
                    {
                        stringifiedProperties[property.Key] = JsonConvert.SerializeObject(property.Value);
                    }
                }
            }

            return stringifiedProperties;
        }
    }
}