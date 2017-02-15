using System.Collections.Generic;
using System.Drawing;

namespace GoProject.Nodes
{
    public sealed class GatewayNode : NodeDataArray
    {
        #region Fields

        private readonly NodeCategory _category;
        private readonly TaskType? _taskType;
        private readonly bool? _isSubProcess;
        private readonly bool? _isGroup;
        private readonly List<object> _boundaryEventArray;
        private readonly string _color;
        private readonly Color? _hexColor;
        private readonly string _size;
        private readonly SizeF? _sizeF;
        private readonly string _group;
        private readonly EventType? _eventType;
        private readonly EventDimension? _eventDimension;


        #endregion

        #region Properties

        public override NodeCategory Category => _category;
        public override TaskType? TaskType => _taskType;
        public override bool? IsSubProcess => _isSubProcess;
        public override bool? IsGroup => _isGroup;
        public override List<object> BoundaryEventArray => _boundaryEventArray;
        public override string Color => _color;
        public override Color? HexColor => _hexColor;
        public override string Size => _size;
        public override SizeF? SizeF => _sizeF;
        public override string Group => _group;
        public override EventType? EventType => _eventType;
        public override EventDimension? EventDimension => _eventDimension;

        #endregion

        #region Constructs

        public GatewayNode()
        {
            Text = Properties.Localization.Event;
            _category = NodeCategory.gateway;
            _taskType = null;
            _isSubProcess = null;
            _isGroup = null;
            _boundaryEventArray = null;
            _color = null;
            _hexColor = null;
            _size = null;
            _sizeF = null;
            _group = null;
            _eventType = null;
            _eventDimension = null;
        }

        #endregion

    }
}