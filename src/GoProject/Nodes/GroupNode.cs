using System;
using System.Collections.Generic;
using System.Linq;

namespace GoProject.Nodes
{
    public class GroupNode : Node, IGoupNode
    {
        public new bool? IsGroup { get; }
        public List<Node> Nodes { get; set; }

        


        public GroupNode()
        {
            Nodes = new List<Node>();
            IsGroup = true;
        }


        public virtual IEnumerable<Node> GetNodes()
        {
            var result = Nodes.Where(n => n is IGoupNode).SelectMany(x => ((IGoupNode)x).GetNodes()).ToList();// find child nodes
            result.AddRange(Nodes); // top level nodes

            return result;
        }
        public void AddNode(Node node)
        {
            node.Group = this.Key;
            Nodes.Add(node);
        }
    }
}
