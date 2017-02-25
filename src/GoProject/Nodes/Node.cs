using System.Collections.Generic;
using System.Drawing;
using GoProject.Enums;

// ReSharper disable VirtualMemberCallInConstructor

namespace GoProject.Nodes
{
    public class Node : INode
    {
        #region Properties

        public virtual NodeCategory Category { get; set; }
        public string Item { get; set; }
        public virtual string Key { get; set; }
        public string Loc { get; set; }
        public string Text { get; set; }
        public EventType? EventType { get; set; }
        public EventDimension? EventDimension { get; set; }
        public GatewayType? GatewayType { get; set; }
        public TaskType? TaskType { get; set; }
        public string Group { get; set; }
        public List<object> BoundaryEventArray { get; set; }
        public bool? IsGroup { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public bool? IsSubProcess { get; set; }
        public string Name { get; set; }
        public IDictionary<string, object> Details { get; set; }
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
        public SizeF? SizeF
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
        public Color? HexColor
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

        #endregion

        #region Constructors

        public Node(){}

        public Node(INode node) : this()
        {
            if (node != null)
            {
                Category = node.Category;
                Item = node.Item;
                Key = node.Key;
                Loc = node.Loc;
                Text = node.Text;
                EventType = node.EventType;
                EventDimension = node.EventDimension;
                Group = node.Group;
                TaskType = node.TaskType;
                BoundaryEventArray = node.BoundaryEventArray;
                IsGroup = node.IsGroup;
                Color = node.Color;
                Size = node.Size;
                GatewayType = node.GatewayType;
                IsSubProcess = node.IsSubProcess;
                Name = node.Name;
                Details = node.Details;
            }
        }


        #endregion
    }
}