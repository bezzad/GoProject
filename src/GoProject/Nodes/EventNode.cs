using System.Collections.Generic;
using System.Drawing;
using GoProject.Properties;

namespace GoProject.Nodes
{
    public class EventNode : Node
    {
      
        #region Properties

        public new NodeCategory Category { get; }

        public new GatewayType? GatewayType { get; }

        public new TaskType? TaskType { get; }

        public new bool? IsSubProcess { get; }

        public new bool? IsGroup { get; }

        public new List<object> BoundaryEventArray { get; }

        public new string Color { get; }

        public new Color? HexColor { get; }

        public new string Size { get; }

        public new SizeF? SizeF { get; }

        #endregion


        public EventNode()
        {
            Text = Localization.Event;
            Category = NodeCategory.@event;
            GatewayType = null;
            TaskType = null;
            IsSubProcess = null;
            IsGroup = null;
            BoundaryEventArray = null;
            Color = null;
            HexColor = null;
            Size = null;
            SizeF = null;
        }
    }
}