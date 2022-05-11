using System;
using System.Windows;
using EasySave.View;
using EasySave.Layouts;
using System.IO;
using System.Diagnostics;

namespace EasySave {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private Language language = new Language();
        private ResourceDictionary dict = new ResourceDictionary();

        public MainWindow() {
            string procName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(procName);

            if (processes.Length > 1) {
                MessageBox.Show("EasySave is already running");
                Application.Current.Shutdown();
            }
            else {
                InitializeComponent();
                CheckSystemFiles();
                SetLanguageDictionary();
                SettingsView.settingsView.SetMainWindow(this);
            }
        }

        private void CheckSystemFiles() {
            string[] requiredFiles = { "..\\..\\..\\..\\CryptoSoft\\bin\\Release\\net5.0\\CryptoSoft.exe", "..\\..\\..\\..\\EasySaveCore\\assets\\system\\BackupJobs.json" };
            string[] requiredFolders = { "..\\..\\..\\Layouts", "..\\..\\..\\Properties", "..\\..\\..\\View", "..\\..\\..\\..\\EasySaveCore\\assets\\system", "..\\..\\..\\..\\EasySaveCore\\assets\\logs" };
            for (int loop = 0; loop < requiredFiles.Length; loop++) {
                if (!File.Exists(requiredFiles[loop])) {
                    Environment.Exit(1);
                }
            }
            for (int loop = 0; loop < requiredFolders.Length; loop++) {
                if (!Directory.Exists(requiredFolders[loop])) {
                    Environment.Exit(1);
                }
            }
        }

        public void SetLanguageDictionary() {
            dict.Source = new Uri(".\\Properties\\Dictionary-" + language.GetLang() + ".xaml", UriKind.Relative);
            this.Resources.MergedDictionaries.Add(dict);
        }

        private void RunBackupBtn(object sender, RoutedEventArgs e) {
            MainFrame.Content = RunBackupJobView.runBackupJobView;
            RunBackupJobView.runBackupJobView.SetDatagrid();
            RunBackupJobView.runBackupJobView.SetLanguageDictionary();
            ReturnBackupJobFrame.Content = RunBackupBtnView.runBackupBtnView;
            ManageBackupJobFrame.Content = null;
            NewBackupJobFrame.Content = null;
            SettingsFrame.Content = null;

        }

        private void ManageBackupBtn(object sender, RoutedEventArgs e) {
            MainFrame.Content = ManageBackupJobsView.manageBackupJobsView;
            ManageBackupJobsView.manageBackupJobsView.SetDatagrid();
            ManageBackupJobsView.manageBackupJobsView.SetLanguageDictionary();
            ManageBackupJobFrame.Content = ManageBackupBtnView.manageBackupBtnView;
            ReturnBackupJobFrame.Content = null;
            NewBackupJobFrame.Content = null;
            SettingsFrame.Content = null;
        }

        private void NewBackupBtn(object sender, RoutedEventArgs e) {
            MainFrame.Content = NewBackupJobView.newBackupJobView;
            NewBackupJobView.newBackupJobView.SetLanguageDictionary();
            NewBackupJobFrame.Content = NewBackupBtnView.newBackupBtnView;
            ManageBackupJobFrame.Content = null;
            ReturnBackupJobFrame.Content = null;
            SettingsFrame.Content = null;
        }

        private void SettingsBtn(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = SettingsView.settingsView;
            SettingsView.settingsView.SetLanguageDictionary();
            SettingsFrame.Content = SettingsBtnView.settingsBtnView;
            ManageBackupJobFrame.Content = null;
            NewBackupJobFrame.Content = null;
            ReturnBackupJobFrame.Content = null;
        }
    }
}
