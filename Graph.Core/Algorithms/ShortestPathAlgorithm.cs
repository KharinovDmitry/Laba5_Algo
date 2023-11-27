using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Graph.Core.Algorithms
{
    public class ShortestPathAlgorithm
    {
        public List<Vertex> Execute(Graph graph, Vertex from, Vertex to)
        {

            //TODO
            //throw new NotImplementedException();
            return Dijkstra(graph, from, to);

        }
        public static List<Vertex> Dijkstra(Graph graph, Vertex from, Vertex to)
        {
            //TODO
            var notVisited = graph.Vertices;
            var track = new Dictionary<Vertex, DijkstraData>();
            track[from] = new DijkstraData { Price = 0, Previous = null };

            while (true) 
            {
                Vertex toOpen = null;
                var bestPrice = double.PositiveInfinity;
                foreach (var e in notVisited)
                {
                    if (track.ContainsKey(e) && track[e].Price < bestPrice)
                    {
                        bestPrice = track[e].Price;
                        toOpen = e;
                    }
                }

                if (toOpen == null) return null;
                if (toOpen == to) break;

                foreach (var e in toOpen.IncidentEdges.Where(z => z.From == toOpen))
                {
                    var currentPrice = track[toOpen].Price + e.Weight;
                    var nextNode = e.OtherNode(toOpen);
                    if (!track.ContainsKey(nextNode) || track[nextNode].Price > currentPrice)
                    {
                        track[nextNode] = new DijkstraData { Previous = toOpen, Price = currentPrice };
                    }
                }
                notVisited.Remove(toOpen);
            }
            var result = new List<Vertex>();
            while (to != null)
            {
                result.Add(to);
                to = track[to].Previous;
            }
            result.Reverse();
            return result;
        }
    }
    class DijkstraData
    {
        public double Price;
        public Vertex Previous;
    }
}
