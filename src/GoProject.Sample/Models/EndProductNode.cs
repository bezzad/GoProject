using GoProject.Nodes;

namespace GoProject.Sample.Models
{
    public class EndProductNode : EventNode
    {

        public EndProductNode()
        {
            Text = "مواد نیمه ساخته";
            EventType = Enums.EventType.Multiple;
            EventDimension = Enums.EventDimension.IntermediateNonInter;
        }
    }
}