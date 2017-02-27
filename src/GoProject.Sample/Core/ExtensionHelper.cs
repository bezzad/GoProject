using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace GoProject.Sample.Core
{
    public static class ExtensionHelper
    {
        public static dynamic DapperDynamicToExpandoObject(this object value)
        {
            IDictionary<string, object> dapperRowProperties = value as IDictionary<string, object>;

            IDictionary<string, object> expando = new ExpandoObject();

            foreach (KeyValuePair<string, object> property in dapperRowProperties)
                expando.Add(property.Key, property.Value);

            return expando as ExpandoObject;
        }

        public static dynamic ToExpandoObjects(this IEnumerable<dynamic> value)
        {
            return value.Select(x => (ExpandoObject)DapperDynamicToExpandoObject(x));
        }
    }
}