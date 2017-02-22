using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using GoProject.DataTableHelper;
using GoProject.Extensions;
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

        [TableIgnore]
        [JsonProperty(PropertyName = "copiesArrays", NullValueHandling = NullValueHandling.Ignore)]
        public bool CopiesArrays { get; set; } = true;

        [TableIgnore]
        [JsonProperty(PropertyName = "copiesArrayObjects", NullValueHandling = NullValueHandling.Ignore)]
        public bool CopiesArrayObjects { get; set; } = true;

        [TableIgnore]
        [JsonProperty(PropertyName = "linkFromPortIdProperty", NullValueHandling = NullValueHandling.Ignore)]
        public string LinkFromPortIdProperty { get; set; }

        [TableIgnore]
        [JsonProperty(PropertyName = "linkToPortIdProperty", NullValueHandling = NullValueHandling.Ignore)]
        public string LinkToPortIdProperty { get; set; }

        [JsonIgnore]
        [TableIgnore]
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
        public List<Link> LinkDataArray { get; set; }

        [TableIgnore]
        [JsonProperty(PropertyName = "modelData", NullValueHandling = NullValueHandling.Ignore)]
        public ModelData ModelData { get; set; }

        [JsonIgnore]
        public string Position
        {
            get { return ModelData?.Position; }
            set { ModelData = new ModelData() { Position = value }; }
        }

        [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "isReadOnly", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsReadOnly { get; set; } = false;
    }
}