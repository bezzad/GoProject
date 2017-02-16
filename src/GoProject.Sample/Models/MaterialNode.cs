using System.Collections.Generic;
using System.Drawing;
using GoProject.Nodes;
using GoProject.Properties;

namespace GoProject.Sample.Models
{
    public class MaterialNode: EventNode
    {
        public MaterialNode()
        {
            Text = "مواد اولیه";
            EventType = GoProject.EventType.None;
            EventDimension = GoProject.EventDimension.Start;
        }
    }

    public class SemiFinishMaterialNode : EventNode
    {
        public SemiFinishMaterialNode()
        {
            Text = "مواد نیمه ساخته";
            EventType = GoProject.EventType.Terminate;
            EventDimension = GoProject.EventDimension.End;
        }
    }

    public class WorkStationNode : ActivityNode
    {
        public WorkStationNode()
        {
            Text = "ایستگاه کاری";
            TaskType = GoProject.TaskType.Service;
        }
    }

    //public class ExpenseCenterNode : Node[]
    //{
    //    List<Node> Nodes = new List<Node>();
    //    public ExpenseCenterNode()
    //    {
    //        Nodes.Add
    //        new Node { Category = NodeCategory.Pool, Text = Localization.Pool, IsGroup = true, Key = "pool" },
    //            new Node { Category = NodeCategory.Lane, Text = Localization.NewLane, Group = "pool", HexColor = Color.LightGoldenrodYellow, IsGroup = true },
    //            new Node { Category = NodeCategory.Lane, Text = Localization.NewLane, Group = "pool", HexColor = Color.LightGreen, IsGroup = true }
    //    }
    //}
}