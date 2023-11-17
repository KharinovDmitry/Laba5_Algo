using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Core.Algorithms.SearchAlgo
{
    public class DFS : IGraphSearch
    {
        private List<Vertex> visited = new List<Vertex>();
        public List<Vertex> Traversal(Graph graph)
        {
            dfs(graph.Vertices[0]);
            return visited;
        }

        private void dfs(Vertex currentVertex)
        {
            visited.Add(currentVertex);
            foreach (Edge edge in currentVertex.Edges)
            {
                if(!visited.Contains(edge.DestNode))
                {
                    dfs(edge.DestNode);
                }
            }
        }

        public override string ToString() => "DFS";
    }
}
