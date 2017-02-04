using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoProject
{
    public class LinkDataArray
    {
        [JsonProperty(PropertyName = "from")]
        public int From { get; set; }

        [JsonProperty(PropertyName = "to")]
        public int To { get; set; }

        [JsonProperty(PropertyName = "fromPort")]
        public string FromPort { get; set; }

        [JsonProperty(PropertyName = "toPort")]
        public string ToPort { get; set; }

        [JsonProperty(PropertyName = "points")]
        public List<double> Points { get; set; }

        [JsonProperty(PropertyName = "visible")]
        public bool? Visible { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }
    }
}