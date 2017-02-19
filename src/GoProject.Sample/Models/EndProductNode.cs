using GoProject.Nodes;

namespace GoProject.Sample.Models
{
    public class EndProductNode : EventNode
    {

        public EndProductNode()
        {
            Text = "مواد نیمه ساخته";
            EventType = GoProject.EventType.Multiple;
            EventDimension = GoProject.EventDimension.IntermediateNonInter;
        }
    }
}