using GoProject.Nodes;

namespace GoProject.Sample.Models
{
    public class SemiFinishMaterialNode : EventNode
    {
        public SemiFinishMaterialNode()
        {
            Text = "محصول نهایی";
            EventType = Enums.EventType.Terminate;
            EventDimension = Enums.EventDimension.End;
        }
    }
}