using System;
using System.Collections.Generic;
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

namespace PrimeWPF
{
    /// <summary>
    /// Interaction logic for LocalStringWindow.xaml
    /// </summary>
    public partial class LocalStringWindow : Window
    {
        public LocaleStrings Localestrings { get; set; }
        public LocalStringWindow(LocaleStrings ls)
        {
            InitializeComponent();
            Localestrings = ls;
            foreach (var item in Localestrings.Strings)
            {
                cb.Items.Add(item);
            }
            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
