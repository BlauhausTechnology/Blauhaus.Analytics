using System.Collections.Generic;

namespace Blauhaus.Analytics.Abstractions.Extensions
{
    public class ObjectDictionary : Dictionary<string, object>
    {
        public ObjectDictionary(params object[] values)
        {
            this.WithValues(values);
        }
        
        public ObjectDictionary(IEnumerable<object> values)
        {
            this.WithValues(values);
        }
    }
}