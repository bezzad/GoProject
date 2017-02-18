using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;

namespace GoProject.Nodes
{
    public class Node : INode
    {
        public NodeCategory Category { get; set; }
        public string Item { get; set; }
        public string Key { get; set; }
        public string Loc { get; set; }
        public string Text { get; set; }
        public EventType? EventType { get; set; }
        public EventDimension? EventDimension { get; set; }
        public string Group { get; set; }
        public TaskType? TaskType { get; set; }
        public List<object> BoundaryEventArray { get; set; }
        public bool? IsGroup { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public GatewayType? GatewayType { get; set; }
        public bool? IsSubProcess { get; set; }
        public string Name { get; set; }
        public Dictionary<string, object> Details { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "position", NullValueHandling = NullValueHandling.Ignore)]
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


        [JsonIgnore]
        [JsonProperty(PropertyName = "sizeF", NullValueHandling = NullValueHandling.Ignore)]
        public SizeF? SizeF
        {
            get
            {
                if (string.IsNullOrEmpty(Size)) return null;
                var data = Size.Split(' ');
                return new SizeF(float.Parse(data[0]), float.Parse(data[1]));
            }

            set
            {
                if (value != null) Size = $"{value.Value.Width} {value.Value.Height}";
            }
        }


        [JsonIgnore]
        [JsonProperty(PropertyName = "htmlColor", NullValueHandling = NullValueHandling.Ignore)]
        public Color? HexColor
        {
            get
            {
                if (string.IsNullOrEmpty(Color)) return null;
                var data = ColorTranslator.FromHtml(Color);
                return data;
            }
            set
            {
                if (value != null) Color = ColorTranslator.ToHtml(value.Value);
            }
        }

    }
}