using System.Collections.Generic;
using System.Drawing;

namespace GoProject.Nodes
{
    public class EventNode : Node
    {
        #region Fields

        private readonly NodeCategory _category;
        private readonly GatewayType? _gatewayType;
        private readonly TaskType? _taskType;
        private readonly bool? _isSubProcess;
        private readonly bool? _isGroup;
        private readonly List<object> _boundaryEventArray;
        private readonly string _color;
        private readonly Color? _hexColor;
        private readonly string _size;
        private readonly SizeF? _sizeF;
        private readonly string _group;

        #endregion

        #region Properties

        public override NodeCategory Category => _category;
        public override GatewayType? GatewayType => _gatewayType;
        public override TaskType? TaskType => _taskType;
        public override bool? IsSubProcess => _isSubProcess;
        public override bool? IsGroup => _isGroup;
        public override List<object> BoundaryEventArray => _boundaryEventArray;
        public override string Color => _color;
        public override Color? HexColor => _hexColor;
        public override string Size => _size;
        public override SizeF? SizeF => _sizeF;
        public override string Group => _group;

        #endregion


        public EventNode()
        {
            Text = Properties.Localization.Event;
            _category = NodeCategory.@event;
            _gatewayType = null;
            _taskType = null;
            _isSubProcess = null;
            _isGroup = null;
            _boundaryEventArray = null;
            _color = null;
            _hexColor = null;
            _size = null;
            _sizeF = null;
            _group = null;
        }
    }
}