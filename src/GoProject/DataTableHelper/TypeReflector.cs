using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace GoProject.DataTableHelper
{
    public static class TypeReflector
    {


        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            var dt = new DataTable();

            // create table columns by object properties
            var properties = typeof(T).GetProperties().Where(p => p.CanRead && p.GetAttribute<TableIgnoreAttribute>() == null).ToList();

            var converterCache = new Dictionary<string, TableConverterAttribute>();

            foreach (var prop in properties)
            {
                converterCache[prop.Name] = prop.GetAttribute<TableConverterAttribute>();
                dt.Columns.Add(converterCache[prop.Name]?.ColumnName ?? prop.Name);
            }

            if (data == null) return dt;


            // add any rows to data table
            foreach (var item in data)
            {
                var row = dt.Rows.Add();

                foreach (var prop in properties)
                {
                    var converter = converterCache[prop.Name];
                    var colName = converter?.ColumnName ?? prop.Name;
                    var colValue = prop.GetValue(item);

                    if (converter?.ItemConverterType != null)
                    {
                        //Coalesce to set the safe value using default(t) or the safe type.
                        colValue = colValue.ChangeType(converter.ItemConverterType);
                    }
                    row[colName] = colValue;
                }
            }
            return dt;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> data, IEnumerable<string> columnsOrder)
        {
            var orders = columnsOrder as List<string> ?? columnsOrder?.ToList();
            if (orders == null || !orders.Any()) return data.ToDataTable();

            var dt = new DataTable();

            // create table columns by object properties
            var properties = typeof(T).GetProperties().Where(p => p.CanRead && p.GetAttribute<TableIgnoreAttribute>() == null)
                .Select(p => new { Attr = p.GetAttribute<TableConverterAttribute>(), Prop = p }).ToList();

            properties.Sort((x1, x2) =>
                orders.FindIndex(co => (x1.Attr?.ColumnName ?? x1.Prop.Name) == co)
                    .CompareTo(orders.FindIndex(co => (x2.Attr?.ColumnName ?? x2.Prop.Name) == co)));

            foreach (var p in properties)
            {
                dt.Columns.Add(p.Attr?.ColumnName ?? p.Prop.Name);
            }

            if (data == null) return dt;

            // add any rows to data table
            foreach (var item in data)
            {
                var row = dt.Rows.Add();

                foreach (var p in properties)
                {
                    var colValue = p.Prop.GetValue(item);

                    if (p.Attr?.ItemConverterType != null)
                    {
                        //Coalesce to set the safe value using default(t) or the safe type.
                        colValue = colValue.ChangeType(p.Attr.ItemConverterType);
                    }
                    row[p.Attr?.ColumnName ?? p.Prop.Name] = colValue;
                }
            }

            return dt;
        }


        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var cur in enumerable)
            {
                action(cur);
            }
        }

        public static T ChangeType<T>(this object value)
        {
            var t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (value == null)
                {
                    return default(T);
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return (T)Convert.ChangeType(value, t);
        }

        public static object ChangeType(this object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }


        public static T GetAttribute<T>(this object attributeProvider) where T : Attribute
        {
            return GetAttribute<T>(attributeProvider, true);
        }

        public static T GetAttribute<T>(this object attributeProvider, bool inherit) where T : Attribute
        {
            T[] attributes = GetAttributes<T>(attributeProvider, inherit);

            return attributes?.FirstOrDefault();
        }

        public static T[] GetAttributes<T>(this object attributeProvider, bool inherit) where T : Attribute
        {
            Attribute[] a = GetAttributes(attributeProvider, typeof(T), inherit);

            T[] attributes = a as T[];
            if (attributes != null)
            {
                return attributes;
            }

            return a.Cast<T>().ToArray();
        }

        public static Attribute[] GetAttributes(this object attributeProvider, Type attributeType, bool inherit)
        {
            if (attributeProvider == null)
                throw new ArgumentNullException(nameof(attributeProvider));

            object provider = attributeProvider;

            // http://hyperthink.net/blog/getcustomattributes-gotcha/
            // ICustomAttributeProvider doesn't do inheritance

            Type t = provider as Type;
            if (t != null)
            {
                object[] array = attributeType != null ? t.GetCustomAttributes(attributeType, inherit) : t.GetCustomAttributes(inherit);
                Attribute[] attributes = array.Cast<Attribute>().ToArray();


                // ye olde .NET GetCustomAttributes doesn't respect the inherit argument
                if (inherit && t.BaseType != null)
                {
                    attributes = attributes.Union(GetAttributes(t.BaseType, attributeType, inherit)).ToArray();
                }


                return attributes;
            }

            Assembly a = provider as Assembly;
            if (a != null)
            {
                return (attributeType != null) ? Attribute.GetCustomAttributes(a, attributeType) : Attribute.GetCustomAttributes(a);
            }

            MemberInfo mi = provider as MemberInfo;
            if (mi != null)
            {
                return (attributeType != null) ? Attribute.GetCustomAttributes(mi, attributeType, inherit) : Attribute.GetCustomAttributes(mi, inherit);
            }

            Module m = provider as Module;
            if (m != null)
            {
                return (attributeType != null) ? Attribute.GetCustomAttributes(m, attributeType, inherit) : Attribute.GetCustomAttributes(m, inherit);
            }

            ParameterInfo p = provider as ParameterInfo;
            if (p != null)
            {
                return (attributeType != null) ? Attribute.GetCustomAttributes(p, attributeType, inherit) : Attribute.GetCustomAttributes(p, inherit);
            }

            ICustomAttributeProvider customAttributeProvider = (ICustomAttributeProvider)attributeProvider;
            object[] result = (attributeType != null) ? customAttributeProvider.GetCustomAttributes(attributeType, inherit) : customAttributeProvider.GetCustomAttributes(inherit);

            return (Attribute[])result;
        }

        private static T GetAttribute<T>(this Type type) where T : Attribute
        {
            var attribute = GetAttribute<T>(type, true);
            if (attribute != null)
            {
                return attribute;
            }

            foreach (Type typeInterface in type.GetInterfaces())
            {
                attribute = GetAttribute<T>(typeInterface, true);
                if (attribute != null)
                {
                    return attribute;
                }
            }

            return null;
        }

        private static T GetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
        {
            var attribute = GetAttribute<T>(memberInfo, true);
            if (attribute != null)
            {
                return attribute;
            }

            if (memberInfo.DeclaringType != null)
            {
                foreach (Type typeInterface in memberInfo.DeclaringType.GetInterfaces())
                {
                    MemberInfo interfaceTypeMemberInfo = GetMemberInfoFromType(typeInterface, memberInfo);

                    if (interfaceTypeMemberInfo != null)
                    {
                        attribute = GetAttribute<T>(interfaceTypeMemberInfo, true);
                        if (attribute != null)
                        {
                            return attribute;
                        }
                    }
                }
            }

            return null;
        }

        public static MemberInfo GetMemberInfoFromType(this Type targetType, MemberInfo memberInfo)
        {
            const BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            switch (memberInfo.MemberType)
            {
                case MemberTypes.Property:
                    PropertyInfo propertyInfo = (PropertyInfo)memberInfo;

                    Type[] types = propertyInfo.GetIndexParameters().Select(p => p.ParameterType).ToArray();

                    return targetType.GetProperty(propertyInfo.Name, bindingAttr, null, propertyInfo.PropertyType, types, null);
                default:
                    return targetType.GetMember(memberInfo.Name, memberInfo.MemberType, bindingAttr).SingleOrDefault();
            }
        }

    }
}