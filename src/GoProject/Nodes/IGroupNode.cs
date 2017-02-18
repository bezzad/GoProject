using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoProject.Nodes
{
    public interface IGroupNode
    {
        [JsonIgnore]
        List<Node> Nodes { get; set; }

        IEnumerable<Node> GetNodes();
    }
}
