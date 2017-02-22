using System;

namespace GoProject.DataTableHelper
{
    /// <summary>
    /// Instructs the <see cref="T:GoProject.GoHelper.ToDataTable()" /> to use the specified when serializing the member or class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class TableConverterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:GoProject.DataTableHelper.TableConverterAttribute" /> class.
        /// </summary>
        public TableConverterAttribute()
        {
        }

        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="T:GoProject.DataTableHelper.ItemConverterType" /> used when serializing the property's collection items.
        /// </summary>
        public Type ItemConverterType { get; set; }
    }
}
