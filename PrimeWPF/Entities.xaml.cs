using Aspose.ThreeD.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using static PrimeWPF.enti;

namespace PrimeWPF
{
    /// <summary>
    /// Interaction logic for Entities.xaml
    /// </summary>
    public partial class Entities : Window
    {
        public Entities()
        {
            InitializeComponent();
        }
        public Entities(enti ent)
        {
            InitializeComponent();
            foreach (var info in ent.EntityInfos)
            {
                var item = new TreeViewItem();
                item.Header = info.EntityName;
                item.Tag = info;
                foreach (var prop in info.Properties)
                {
                    var item2 = new TreeViewItem();
                    //Get Properties via Reflection cause Generic succs
                    item2.Header = prop.GetType().GetProperty("Name").GetValue(prop);
                    item2.Items.Add("Type: " + prop.GetType().GetProperty("Type").GetValue(prop));
                    item2.Items.Add("Value: " + prop.GetType().GetProperty("Value").GetValue(prop));
                    item.Items.Add(item2);
                }
                treeView.Items.Add(item);
                
            }
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if ((e.NewValue as TreeViewItem).Tag is not EntityInfo) return;
            var selectedEntity = (e.NewValue as TreeViewItem).Tag as EntityInfo;
            posX.Text = Convert.ToString(Math.Round(selectedEntity.Position.x, 2));
            posY.Text = Convert.ToString(Math.Round(selectedEntity.Position.y, 2));
            posZ.Text = Convert.ToString(Math.Round(selectedEntity.Position.z, 2));
            scaleX.Text = Convert.ToString(Math.Round(selectedEntity.Scale.x, 2));
            scaleY.Text = Convert.ToString(Math.Round(selectedEntity.Scale.y, 2));
            scaleZ.Text = Convert.ToString(Math.Round(selectedEntity.Scale.z, 2));
            rotX.Text = Convert.ToString(Math.Round(selectedEntity.Rotation.x, 2));
            rotY.Text = Convert.ToString(Math.Round(selectedEntity.Rotation.y, 2));
            rotZ.Text = Convert.ToString(Math.Round(selectedEntity.Rotation.z, 2));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using var f = new BinaryWriter(File.OpenWrite(TRB._fileName));
            f.BaseStream.Seek(((treeView.SelectedItem as TreeViewItem).Tag as EntityInfo).PositionPos, SeekOrigin.Begin);
            Matrix4 mat = new();
            mat.SetTRS(new(Convert.ToSingle(posX.Text), Convert.ToSingle(posY.Text), Convert.ToSingle(posZ.Text)),
                new(Convert.ToSingle(rotX.Text), Convert.ToSingle(rotY.Text), Convert.ToSingle(rotZ.Text)),
                new(Convert.ToSingle(scaleX.Text), Convert.ToSingle(scaleY.Text), Convert.ToSingle(scaleZ.Text)));
            foreach (float item in mat.ToArray())
            {
                f.Write(item);
            }
        }
    }
}
