using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using GoProject.Nodes;
using Newtonsoft.Json;

namespace GoProject
{
    public class Diagram
    {
        public Diagram()
        {
            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture =
                Thread.CurrentThread.CurrentUICulture = CultureInfo.DefaultThreadCurrentCulture ?? CultureInfo.CreateSpecificCulture("en");
        }

        public Diagram(CultureInfo ci)
        {
            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture =
                Thread.CurrentThread.CurrentUICulture = ci;
        }

        [JsonProperty(PropertyName = "class", NullValueHandling = NullValueHandling.Ignore)]
        public string Class { get; set; } = "go.GraphLinksModel";

        [JsonProperty(PropertyName = "copiesArrays", NullValueHandling = NullValueHandling.Ignore)]
        public bool CopiesArrays { get; set; } = true;

        [JsonProperty(PropertyName = "copiesArrayObjects", NullValueHandling = NullValueHandling.Ignore)]
        public bool CopiesArrayObjects { get; set; } = true;

        [JsonProperty(PropertyName = "linkFromPortIdProperty", NullValueHandling = NullValueHandling.Ignore)]
        public string LinkFromPortIdProperty { get; set; }

        [JsonProperty(PropertyName = "linkToPortIdProperty", NullValueHandling = NullValueHandling.Ignore)]
        public string LinkToPortIdProperty { get; set; }

        [JsonIgnore]
        public List<Node> TreeNodes
        {
            get
            {
                return NodeDataArray?.ConvertToTreeNodes();
            }
            set
            {
                NodeDataArray = value?.Where(n => n is IGroupNode).SelectMany(x => ((IGroupNode)x).GetNodes()).ToList();// find child nodes
                NodeDataArray?.AddRange(value); // top level nodes
            }
        }

        [JsonProperty(PropertyName = "nodeDataArray", NullValueHandling = NullValueHandling.Ignore)]
        public List<Node> NodeDataArray { get; set; }


        [JsonProperty(PropertyName = "linkDataArray", NullValueHandling = NullValueHandling.Ignore)]
        public List<LinkDataArray> LinkDataArray { get; set; }

        [JsonProperty(PropertyName = "modelData", NullValueHandling = NullValueHandling.Ignore)]
        public ModelData ModelData { get; set; }

        [JsonIgnore]
        public string Position { get { return ModelData.Position; } set { ModelData.Position = value; } }

        [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "isReadonly", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsReadonly { get; set; } = false;
    }
}