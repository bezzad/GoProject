using System.Collections.Generic;
using System.Drawing;
using GoProject.Properties;

namespace GoProject.Nodes
{
    public class SubProcessNode : Node
    {
        #region Properties

        public new NodeCategory Category { get; }

        public new GatewayType? GatewayType { get; }

        public new EventType? EventType { get; }

        public new EventDimension? EventDimension { get; }

        public new TaskType? TaskType { get; }

        public new bool? IsSubProcess { get; }

        public new bool? IsGroup { get; }

        public new List<object> BoundaryEventArray { get; }

        public new string Color { get; }

        public new Color? HexColor { get; }

        public new string Size { get; }

        public new SizeF? SizeF { get; }

        public new string Group { get; }

        #endregion

        #region Constructs

        public SubProcessNode()
        {
            Text = Localization.Subprocess;
            Category = NodeCategory.subprocess;
            EventType = null;
            EventDimension = null;
            GatewayType = null;
            TaskType = null;
            IsSubProcess = true;
            IsGroup = true;
            BoundaryEventArray = null;
            Color = null;
            HexColor = null;
            Size = null;
            SizeF = null;
            Group = null;
        }

        #endregion

    }
}