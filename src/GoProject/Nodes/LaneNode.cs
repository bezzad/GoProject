using System;
using System.Collections.Generic;
using GoProject.Properties;

namespace GoProject.Nodes
{
    public class LaneNode : GroupNode
    {
        #region Properties

        public new NodeCategory Category { get; }

        public new GatewayType? GatewayType { get; }

        public new EventType? EventType { get; }

        public new EventDimension? EventDimension { get; }

        public new TaskType? TaskType { get; }

        public new bool? IsSubProcess { get; }
        
        public new List<object> BoundaryEventArray { get; }

        #endregion

        #region Constructs

        public LaneNode()
        {
            Key = $"lane_{Guid.NewGuid()}";
            Text = Localization.NewLane;
            Category = NodeCategory.Lane;

            EventType = null;
            EventDimension = null;
            TaskType = null;
            GatewayType = null;
            IsSubProcess = null;
            BoundaryEventArray = null;

            var rand = new Random();
            HexColor = System.Drawing.Color.FromArgb(rand.Next(150, 255), rand.Next(150, 255), rand.Next(150, 255));
        }

        #endregion

    }
}