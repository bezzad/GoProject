using System;
using System.Collections.Generic;
using System.Drawing;
using GoProject.Properties;

namespace GoProject.Nodes
{
    public class PoolNode : GroupNode
    {
        #region Properties

        public new NodeCategory Category { get; }

        public new GatewayType? GatewayType { get; }

        public new EventType? EventType { get; }

        public new EventDimension? EventDimension { get; }

        public new TaskType? TaskType { get; }

        public new bool? IsSubProcess { get; }

        public new List<object> BoundaryEventArray { get; }

        public new string Color { get; }

        public new Color? HexColor { get; }
        

        #endregion

        #region Constructs

        public PoolNode()
        {
            Key = $"pool_{Guid.NewGuid()}";
            Text = Localization.Pool;
            Category = NodeCategory.Pool;

            EventType = null;
            EventDimension = null;
            TaskType = null;
            GatewayType = null;
            IsSubProcess = null;
            BoundaryEventArray = null;
            Color = null;
            HexColor = null;
        }

        #endregion
    }
}