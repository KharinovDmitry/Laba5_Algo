using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Graph.Core
{
    public class Edge : IComparable<Edge>
    {
        public int Weight { get; set; }
        public Vertex From { get; set; }
        public Vertex To { get; set; }

        public Vertex DestNode { get; set; }

        public Vertex OtherNode(Vertex vertex)
        {
            if (!IsIncident(vertex)) throw new ArgumentException();
            if (From == vertex) return To;
            return From;
        }
        public bool IsIncident(Vertex vertex)
        {
            return From == vertex || To == vertex;
        }

        public int CompareTo(Edge other)
        {
            if (other == null) return 1;
            return Weight.CompareTo(other.Weight);
        }

        public Edge(Vertex to, int weight) 
        {
            DestNode = to;
            Weight = weight;
        }
        public Edge(Vertex from, Vertex to, int weight) 
        {
            From = from;
            To = to;
            Weight = weight;
        }
    }
}
