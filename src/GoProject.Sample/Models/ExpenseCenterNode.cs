using System.Collections.ObjectModel;
using GoProject.Nodes;

namespace GoProject.Sample.Models
{
    public class ExpenseCenterNode : PoolNode
    {

        public ExpenseCenterNode()
        {
            Text = "مرکز هزینه";
            Nodes.Add(new SubExpenseCenterNode() { Text = "خط تولید اول" });
        }
    }
}