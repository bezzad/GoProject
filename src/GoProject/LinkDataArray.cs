using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoProject
{
    public class LinkDataArray
    {
        [JsonProperty(PropertyName = "from", NullValueHandling = NullValueHandling.Ignore)]
        public int From { get; set; }

        [JsonProperty(PropertyName = "to", NullValueHandling = NullValueHandling.Ignore)]
        public int To { get; set; }

        [JsonProperty(PropertyName = "fromPort", NullValueHandling = NullValueHandling.Ignore)]
        public string FromPort { get; set; }

        [JsonProperty(PropertyName = "toPort", NullValueHandling = NullValueHandling.Ignore)]
        public string ToPort { get; set; }

        [JsonProperty(PropertyName = "points", NullValueHandling = NullValueHandling.Ignore)]
        public List<double> Points { get; set; }

        [JsonProperty(PropertyName = "visible", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Visible { get; set; }

        [JsonProperty(PropertyName = "text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }
    }
}