using System.Collections.Generic;

namespace GoProject.Nodes
{
    public interface IGoupNode
    {
        IEnumerable<Node> GetNodes();
    }
}
