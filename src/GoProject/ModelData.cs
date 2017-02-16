using System;
using Newtonsoft.Json;
using System.Drawing;

namespace GoProject
{
    public class ModelData
    {
        [JsonProperty(PropertyName = "position", NullValueHandling = NullValueHandling.Ignore)]
        public string Position { get; set; }

        [JsonIgnore]
        public PointF? PositionPoint
        {
            get
            {
                if (string.IsNullOrEmpty(Position)) return null;
                var data = Position.Split(' ');
                return new PointF(float.Parse(data[0]), float.Parse(data[1]));
            }

            set
            {
                if (value != null) Position = $"{value.Value.X} {value.Value.Y}";
            }
        }
    }
}
