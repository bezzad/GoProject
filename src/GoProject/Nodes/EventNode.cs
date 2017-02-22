using System.Collections.Generic;
using System.Drawing;
using GoProject.Enums;
using GoProject.Properties;

namespace GoProject.Nodes
{
    public class EventNode : Node
    {

        #region Properties

        private readonly NodeCategory _category = NodeCategory.@event;

        public override NodeCategory Category => _category;
        public new GatewayType? GatewayType => null;
        public new TaskType? TaskType => null;
        public new bool? IsSubProcess => null;
        public new bool? IsGroup => null;
        public new List<object> BoundaryEventArray => null;
        public new string Color => null;
        public new Color? HexColor => null;
        public new string Size => null;
        public new SizeF? SizeF => null;

        #endregion


        #region Constructors
        
        public EventNode()
        {
            Text = Localization.Event;
        }

        public EventNode(INode node) : base(node)
        {}

        #endregion

    }
}