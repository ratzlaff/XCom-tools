using System.Collections.Generic;

namespace RulesetView.Models
{
    public class Node<TData> : Node
    {
        public TData Data { get; set; }

        public override string ToString()
        {
            return Name + " " + base.ToString();
        }
    }

    public class Node
    {
        public string Name { get; set; }
        public List<Node> ChildList { get; set; }
    }
}