using GoProject.Nodes;

namespace GoProject.Sample.Models
{
    public class SemiFinishMaterialNode : EventNode
    {
        public SemiFinishMaterialNode()
        {
            Text = "محصول نهایی";
            EventType = GoProject.EventType.Terminate;
            EventDimension = GoProject.EventDimension.End;
        }
    }
}