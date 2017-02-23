using System.Collections.Generic;
using GoProject.DataTableHelper;
using GoProject.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoProject
{
    public class Link
    {
        [JsonProperty(PropertyName = "from", NullValueHandling = NullValueHandling.Ignore)]
        public int From { get; set; }

        [JsonProperty(PropertyName = "to", NullValueHandling = NullValueHandling.Ignore)]
        public int To { get; set; }

        [JsonProperty(PropertyName = "fromPort", NullValueHandling = NullValueHandling.Ignore)]
        public string FromPort { get; set; }

        [JsonProperty(PropertyName = "toPort", NullValueHandling = NullValueHandling.Ignore)]
        public string ToPort { get; set; }

        [TableIgnore]
        [JsonProperty(PropertyName = "points", NullValueHandling = NullValueHandling.Ignore)]
        public List<double> Points { get; set; }

        [JsonIgnore]
        public string PointsJson
        {
            get
            {
                if (Points == null) return null;
                return $"[{string.Join(",", Points)}]";
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length > 2)
                {
                    Points = JsonConvert.DeserializeObject<List<double>>(value);
                }
            }
        }


        /// <summary>
        /// Show connector label or not?
        /// Sample:   '------lable-------+>'
        /// </summary>
        [JsonProperty(PropertyName = "visible", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Visible { get; set; }

        [JsonProperty(PropertyName = "text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "category", NullValueHandling = NullValueHandling.Ignore)]
        public LinkCategory? Category { get; set; }

        /// <summary>
        /// Is Default Connection by symbole '--/----+>'
        /// </summary>
        [JsonProperty(PropertyName = "isDefault", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsDefault { get; set; }
    }
}