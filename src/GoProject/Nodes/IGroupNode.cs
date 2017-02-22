using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace GoProject.Nodes
{
    public interface IGroupNode
    {
        [JsonIgnore]
        ObservableCollection<Node> Nodes { get; set; }

        IEnumerable<Node> GetNodes();
    }
}