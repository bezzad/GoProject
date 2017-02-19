using System;
using System.Collections.Generic;
using System.Drawing;
using GoProject.Properties;

namespace GoProject.Nodes
{
    public class PoolNode : GroupNode
    {

        #region Properties

        private readonly NodeCategory _category = NodeCategory.Pool;

        public override NodeCategory Category => _category;
        public new GatewayType? GatewayType => null;
        public new EventType? EventType => null;
        public new EventDimension? EventDimension => null;
        public new TaskType? TaskType => null;
        public new bool? IsSubProcess => null;
        public new List<object> BoundaryEventArray => null;
        public new string Color => null;
        public new Color? HexColor => null;

        #endregion

        #region Constructs

        public PoolNode()
        {
            Key = $"pool_{Guid.NewGuid()}";
            Text = Localization.Pool;
        }

        public PoolNode(INode node) : base(node)
        {}

        #endregion
    }
}