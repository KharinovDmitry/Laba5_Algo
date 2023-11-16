using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Laba5_Algo.ViewModels
{
    public class EdgeVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private int vertexRadius;

        private VertexVM from;
        public VertexVM From
        {
            get { return from; }
            set { from = value; OnPropertyChanged(nameof(From)); }
        }
        private VertexVM to;
        public VertexVM To
        {
            get { return to; }
            set { to = value; OnPropertyChanged(nameof(To)); }
        }
        public int X1 => From.X + vertexRadius / 2;
        public int Y1 => From.Y + vertexRadius / 2;

        public int X2 => To.X + vertexRadius / 2;
        public int Y2 => To.Y + vertexRadius / 2;

        private int weight;
        public int Weight
        {
            get { return weight; }
            set { weight = value; OnPropertyChanged(nameof(Weight)); }
        }

        public EdgeVM(VertexVM from, VertexVM to, int vertexRadius, int weight)
        {
            From = from;
            To = to;
            this.vertexRadius = vertexRadius;
            Weight = weight;
        }
    }
}
