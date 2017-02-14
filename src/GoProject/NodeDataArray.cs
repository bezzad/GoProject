using System.Collections.Generic;
using Newtonsoft.Json;
using System.Drawing;

namespace GoProject
{
    public class NodeDataArray
    {
        [JsonProperty(PropertyName = "category", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public NodeCategory Category { get; set; }

        [JsonProperty(PropertyName = "item", NullValueHandling = NullValueHandling.Ignore)]
        public string Item { get; set; }

        [JsonProperty(PropertyName = "key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "loc", NullValueHandling = NullValueHandling.Ignore)]
        public string Loc { get; set; }

        [JsonProperty(PropertyName = "position", NullValueHandling = NullValueHandling.Ignore)]
        [JsonIgnore]
        public PointF? Position
        {
            get
            {
                if (string.IsNullOrEmpty(Loc)) return null;
                var data = Loc.Split(' ');
                return new PointF(float.Parse(data[0]), float.Parse(data[1]));
            }

            set
            {
                if (value != null) Loc = $"{value.Value.X} {value.Value.Y}";
            }
        }

        [JsonProperty(PropertyName = "text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "eventType", NullValueHandling = NullValueHandling.Ignore)]
        public EventType? EventType { get; set; }

        [JsonProperty(PropertyName = "eventDimension", NullValueHandling = NullValueHandling.Ignore)]
        public EventDimension? EventDimension { get; set; }

        [JsonProperty(PropertyName = "group", NullValueHandling = NullValueHandling.Ignore)]
        public string Group { get; set; }

        [JsonProperty(PropertyName = "taskType", NullValueHandling = NullValueHandling.Ignore)]
        public TaskType? TaskType { get; set; }

        [JsonProperty(PropertyName = "boundaryEventArray", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> BoundaryEventArray { get; set; }

        [JsonProperty(PropertyName = "isGroup", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsGroup { get; set; }

        [JsonProperty(PropertyName = "color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "size", NullValueHandling = NullValueHandling.Ignore)]
        public string Size { get; set; }

        [JsonProperty(PropertyName = "gatewayType", NullValueHandling = NullValueHandling.Ignore)]
        public GatewayType? GatewayType { get; set; }

        [JsonProperty(PropertyName = "isSubProcess", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSubProcess { get; set; }

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Extra data of this node elements
        /// </summary>
        [JsonProperty(PropertyName = "details", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Details { get; set; }


        public static List<NodeDataArray> PaletteNodes()
        {
            var nodes = new List<NodeDataArray>();

            // -------------------------- Event Nodes -------------------------------
            nodes.AddRange(new List<NodeDataArray>()
            {
                new NodeDataArray() { Category = NodeCategory.@event, Text = Properties.Localization.Start, EventType = GoProject.EventType.Empty, EventDimension = GoProject.EventDimension.Solid },
                new NodeDataArray() { Category = NodeCategory.@event, Text = Properties.Localization.Message, EventType = GoProject.EventType.Message, EventDimension = GoProject.EventDimension.Chromatic },
                new NodeDataArray() { Category = NodeCategory.@event, Text = Properties.Localization.Timer, EventType = GoProject.EventType.Timer, EventDimension = GoProject.EventDimension.Dashed },
                new NodeDataArray() { Category = NodeCategory.@event, Text = Properties.Localization.End, EventType = GoProject.EventType.Empty, EventDimension = GoProject.EventDimension.RedFill },
                new NodeDataArray() { Category = NodeCategory.@event, Text = Properties.Localization.Message, EventType = GoProject.EventType.Message, EventDimension = GoProject.EventDimension.RedFill },
                new NodeDataArray() { Category = NodeCategory.@event, Text = Properties.Localization.Terminate, EventType = GoProject.EventType.Inclusive, EventDimension = GoProject.EventDimension.RedFill }
            });

            // -------------------------- Task/Activity Nodes -------------------------------
            nodes.AddRange(new List<NodeDataArray>()
            {
                new NodeDataArray() { Category = NodeCategory.activity, Text = Properties.Localization.Task, TaskType = GoProject.TaskType.EmptyTask },
                new NodeDataArray() { Category = NodeCategory.activity, Text = Properties.Localization.UserTask, TaskType = GoProject.TaskType.User },
                new NodeDataArray() { Category = NodeCategory.activity, Text = Properties.Localization.ServiceTask, TaskType = GoProject.TaskType.Service }
            });


            // -------------------------- Subprocess and start and end -------------------------------
            nodes.AddRange(new List<NodeDataArray>()
            {
                new NodeDataArray() { Key = "task", Category = NodeCategory.subprocess, Text = Properties.Localization.Subprocess, TaskType = GoProject.TaskType.EmptyTask, IsSubProcess = true, IsGroup = true },
                new NodeDataArray() { Category = NodeCategory.@event, Text = Properties.Localization.Start, EventType = GoProject.EventType.Empty, EventDimension = GoProject.EventDimension.Solid, Group = "task" },
                new NodeDataArray() { Category = NodeCategory.@event, Text = Properties.Localization.End, EventType = GoProject.EventType.Empty, EventDimension = GoProject.EventDimension.RedFill, Group = "task" }
            });

            // -------------------------- Gateway Nodes, Data, Pool and Annotation -------------------------------
            nodes.AddRange(new List<NodeDataArray>()
            {
                new NodeDataArray() { Category = NodeCategory.gateway, Text = Properties.Localization.Parallel, GatewayType = GoProject.GatewayType.Parallel },
                new NodeDataArray() { Category = NodeCategory.gateway, Text = Properties.Localization.Exclusive, GatewayType = GoProject.GatewayType.Exclusive },

                new NodeDataArray() { Category = NodeCategory.dataobject, Text = Properties.Localization.DataObject },
                new NodeDataArray() { Category = NodeCategory.datastore, Text = Properties.Localization.DataStorage },
                new NodeDataArray() { Category = NodeCategory.privateProcess, Text = Properties.Localization.BlackBox },
                new NodeDataArray() { Category = NodeCategory.annotation, Text = Properties.Localization.Note },

                new NodeDataArray() { Category = NodeCategory.Pool, Text = Properties.Localization.Pool, IsGroup = true, Key = "pool" },
                new NodeDataArray() { Category = NodeCategory.Lane, Text = Properties.Localization.NewLane, Group = "pool", Color = "lightyellow", IsGroup = true },
                new NodeDataArray() { Category = NodeCategory.Lane, Text = Properties.Localization.NewLane, Group = "pool", Color = "lightgreen", IsGroup = true  }
            });


            return nodes;
        }
    }
}