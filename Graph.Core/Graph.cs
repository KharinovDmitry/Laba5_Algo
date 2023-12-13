using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        public void CompareByWeight() 
        {

        }
        public void Sort()
        {
            Vertices.Sort();
        }

        public string[,] GetAdjacencyMatrix()
        {
            var matrix = new string[Vertices.Count + 1, Vertices.Count + 1];

            Dictionary<string, int> indexes = new Dictionary<string, int>();

            for (int i = 0; i < Vertices.Count; i++)
            {
                matrix[i + 1, 0] = Vertices[i].Name;
                matrix[0, i + 1] = Vertices[i].Name;
                indexes[Vertices[i].Name] = i + 1;
            }

            for (int i = 0; i < Vertices.Count; i++)
            {
                for (int j = 0; j < Vertices.Count; j++)
                {
                    matrix[i + 1, j + 1] = "0";
                }
            }

            foreach (var vertex in Vertices)
            {
                foreach (var edge in vertex.Edges) 
                {
                    matrix[indexes[edge.From.Name], indexes[edge.To.Name]] = edge.Weight.ToString();
                }
            }
  
            return matrix;
        }
        
    }
}
