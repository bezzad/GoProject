using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoProject
{
    public class NodeDataArray
    {
        [JsonProperty(PropertyName = "category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "item", NullValueHandling = NullValueHandling.Ignore)]
        public string Item { get; set; }

        [JsonProperty(PropertyName = "key", NullValueHandling = NullValueHandling.Ignore)]
        public object Key { get; set; }

        [JsonProperty(PropertyName = "loc", NullValueHandling = NullValueHandling.Ignore)]
        public string Loc { get; set; }

        [JsonProperty(PropertyName = "text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "eventType", NullValueHandling = NullValueHandling.Ignore)]
        public int EventType { get; set; }

        [JsonProperty(PropertyName = "eventDimension", NullValueHandling = NullValueHandling.Ignore)]
        public int EventDimension { get; set; }

        [JsonProperty(PropertyName = "group", NullValueHandling = NullValueHandling.Ignore)]
        public object Group { get; set; }

        [JsonProperty(PropertyName = "taskType", NullValueHandling = NullValueHandling.Ignore)]
        public int? TaskType { get; set; }

        [JsonProperty(PropertyName = "boundaryEventArray", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> BoundaryEventArray { get; set; }

        [JsonProperty(PropertyName = "isGroup", NullValueHandling = NullValueHandling.Ignore)]
        public object IsGroup { get; set; }

        [JsonProperty(PropertyName = "color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "size", NullValueHandling = NullValueHandling.Ignore)]
        public string Size { get; set; }

        [JsonProperty(PropertyName = "gatewayType", NullValueHandling = NullValueHandling.Ignore)]
        public int? GatewayType { get; set; }
    }
}