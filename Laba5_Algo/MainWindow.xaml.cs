using Laba5_Algo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Laba5_Algo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowVM vm;
        
        public MainWindow()
        {
            InitializeComponent();
            vm = new MainWindowVM();
            DataContext = vm;
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            vm.CanvasClick(sender, e);
        }
        private void Vertex_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            vm.VertexClick(sender, e);
        }
        private void ConnectVertex_Click(object sender, RoutedEventArgs e)
        {
            vm.ConnectVertex(sender, e);
        }
        private void RemoveVertex_Click(object sender, RoutedEventArgs e)
        {
            vm.RemoveVertex(sender, e);
        }
        private void EditEdge_Click(object sender, RoutedEventArgs e)
        {
            vm.EditEdge(sender, e);
        }
        private void RemoveEdge_Click(object sender, RoutedEventArgs e)
        {
            vm.RemoveEdge(sender, e);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            vm.MouseMove(sender, e);
        }

        public void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            vm.Vertex_MouseLeftButtonUp(sender, e);
        }
    }
}
