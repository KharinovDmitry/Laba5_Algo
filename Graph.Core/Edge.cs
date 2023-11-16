using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Core
{
    public class Edge
    {
        public int Weight { get; set; }

        public Node AdjacentNode { get; set; }

        public Edge(int weight, Node node) 
        {
            Weight = weight;
            AdjacentNode = node;
        }
    }
}
