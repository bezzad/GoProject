using System.Collections.Generic;
using System.Drawing;
using GoProject.Properties;

namespace GoProject.Nodes
{
    public class DataNode : Node
    {
        #region Properties
        
        public new GatewayType? GatewayType { get; }

        public new EventType? EventType { get; }

        public new EventDimension? EventDimension { get; }

        public new TaskType? TaskType { get; }

        public new bool? IsSubProcess { get; }

        public new bool? IsGroup { get; }

        public new List<object> BoundaryEventArray { get; }

        public new string Color { get; }

        public new Color? HexColor { get; }
        
        public new string Group { get; }

        #endregion

        #region Constructs

        public DataNode()
        {
            Text = Localization.Data;
            Category = NodeCategory.activity;
            EventType = null;
            EventDimension = null;
            TaskType = null;
            GatewayType = null;
            IsSubProcess = null;
            IsGroup = null;
            BoundaryEventArray = null;
            Color = null;
            HexColor = null;
            Group = null;
        }

        #endregion

    }
}