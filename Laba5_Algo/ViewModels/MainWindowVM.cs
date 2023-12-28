using Graph.Core;
using Graph.Core.Algorithms;
using Graph.Core.Algorithms.SearchAlgo;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using static Graph.Core.Algorithms.MinTreeAlgorithm;

namespace Laba5_Algo.ViewModels
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private FileDialogService openFileDialog;

        public ObservableCollection<IGraphSearch> SearchAlgorithms { get; }

        private IGraphSearch selectedSearchAlgo;
        public IGraphSearch SelectedSearchAlgo
        {
            get { return selectedSearchAlgo; }
            set { selectedSearchAlgo = value; OnPropertyChanged(nameof(SelectedSearchAlgo)); }
        }

        private ObservableCollection<string> logs;
        public ObservableCollection<string> Logs
        {
            get
            {
                return logs;
                OnPropertyChanged(nameof(Logs));
            }
        }

        public MainWindowVM()
        {
            SearchAlgorithms = new ObservableCollection<IGraphSearch>()
            {
                new BFS(),
                new DFS()
            };
            SelectedSearchAlgo = SearchAlgorithms[0];

            VertexRadius = 30;
            Vertices = new ObservableCollection<VertexVM>();
          
            Edges = new ObservableCollection<EdgeVM>();
            logs = new ObservableCollection<string>();

            Clear = new RelayCommand(clear);
            Save = new RelayCommand(save);
            Load = new RelayCommand(load);
            SaveMatrix = new RelayCommand(saveMatrix);
            StartSearchAlgo = new RelayCommandAsync(startSearchAlgo);
            StartMaxFlowAlgo = new RelayCommandAsync(startMaxFlowAlgo);
            StartMinTreeAlgo = new RelayCommandAsync(startMinTreeAlgo);
            StartMinPathAlgo = new RelayCommandAsync(startMinPath);

            openFileDialog = new FileDialogService();

            isDragging = false;
            IsConnecting = false;
            IsOriented = false;
            Visible = Visibility.Collapsed;
        }


        private bool isDragging;
        private int vertexRadius;
        private int vertexCount;

        public int VertexRadius
        {
            get { return vertexRadius; }
            set
            {
                vertexRadius = value;
                OnPropertyChanged(nameof(VertexRadius));
            }
        }

        public ICommand Clear { get; set; }
        public ICommand Save { get; set; }
        public ICommand Load { get; set; }

        public ICommand SaveMatrix { get; set; }
        public ICommand StartSearchAlgo { get; set; }
        public ICommand StartMaxFlowAlgo { get; set; }
        public ICommand StartMinPathAlgo { get; set; }
        public ICommand StartMinTreeAlgo { get; set; }

        private ObservableCollection<VertexVM> vertices;
        public ObservableCollection<VertexVM> Vertices
        {
            get { return vertices; }
            set
            {
                vertices = value;
                OnPropertyChanged(nameof(Vertices));
            }
        }

     

        private ObservableCollection<EdgeVM> edges;
        public ObservableCollection<EdgeVM> Edges
        {
            get { return edges; }
            set
            {
                edges = value;
                OnPropertyChanged(nameof(Edges));
            }
        }

        private VertexVM selectedVertex;
        public VertexVM SelectedVertex
        {
            get { return selectedVertex; }
            set
            {
                selectedVertex = value;
                OnPropertyChanged(nameof(SelectedVertex));
            }
        }

        private VertexVM fromVertex;
        public VertexVM FromVertex
        {
            get { return fromVertex; }
            set
            {
                fromVertex = value;
                OnPropertyChanged(nameof(FromVertex));
            }
        }

        private VertexVM toVertex;
        public VertexVM ToVertex
        {
            get { return toVertex; }
            set
            {
                toVertex = value;
                OnPropertyChanged(nameof(ToVertex));
            }
        }

        private bool isOriented;
        public bool IsOriented
        {
            get { return isOriented; }
            set
            {
                clear(null);
                isOriented = value;
                OnPropertyChanged(nameof(IsOriented));
            }
        }

        private bool isConnecting;
        public bool IsConnecting
        {
            get { return isConnecting; }
            set
            {
                isConnecting = value;
                OnPropertyChanged(nameof(IsConnecting));
                Visible = IsConnecting ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility visible;
        public Visibility Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                OnPropertyChanged(nameof(Visible));
            }
        }

        private int mousePointX;
        public int MousePointX
        {
            get { return mousePointX; }
            set
            {
                mousePointX = value;
                OnPropertyChanged(nameof(MousePointX));
            }
        }

        private int mousePointY;
        public int MousePointY
        {
            get { return mousePointY; }
            set
            {
                mousePointY = value;
                OnPropertyChanged(nameof(MousePointY));
            }
        }

        private void clear(object obj)
        {
            Vertices.Clear();
            Edges.Clear();
            vertexCount = 0;
        }

        private void load(object obj)
        {
            Vertices.Clear();
            Edges.Clear();

            if (!openFileDialog.OpenFileDialog())
                return;

            string path = openFileDialog.FilePath;
            try
            {
                var res = GraphFileConverter.Load(path);

                IsOriented = res.Item3;
                Vertices = new ObservableCollection<VertexVM>(res.Item1);
                vertexCount = Vertices.Count;
                Edges = new ObservableCollection<EdgeVM>(res.Item2);
            }
            catch
            {
                MessageBox.Show("Неваллидный файл");
            }
        }

        private void save(object obj)
        {
            if (!openFileDialog.SaveFileDialog())
                return;

            string path = openFileDialog.FilePath;
            GraphFileConverter.Save(Vertices.ToList(), Edges.ToList(), IsOriented, path);
        }

        private void saveMatrix(object obj)
        {
            var graph = GraphVMConverter.ToModel(Vertices.ToList(), Edges.ToList(), IsOriented);
            var matrix = graph.GetAdjacencyMatrix();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Math.Sqrt(matrix.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(matrix.Length); j++)
                {
                    sb.Append(matrix[i, j] + ";");
                }
                sb.AppendLine();
            }
            if (!openFileDialog.SaveFileDialog())
                return;

            string path = openFileDialog.FilePath + ".csv";
            File.WriteAllText(path, sb.ToString());
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition((UIElement)sender);
            MousePointX = (int)p.X - 15;
            MousePointY = (int)p.Y - 15;

            if (isDragging && p.X > VertexRadius / 2 && selectedVertex != null)
            {
                SelectedVertex.X = MousePointX;
                SelectedVertex.Y = MousePointY;

                var edgesToRemove = GetConnectedEdge(SelectedVertex);
                edgesToRemove.ForEach(edge => { Edges.Remove(edge); });
                edgesToRemove.ForEach(edge => { Edges.Add(edge); });
            }
        }

        public void CanvasClick(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition((UIElement)sender);
            Point newVertexPoint = new Point(p.X - VertexRadius / 2, p.Y - VertexRadius / 2);

            if (GetVertexFromPoint(newVertexPoint) != null)
                return;

            Vertices.Add(new VertexVM((int)newVertexPoint.X, (int)newVertexPoint.Y, vertexCount++.ToString()));
        }

        public void RemoveEdge(object sender, RoutedEventArgs e)
        {
            Edges.Remove((sender as MenuItem).Tag as EdgeVM);
        }

        public void EditEdge(object sender, RoutedEventArgs e)
        {
            var edge = (sender as MenuItem).Tag as EdgeVM;

            int weight = new EditEdgeVM(edge.Weight).ShowDialog();
            edge.Weight = weight == -1 ? edge.Weight : weight;
        }

        public void RemoveVertex(object sender, RoutedEventArgs e)
        {
            var vertex = (sender as MenuItem).Tag as VertexVM;

            var edgesToRemove = GetConnectedEdge(vertex);
            edgesToRemove.ForEach(edge => { Edges.Remove(edge); });

            Vertices.Remove(vertex);
        }

        public void ConnectVertex(object sender, RoutedEventArgs e)
        {
            var vertex = (sender as MenuItem).Tag as VertexVM;
            if (vertex == null)
                throw new NullReferenceException(nameof(vertex));

            SelectedVertex = vertex;
            IsConnecting = true;
        }

        public void VertexClick(object sender, MouseButtonEventArgs e)
        {
            Point clickedPoint = e.GetPosition((UIElement)sender);
            var vertex = GetVertexFromPoint(clickedPoint);
            if (vertex == null)
                return;

            if (IsConnecting)
            {
                int weight = new EditEdgeVM().ShowDialog();
                if (weight != -1)
                {
                    Edges.Add(new EdgeVM(SelectedVertex, vertex, VertexRadius, weight, IsOriented));
                }

                IsConnecting = false;
            }
            else
            {
                SelectedVertex = vertex;
                isDragging = true;
            }
        }

        public void Vertex_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }

        private VertexVM? GetVertexFromPoint(Point point)
        {
            foreach (var vertex in Vertices)
            {
                if (IsPointInsideEllipse(point, vertex, VertexRadius))
                {
                    return vertex;
                }
            }
            return null;
        }

        private List<EdgeVM> GetConnectedEdge(VertexVM vertex)
        {
            var edges = new List<EdgeVM>();
            foreach (var edge in Edges)
            {
                if (edge.From == vertex || edge.To == vertex)
                {
                    edges.Add(edge);
                }
            }
            return edges;
        }

        private bool IsPointInsideEllipse(Point point, VertexVM vertex, double radius)
        {
            double dx = point.X - vertex.X;
            double dy = point.Y - vertex.Y;

            return (dx * dx) / (radius * radius) + (dy * dy) / (radius * radius) <= 1;
        }

        private async Task startSearchAlgo()
        {
            var graph = GraphVMConverter.ToModel(Vertices.ToList(), Edges.ToList(), IsOriented);
            var nodes = SelectedSearchAlgo.Traversal(graph);

            foreach (var node in nodes)
            {
                Vertices.Where(n => n.Name == node.Name).First().SetGreen();
                Logs.Insert(0, $"Посещаем вершину {node.Name}");
                await Task.Delay(500);
            }
            MessageBox.Show("Выполнено");

            foreach (var vertex in Vertices)
            {
                vertex.SetDefaultColor();
            }
            Logs.Clear();
        }

        private async Task startMinPath()
        {
            var graph = GraphVMConverter.ToModel(Vertices.ToList(), Edges.ToList(), IsOriented);

            if (FromVertex == null || ToVertex == null)
            {
                MessageBox.Show("Выберите вершины");
                return;
            }
            ShortestPathAlgorithm algorithm = new();
            var vertexFrom = graph.Vertices.First(n => n.Name == FromVertex.Name);
            var vertexTo = graph.Vertices.First(n => n.Name == ToVertex.Name);

            var (vertices, price, edges) = algorithm.Execute(graph, vertexFrom, vertexTo);

            foreach (var vertex in vertices)
            {
                var vmVertex = Vertices.Where(n => n.Name == vertex.Name).First();
                vmVertex.Colour = new SolidColorBrush(Colors.Orange);

                foreach (var edge in vertex.Edges)
                {
                    var currVertex = Vertices.Where(n => n.Name == edge.DestNode.Name).First();
                    var edgeVM = this.Edges.Where(x => (x.From.Name == edge.From.Name && x.To.Name == edge.To.Name) || (x.To.Name == edge.From.Name && x.From.Name == edge.To.Name)).FirstOrDefault();

                    var oldColorEdge = edgeVM.Colour.Color;
                    var oldColor = currVertex.Colour.Color;

                    currVertex.Colour = new SolidColorBrush(Colors.LightGreen);
                    edgeVM.Colour = new SolidColorBrush(Colors.Blue);
                    Logs.Add($"Check edge {edge.From.Name}-{edge.To.Name} with weight:{edge.Weight}");
                    await Task.Delay(500);
                    edgeVM.Colour = new SolidColorBrush(oldColorEdge);
                    currVertex.Colour = new SolidColorBrush(oldColor);
                    await Task.Delay(500);
                    Logs.Add($"Comparing edges");
                }
                foreach (var edge in edges)
                {
                    var edgeVM = this.Edges.Where(x => (x.From.Name == edge.From.Name && x.To.Name == edge.To.Name) || (x.To.Name == edge.From.Name && x.From.Name == edge.To.Name)).FirstOrDefault();
                    if (edgeVM != null)
                    {
                        edgeVM.Colour = new SolidColorBrush(Colors.Red);
                        Logs.Add($"Edge {edgeVM.From.Name}-{edgeVM.To.Name} chosen");
                        await Task.Delay(500);
                        edges.Remove(edge);
                        break;
                    }
                }

            }
            await Task.Delay(1000);
            foreach (var vertex in Vertices)
            {
                vertex.SetDefaultColor();
            }
            foreach (var edge in this.Edges)
            {
                edge.Colour.Color = Colors.Black;
            }
            await Task.Delay(500);
            Logs.Clear();
            MessageBox.Show(String.Format("Минимальный путь: {0}", price.ToString()));
        }
      
        private void converToPrims()
        {
            //for vertecies[i] = new PromimsVertex(vertecies[i])
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vertices[i] = new PrimsVertexVM(Vertices[i].X, Vertices[i].Y, Vertices[i].Name, 0, null);
            }
        }
        private async Task startMinTreeAlgo()
        {
            converToPrims();
            var graph = GraphVMConverter.ToModel(Vertices.ToList(), Edges.ToList(), IsOriented);
            
            MinTreeAlgorithm algorithm = new();
            var (vertices, edges, origToTemp) = algorithm.Execute(graph);
            var prevVertex = Vertices.Where(n => n.Name == vertices[0].Name).First();
            prevVertex.Colour = new SolidColorBrush(Colors.Orange);
            var vmVertex = prevVertex;
            int currEdgeIndex = 0;
            
            foreach (var vertex in vertices)
            {

                vmVertex = Vertices.Where(n => n.Name == vertex.Name).First();
                vmVertex.Colour = new SolidColorBrush(Colors.Orange);
                EdgeVM edgeVM;

                foreach (var edge in vertex.Edges)
                {

                    var currVertex = Vertices.Where(n => n.Name == edge.DestNode.Name).First() as PrimsVertexVM;
                    edgeVM = this.Edges.Where(x => (x.From.Name == edge.From.Name && x.To.Name == edge.To.Name) || (x.To.Name == edge.From.Name && x.From.Name == edge.To.Name)).FirstOrDefault();

                    var oldColor = currVertex.Colour.Color;
                    var oldColorEdge = edgeVM.Colour.Color;
                    
                    foreach (KeyValuePair<Vertex, PrimsVertex> data in origToTemp)
                    {
                        if (currVertex.Name == data.Key.Name)
                        {
                            currVertex.MinPath = data.Value.MinPath;
                            if (data.Value.MinVertex != null)
                            {
                                currVertex.MinVertex = data.Value.MinVertex.Name;
                                break;
                            }
                        }                     
                    }
                    
                    currVertex.Colour = new SolidColorBrush(Colors.LightGreen);
                    edgeVM.Colour = new SolidColorBrush(Colors.Blue);
                    Logs.Add($"Check edge {edge.From.Name}-{edge.To.Name} with weight:{edge.Weight}");
                    await Task.Delay(500);
                    edgeVM.Colour = new SolidColorBrush(oldColorEdge);
                    currVertex.Colour = new SolidColorBrush(oldColor);
                    await Task.Delay(500);
                    Logs.Add($"Comparing edges");
                }

                var primEdge = edges[Math.Min(currEdgeIndex, edges.Count - 1)];

                edgeVM = this.Edges.Where(edge => edge.From.Name == primEdge.From.Name && edge.To.Name == primEdge.To.Name).FirstOrDefault();
                if (edgeVM != null)
                {
                    edgeVM.Colour = new SolidColorBrush(Colors.Red);
                    Logs.Add($"Edge {edgeVM.From.Name}-{edgeVM.To.Name} chosen");
                }
                edgeVM = this.Edges.Where(edge => edge.To.Name == primEdge.From.Name && edge.From.Name == primEdge.To.Name).FirstOrDefault();
                if (edgeVM != null)
                {
                    edgeVM.Colour = new SolidColorBrush(Colors.Red);
                    Logs.Add($"Edge {edgeVM.From.Name}-{edgeVM.To.Name} chosen");
                }
                currEdgeIndex++;


                prevVertex = vmVertex;
                await Task.Delay(500);

            }


            foreach (var vertex in Vertices)
            {
                vertex.SetDefaultColor();
            }
            foreach (var edge in this.Edges)
            {
                edge.Colour.Color = Colors.Black;
            }
            await Task.Delay(500);
            Logs.Clear();
        }

        private async Task startMaxFlowAlgo()
        {
            var maxFlowAlgo = new MaxFlowAlgorithm();

            foreach (var edge in Edges)
                edge.Text = $"{0} / {edge.Weight}";

            var graph = GraphVMConverter.ToModel(Vertices.ToList(), Edges.ToList(), IsOriented);

            if (FromVertex == null || ToVertex == null)
            {
                MessageBox.Show("Выберите вершины");
                return;
            }

            var result = maxFlowAlgo.Execute(
                graph,
                graph.Vertices.First(v => v.Name == FromVertex.Name),
                graph.Vertices.First(v => v.Name == ToVertex.Name));

            if (result == null)
            {
                MessageBox.Show("Нет путей");
                return;
            }

            int i = 0;
            int lastFlow = 0;
            List<EdgeVM> visitedEdgesVM = new List<EdgeVM>();
            List<EdgeVM> lastVisitedEdgesVM = new List<EdgeVM>();
            foreach (List<Vertex> path in result.Value.Item1)
            {
                Logs.Insert(0, $"Обновляем остаточный граф");

                foreach (var vertex in Vertices)
                    vertex.SetDefaultColor();

                foreach (var edge in Edges)
                {
                    if (!visitedEdgesVM.Contains(edge) && lastVisitedEdgesVM.Contains(edge))
                        visitedEdgesVM.Add(edge);

                    if (lastVisitedEdgesVM.Contains(edge) && edge.Weight - lastFlow >= 0)
                        edge.Weight -= lastFlow;

                    edge.Text = $"{0} / {edge.Weight}";
                }

                lastVisitedEdgesVM = new List<EdgeVM>();

                path.Reverse();
                VertexVM lastVertexVM = Vertices.Where(n => n.Name == path[0].Name).First();
                lastVertexVM.SetGreen();
                Logs.Insert(0, $"Начинаем из вершины {lastVertexVM.Name}");

                await Task.Delay(1000);

                for (int j = 1; j < path.Count; j++)
                {
                    VertexVM v = Vertices.Where(n => n.Name == path[j].Name).First();
                    v.SetGreen();
                    Logs.Insert(0, $"Посещаем вершину {v.Name}");

                    EdgeVM? e = Edges.Where(e => e.To.Name == v.Name &&
                                            e.From.Name == lastVertexVM.Name).FirstOrDefault();

                    if (e != null)
                    {
                        lastVisitedEdgesVM.Add(e);
                        e.Text = $"{result.Value.Item2[i]} / {e.Weight}";
                    }

                    lastVertexVM = v;
                    await Task.Delay(1000);
                }

                lastFlow = result.Value.Item2[i];
                i++;
            }

            MessageBox.Show($"Выполнено!\nМаксимальный поток: {result.Value.Item2.Sum()}");

            foreach (var vertex in Vertices)
            {
                vertex.SetDefaultColor();
                foreach (var edge in Edges)
                    edge.Text = edge.Weight.ToString();
            }
        }
    }
}
