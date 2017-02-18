using System.Collections.Generic;
using GoProject.Nodes;

namespace GoProject.Sample.Models
{
    public class MaterialNode : EventNode
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

    public class ExpenseCenterNode : PoolNode
    {

        public ExpenseCenterNode()
        {
            Text = "مرکز هزینه";
            Nodes = new List<Node>() { new SubExpenseCenterNode() { Text = "خط تولید اول" } };
        }
    }


    public class SubExpenseCenterNode : LaneNode
    {
        public SubExpenseCenterNode()
        {
            Text = "زیر مرکز هزینه";
        }
    }
}