using System.Collections.Generic;
using System.Drawing;
using GoProject.Nodes;
using GoProject.Properties;

namespace GoProject
{
    public static class GoHelper
    {
        public static List<Node> PaletteNodes()
        {
            var nodes = new List<Node>();

            // -------------------------- Event Nodes -------------------------------
            nodes.AddRange(new List<Node>
            {
                new EventNode { Text = Localization.Start, EventType = EventType.None, EventDimension = EventDimension.StartNone },
                new EventNode { Text = Localization.Message, EventType = EventType.Message, EventDimension = EventDimension.StartInter },
                new EventNode { Text = Localization.Timer, EventType = EventType.Timer, EventDimension = EventDimension.StartNonInter },
                new EventNode { Text = Localization.End, EventType = EventType.None, EventDimension = EventDimension.End },
                new EventNode { Text = Localization.Message, EventType = EventType.Message, EventDimension = EventDimension.End },
                new EventNode { Text = Localization.Terminate, EventType = EventType.Terminate, EventDimension = EventDimension.End }
            });

            // -------------------------- Task/Activity Nodes -------------------------------
            nodes.AddRange(new List<Node>
            {
                new ActivityNode { Text = Localization.Task, TaskType = TaskType.EmptyTask },
                new ActivityNode { Text = Localization.UserTask, TaskType = TaskType.User },
                new ActivityNode { Text = Localization.ServiceTask, TaskType = TaskType.Service }
            });


            // -------------------------- Subprocess and start and end -------------------------------
            nodes.AddRange(new List<Node>
            {
                new SubProcessNode { Key = "task", Text = Localization.Subprocess },
                new Node { Category = NodeCategory.@event, Text = Localization.Start, EventType = EventType.None, EventDimension = EventDimension.StartNone, Group = "task", Position = new PointF(0, 0) },
                new Node { Category = NodeCategory.@event, Text = Localization.End, EventType = EventType.None, EventDimension = EventDimension.End, Group = "task", Position = new PointF(250, 0) },
                new Node { Category = NodeCategory.gateway, Text = Localization.Parallel, GatewayType = GatewayType.Parallel, Group = "task", Position = new PointF(125, 0)  }
            });

            // -------------------------- Gateway Nodes, Data, Pool and Annotation -------------------------------
            nodes.AddRange(new List<Node>
            {
                new GatewayNode { Text = Localization.Parallel, GatewayType = GatewayType.Parallel },
                new GatewayNode { Text = Localization.Exclusive, GatewayType = GatewayType.Exclusive },

                new DataNode { Category = NodeCategory.dataobject, Text = Localization.DataObject },
                new DataNode { Category = NodeCategory.datastore, Text = Localization.DataStorage },
                new DataNode { Category = NodeCategory.privateProcess, Text = Localization.BlackBox, SizeF = new SizeF(300, 80)},
                new DataNode { Category = NodeCategory.annotation, Text = Localization.Note },

                new Node { Category = NodeCategory.Pool, Text = Localization.Pool, IsGroup = true, Key = "pool" },
                new Node { Category = NodeCategory.Lane, Text = Localization.NewLane, Group = "pool", HexColor = Color.LightGoldenrodYellow, IsGroup = true },
                new Node { Category = NodeCategory.Lane, Text = Localization.NewLane, Group = "pool", HexColor = Color.LightGreen, IsGroup = true  }
            });


            return nodes;
        }
    }
}
