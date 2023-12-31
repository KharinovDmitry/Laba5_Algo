﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Graph.Core.Algorithms
{
    public class MinTreeAlgorithm
    {

        public class PrimsVertex : Vertex
        {
            public int MinPath { get; set; }
            public PrimsVertex MinVertex { get; set; }
            public bool Include { get; set; }
            public PrimsVertex(string name) : base(name)
            {
            }

        }

        public class PrimsEdge : Edge
        {
            public new Vertex From { get; set; }
            public new Vertex To { get; set; }

            public PrimsEdge(Vertex from, Vertex to, int weight) : base(to, weight)
            {
                From = from;
                To = to;
            }
        }

        private class PrimsGraph : Graph
        {

            public new List<PrimsVertex> Vertices { get; }
            public PrimsGraph(bool isOriented) : base(isOriented)
            {
            }

        }
        public (List<Vertex>, List<PrimsEdge>, Dictionary<Vertex, PrimsVertex>) Execute(Graph graph)
        {
            if (graph.IsOriented)
                return new();
            return Prim(graph);

        }


        private (List<Vertex>, List<PrimsEdge>, Dictionary<Vertex, PrimsVertex>) Prim(Graph graph)
        {
            var vertex = graph.Vertices[0];
            List<String> Logs = new ();

            Dictionary<Vertex, PrimsVertex> origToTemp = new();
            List<Vertex> vertices = new();
            List<PrimsEdge> edges = new();

            var tempVertex = new PrimsVertex(vertex.Name);
            tempVertex.Include = true;
            origToTemp.Add(vertex, tempVertex);
            vertices.Add(vertex);
            //step 0. add vertex with minimal path
            //step 1. get new edges and add new vertices
            //step 2. nop
            //step 3. update minimal path and minimal vertex 

            while (true)
            {
                foreach (var edge in vertex.Edges)
                {
                    //check vertex and add new vtxs
                    if (!origToTemp.ContainsKey(edge.DestNode))
                    {
                        var newVertex = new PrimsVertex(edge.DestNode.Name);
                        //init new vertex minPath and minVertex
                        newVertex.MinVertex = origToTemp[vertex];
                        newVertex.MinPath = edge.Weight;

                        origToTemp.Add(edge.DestNode, newVertex);
                        Logs.Add($"Warning: Found new vertex {newVertex.Name} ! with weight {edge.Weight} from {origToTemp[vertex].Name} ");
                    }
                    else
                    {
                        Logs.Add($"Warning: Entered previous vertex {origToTemp[edge.DestNode].Name} ! with weight {edge.Weight} from {origToTemp[vertex].Name} ");
                        if (origToTemp[edge.DestNode].Include)
                        {
                            Logs.Add($"Warning: Skip Included vertex {origToTemp[edge.DestNode].Name} ! with weight {edge.Weight} from {origToTemp[vertex].Name} ");
                            continue;
                        }
                        //step 3. update minimal path and minimal verte
                        if (origToTemp[edge.DestNode].MinPath < edge.Weight)
                        {
                            origToTemp[edge.DestNode].MinPath = edge.Weight;
                            origToTemp[edge.DestNode].MinVertex = origToTemp[vertex];
                            Logs.Add($"Warning: Updated {origToTemp[edge.DestNode].Name} new minPath={edge.Weight} new minVertex={origToTemp[vertex]}");
                        }
                    }
                }
                var notIcludedVertecies = origToTemp.Values.Where(vtx => !vtx.Include);
                if (notIcludedVertecies.Count() == 0)
                {
                    Logs.Add($"Warning: The end ! Verts: {vertices.Count} Edges: {edges.Count} ");
                    return new(vertices, edges, origToTemp);
                }

                vertex = notIcludedVertecies.Aggregate((curMinVtx, vtx) => (vtx.MinPath < curMinVtx.MinPath ? vtx : curMinVtx));

                vertex = origToTemp.FirstOrDefault(vtx => vtx.Value == vertex).Key;
                origToTemp[vertex].AddEdge(origToTemp[vertex].MinVertex, origToTemp[vertex].MinPath);
                origToTemp[vertex].MinVertex.AddEdge(vertex, origToTemp[vertex].MinPath);

                Logs.Add($"Warning: New Edge from  {origToTemp[vertex].MinVertex.Name} to {origToTemp[vertex].Name}");
                edges.Add(new(origToTemp[vertex].MinVertex, origToTemp[vertex], origToTemp[vertex].MinPath));


                origToTemp[vertex].Include = true;

                Logs.Add($"Warning: New Included Vertex {origToTemp[vertex].Name} ");
                vertices.Add(vertex);
            }
        }
    }
}
