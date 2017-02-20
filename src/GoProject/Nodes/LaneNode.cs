using System;
using System.Collections.Generic;
using System.Drawing;
using GoProject.Enums;
using GoProject.Properties;

namespace GoProject.Nodes
{
    public class LaneNode : GroupNode
    {

        #region Properties

        private readonly NodeCategory _category = NodeCategory.Lane;

        public override NodeCategory Category => _category;
        public new GatewayType? GatewayType => null;
        public new EventType? EventType => null;
        public new EventDimension? EventDimension => null;
        public new TaskType? TaskType => null;
        public new bool? IsSubProcess => null;
        public new List<object> BoundaryEventArray => null;

        #endregion

        #region Constructs

        public LaneNode()
        {
            Key = $"lane_{Guid.NewGuid()}";
            Text = Localization.NewLane;

            var rand = new Random();
            HexColor = System.Drawing.Color.FromArgb(rand.Next(150, 255), rand.Next(150, 255), rand.Next(150, 255));
            SizeF = new SizeF(300, 40);
        }

        public LaneNode(INode node) : base(node)
        {}

        #endregion

    }
}