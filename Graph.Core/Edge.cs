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

        public Vertex DestNode { get; set; }

        public Edge(Vertex to, int weight) 
        {
            DestNode = to;
            Weight = weight;
        }
    }
}
