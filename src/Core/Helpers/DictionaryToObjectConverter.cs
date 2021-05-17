using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Core.Helpers
{
    /// <summary>
    /// Create a class instance with the respective property values based on the content of the dictionary
    /// </summary>
    public static class DictionaryToObjectConverter
    {
        public static object ConvertToAnonymousObject(this Dictionary<string, object> dict)
        {
            var eo = new ExpandoObject();
            var eoColl = (ICollection<KeyValuePair<string, object>>)eo;
            
            foreach (var kvp in dict)
            {
                eoColl.Add(kvp);
            }
            dynamic eoDynamic = eo;
            return eoDynamic;
        }
    }
}
