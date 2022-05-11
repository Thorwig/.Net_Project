using EasySaveGUI;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace EasySave.View {
    /// <summary>
    /// Logique d'interaction pour ManageBackupJobsView.xaml
    /// </summary>
    public partial class ManageBackupJobsView : Page {
        public static ManageBackupJobsView manageBackupJobsView = new ManageBackupJobsView();
        private Language language = new Language();
        private ResourceDictionary dict = new ResourceDictionary();
        private int backupJobIDTemp;

        private ManageBackupJobsView() {
            InitializeComponent();
            SetLanguageDictionary();
            SetIDTemp();
            DisableButton();
        }

        public void SetDatagrid() {
            ManageBackupJobDataGrid.Items.Clear();
            if (ManageBackupJobDataGrid.Items.Count < BackupJobs.backupJobs.GetArrayBackupJobName().Length) {
                for (int i = ManageBackupJobDataGrid.Items.Count; i < BackupJobs.backupJobs.GetArrayBackupJobName().Length; i++) {
                    List<DatagridBackupJob> user = new List<DatagridBackupJob>();
                    user.Add(new DatagridBackupJob() { Name = BackupJobs.backupJobs.GetArrayBackupJobName()[i], Source = BackupJobs.backupJobs.GetArrayBackupJobSource()[i], Destination = BackupJobs.backupJobs.GetArrayBackupJobDestination()[i], IsEncrypted = BackupJobs.backupJobs.GetArrayBackupJobEncrypted()[i], Type = BackupJobs.backupJobs.GetArrayBackupJobType()[i] });
                    ManageBackupJobDataGrid.Items.Add(user);
                }
            }
        }

        public void SetLanguageDictionary() {
            dict.Source = new Uri(".\\Properties\\Dictionary-" + language.GetLang() + ".xaml", UriKind.Relative);
            this.Resources.MergedDictionaries.Add(dict);
        }

        private void ManageBackupJobDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            backupJobIDTemp = ManageBackupJobDataGrid.Items.IndexOf(ManageBackupJobDataGrid.CurrentItem);
            DisableButton();
        }

        private void SetIDTemp() {
            backupJobIDTemp = ManageBackupJobDataGrid.Items.IndexOf(ManageBackupJobDataGrid.CurrentItem);
        }

        private void DisableButton() {
            if(backupJobIDTemp < 0) {
                DeleteButton.IsEnabled = false;
            }
            else {
                DeleteButton.IsEnabled = true;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e) {
            BackupJobCreate.backupJobCreate.DeleteBackupJob(backupJobIDTemp);
            SetDatagrid();
        }
    }
}
