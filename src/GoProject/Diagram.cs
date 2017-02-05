using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoProject
{
    public class Diagram
    {
        [JsonProperty(PropertyName = "class", NullValueHandling = NullValueHandling.Ignore)]
        public string Class { get; set; }

        [JsonProperty(PropertyName = "copiesArrays", NullValueHandling = NullValueHandling.Ignore)]
        public bool CopiesArrays { get; set; }

        [JsonProperty(PropertyName = "copiesArrayObjects", NullValueHandling = NullValueHandling.Ignore)]
        public bool CopiesArrayObjects { get; set; }

        [JsonProperty(PropertyName = "linkFromPortIdProperty", NullValueHandling = NullValueHandling.Ignore)]
        public string LinkFromPortIdProperty { get; set; }

        [JsonProperty(PropertyName = "linkToPortIdProperty", NullValueHandling = NullValueHandling.Ignore)]
        public string LinkToPortIdProperty { get; set; }

        [JsonProperty(PropertyName = "nodeDataArray", NullValueHandling = NullValueHandling.Ignore)]
        public List<NodeDataArray> NodeDataArray { get; set; }

        [JsonProperty(PropertyName = "linkDataArray", NullValueHandling = NullValueHandling.Ignore)]
        public List<LinkDataArray> LinkDataArray { get; set; }

        [JsonProperty(PropertyName = "modelData", NullValueHandling = NullValueHandling.Ignore)]
        public ModelData ModelData { get; set; }
        
    }
}