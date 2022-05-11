using EasySaveGUI;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Threading;
using System.IO;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using SuperSimpleTcp;
using System.Windows.Forms;
using System.Text;

namespace EasySave.View {
    /// <summary>
    /// Logique d'interaction pour RunBackupJobView.xaml
    /// </summary>
    public partial class RunBackupJobView : Page, INotifyPropertyChanged {
        public static RunBackupJobView runBackupJobView = new RunBackupJobView();
        private Language language = new Language();
        private ResourceDictionary dict = new ResourceDictionary();
        SimpleTcpServer server = new SimpleTcpServer("127.0.0.1:9000");
        private int backupJobIDTemp;
        private int pourcent = 0;
        private int endedThread;
        private List<Thread> currentBackupJobThreadList = new List<Thread>();
        private int numberOfBackupJobs;
        private List<string> ClientIPList = new List<string>();

        public event PropertyChangedEventHandler PropertyChanged;
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

        private RunBackupJobView() {
            DataContext = this;
            server.Events.DataReceived += Events_DataReceived;
            server.Events.ClientConnected += Events_ClientConnected;
            InitializeComponent();
            SetLanguageDictionary();
            StopButton.Visibility = Visibility.Hidden;
            PauseButton.Visibility = Visibility.Hidden;
            PlayButton.Visibility = Visibility.Hidden;
        }

        private void Events_ClientConnected(object sender, ConnectionEventArgs e) {
            ClientIPList.Add(e.IpPort.ToString());
        }

        private void Events_DataReceived(object sender, SuperSimpleTcp.DataReceivedEventArgs e) {
            string message = Encoding.UTF8.GetString(e.Data);
            if(message == "play") {
                LaunchBackupJob.launchBackupJob.ResumeBackup();
            }
            if(message == "pause") {
                LaunchBackupJob.launchBackupJob.PauseBackup();
            }
        }

        public void SetDatagrid() {
            RunBackupJobDataGrid.Items.Clear();
            if (RunBackupJobDataGrid.Items.Count < BackupJobs.backupJobs.GetArrayBackupJobName().Length) {
                for (int i = RunBackupJobDataGrid.Items.Count; i < BackupJobs.backupJobs.GetArrayBackupJobName().Length; i++) {
                    List<DatagridBackupJob> user = new List<DatagridBackupJob>();
                    user.Add(new DatagridBackupJob() { Name = BackupJobs.backupJobs.GetArrayBackupJobName()[i], Source = BackupJobs.backupJobs.GetArrayBackupJobSource()[i], Destination = BackupJobs.backupJobs.GetArrayBackupJobDestination()[i], IsEncrypted = BackupJobs.backupJobs.GetArrayBackupJobEncrypted()[i], Type = BackupJobs.backupJobs.GetArrayBackupJobType()[i] });
                    RunBackupJobDataGrid.Items.Add(user);
                }
            }
        }

        public static long DirSize(DirectoryInfo d) {
            long size = 0;
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis) {
                size += fi.Length;
            }
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis) {
                size += DirSize(di);
            }
            return size;
        }

        public void SetLanguageDictionary() {
            dict.Source = new Uri(".\\Properties\\Dictionary-" + language.GetLang() + ".xaml", UriKind.Relative);
            this.Resources.MergedDictionaries.Add(dict);
        }

        private void RunAllBackupJobsButton_Click(object sender, RoutedEventArgs e) {
            StopButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Hidden;
            LaunchBackupJob.launchBackupJob.SetExtensionsToEncrypt(language.GetExtensions());

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += ReportProgressBackgroundWorkerForAllBackupJobs;
            worker.ProgressChanged += UpdateProgressBar;

            numberOfBackupJobs = RunBackupJobDataGrid.Items.Count;
            endedThread = 0;
            List<Thread> backupJobsThreadArray = new List<Thread>();

            for (int i = 0; i < numberOfBackupJobs; i++) {
                Thread thread = new Thread(StartAllBackupJobsThreadTask);
                thread.Name = "Backup" + i.ToString();
                backupJobsThreadArray.Add(thread);
            }

            backupJobIDTemp = 0;
            worker.RunWorkerAsync();

            foreach (Thread thread in backupJobsThreadArray) {
                thread.Start();
                Thread.Sleep(100);
                backupJobIDTemp++;
            }
        }

        private void StartAllBackupJobsThreadTask() {
            StartBackup();
            endedThread++;
        }

        private void StartSelectedBackupJobThreadTask() {
            StartBackup();
        }

        private void RunSelectedBackupJobButton_Click(object sender, RoutedEventArgs e) {
            Stop();
            Thread.Sleep(100);
            StopButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Hidden;
            LaunchBackupJob.launchBackupJob.SetExtensionsToEncrypt(language.GetExtensions());

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += ReportProgressBackgroundWorker;
            worker.ProgressChanged += UpdateProgressBar;

            worker.RunWorkerAsync();

            Thread backupJobThread = new Thread(StartSelectedBackupJobThreadTask);

            currentBackupJobThreadList.Clear();
            currentBackupJobThreadList.Add(backupJobThread);

            backupJobThread.Start();
        }

        private bool CheckSoftwareExceptions() {
            return Process.GetProcessesByName(language.GetSoftware()).Length > 0;
        }

        private int GetEndedThread() {
            return endedThread * 100;
        }

        private void ReportProgressBackgroundWorkerForAllBackupJobs(object sender, DoWorkEventArgs e) {
            bool isPaused = false;
            Thread.Sleep(100);
            while (endedThread < numberOfBackupJobs) {
                if (CheckSoftwareExceptions()) {
                    Thread.Sleep(100);
                    System.Windows.MessageBox.Show("Software exception raised");
                    Pause();
                    isPaused = true;
                    Thread.Sleep(100);
                }
                else if (isPaused) {
                    Thread.Sleep(100);
                    Play();
                    Thread.Sleep(100);
                }
                pourcent = GetEndedThread() / numberOfBackupJobs;
                if (pourcent > 90) {
                    pourcent = 100;
                }
                Thread.Sleep(100);
                (sender as BackgroundWorker).ReportProgress(pourcent);
            }
            if (endedThread == numberOfBackupJobs) {
                Thread.Sleep(100);
                (sender as BackgroundWorker).ReportProgress(100);
            }
            pourcent = 0;
        }

        private void ReportProgressBackgroundWorker(object sender, DoWorkEventArgs e) {
            bool isPaused = false;
            Thread.Sleep(100);
            int step = LaunchBackupJob.launchBackupJob.FilesNumber;
            while (pourcent < 100) {
                if(CheckSoftwareExceptions()) {
                    isPaused = true;
                    while (CheckSoftwareExceptions()) {
                        Thread.Sleep(100);
                        LaunchBackupJob.launchBackupJob.PauseBackup();
                        Thread.Sleep(100);
                        System.Windows.MessageBox.Show("Software exception raised");
                    }
                }
                if(!CheckSoftwareExceptions() && isPaused) {
                    isPaused = false;
                    Thread.Sleep(100);
                    LaunchBackupJob.launchBackupJob.ResumeBackup();
                    Thread.Sleep(100);
                }
                pourcent = LaunchBackupJob.launchBackupJob.Pourcentage * (100 / step);
                if (pourcent > 90) {
                    pourcent = 100;
                }
                Thread.Sleep(100);
                (sender as BackgroundWorker).ReportProgress(pourcent);
                Thread.Sleep(50);
                SendServerToClient(pourcent.ToString());
                Thread.Sleep(50);
            }
            if (currentBackupJobThreadList[0].ThreadState == System.Threading.ThreadState.Stopped) {
                Thread.Sleep(100);
                (sender as BackgroundWorker).ReportProgress(100);
                Thread.Sleep(50);
                SendServerToClient("100");
                Thread.Sleep(50);
            }
            pourcent = 0;
        }

        private void UpdateProgressBar(object sender, ProgressChangedEventArgs e) {
            Progress = e.ProgressPercentage.ToString();
        }

        private void StartBackup() {
            LaunchBackupJob.launchBackupJob.SetRunBackupTrue();
            CreatedBackupJobSelect.createdBackupJobSelect.StartBackup(backupJobIDTemp);
        }

        private void RunBackupJobDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            backupJobIDTemp = RunBackupJobDataGrid.Items.IndexOf(RunBackupJobDataGrid.CurrentItem);
        }

        private void Pause() {
            StopButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Hidden;
            PlayButton.Visibility = Visibility.Visible;
            LaunchBackupJob.launchBackupJob.PauseBackup();
        }

        private void Play() {
            StopButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Hidden;
            LaunchBackupJob.launchBackupJob.ResumeBackup();
        }

        private void Stop() {
            StopButton.Visibility = Visibility.Hidden;
            PauseButton.Visibility = Visibility.Hidden;
            PlayButton.Visibility = Visibility.Hidden;
            LaunchBackupJob.launchBackupJob.StopBackup();
            SendServerToClient("0");
            Thread.Sleep(500);
            LaunchBackupJob.launchBackupJob.Pourcentage = 0;
            pourcent = 0;
            Progress = "0";
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e) {
            Pause();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e) {
            Play();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e) {
            Stop();
        }

        private void SendServerToClient(string message) {
            if (server.IsListening) {
                server.Send(ClientIPList[0], message);
            }
        }

        private void StartServerButton_Click(object sender, RoutedEventArgs e) {
            server.Start();
            ServerStatusTextBlock.Text = "Server started";
            StartServerButton.IsEnabled = false;
        }
    }
}
