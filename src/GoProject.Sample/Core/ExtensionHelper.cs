using System.Collections.Generic;
using System.Data;

namespace GoProject.Sample.Core
{
    public static class ExtensionHelper
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            var dt = new DataTable();

            // create table columns by object properties
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                dt.Columns.Add(prop.Name);
            }

            if (data == null) return dt;


            // add any rows to data table
            foreach (var item in data)
            {
                var row = dt.Rows.Add();

                foreach (var prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }
            }

            return dt;
        }
    }
}