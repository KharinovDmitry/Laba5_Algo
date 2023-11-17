using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Laba5_Algo.ViewModels
{
    public class EdgeVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private bool isOriented;
        public Visibility IsOriented
        {
            get { return isOriented ? Visibility.Visible : Visibility.Collapsed; }
        }

        private int vertexRadius;

        private VertexVM from;
        public VertexVM From
        {
            get { return from; }
            set { 
                from = value; 
                OnPropertyChanged(nameof(From));
            }
        }

        private VertexVM to;
        public VertexVM To
        {
            get { return to; }
            set { 
                to = value; 
                OnPropertyChanged(nameof(To));
            }
        }

        public int X1
        { 
            get { return From.X + vertexRadius / 2; }
            set 
            { 
                OnPropertyChanged(nameof(X1));
                ArrowGeometry = GetArrowGeometry();
            }
        }

        public int Y1
        {
            get { return From.Y + vertexRadius / 2; ; }
            set { OnPropertyChanged(nameof(Y1)); ArrowGeometry = GetArrowGeometry(); }
        }

        public int X2
        {
            get { return To.X + vertexRadius / 2; }
            set { OnPropertyChanged(nameof(X2)); ArrowGeometry = GetArrowGeometry(); }
        }

        public int Y2
        {
            get { return To.Y + vertexRadius / 2; }
            set { OnPropertyChanged(nameof(Y2)); ArrowGeometry = GetArrowGeometry(); }
        }

        public Geometry ArrowGeometry 
        { 
            get { return GetArrowGeometry(); }
            set { OnPropertyChanged(nameof(ArrowGeometry));}
        }

        public Geometry GetArrowGeometry()
        {
            double arrowLength = 10;
            double arrowAngle = Math.PI / 6;

            double dx = X2 - X1;
            double dy = Y2 - Y1;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            double normalizedDx = dx / distance;
            double normalizedDy = dy / distance;

            double arrowEndX = X1 + (distance - vertexRadius / 2) * normalizedDx;
            double arrowEndY = Y1 + (distance - vertexRadius / 2) * normalizedDy;

            StreamGeometry streamGeometry = new StreamGeometry();

            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                double angle = Math.Atan2(arrowEndY - Y1, arrowEndX - X1);
                double arrowX1 = arrowEndX - arrowLength * Math.Cos(angle + arrowAngle);
                double arrowY1 = arrowEndY - arrowLength * Math.Sin(angle + arrowAngle);
                double arrowX2 = arrowEndX - arrowLength * Math.Cos(angle - arrowAngle);
                double arrowY2 = arrowEndY - arrowLength * Math.Sin(angle - arrowAngle);

                geometryContext.BeginFigure(new Point(X1, Y1), false, false);
                geometryContext.LineTo(new Point(arrowEndX, arrowEndY), true, true);
                geometryContext.LineTo(new Point(arrowX1, arrowY1), true, true);
                geometryContext.LineTo(new Point(arrowEndX, arrowEndY), true, true);
                geometryContext.LineTo(new Point(arrowX2, arrowY2), true, true);
            }

            return streamGeometry;
        }

        private int weight;
        public int Weight
        {
            get { return weight; }
            set { weight = value; OnPropertyChanged(nameof(Weight)); }
        }

        public int XText
        {
            get { return (int)GetTextPoint().X; }
            set { OnPropertyChanged(nameof(XText)); }
        }

        public int YText
        {
            get { return (int)GetTextPoint().Y; }
            set { OnPropertyChanged(nameof(YText)); }
        }

        public Point GetTextPoint()
        {
            int x = (X1 + X2) / 2;
            int y = (Y1 + Y2) / 2 - 20;
            return new Point(x, y);
        }

        public EdgeVM(VertexVM from, VertexVM to, int vertexRadius, int weight, bool isOriented)
        {
            this.isOriented = isOriented;
            this.vertexRadius = vertexRadius;
            Weight = weight;
            From = from;
            To = to;
        }
    }
}
