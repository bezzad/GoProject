using GoProject.Nodes;

namespace GoProject.Sample.Models
{
    public class SemiFinishMaterialNode : EventNode
    {
        public SemiFinishMaterialNode()
        {
            Text = "مواد نیمه ساخته";
            EventType = Enums.EventType.Multiple;
            EventDimension = Enums.EventDimension.IntermediateNonInter;
        }
    }
}