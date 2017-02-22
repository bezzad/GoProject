using GoProject.Nodes;

namespace GoProject.Sample.Models
{
    public class WorkStationNode : ActivityNode
    {
        public WorkStationNode()
        {
            Text = "ایستگاه کاری";
            TaskType = Enums.TaskType.Service;
        }
    }
}