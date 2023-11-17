using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Laba5_Algo.ViewModels
{
    public class VertexVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private int x;
        public int X
        {
            get { return x; }
            set
            {
                x = value;
                OnPropertyChanged(nameof(X));
                XName = X + 11 - 4 * (Name.Length - 1);
            }
        }
        private int y;
        public int Y
        {
            get { return y; }
            set
            {
                y = value;
                OnPropertyChanged(nameof(Y));
                YName = Y + 7; 
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private int xName;
        public int XName
        {
            get { return xName; }
            set { xName = value; OnPropertyChanged(nameof(XName)); }
        }

        private int yName;
        public int YName
        {
            get { return yName; }
            set { yName = value; OnPropertyChanged(nameof(YName));}
        }

        public VertexVM(int x, int y, string name)
        {
            Name = name;
            X = x;
            Y = y;
        }
    }
}
