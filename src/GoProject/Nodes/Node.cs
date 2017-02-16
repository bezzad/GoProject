using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;

namespace GoProject.Nodes
{
    public class Node
    {
        #region Properties

        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        [JsonProperty(PropertyName = "category", NullValueHandling = NullValueHandling.Ignore)]
        public virtual NodeCategory Category { get; set; }

        [JsonProperty(PropertyName = "item", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Item { get; set; }

        [JsonProperty(PropertyName = "key", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Key { get; set; }

        [JsonProperty(PropertyName = "loc", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Loc { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "position", NullValueHandling = NullValueHandling.Ignore)]
        public virtual PointF? Position
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
        public virtual EventType? EventType { get; set; }

        [JsonProperty(PropertyName = "eventDimension", NullValueHandling = NullValueHandling.Ignore)]
        public virtual EventDimension? EventDimension { get; set; }

        [JsonProperty(PropertyName = "group", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Group { get; set; }

        [JsonProperty(PropertyName = "taskType", NullValueHandling = NullValueHandling.Ignore)]
        public virtual TaskType? TaskType { get; set; }

        [JsonProperty(PropertyName = "boundaryEventArray", NullValueHandling = NullValueHandling.Ignore)]
        public virtual List<object> BoundaryEventArray { get; set; }

        [JsonProperty(PropertyName = "isGroup", NullValueHandling = NullValueHandling.Ignore)]
        public virtual bool? IsGroup { get; set; }

        [JsonProperty(PropertyName = "color", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Color { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "htmlColor", NullValueHandling = NullValueHandling.Ignore)]
        public virtual Color? HexColor
        {
            get
            {
                if (string.IsNullOrEmpty(Color)) return null;
                var data = ColorTranslator.FromHtml(Color);
                return data;
            }
            set
            {
                if (value != null) Color = ColorTranslator.ToHtml(value.Value);
            }
        }

        [JsonProperty(PropertyName = "size", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Size { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "sizeF", NullValueHandling = NullValueHandling.Ignore)]
        public virtual SizeF? SizeF
        {
            get
            {
                if (string.IsNullOrEmpty(Size)) return null;
                var data = Size.Split(' ');
                return new SizeF(float.Parse(data[0]), float.Parse(data[1]));
            }

            set
            {
                if (value != null) Size = $"{value.Value.Width} {value.Value.Height}";
            }
        }

        [JsonProperty(PropertyName = "gatewayType", NullValueHandling = NullValueHandling.Ignore)]
        public virtual GatewayType? GatewayType { get; set; }

        [JsonProperty(PropertyName = "isSubProcess", NullValueHandling = NullValueHandling.Ignore)]
        public virtual bool? IsSubProcess { get; set; }

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Extra data of this node elements
        /// </summary>
        [JsonProperty(PropertyName = "details", NullValueHandling = NullValueHandling.Ignore)]
        public virtual Dictionary<string, object> Details { get; set; }

        #endregion

        public static List<Node> PaletteNodes()
        {
            var nodes = new List<Node>();

            // -------------------------- Event Nodes -------------------------------
            nodes.AddRange(new List<Node>
            {
                new EventNode { Text = Properties.Localization.Start, EventType = GoProject.EventType.None, EventDimension = GoProject.EventDimension.StartNone },
                new EventNode { Text = Properties.Localization.Message, EventType = GoProject.EventType.Message, EventDimension = GoProject.EventDimension.StartInter },
                new EventNode { Text = Properties.Localization.Timer, EventType = GoProject.EventType.Timer, EventDimension = GoProject.EventDimension.StartNonInter },
                new EventNode { Text = Properties.Localization.End, EventType = GoProject.EventType.None, EventDimension = GoProject.EventDimension.End },
                new EventNode { Text = Properties.Localization.Message, EventType = GoProject.EventType.Message, EventDimension = GoProject.EventDimension.End },
                new EventNode { Text = Properties.Localization.Terminate, EventType = GoProject.EventType.Terminate, EventDimension = GoProject.EventDimension.End }
            });

            // -------------------------- Task/Activity Nodes -------------------------------
            nodes.AddRange(new List<Node>
            {
                new Node { Category = NodeCategory.activity, Text = Properties.Localization.Task, TaskType = GoProject.TaskType.EmptyTask },
                new Node { Category = NodeCategory.activity, Text = Properties.Localization.UserTask, TaskType = GoProject.TaskType.User },
                new Node { Category = NodeCategory.activity, Text = Properties.Localization.ServiceTask, TaskType = GoProject.TaskType.Service }
            });


            // -------------------------- Subprocess and start and end -------------------------------
            nodes.AddRange(new List<Node>
            {
                new Node { Key = "task", Category = NodeCategory.subprocess, Text = Properties.Localization.Subprocess, TaskType = GoProject.TaskType.EmptyTask, IsSubProcess = true, IsGroup = true },
                new Node { Category = NodeCategory.@event, Text = Properties.Localization.Start, EventType = GoProject.EventType.None, EventDimension = GoProject.EventDimension.StartNone, Group = "task", Position = new PointF(0, 0) },
                new Node { Category = NodeCategory.@event, Text = Properties.Localization.End, EventType = GoProject.EventType.None, EventDimension = GoProject.EventDimension.End, Group = "task", Position = new PointF(250, 0) },
                new Node { Category = NodeCategory.gateway, Text = Properties.Localization.Parallel, GatewayType = GoProject.GatewayType.Parallel, Group = "task", Position = new PointF(125, 0)  }
            });

            // -------------------------- Gateway Nodes, Data, Pool and Annotation -------------------------------
            nodes.AddRange(new List<Node>
            {
                new Node { Category = NodeCategory.gateway, Text = Properties.Localization.Parallel, GatewayType = GoProject.GatewayType.Parallel },
                new Node { Category = NodeCategory.gateway, Text = Properties.Localization.Exclusive, GatewayType = GoProject.GatewayType.Exclusive },

                new Node { Category = NodeCategory.dataobject, Text = Properties.Localization.DataObject },
                new Node { Category = NodeCategory.datastore, Text = Properties.Localization.DataStorage },
                new Node { Category = NodeCategory.privateProcess, Text = Properties.Localization.BlackBox, SizeF = new SizeF(300, 80)},
                new Node { Category = NodeCategory.annotation, Text = Properties.Localization.Note },

                new Node { Category = NodeCategory.Pool, Text = Properties.Localization.Pool, IsGroup = true, Key = "pool" },
                new Node { Category = NodeCategory.Lane, Text = Properties.Localization.NewLane, Group = "pool", HexColor = System.Drawing.Color.LightGoldenrodYellow, IsGroup = true },
                new Node { Category = NodeCategory.Lane, Text = Properties.Localization.NewLane, Group = "pool", HexColor = System.Drawing.Color.LightGreen, IsGroup = true  }
            });


            return nodes;
        }
    }
}