using System.Collections.Generic;
using System.Linq;

namespace GoProject.Nodes
{
    public class GroupNode : Node, IGroupNode
    {
        private List<Node> _nodes;

        public new bool? IsGroup => true;
        public List<Node> Nodes { get { return _nodes; } set { value.ForEach(AddNode); } }

        
        public GroupNode()
        {
            _nodes = new List<Node>();
        }
        public GroupNode(INode node) : base(node)
        {
            _nodes = new List<Node>();
        }

        public virtual IEnumerable<Node> GetNodes()
        {
            var result = Nodes.Where(n => n is IGroupNode).SelectMany(x => ((IGroupNode)x).GetNodes()).ToList();// find child nodes
            result.AddRange(Nodes); // top level nodes

            return result;
        }
        public void AddNode(Node node)
        {
            node.Group = Key;
            Nodes.Add(node);
        }
    }
}
