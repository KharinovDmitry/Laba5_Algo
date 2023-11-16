using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Laba5_Algo.ViewModels
{
    public class EditEdgeVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private EdgeEditWindow view;
        public ICommand Create { get; set; }
        public ICommand Cancel { get; set; }

        private string input;
        public string Input
        {
            get { return input; }
            set { input = value; OnPropertyChanged(nameof(Input)); }
        }

        public int Weight { get; set; }

        private static readonly Regex _regex = new Regex("[^0-9]+");
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        public EditEdgeVM()
        {
            Create = new RelayCommand(create);
            Cancel = new RelayCommand(cancel);
            view = new EdgeEditWindow();
            view.DataContext = this;

            Weight = -1;
            Input = string.Empty;
        }

        public EditEdgeVM(int weight)
        {
            Create = new RelayCommand(create);
            Cancel = new RelayCommand(cancel);
            view = new EdgeEditWindow();
            view.DataContext = this;

            Weight = weight;
            Input = weight.ToString();
        }

        private void cancel(object obj)
        {
            view.Close();
        }

        private void create(object obj)
        {
            if(!IsTextAllowed(Input) || Input == String.Empty)
            {
                MessageBox.Show("Вообще-то неотрицаиельное число надо");
                return;
            }
            Weight = int.Parse(Input);
            view.DialogResult = true;
            view.Close();
        }

        public int ShowDialog()
        {
            if (view.ShowDialog() == true)
                return Weight;
            return -1;
        }
    }
}
