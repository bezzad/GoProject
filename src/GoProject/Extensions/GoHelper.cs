using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GoProject.Enums;
using GoProject.Nodes;
using GoProject.Properties;

namespace GoProject.Extensions
{
    public static class GoHelper
    {

        public static List<Node> PaletteTreeNodes()
        {
            var nodes = new List<Node>();

            // -------------------------- Event Nodes -------------------------------
            nodes.AddRange(new List<Node>
            {
                new EventNode
                {
                    Text = Localization.Start,
                    EventType = EventType.None,
                    EventDimension = EventDimension.Start
                },
                new EventNode
                {
                    Text = Localization.Message,
                    EventType = EventType.Message,
                    EventDimension = EventDimension.StartInter
                },
                new EventNode
                {
                    Text = Localization.Timer,
                    EventType = EventType.Timer,
                    EventDimension = EventDimension.StartNonInter
                },
                new EventNode {Text = Localization.End, EventType = EventType.None, EventDimension = EventDimension.End},
                new EventNode
                {
                    Text = Localization.Message,
                    EventType = EventType.Message,
                    EventDimension = EventDimension.End
                },
                new EventNode
                {
                    Text = Localization.Terminate,
                    EventType = EventType.Terminate,
                    EventDimension = EventDimension.End
                }
            });

            // -------------------------- Task/Activity Nodes -------------------------------
            nodes.AddRange(new List<Node>
            {
                new ActivityNode {Text = Localization.Task, TaskType = TaskType.EmptyTask},
                new ActivityNode {Text = Localization.UserTask, TaskType = TaskType.User},
                new ActivityNode {Text = Localization.ServiceTask, TaskType = TaskType.Service}
            });


            // -------------------------- Subprocess and start and end -------------------------------
            nodes.AddRange(new List<Node>
            {
                new SubProcessNode
                {
                    Key = "task",
                    Text = Localization.Subprocess,
                    Nodes =
                    {
                        new EventNode
                        {
                            Text = Localization.Start,
                            EventType = EventType.None,
                            EventDimension = EventDimension.Start,
                            Group = "task",
                            Position = new PointF(0, 0)
                        },
                        new EventNode
                        {
                            Text = Localization.End,
                            EventType = EventType.None,
                            EventDimension = EventDimension.End,
                            Group = "task",
                            Position = new PointF(250, 0)
                        }
                    }
                }
            });

            // -------------------------- Gateway Nodes, Data, Pool and Annotation -------------------------------
            nodes.AddRange(new List<Node>
            {
                new GatewayNode {Text = Localization.Parallel, GatewayType = GatewayType.Parallel},
                new GatewayNode {Text = Localization.Exclusive, GatewayType = GatewayType.Exclusive},

                new DataNode {Category = NodeCategory.dataobject, Text = Localization.DataObject},
                new DataNode {Category = NodeCategory.datastore, Text = Localization.DataStorage},
                new DataNode
                {
                    Category = NodeCategory.privateProcess,
                    Text = Localization.BlackBox,
                    SizeF = new SizeF(300, 80)
                },
                new DataNode {Category = NodeCategory.annotation, Text = Localization.Note}
            });

            nodes.Add(new PoolNode
            {
                Nodes =
                {
                    new LaneNode(),
                    new LaneNode()
                }
            });

            return nodes;
        }

        public static Node GetTrueTypeNode(this INode node)
        {
            switch (node.Category)
            {
                case NodeCategory.Pool:
                    return new PoolNode(node);
                case NodeCategory.Lane:
                    return new LaneNode(node);
                case NodeCategory.subprocess:
                    return new SubProcessNode(node);
                case NodeCategory.@event:
                    return new EventNode(node);
                case NodeCategory.activity:
                    return new ActivityNode(node);
                case NodeCategory.gateway:
                    return new GatewayNode(node);
                default:
                    return new DataNode(node);
            }
        }

        /// <summary>
        /// Find any node real place in a tree for sibling, children or parent placement
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static List<Node> ConvertToTreeNodes(this List<Node> nodes)
        {
            var tree = new List<Node>();

            var trueTypes = nodes.Select(no => no.GetTrueTypeNode()).ToList();

            foreach (var node in trueTypes)
            {
                if (string.IsNullOrEmpty(node.Group)) // no have parent group key
                {
                    tree.Add(node); // is parent node
                }
                else // is child! but maybe has parent also!
                {
                    // find node parent:
                    var parent =
                        trueTypes.FirstOrDefault(
                            x =>
                                string.Equals(node.Group, x.Key, StringComparison.InvariantCultureIgnoreCase) &&
                                x.IsGroup == true);
                    ((IGroupNode)parent)?.Nodes.Add(node);
                }
            }

            return tree;
        }

    }
}