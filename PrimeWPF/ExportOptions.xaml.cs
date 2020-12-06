using Aspose.ThreeD;
using Aspose.ThreeD.Entities;
using Aspose.ThreeD.Utilities;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace PrimeWPF
{
    /// <summary>
    /// Interaction logic for ExportOptions.xaml
    /// </summary>
    public partial class ExportOptions : Window
    {
        public object Data { get; set; }
        public int Index { get; set; }
        public ExportOptions(object obj)
        {
            InitializeComponent();
            Data = obj;
            if (Data is PTEX) ddsCb.Visibility = Visibility.Visible;
            else if (Data is PMDL)
            {
                objCb.Visibility = Visibility.Visible;
                fbxCb.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Data is PMDL)
            {
                var pmdl = Data as PMDL;
                if (!(bool)objCb.IsChecked && !(bool)fbxCb.IsChecked) return;
                var scene = new Scene();

                for (int i = 0; i < pmdl.MeshData.Length; i++)
                {
                    var elementNormal = pmdl.MeshData[i].CreateElement(VertexElementType.Normal, MappingMode.ControlPoint, ReferenceMode.Direct) as VertexElementNormal;
                    var elementUV = pmdl.MeshData[i].CreateElementUV(TextureMapping.Diffuse, MappingMode.ControlPoint, ReferenceMode.Direct);
                    elementNormal.Data.AddRange(pmdl.Normals[i]);
                    elementUV.Data.AddRange(pmdl.Uvs[i]);
                    var meshNode = new Node("Mesh")
                    {
                        Entity = pmdl.MeshData[i]
                    };
                    scene.RootNode.AddChildNode(meshNode);
                    meshNode.Transform.Rotation = Aspose.ThreeD.Utilities.Quaternion.FromRotation(new Vector3(0, 1, 0), new Vector3(180, 0, 0));
                }

                if ((bool)objCb.IsChecked)
                {
                    var saveFileDialog = new SaveFileDialog
                    {
                        Filter = "OBJ File|*.obj",
                        DefaultExt = ".obj",
                        Title = "Save as obj file",
                        FileName = pmdl.FullName
                    };
                    if ((bool)saveFileDialog.ShowDialog())
                    {
                        Close();
                        scene.Save($"{saveFileDialog.FileName}", FileFormat.WavefrontOBJ);
                    }
                }
                else if ((bool)fbxCb.IsChecked)
                {
                    
                    var saveFileDialog = new SaveFileDialog
                    {
                        Filter = "FBX File|*.fbx",
                        DefaultExt = ".fbx",
                        Title = "Save as fbx file",
                        FileName = pmdl.FullName
                    };
                    if ((bool)saveFileDialog.ShowDialog())
                    {
                        Close();
                        scene.Save($"{saveFileDialog.FileName}", FileFormat.FBX7500Binary);
                    }

                }
            }
            else if (Data is PTEX)
            {
                ddsCb.Visibility = Visibility.Visible;
                var ptex = Data as PTEX;
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "DDS File|*.dds",
                    DefaultExt = ".dds",
                    Title = "Save as dds file",
                    FileName = ptex.FullName
                };
                if ((bool)saveFileDialog.ShowDialog())
                {
                    Close();
                    using (BinaryWriter writer = new BinaryWriter(File.Open($"{saveFileDialog.FileName}", FileMode.Create)))
                    {
                        writer.Write(ptex.RawImage);
                    }
                }
            }
            
        }
    }
}
