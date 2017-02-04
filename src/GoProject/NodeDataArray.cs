using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoProject
{
    public class NodeDataArray
    {
        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "item")]
        public string Item { get; set; }

        [JsonProperty(PropertyName = "key")]
        public object Key { get; set; }

        [JsonProperty(PropertyName = "loc")]
        public string Loc { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "eventType")]
        public int EventType { get; set; }

        [JsonProperty(PropertyName = "eventDimension")]
        public int EventDimension { get; set; }

        [JsonProperty(PropertyName = "group")]
        public object Group { get; set; }

        [JsonProperty(PropertyName = "taskType")]
        public int? TaskType { get; set; }

        [JsonProperty(PropertyName = "boundaryEventArray")]
        public List<object> BoundaryEventArray { get; set; }

        [JsonProperty(PropertyName = "isGroup")]
        public object IsGroup { get; set; }

        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "size")]
        public string Size { get; set; }

        [JsonProperty(PropertyName = "gatewayType")]
        public int? GatewayType { get; set; }
    }
}