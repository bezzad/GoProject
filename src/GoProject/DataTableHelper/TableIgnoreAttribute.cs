using System;

namespace GoProject.DataTableHelper
{
    /// <summary>
    /// Instructs the <see cref="T:GoProject.GoHelper.ToDataTable()" /> not to serialize the public field or public read/write property value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class TableIgnoreAttribute : Attribute
    {
    }
}