using Aspose.ThreeD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace PrimeWPF
{
    /// <summary>
    /// Interaction logic for Content.xaml
    /// </summary>
    public partial class Content : Window
    {
        public List<object> Tags { get; set; }
        public Content(List<object> pmdls)
        {
            InitializeComponent();
            if (pmdls.Count > 0)
            {
                Tags = pmdls;
                var test = Tags.GroupBy(x => ((Tag)x).Name);
                foreach (var item in test)
                {
                    var tvi = new TreeViewItem
                    {
                        Header = item.Key
                    };
                    foreach (var subItem in item)
                    {
                        tvi.Items.Add(((Tag)subItem).FullName);
                    }
                    treeView.Items.Add(tvi);
                }
            }
            
            
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var si = (string)treeView.SelectedItem;
            
            var ptex = Tags.Where(x => ((Tag)x).FullName == si && ((Tag)x).Name == "PTEX").ToArray();
            if (ptex.Count() != 0)
            {
                img.Source = PTEX.loadBitmap(((PTEX)ptex[0]).Image);
            }
            var chosenTag = Tags.Where(x => ((Tag)x).FullName == si && ((Tag)x).Name == "PMDL").ToArray();
            if (chosenTag.Count() != 0)
            {
                grid.IsEnabled = true;
                var scaleTransform = new ScaleTransform3D();
                var modelGroup = new Model3DGroup();
                myViewport.Children.Clear();
                foreach (var item in ((PMDL)chosenTag[0]).MeshData)
                {
                    var geometryModel = new GeometryModel3D();
                    var mesh = new MeshGeometry3D();
                    foreach (var cp in item.ControlPoints)
                    {
                        mesh.Positions.Add(new Point3D(cp.x, cp.y, cp.z));
                    }
                    foreach (var i in item.Polygons)
                    {
                        mesh.TriangleIndices.Add(i[0]); mesh.TriangleIndices.Add(i[1]); mesh.TriangleIndices.Add(i[2]);
                    }
                    geometryModel.Geometry = mesh;
                    var diffuse = new DiffuseMaterial
                    {
                        Brush = new SolidColorBrush(Color.FromRgb(166, 166, 166))
                    };
                    geometryModel.Material = diffuse;
                    var directionalLight = new DirectionalLight
                    {
                        Color = Color.FromRgb(255, 255, 255),
                        Direction = new Vector3D(-1, -1, -1)
                    };
                    var directionalLight2 = new DirectionalLight
                    {
                        Color = Color.FromRgb(255, 255, 255),
                        Direction = new Vector3D(5, 5, 5)
                    };
                    modelGroup.Children.Add(directionalLight);
                    modelGroup.Children.Add(directionalLight2);
                    modelGroup.Children.Add(geometryModel);
                }
                var modelVisual = new ModelVisual3D
                {
                    Content = modelGroup
                };
                myViewport.Children.Add(modelVisual);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var si = (string)treeView.SelectedItem;
            var chosenTag = (PMDL)Tags.Where(x => ((Tag)x).FullName == si && ((Tag)x).Name == "PMDL").ToArray()[0];
            var exportOptions = new ExportOptions(chosenTag);
            exportOptions.ShowDialog();
        }
    }
}
