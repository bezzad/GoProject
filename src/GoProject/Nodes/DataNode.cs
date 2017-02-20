using System.Collections.Generic;
using System.Drawing;
using GoProject.Enums;
using GoProject.Properties;

namespace GoProject.Nodes
{
    public class DataNode : Node
    {
        #region Properties

        public new GatewayType? GatewayType => null;
        public new EventType? EventType => null;
        public new EventDimension? EventDimension => null;
        public new TaskType? TaskType => null;
        public new bool? IsSubProcess => null;
        public new bool? IsGroup => null;
        public new List<object> BoundaryEventArray => null;
        public new string Color => null;
        public new Color? HexColor => null;

        #endregion

        #region Constructs

        public DataNode()
        {
            Text = Localization.Data;
            Category = NodeCategory.dataobject;
        }

        public DataNode(INode node) : base(node)
        { }

        #endregion
    }
}