using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Core
{
    public class Node : IComparable<Node>
    {
        public string Name { get; set; }

        public string Edge { get; set; }

        public Node(string name) 
        {
            Name = name;
        }

        public int CompareTo(Node? other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}
