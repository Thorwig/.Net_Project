using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private string messageReceived;
        private string Percent;

        public string Progress {
            get { return Percent; }
            set {
                Percent = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string Percent = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Percent));
        }

        public MainWindow() {
            DataContext = this;
            client.Events.DataReceived += Events_DataReceived;
            InitializeComponent();
        }

        private void AnalysePourcent(object? sender, DoWorkEventArgs e) {
            while(messageReceived != "100") {
                Thread.Sleep(100);
                (sender as BackgroundWorker).ReportProgress(Int16.Parse(messageReceived));
            }
            Thread.Sleep(100);
            (sender as BackgroundWorker).ReportProgress(Int16.Parse(messageReceived));
        }

        private void UpdateProgress(object? sender, ProgressChangedEventArgs e) {
            Progress = e.ProgressPercentage.ToString();
        }

        private void Events_DataReceived(object? sender, DataReceivedEventArgs e) {
            messageReceived = Encoding.UTF8.GetString(e.Data);
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += AnalysePourcent;
            worker.ProgressChanged += UpdateProgress;

            worker.RunWorkerAsync();
        }

        SimpleTcpClient client = new SimpleTcpClient("127.0.0.1:9000");

        private void SendMessageToServer(string message) {
            if (client.IsConnected) {
                client.Send(message);
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e) {
            SendMessageToServer("pause");
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e) {
            SendMessageToServer("play");
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e) {
            try {
                client.Connect();
                ConnectButton.IsEnabled = false;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
