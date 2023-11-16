using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Core.Algorithms.SearchAlgo
{
    public interface IGraphSearch
    {
        public List<Node> Traversal(Graph graph);
    }
}
