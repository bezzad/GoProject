using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoProject.Nodes
{
    public interface INode
    {
        #region Properties

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "category", NullValueHandling = NullValueHandling.Ignore)]
        NodeCategory Category { get; set; }

        [JsonProperty(PropertyName = "item", NullValueHandling = NullValueHandling.Ignore)]
        string Item { get; set; }

        [JsonProperty(PropertyName = "key", NullValueHandling = NullValueHandling.Ignore)]
        string Key { get; set; }

        [JsonProperty(PropertyName = "loc", NullValueHandling = NullValueHandling.Ignore)]
        string Loc { get; set; }

        [JsonProperty(PropertyName = "text", NullValueHandling = NullValueHandling.Ignore)]
        string Text { get; set; }

        [JsonProperty(PropertyName = "eventType", NullValueHandling = NullValueHandling.Ignore)]
        EventType? EventType { get; set; }

        [JsonProperty(PropertyName = "eventDimension", NullValueHandling = NullValueHandling.Ignore)]
        EventDimension? EventDimension { get; set; }

        [JsonProperty(PropertyName = "group", NullValueHandling = NullValueHandling.Ignore)]
        string Group { get; set; }

        [JsonProperty(PropertyName = "taskType", NullValueHandling = NullValueHandling.Ignore)]
        TaskType? TaskType { get; set; }

        [JsonProperty(PropertyName = "boundaryEventArray", NullValueHandling = NullValueHandling.Ignore)]
        List<object> BoundaryEventArray { get; set; }

        [JsonProperty(PropertyName = "isGroup", NullValueHandling = NullValueHandling.Ignore)]
        bool? IsGroup { get; set; }

        [JsonProperty(PropertyName = "color", NullValueHandling = NullValueHandling.Ignore)]
        string Color { get; set; }

        [JsonProperty(PropertyName = "size", NullValueHandling = NullValueHandling.Ignore)]
        string Size { get; set; }

        [JsonProperty(PropertyName = "gatewayType", NullValueHandling = NullValueHandling.Ignore)]
        GatewayType? GatewayType { get; set; }

        [JsonProperty(PropertyName = "isSubProcess", NullValueHandling = NullValueHandling.Ignore)]
        bool? IsSubProcess { get; set; }

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        string Name { get; set; }

        /// <summary>
        /// Extra data of this node elements
        /// </summary>
        [JsonProperty(PropertyName = "details", NullValueHandling = NullValueHandling.Ignore)]
        Dictionary<string, object> Details { get; set; }

        #endregion
    }
}