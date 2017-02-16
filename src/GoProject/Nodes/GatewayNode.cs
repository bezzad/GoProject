using System.Collections.Generic;
using System.Drawing;

namespace GoProject.Nodes
{
    public class GatewayNode : Node
    {
        #region Fields

        private readonly NodeCategory _category;
        private readonly TaskType? _taskType;
        private readonly EventType? _eventType;
        private readonly EventDimension? _eventDimension;
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
        public override TaskType? TaskType => _taskType;
        public override EventType? EventType => _eventType;
        public override EventDimension? EventDimension => _eventDimension;
        public override bool? IsSubProcess => _isSubProcess;
        public override bool? IsGroup => _isGroup;
        public override List<object> BoundaryEventArray => _boundaryEventArray;
        public override string Color => _color;
        public override Color? HexColor => _hexColor;
        public override string Size => _size;
        public override SizeF? SizeF => _sizeF;
        public override string Group => _group;

        #endregion

        #region Constructs

        public GatewayNode()
        {
            Text = Properties.Localization.Gateway;
            _category = NodeCategory.gateway;
            _isSubProcess = null;
            _isGroup = null;
            _boundaryEventArray = null;
            _color = null;
            _hexColor = null;
            _size = null;
            _sizeF = null;
            _group = null;
            _taskType = null;
            _eventType = null;
            _eventDimension = null;
        }

        #endregion
    }
}