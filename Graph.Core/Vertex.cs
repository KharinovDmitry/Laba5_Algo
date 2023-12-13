using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Core
{
    public class Vertex : IComparable<Vertex>
    {
        public string Name { get; set; }

        public List<Edge> Edges { get; set; }

        public Vertex(string name) 
        {
            Name = name;
            Edges = new List<Edge>();
        }

        public Edge AddEdge(Vertex dest, int weight) 
        {
            var edge = new Edge(dest, weight);
            edge.From = this;
            edge.To = dest;
            Edges.Add(edge);
            return edge;
        }

        public IEnumerable<Edge> IncidentEdges
        {
            get
            {
                foreach (var e in Edges) yield return e;
            }
        }
        public int CompareTo(Vertex? other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}
