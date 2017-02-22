using System.Collections.Generic;
using System.Drawing;
using GoProject.Enums;
using GoProject.Properties;

namespace GoProject.Nodes
{
    public class GatewayNode : Node
    {

        #region Properties

        private readonly NodeCategory _category=NodeCategory.gateway;

        public override NodeCategory Category => _category;
        public new TaskType? TaskType => null;
        public new EventType? EventType => null;
        public new EventDimension? EventDimension => null;
        public new bool? IsSubProcess => null;
        public new bool? IsGroup => null;
        public new List<object> BoundaryEventArray => null;
        public new string Color => null;
        public new Color? HexColor => null;
        public new string Size => null;
        public new SizeF? SizeF => null;

        #endregion

        #region Constructs

        public GatewayNode()
        {
            Text = Localization.Gateway;
        }

        public GatewayNode(INode node) : base(node)
        {}

        #endregion
    }
}