using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Core.Algorithms.SearchAlgo
{
    public class BFS : IGraphSearch
    {
        public List<Vertex> Traversal(Graph graph)
        {
            var visited = new List<Vertex>();

            var startVertex = graph.Vertices[0];

            Queue<Vertex> queue = new Queue<Vertex>();

            queue.Enqueue(startVertex);
            visited.Add(startVertex);

            while (queue.Count != 0)
            {
                var currenteVertex = queue.Dequeue();
                foreach(var edge in currenteVertex.Edges)
                {
                    if(!visited.Contains(edge.DestNode))
                    {
                        visited.Add(edge.DestNode);
                        queue.Enqueue(edge.DestNode);
                    }
                }
            }

            return visited;
        }

        public override string ToString() => "BFS";
    }
}
