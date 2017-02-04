using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoProject
{
    public class Diagram
    {
        [JsonProperty(PropertyName = "class")]
        public string Class { get; set; }

        [JsonProperty(PropertyName = "copiesArrays")]
        public bool CopiesArrays { get; set; }

        [JsonProperty(PropertyName = "copiesArrayObjects")]
        public bool CopiesArrayObjects { get; set; }

        [JsonProperty(PropertyName = "linkFromPortIdProperty")]
        public string LinkFromPortIdProperty { get; set; }

        [JsonProperty(PropertyName = "linkToPortIdProperty")]
        public string LinkToPortIdProperty { get; set; }

        [JsonProperty(PropertyName = "nodeDataArray")]
        public List<NodeDataArray> NodeDataArray { get; set; }

        [JsonProperty(PropertyName = "linkDataArray")]
        public List<LinkDataArray> LinkDataArray { get; set; }

        [JsonProperty(PropertyName = "modelData")]
        public ModelData ModelData { get; set; }
        
    }
}