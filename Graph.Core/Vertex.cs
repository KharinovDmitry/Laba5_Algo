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

        public void AddEdge(Vertex dest, int weight) 
        {
            Edges.Add(new Edge(dest, weight));
        }


        public int CompareTo(Vertex? other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}
