using Laba5_Algo.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba5_Algo
{
    public static class GraphFileConverter
    {
        /*
         * 1/0 - is oriented or not
         * 3
         * 1:23,23
         * 2:23,45
         * 3:34,53
         * 2
         * 1,3=10
         * 2,4=20
        */
        public static void Save(List<VertexVM> vertices, List<EdgeVM> edges, bool isOriented, string path)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(isOriented ? "1" : "0");
            sb.AppendLine(vertices.Count.ToString());
            foreach (VertexVM v in vertices)
            {
                sb.AppendLine($"{v.Name}:{v.X},{v.Y}");
            }
            sb.AppendLine(edges.Count.ToString());
            foreach(EdgeVM e in edges)
            {
                sb.AppendLine($"{e.From.Name},{e.To.Name}={e.Weight}");
            }
            File.WriteAllText(path, sb.ToString());
        }

        public static (List<VertexVM>, List<EdgeVM>, bool) Load(string path)
        {
            var vertices = new List<VertexVM>();
            var edges = new List<EdgeVM>();

            var input = File.ReadAllText(path).Split("\n");
            var isOrientedInt = int.Parse(input[0]);
            bool isOriented = isOrientedInt == 1 ? true : false;
            var vCount = int.Parse(input[1]);
            var eCount = int.Parse(input[vCount + 2]);
            for (int i = 0; i < vCount; i++)
            {
                var vertexText = input[i + 2];
                string name = vertexText.Split(":")[0];
                int x = int.Parse(vertexText.Split(":")[1].Split(",")[0]);
                int y = int.Parse(vertexText.Split(":")[1].Split(",")[1]);
                vertices.Add(new VertexVM(x, y, name));
            }
            for (int i = 0; i < eCount; i++)
            {
                var edgeText = input[i + vCount + 3];
                int weight = int.Parse(edgeText.Split("=")[1]);
                VertexVM from = vertices.Find(x => x.Name == edgeText.Split("=")[0].Split(",")[0]);
                VertexVM to = vertices.Find(x => x.Name == edgeText.Split("=")[0].Split(",")[1]);
                edges.Add(new EdgeVM(from, to, 30, weight, true));
            }
            return (vertices, edges, isOriented);
        }

    }
}
