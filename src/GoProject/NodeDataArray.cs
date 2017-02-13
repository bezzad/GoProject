using System.Collections.Generic;
using Newtonsoft.Json;
using System.Drawing;

namespace GoProject
{
    public class NodeDataArray
    {
        [JsonProperty(PropertyName = "category", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public NodeCategory Category { get; set; }

        [JsonProperty(PropertyName = "item", NullValueHandling = NullValueHandling.Ignore)]
        public string Item { get; set; }

        [JsonProperty(PropertyName = "key", NullValueHandling = NullValueHandling.Ignore)]
        public object Key { get; set; }

        [JsonProperty(PropertyName = "loc", NullValueHandling = NullValueHandling.Ignore)]
        public string Loc { get; set; }

        [JsonProperty(PropertyName = "position", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public PointF? Position
        {
            get
            {
                if (string.IsNullOrEmpty(Loc)) return null;
                var data = Loc.Split(' ');
                return new PointF(float.Parse(data[0]), float.Parse(data[1]));
            }

            set
            {
                if (value != null) Loc = $"{value.Value.X} {value.Value.Y}";
            }
        }

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
        public bool IsGroup { get; set; }

        [JsonProperty(PropertyName = "color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "size", NullValueHandling = NullValueHandling.Ignore)]
        public string Size { get; set; }

        [JsonProperty(PropertyName = "gatewayType", NullValueHandling = NullValueHandling.Ignore)]
        public int? GatewayType { get; set; }

        [JsonProperty(PropertyName = "isSubProcess", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSubProcess { get; set; }

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Extra data of this node elements
        /// </summary>
        [JsonProperty(PropertyName = "details", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Details { get; set; }
    }
}