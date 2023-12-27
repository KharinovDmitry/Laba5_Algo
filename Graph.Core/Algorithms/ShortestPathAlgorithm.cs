using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Graph.Core.Algorithms.MinTreeAlgorithm;

namespace Graph.Core.Algorithms
{
    public class ShortestPathAlgorithm
    {
        
        private class DijVertex : Vertex
        {
            public int MinPath { get; set; }
            public DijVertex MinVertex { get; set; }
            public bool Include { get; set; }
            public DijVertex(string name) : base(name)
            {
            }

        }
          private class DijGraph : Graph
        {

            public new List<DijVertex> Vertices { get; }
            public DijGraph(bool isOriented) : base(isOriented)
            {
            }

        }
        
        public class DijEdge : Edge
        {
            public new Vertex From { get; set; }
            public new Vertex To { get; set; }

            public DijEdge(Vertex from, Vertex to, int weight) : base(to, weight)
            {
                From = from;
                To = to;
            }
        }

      
        
        public (List<Vertex>, double, List<Edge>) Execute(Graph graph, Vertex from, Vertex to)
        {
            //TODO
            //throw new NotImplementedException();
            return Dijkstra(graph, from, to);

        }
        public static (List<Vertex>, double, List<Edge>) Dijkstra(Graph graph, Vertex from, Vertex to)
        {
            //TODO
            var notVisited = graph.Vertices.ToList();
            var track = new Dictionary<Vertex, DijkstraData>();
            List<Edge> edges = new();
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

                if (toOpen == null) return (null, 0, null);
                if (toOpen == to) break;

                foreach (var e in toOpen.IncidentEdges.Where(z => z.From == toOpen))
                {
                    var currentPrice = track[toOpen].Price + e.Weight;
                    var nextEdge = e.OtherEdge(toOpen);
                    if (!track.ContainsKey(nextEdge) || track[nextEdge].Price > currentPrice)
                    {
                        track[nextEdge] = new DijkstraData { Previous = toOpen, Price = currentPrice };
                    }
                }
                
                notVisited.Remove(toOpen);
            }
            var result = new List<Vertex>();
            double price = track[to].Price;
            while (to != null)
            {
                result.Add(to);
                if (track[to].Previous != null)
                    edges.Add(to.Edges.Where(x => x.From.Name == track[to].Previous.Name || x.To.Name == track[to].Previous.Name).FirstOrDefault());
                to = track[to].Previous;
                
            }
            result.Reverse();
            edges.Reverse();
            return (result, price, edges);
        }
    }
    class DijkstraData
    {
        public double Price;
        public Vertex Previous;
    }
}
