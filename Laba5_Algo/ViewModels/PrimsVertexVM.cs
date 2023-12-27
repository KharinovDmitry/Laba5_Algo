using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace Laba5_Algo.ViewModels
{
    public class PrimsVertexVM : VertexVM
    {
        private int minPath;
        private int minVertex;
        public int MinPath
        {
            get { return minPath; }
            set { minPath = value; OnPropertyChanged(nameof(MinPath)); }
        }
    
        public int MinVertex
        {
            get { return minVertex; }
            set { minVertex = value; OnPropertyChanged(nameof(MinVertex)); }
        }
        
        public PrimsVertexVM(int x, int y, string name, int minPath, int minVertex) : base(x, y, name)
        {
            MinPath = minPath;
            MinVertex = minVertex;
        }
        private int pathX;
        private int pathY;
        private int vertexX;
        private int vertexY;

        public int PathX
        {
            get { return pathX; }
            set { pathX = value; OnPropertyChanged(nameof(PathX)); }      
        }

        public int VertexX
        {
            get { return vertexX; }
            set { vertexX = value; OnPropertyChanged(nameof(VertexX)); }
        }

        public int PathY
        {
            get { return pathY; }
            set { pathY = value; OnPropertyChanged(nameof(PathY)); }
        }

        public int VertexY
        {
            get { return vertexY; }
            set { vertexY = value; OnPropertyChanged(nameof(VertexY)); }
        }
        
        public new int X
        {
            get { return x; }
            set
            {
                x = value;
                OnPropertyChanged(nameof(X));
                XName = X + 11 - 4 * (Name.Length - 1);
                PathX = XName - 5;
                VertexX = XName - 35;
            }
        }
        public new int Y
        {
            get { return y; }
            set
            {
                y = value;
                OnPropertyChanged(nameof(Y));
                YName = Y + 7;
                PathY = YName - 2;
                VertexY = PathY;
            }
        }
        


    }
}
