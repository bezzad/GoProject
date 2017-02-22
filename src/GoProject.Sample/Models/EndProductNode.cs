using GoProject.Nodes;

namespace GoProject.Sample.Models
{
    public class EndProductNode : EventNode
    {

        public EndProductNode()
        {
            Text = "محصول نهایی";
            EventType = Enums.EventType.Terminate;
            EventDimension = Enums.EventDimension.End;
        }
    }
}