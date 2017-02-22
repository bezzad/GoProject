using System.Collections.Generic;
using GoProject.Nodes;

namespace GoProject.Sample.Models
{
    public class MaterialNode : EventNode
    {
        public MaterialNode()
        {
            Text = "مواد اولیه";
            EventType = Enums.EventType.None;
            EventDimension = Enums.EventDimension.Start;
        }
    }
}