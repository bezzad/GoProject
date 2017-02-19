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
}