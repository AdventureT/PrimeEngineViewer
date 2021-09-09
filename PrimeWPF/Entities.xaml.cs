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
                    item2.Tag = prop;
                    //Get Properties via Reflection cause Generic succs
                    item2.Header = prop.GetType().GetProperty("Name").GetValue(prop);
                    item2.Items.Add(new TreeViewItem() { Header = "Type: " + prop.Type, Tag = prop });
                    item2.Items.Add(new TreeViewItem() { Header = "Value: " + prop.Value, Tag = prop });
                    item.Items.Add(item2);
                }
                treeView.Items.Add(item);
                
            }
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            EntityInfo selectedEntity;
            var tvi = treeView.SelectedItem as TreeViewItem;
            if (tvi.Tag is EntityInfo)
            {
                selectedEntity = tvi.Tag as EntityInfo;
            }
            else
            {
                selectedEntity = (GetSelectedTreeViewItemParent(tvi) as TreeViewItem).Tag is EntityInfo ? (GetSelectedTreeViewItemParent(tvi) as TreeViewItem).Tag as EntityInfo : (GetSelectedTreeViewItemParent(GetSelectedTreeViewItemParent(tvi) as TreeViewItem) as TreeViewItem).Tag as EntityInfo;
            }
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
            var tvi = treeView.SelectedItem as TreeViewItem;
            EntityInfo entityInfo;
            if (tvi.Tag is EntityInfo)
            {
                entityInfo = tvi.Tag as EntityInfo; 
            }
            else
            {
                entityInfo = (GetSelectedTreeViewItemParent(tvi) as TreeViewItem).Tag is EntityInfo ? (GetSelectedTreeViewItemParent(tvi) as TreeViewItem).Tag as EntityInfo : (GetSelectedTreeViewItemParent(GetSelectedTreeViewItemParent(tvi) as TreeViewItem) as TreeViewItem).Tag as EntityInfo;
            }
            using var f = new BinaryWriter(File.OpenWrite(TRB._fileName));
            f.BaseStream.Seek(entityInfo.PositionPos, SeekOrigin.Begin);
            Matrix4 mat = new();
            mat.SetTRS(new(Convert.ToSingle(posX.Text), Convert.ToSingle(posY.Text), Convert.ToSingle(posZ.Text)),
                new(Convert.ToSingle(rotX.Text), Convert.ToSingle(rotY.Text), Convert.ToSingle(rotZ.Text)),
                new(Convert.ToSingle(scaleX.Text), Convert.ToSingle(scaleY.Text), Convert.ToSingle(scaleZ.Text)));
            foreach (float item in mat.ToArray())
            {
                f.Write(item);
            }
            if (tvi.Tag is EntityInfo) return;
            var prop = tvi.Tag as EntityProperty;
            var pos = prop.Position;
            f.BaseStream.Seek(pos + 8, SeekOrigin.Begin);
            switch (prop.Type)
            {
                case PropertyType.INT:
                    f.Write(Convert.ToInt32(changeValue.Text));
                    break;
                case PropertyType.UINT:
                    f.Write(Convert.ToInt32(changeValue.Text));
                    break;
                case PropertyType.FLOAT:
                    f.Write(Convert.ToSingle(changeValue.Text));
                    break;
                case PropertyType.BOOL:
                    f.Write(changeValue.Text == "True" || changeValue.Text == "true" ? 1 : 0);
                    break;
                case PropertyType.TEXTOFFSET:
                    f.Write(Convert.ToInt32(changeValue.Text));
                    break;
                case PropertyType.VECTOR4:
                    f.Write(Convert.ToInt32(changeValue.Text));
                    break;
                case PropertyType.Unknown2:
                    break;
                case PropertyType.Unknown3:
                    break;
                case PropertyType.Unknown4:
                    break;
                case PropertyType.OFFSET:
                    f.Write(Convert.ToInt32(changeValue.Text));
                    break;
                default:
                    break;
            }

        }
        private ItemsControl GetSelectedTreeViewItemParent(TreeViewItem item)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(item);
            while (!(parent is TreeViewItem || parent is TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as ItemsControl;
        }
    }
}
