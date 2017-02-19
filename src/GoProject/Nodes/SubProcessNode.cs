using System;
using System.Collections.Generic;
using System.Drawing;
using GoProject.Properties;

namespace GoProject.Nodes
{
    public class SubProcessNode : GroupNode
    {

        #region Properties

        private readonly NodeCategory _category = NodeCategory.subprocess;

        public override NodeCategory Category => _category;
        public new GatewayType? GatewayType => null;
        public new EventType? EventType => null;
        public new EventDimension? EventDimension => null;
        public new TaskType? TaskType => null;
        public new string Color => null;
        public new Color? HexColor => null;
        public new string Size => null;
        public new SizeF? SizeF => null;
        public new bool? IsSubProcess => true;

        #endregion

        #region Constructs

        public SubProcessNode()
        {
            Key = $"subProcess_{Guid.NewGuid()}";
            Text = Localization.Subprocess;
        }

        public SubProcessNode(INode node) : base(node)
        {}

        #endregion

    }
}