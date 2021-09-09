using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;

namespace PrimeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TRB _trb;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            worker.RunWorkerAsync();

        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_trb._items[0] is enti)
            {
                new Entities(_trb._items[0] as enti).Show();
            }
            else
            {
                var content = new Content(_trb._items);
                content.ShowDialog();
            }
            
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "TRB Files|*.trb",
                DefaultExt = ".trb",
                Title = "Select a trb file"
            };
            if ((bool)openFileDialog.ShowDialog())
            {
                _trb = new TRB(openFileDialog.FileName);
            }
            (sender as BackgroundWorker).ReportProgress(100);
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
        }
    }
}
