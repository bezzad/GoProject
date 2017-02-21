using System.Drawing;
using GoProject.Enums;
using GoProject.Properties;
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace GoProject.Nodes
{
    public class ActivityNode : Node
    {

        #region Properties

        private readonly NodeCategory _category = NodeCategory.activity;

        public override NodeCategory Category => _category;
        public new GatewayType? GatewayType => null;
        public new EventType? EventType => null;
        public new EventDimension? EventDimension => null;
        public new bool? IsSubProcess => null;
        public new bool? IsGroup => null;
        public new string Color => null;
        public new Color? HexColor => null;
        
        #endregion

        #region Constructs

        public ActivityNode()
        {
            Text = Localization.Activity;
        }

        public ActivityNode(INode node) : base(node)
        {}

        #endregion

    }
}