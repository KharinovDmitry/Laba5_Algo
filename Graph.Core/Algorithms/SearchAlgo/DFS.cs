using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Core.Algorithms.SearchAlgo
{
    public class DFS : IGraphSearch
    {
        public List<Vertex> Traversal(Graph graph)
        {
            var visited = new List<Vertex>();

            var startVertex = graph.Vertices[0];




            return visited;
        }

        public override string ToString() => "DFS";
    }
}
