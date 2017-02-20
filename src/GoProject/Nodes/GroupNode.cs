using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GoProject.DataTableHelper;
using Newtonsoft.Json;

namespace GoProject.Nodes
{
    public class GroupNode : Node, IGroupNode
    {
        #region Members

        private string _key;
        private ObservableCollection<Node> _nodes;
        private readonly object _locker = new object();


        public override string Key { get { return _key; } set { _key = value; Nodes?.ForEach(c => c.Group = _key); } }
        public new bool? IsGroup => true;

        [TableIgnore]
        [JsonIgnore]
        public ObservableCollection<Node> Nodes
        {
            get
            {
                return _nodes;
            }
            set
            {
                lock (_locker)
                {
                    if (_nodes != null) _nodes.CollectionChanged -= nodes_CollectionChanged;
                    _nodes = value;
                    if (_nodes != null)
                    {
                        _nodes?.ForEach(c => c.Group = _key);
                        _nodes.CollectionChanged += nodes_CollectionChanged;
                    }
                }
            }
        }

        #endregion

        #region Constructors

        public GroupNode()
        {
            _nodes = new ObservableCollection<Node>();
            _nodes.CollectionChanged += nodes_CollectionChanged;
        }

        public GroupNode(INode node) : base(node)
        {
            _nodes = new ObservableCollection<Node>();
            _nodes.CollectionChanged += nodes_CollectionChanged;
        }

        #endregion

        #region Methods

        public virtual IEnumerable<Node> GetNodes()
        {
            var result = Nodes.Where(n => n is IGroupNode).SelectMany(x => ((IGroupNode)x).GetNodes()).ToList();// find child nodes
            result.AddRange(Nodes); // top level nodes

            return result;
        }

        public void AddNode(Node node)
        {
            node.Group = Key;
            lock (_locker)
            {
                Nodes.Add(node);
            }
        }

        private void nodes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (Node item in e.NewItems)
            {
                item.Group = Key;
            }
        }

        ~GroupNode()
        {
            if (Nodes != null) Nodes.CollectionChanged -= nodes_CollectionChanged;
        }

        #endregion
    }
}