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
      
        public new int X
        {
            get { return x; }
            set
            {
                x = value;
                OnPropertyChanged(nameof(X));
                XName = X + 11 - 4 * (Name.Length - 1);
                PathX = X + 15;
                VertexX = X - 5;
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
                PathY = Y - 7;
                VertexY = PathY;
            }
        }
        private int minPath;
        private string minVertex;
        public int MinPath
        {
            get { return minPath; }
            set { minPath = value; OnPropertyChanged(nameof(MinPath)); }
        }
    
        public string MinVertex
        {
            get { return minVertex; }
            set { minVertex = value; OnPropertyChanged(nameof(MinVertex)); }
        }
                               
        
        private int pathX;
        public int PathX
        {
            get { return pathX; }
            set { pathX = value; OnPropertyChanged(nameof(PathX)); }      
        }
        private int pathY;
        public int PathY
        {
            get { return pathY; }
            set { pathY = value; OnPropertyChanged(nameof(PathY)); }
        }
        private int vertexX;
        public int VertexX
        {
            get { return vertexX; }
            set { vertexX = value; OnPropertyChanged(nameof(VertexX)); }
        }
        private int vertexY;
        public int VertexY
        {
            get { return vertexY; }
            set { vertexY = value; OnPropertyChanged(nameof(VertexY)); }
        }
           
        public PrimsVertexVM(int x, int y, string name, int minPath, string minVertex) : base(x, y, name)
        {
            MinPath = minPath;
            MinVertex = minVertex;
            X = x;
            Y = y;
        }
    }
}
