using System;
using Newtonsoft.Json;

namespace GoProject
{
    public class ModelData
    {
        [JsonProperty(PropertyName = "position")]
        public string Position { get; set; }
    }
}
