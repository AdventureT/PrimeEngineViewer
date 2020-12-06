using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PrimeWPF
{
    /// <summary>
    /// Interaction logic for MeshSelection.xaml
    /// </summary>
    public partial class MeshSelection : Window
    {
        public List<int> ChosenMeshes { get; set; }
        private int _numberOfMeshes;

        public MeshSelection(int numberOfMeshes)
        {
            InitializeComponent();
            _numberOfMeshes = numberOfMeshes;
            ChosenMeshes = new();
            CreateCheckBoxes();
        }

        private void CreateCheckBoxes()
        {
            for (int i = 0; i < _numberOfMeshes; i++)
            {
                CheckBox cb = new CheckBox
                {
                    Margin = new Thickness(20, 20 * i, 0, 0),
                    Content = $"Mesh {i + 1}"
                };
                grid.Children.Add(cb);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            foreach (var item in grid.Children)
            {
                if (item is CheckBox)
                {
                    var cb = item as CheckBox;
                    if ((bool)cb.IsChecked) ChosenMeshes.Add(Convert.ToInt32(((string)cb.Content)[5..])-1);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) => Close();

    }
}
