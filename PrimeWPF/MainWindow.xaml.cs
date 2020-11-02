using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrimeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TRB Files|*.trb";
            openFileDialog.DefaultExt = ".trb";
            openFileDialog.Title = "Select a trb file";
            if ((bool)openFileDialog.ShowDialog())
            {
                var trb = new TRB(openFileDialog.FileName);

            }
            //(sender as BackgroundWorker).ReportProgress(100);
            //var worker = new BackgroundWorker();
            //worker.WorkerReportsProgress = true;
            //worker.DoWork += worker_DoWork;
            //worker.ProgressChanged += worker_ProgressChanged;

            //worker.RunWorkerAsync();
            
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //var openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "TRB Files|*.trb";
            //openFileDialog.DefaultExt = ".trb";
            //openFileDialog.Title = "Select a trb file";
            //if ((bool)openFileDialog.ShowDialog())
            //{
            //    var trb = new TRB(openFileDialog.FileName);

            //}
            //(sender as BackgroundWorker).ReportProgress(100);
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
        }
    }
}
