using System.Collections.Generic;
using System.Drawing;
using GoProject.DataTableHelper;
using GoProject.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoProject.Nodes
{
    public interface INode
    {
        #region Properties

        [JsonProperty(PropertyName = "key", NullValueHandling = NullValueHandling.Ignore)]
        string Key { get; set; }

        [TableConverter(ItemConverterType = typeof(int))]
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "category", NullValueHandling = NullValueHandling.Ignore)]
        NodeCategory Category { get; set; }

        [TableIgnore]
        [JsonProperty(PropertyName = "item", NullValueHandling = NullValueHandling.Ignore)]
        string Item { get; set; }

        [JsonProperty(PropertyName = "loc", NullValueHandling = NullValueHandling.Ignore)]
        string Loc { get; set; }

        [JsonProperty(PropertyName = "text", NullValueHandling = NullValueHandling.Ignore)]
        string Text { get; set; }

        [TableConverter(ItemConverterType = typeof(int?))]
        [JsonProperty(PropertyName = "eventType", NullValueHandling = NullValueHandling.Ignore)]
        EventType? EventType { get; set; }

        [TableConverter(ItemConverterType = typeof(int?))]
        [JsonProperty(PropertyName = "eventDimension", NullValueHandling = NullValueHandling.Ignore)]
        EventDimension? EventDimension { get; set; }

        [JsonProperty(PropertyName = "group", NullValueHandling = NullValueHandling.Ignore)]
        string Group { get; set; }

        [TableConverter(ItemConverterType = typeof(int?))]
        [JsonProperty(PropertyName = "taskType", NullValueHandling = NullValueHandling.Ignore)]
        TaskType? TaskType { get; set; }

        [TableIgnore]
        [JsonProperty(PropertyName = "boundaryEventArray", NullValueHandling = NullValueHandling.Ignore)]
        List<object> BoundaryEventArray { get; set; }

        [JsonProperty(PropertyName = "isGroup", NullValueHandling = NullValueHandling.Ignore)]
        bool? IsGroup { get; set; }

        [JsonProperty(PropertyName = "color", NullValueHandling = NullValueHandling.Ignore)]
        string Color { get; set; }

        [JsonProperty(PropertyName = "size", NullValueHandling = NullValueHandling.Ignore)]
        string Size { get; set; }

        [TableConverter(ItemConverterType = typeof(int?))]
        [JsonProperty(PropertyName = "gatewayType", NullValueHandling = NullValueHandling.Ignore)]
        GatewayType? GatewayType { get; set; }

        [JsonProperty(PropertyName = "isSubProcess", NullValueHandling = NullValueHandling.Ignore)]
        bool? IsSubProcess { get; set; }

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        string Name { get; set; }

        /// <summary>
        /// Extra data of this node elements
        /// </summary>
        [TableIgnore]
        [JsonProperty(PropertyName = "details", NullValueHandling = NullValueHandling.Ignore)]
        Dictionary<string, object> Details { get; set; }

        [TableIgnore]
        [JsonIgnore]
        [JsonProperty(PropertyName = "position", NullValueHandling = NullValueHandling.Ignore)]
        PointF? Position { get; set; }

        [TableIgnore]
        [JsonIgnore]
        [JsonProperty(PropertyName = "sizeF", NullValueHandling = NullValueHandling.Ignore)]
        SizeF? SizeF { get; set; }

        [TableIgnore]
        [JsonIgnore]
        [JsonProperty(PropertyName = "htmlColor", NullValueHandling = NullValueHandling.Ignore)]
        Color? HexColor { get; set; }

        #endregion
    }
}