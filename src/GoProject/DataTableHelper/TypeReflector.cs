using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace GoProject.DataTableHelper
{
    public static class TypeReflector
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var cur in enumerable)
            {
                action(cur);
            }
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            var dt = new DataTable();

            // create table columns by object properties
            var properties = typeof(T).GetProperties().Where(p => p.GetAttribute<TableIgnoreAttribute>() == null).ToList();

            foreach (var prop in properties)
            {
                var tableAttr = prop.GetAttribute<TableConverterAttribute>();
                var colName = tableAttr?.ColumnName ?? prop.Name;

                dt.Columns.Add(colName);
            }

            if (data == null) return dt;


            // add any rows to data table
            foreach (var item in data)
            {
                var row = dt.Rows.Add();

                foreach (var prop in properties)
                {
                    var tableAttr = prop.GetAttribute<TableConverterAttribute>();

                    var colName = tableAttr?.ColumnName ?? prop.Name;
                    var colValue = prop.GetValue(item);

                    if (tableAttr?.ItemConverterType != null)
                    {
                        var colType = tableAttr.ItemConverterType;
                        colValue = Convert.ChangeType(colValue, colType);
                    }
                    row[colName] = colValue;
                }
            }

            return dt;
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
