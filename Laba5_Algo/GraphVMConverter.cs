using Laba5_Algo.ViewModels;
using Graph.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba5_Algo
{
    internal static class GraphVMConverter
    {
        public static Graph.Core.Graph ToModel(List<VertexVM> vertices, List<EdgeVM> edges, bool isOriented)
        {
            var res = new Graph.Core.Graph(isOriented);

            vertices.ForEach(vm => res.AddVertex(vm.Name));

            foreach(var edge in edges)
            {
                var vertexFrom = res.Vertices.Where(x => x.Name == edge.From.Name).First();
                var vertexTo = res.Vertices.Where(x => x.Name == edge.To.Name).First();
                vertexFrom.AddEdge(vertexTo, edge.Weight);
                vertexTo.AddEdge(vertexFrom, edge.Weight);
            }


            return res;
        }

        public static (List<VertexVM>, List<EdgeVM>) ToViewModel(Graph.Core.Graph graph)
        {
            var vertices = new List<VertexVM>();
            var edges = new List<EdgeVM>();

            //TODO

            return (vertices, edges);
        }
    }
}
