using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

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
                if (Tags.FirstOrDefault() is LocaleStrings)
                {
                    var tvi = new TreeViewItem
                    {
                        Header = "LocaleStrings"
                    };
                    treeView.Items.Add(tvi);
                    return;
                }
                var test = Tags.GroupBy(x => ((Tag)x).Name);
                foreach (var item in test)
                {
                    var tvi = new TreeViewItem
                    {
                        Header = item.Key
                    };
                    foreach (var subItem in item)
                    {
                        var tvi2 = new TreeViewItem { Header = ((Tag)subItem).FullName, Uid = item.Key };
                        if (subItem is PCOL)
                        {
                            for (int i = 0; i < (subItem as PCOL).CollisionCount; i++)
                            {
                                tvi2.Items.Add(new TreeViewItem { Header = $"Collision {i+1}", Uid = $"{item.Key} {((Tag)subItem).FullName} {i}" });
                            }
                        }

                        tvi.Items.Add(tvi2);
                    }
                    treeView.Items.Add(tvi);
                }
            }
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var si = ((TreeViewItem)treeView.SelectedItem);
            if (Tags.FirstOrDefault() is LocaleStrings)
            {
                LocalStringWindow lsw = new(Tags.First() as LocaleStrings);
                lsw.ShowDialog();
                return;
            }
            var ptex = Tags.Where(x => ((Tag)x).FullName == si.Header.ToString() && si.Uid == "PTEX" && si.Uid == ((Tag)x).Name).ToArray();
            if (ptex.Count() != 0)
            {
                exportTextureBtn.IsEnabled = true;
                img.Source = PTEX.LoadBitmap(((PTEX)ptex[0]).Image);
                img.Width = ((PTEX)ptex[0]).Image.Width;
                img.Height = ((PTEX)ptex[0]).Image.Height;
            }
            var chosenTag = Tags.Where(x => ((Tag)x).FullName == si.Header.ToString() && si.Uid == "PMDL" && si.Uid == ((Tag)x).Name).ToArray();
            if (chosenTag.Count() != 0)
            {
                exportModelBtn.IsEnabled = true;
                var modelGroup = new Model3DGroup();
                myViewport.Children.Clear();
                var count = 0;
                foreach (var item in ((PMDL)chosenTag[0]).MeshData)
                {
                    var gm = new GeometryModel3D();
                    var mesh = new MeshGeometry3D();
                    foreach (var cp in item.ControlPoints)
                    {
                        mesh.Positions.Add(new Point3D(cp.x, cp.y, cp.z));
                    }
                    foreach (var i in item.Polygons)
                    {
                        mesh.TriangleIndices.Add(i[0]); mesh.TriangleIndices.Add(i[1]); mesh.TriangleIndices.Add(i[2]);
                    }
                    foreach (var i in ((PMDL)chosenTag[0]).Normals[count])
                    {
                        mesh.Normals.Add(new Vector3D(i.x, i.y, i.z));
                    }
                    foreach (var i in ((PMDL)chosenTag[0]).Uvs[count])
                    {
                        mesh.TextureCoordinates.Add(new Point(i.x, i.y));
                    }
                    gm.Geometry = mesh;
                    var diffuse = new DiffuseMaterial
                    {
                        Brush = new SolidColorBrush(Color.FromRgb(166, 166, 166))
                    };
                    gm.Material = diffuse;


                    modelGroup.Children.Add(gm);
                    count++;
                }
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
                var modelVisual = new ModelVisual3D
                {
                    Content = modelGroup
                };
                myViewport.Children.Add(modelVisual);
            }
            var infos = si.Uid.Split(' ');
            if (infos.Length < 3) return;
            var chosenTag2 = Tags.Where(x => ((Tag)x).FullName == infos[1] && infos[0] == ((Tag)x).Name).ToArray();
            if (chosenTag2.Count() != 0)
            {
                var collsionIndex = Convert.ToInt32(infos[2]);
                var pcol = ((PCOL)chosenTag2[0]);
                exportModelBtn.IsEnabled = true;
                var modelGroup = new Model3DGroup();
                myViewport.Children.Clear();
                var gm = new GeometryModel3D();
                var mesh = new MeshGeometry3D();
                foreach (var cp in pcol.Vertices[collsionIndex])
                {
                    mesh.Positions.Add(new Point3D(cp.x, cp.y, cp.z));
                }
                foreach (var i in pcol.Faces[collsionIndex])
                {
                    mesh.TriangleIndices.Add((int)i);
                }
                gm.Geometry = mesh;
                var diffuse = new DiffuseMaterial
                {
                    Brush = new SolidColorBrush(Color.FromRgb(166, 166, 166))
                };
                gm.Material = diffuse;


                modelGroup.Children.Add(gm);
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
                var modelVisual = new ModelVisual3D
                {
                    Content = modelGroup
                };
                myViewport.Children.Add(modelVisual);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var si = (TreeViewItem)treeView.SelectedItem;
            var infos = si.Uid.Split(' ');
            object chosenTag = null;
            if (infos.Length == 1) chosenTag = Tags.Where(x => ((Tag)x).FullName == si.Header.ToString() && si.Uid == ((Tag)x).Name).ElementAtOrDefault(0);
            if (infos.Length == 3) chosenTag = Tags.Where(x => ((Tag)x).FullName == infos[1] && infos[0] == ((Tag)x).Name).ElementAtOrDefault(0);
            if (chosenTag == null) return;
            var exportOptions = new ExportOptions(chosenTag);
            exportOptions.ShowDialog();
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var si = ((TreeViewItem)treeView.SelectedItem).Header.ToString();
            var chosenTag = (PTEX)Tags.Where(x => ((Tag)x).FullName == si && ((Tag)x).Name == "PTEX").ElementAtOrDefault(0);
            if (chosenTag == null) return;
            var exportOptions = new ExportOptions(chosenTag);
            exportOptions.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var diffuse = new DiffuseMaterial
            {
                Brush = new ImageBrush(img.Source)
            };
            var mg = (Model3DGroup)((ModelVisual3D)myViewport.Children[0]).Content;
            var gms = mg.Children.OfType<GeometryModel3D>();
            MeshSelection meshSelection = new MeshSelection(gms.Count());
            meshSelection.ShowDialog();
            var chosenMeshes = meshSelection.ChosenMeshes;
            ChangeMaterial(diffuse, chosenMeshes);
        }

        private void ChangeMaterial(DiffuseMaterial material, List<int> chosenMeshes)
        {
            var mg = (Model3DGroup)((ModelVisual3D)myViewport.Children[0]).Content;
            var gms = mg.Children.OfType<GeometryModel3D>();
            foreach (var i in chosenMeshes)
            {
                gms.ElementAt(i).Material = material;
            }
        }

    }
}
