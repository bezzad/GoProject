using System;
using Newtonsoft.Json;

namespace GoProject
{
    public class ModelData
    {
        [JsonProperty(PropertyName = "position", NullValueHandling = NullValueHandling.Ignore)]
        public string Position { get; set; }
    }
}
