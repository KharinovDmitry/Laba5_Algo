using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Core
{
    public class Graph
    {
        public HashSet<Node> Nodes { get; set; }
        public bool IsOriented;


        public Graph() 
        { 
            Nodes = new HashSet<Node>();
        }

        public bool AddNode(string Name)
        {
            return Nodes.Add(new Node(Name));
        }
        
    }
}
