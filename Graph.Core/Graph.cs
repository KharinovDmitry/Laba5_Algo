using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Core
{
    public class Graph
    {
        public List<Vertex> Vertices { get; set; }

        public bool IsOriented;


        public Graph(bool isOriented) 
        { 
            IsOriented = isOriented;
            Vertices = new List<Vertex>();
        }

        public void AddVertex(string Name)
        {
            Vertices.Add(new Vertex(Name));
        }
        public void AddVertex(Vertex vertex)
        {
            Vertices.Add(vertex);
        }

        public int[][] GetAdjacencyMatrix()
        {
            //TODO
            throw new NotImplementedException();
        }
        
    }
}
