using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Core.Algorithms
{
    public class MaxFlowAlgorithm
    {
        public (List<List<Vertex>>, List<int>)? Execute(Graph graph, Vertex from, Vertex to)
        {
            if (!graph.IsOriented)
                return null;

            int[,] capacity = new int[graph.Vertices.Count, graph.Vertices.Count];
            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                foreach (Edge edge in graph.Vertices[i].Edges)
                {
                    int j = graph.Vertices.IndexOf(edge.DestNode);
                    capacity[i, j] = edge.Weight;
                }
            }

            List<(List<int>, int)> result = FordFulkersonAlgorithm(capacity, graph.Vertices.IndexOf(from), graph.Vertices.IndexOf(to));

            if (result != null)
            {
                List<List<Vertex>> resultVertices = new List<List<Vertex>>();
                List<int> resultFlows = new List<int>();

                for (int i = 0; i < result.Count; i++)
                {
                    List<Vertex> vertices = new List<Vertex>();
                    for (int j = 0; j < result[i].Item1.Count; j++)
                        vertices.Add(graph.Vertices[result[i].Item1[j]]);

                    resultVertices.Add(vertices);
                    resultFlows.Add(result[i].Item2);
                }

                return (resultVertices, resultFlows);
            }

            return null;
        }

        private bool Bfs(int[,] rGraph, int s, int t, int[] parent)
        {
            int V = rGraph.GetLength(0);

            bool[] visited = new bool[V];
            for (int i = 0; i < V; ++i)
                visited[i] = false;

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(s);
            visited[s] = true;
            parent[s] = -1;

            while (queue.Count != 0)
            {
                int u = queue.Dequeue();

                for (int v = 0; v < V; v++)
                {
                    if (visited[v] == false && rGraph[u, v] > 0)
                    {
                        if (v == t)
                        {
                            parent[v] = u;
                            return true;
                        }
                        queue.Enqueue(v);
                        parent[v] = u;
                        visited[v] = true;
                    }
                }
            }

            return false;
        }

        public List<(List<int>, int)> FordFulkersonAlgorithm(int[,] graph, int s, int t)
        {
            int V = graph.GetLength(0);
            int u, v;

            int[,] residualGraph = new int[V, V];

            for (u = 0; u < V; u++)
                for (v = 0; v < V; v++)
                    residualGraph[u, v] = graph[u, v];

            int[] parent = new int[V];
            List<(List<int>, int)> augmentingPaths = new List<(List<int>, int)>();

            int maxFlow = 0;

            while (Bfs(residualGraph, s, t, parent))
            {
                int pathFlow = int.MaxValue;

                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    pathFlow = Math.Min(pathFlow, residualGraph[u, v]);
                }

                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    residualGraph[u, v] -= pathFlow;
                    residualGraph[v, u] += pathFlow;
                }

                maxFlow += pathFlow;
                List<int> augmentingPath = new List<int>() { t };

                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    augmentingPath.Add(u);
                }

                augmentingPaths.Add((augmentingPath, pathFlow));
            }

            return augmentingPaths;
        }

        public override string ToString() => "MaxFlowAlgorithm";
    }
}