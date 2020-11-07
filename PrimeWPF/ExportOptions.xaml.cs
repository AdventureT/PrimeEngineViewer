using Aspose.ThreeD;
using Aspose.ThreeD.Utilities;
using Microsoft.Win32;
using System.Windows;

namespace PrimeWPF
{
    /// <summary>
    /// Interaction logic for ExportOptions.xaml
    /// </summary>
    public partial class ExportOptions : Window
    {
        public PMDL Pmdl { get; set; }
        public int Index { get; set; }
        public ExportOptions(PMDL pmdl)
        {
            InitializeComponent();
            Pmdl = pmdl;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(bool)objCb.IsChecked && !(bool)fbxCb.IsChecked) return;
            var scene = new Scene();
            for (int i = 0; i < Pmdl.MeshData.Length; i++)
            {
                var meshNode = new Node("Mesh")
                {
                    Entity = Pmdl.MeshData[i]
                };
                scene.RootNode.AddChildNode(meshNode);
                meshNode.Transform.Rotation = Aspose.ThreeD.Utilities.Quaternion.FromRotation(new Vector3(0, 1, 0), new Vector3(180, 0, 0));
            }
            
            if ((bool)objCb.IsChecked)
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "OBJ Files|*.obj",
                    DefaultExt = ".obj",
                    Title = "Save as obj file"
                };
                if ((bool)saveFileDialog.ShowDialog())
                {
                    scene.Save($"{System.IO.Path.GetDirectoryName(TRB._fileName)}\\{Pmdl.FullName}.obj", FileFormat.WavefrontOBJ);
                }
            }
            if ((bool)fbxCb.IsChecked)
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "FBX Files|*.fbx",
                    DefaultExt = ".fbx",
                    Title = "Save as fbx file"
                };
                if ((bool)saveFileDialog.ShowDialog())
                {
                    scene.Save($"{System.IO.Path.GetDirectoryName(TRB._fileName)}\\{Pmdl.FullName}.fbx", FileFormat.FBX7400ASCII);
                }
               
            }
        }
    }
}
