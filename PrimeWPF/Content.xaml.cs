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
                var tvi = new TreeViewItem
                {
                    Header = ((Tag)pmdls[0]).Name
                };
                foreach (Tag item in pmdls)
                {
                    tvi.Items.Add(item.FullName);
                }
                treeView.Items.Add(tvi);
            }
            
            
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var si = (string)treeView.SelectedItem;
            var chosenTag = (PMDL)Tags.Where(x => ((Tag)x).FullName == si && ((Tag)x).Name == "PMDL").ToArray()[0];
            //var model = new GeometryModel3D();
            //var mesh = new MeshGeometry3D();
            //var scene = new Scene()
            //foreach (var cp in chosenTag.CombinedMeshData.ControlPoints)
            //{
            //    mesh.Positions.Add(new Point3D(cp.x, cp.y, cp.z));
            //}
            //foreach (var i in chosenTag.CombinedMeshData.Polygons)
            //{
            //    mesh.TriangleIndices.Add(i[0]); mesh.TriangleIndices.Add(i[1]); mesh.TriangleIndices.Add(i[2]);
            //}
            //model.Geometry = mesh;
            //var dm = new DiffuseMaterial();
            //Color c = new Color
            //{
            //    ScA = 1,
            //    ScB = 255,
            //    ScR = 0,
            //    ScG = 0
            //};
            //dm.Brush = new SolidColorBrush(c);
            //model.Material = dm;
            //modelGroup.Children.Add(model);
            var st = new ScaleTransform3D();
            
            var mg = new Model3DGroup();
            myViewport.Children.Clear();
            foreach (var item in chosenTag.MeshData)
            {
                var model = new GeometryModel3D();
                var mesh = new MeshGeometry3D();
                foreach (var cp in item.ControlPoints)
                {
                    mesh.Positions.Add(new Point3D(cp.x, cp.y, cp.z));
                }
                foreach (var i in item.Polygons)
                {
                    mesh.TriangleIndices.Add(i[0]); mesh.TriangleIndices.Add(i[1]); mesh.TriangleIndices.Add(i[2]);
                }
                model.Geometry = mesh;
                var dm = new DiffuseMaterial
                {
                    Brush = new SolidColorBrush(Color.FromRgb(166, 166, 166))
                };
                model.Material = dm;
                var dl = new DirectionalLight
                {
                    Color = Color.FromRgb(255, 255, 255),
                    Direction = new Vector3D(-1, -1, -1)
                };
                var dl2 = new DirectionalLight
                {
                    Color = Color.FromRgb(255, 255, 255),
                    Direction = new Vector3D(5, 5, 5)
                };
                mg.Children.Add(dl);
                mg.Children.Add(dl2);
                mg.Children.Add(model);
            }
            var mv = new ModelVisual3D
            {
                Content = mg
            };
            myViewport.Children.Add(mv);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
