using System;
using System.Windows;
using System.Windows.Controls;

namespace EasySave.View {
    /// <summary>
    /// Logique d'interaction pour NewBackupJobView.xaml
    /// </summary>
    public partial class NewBackupJobView : Page {
        public static NewBackupJobView newBackupJobView = new NewBackupJobView();
        private Language language = new Language();
        private ResourceDictionary dict = new ResourceDictionary();

        private NewBackupJobView() {
            InitializeComponent();
            SetLanguageDictionary();
            DisableButton();
        }

        public void SetLanguageDictionary() {
            dict.Source = new Uri(".\\Properties\\Dictionary-" + language.GetLang() + ".xaml", UriKind.Relative);
            this.Resources.MergedDictionaries.Add(dict);
        }

        private void SourceFolderFileExplorerButton_Click(object sender, RoutedEventArgs e) {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog openFolderDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            openFolderDialog.ShowDialog();
            SourceFolderTextBox.Text = openFolderDialog.SelectedPath;
        }

        private void DestinationFolderFileExplorerButton_Click(object sender, RoutedEventArgs e) {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog openFolderDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            openFolderDialog.ShowDialog();
            DestinationFolderTextBox.Text = openFolderDialog.SelectedPath;
        }

        private void CreateTheBackupJobButton_Click(object sender, RoutedEventArgs e) {
            string type, encrypted;
            if((bool)FullRadioButton.IsChecked) {
                type = "Full";
            }
            else {
                type = "Differential";
            }
            if((bool)SpecifiedExtensionsRadioButton.IsChecked) {
                encrypted = "Yes";
            }
            else {
                encrypted = "No";
            }
            string[] parameters = { type, SourceFolderTextBox.Text, DestinationFolderTextBox.Text, BackupJobNameTextBox.Text, encrypted };
            CreateBackupJobLauncher.createBackupJobLauncher.CreateBackupJob(parameters);
            BackupJobNameTextBox.Text = "";
            SourceFolderTextBox.Text = "";
            DestinationFolderTextBox.Text = "";
            NoneRadioButton.IsChecked = false;
            SpecifiedExtensionsRadioButton.IsChecked = false;
            DifferentialRadioButton.IsChecked = false;
            FullRadioButton.IsChecked = false;
        }

        private void DisableButton() {
            bool encryption = (bool)(NoneRadioButton.IsChecked) || (bool)(SpecifiedExtensionsRadioButton.IsChecked);
            bool type = (bool)(FullRadioButton.IsChecked) || (bool)(DifferentialRadioButton.IsChecked);
            if(BackupJobNameTextBox.Text != "" && SourceFolderTextBox.Text != "" && DestinationFolderTextBox.Text != "" && type && encryption) {
                CreateTheBackupJobButton.IsEnabled = true;
            }
            else {
                CreateTheBackupJobButton.IsEnabled = false;
            }
        }

        private void BackupJobNameTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            DisableButton();
        }

        private void SourceFolderTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            DisableButton();
        }

        private void DestinationFolderTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            DisableButton();
        }

        private void NoneRadioButton_Checked(object sender, RoutedEventArgs e) {
            DisableButton();
        }

        private void SpecifiedExtensionsRadioButton_Checked(object sender, RoutedEventArgs e) {
            DisableButton();
        }

        private void DifferentialRadioButton_Checked(object sender, RoutedEventArgs e) {
            DisableButton();
        }

        private void FullRadioButton_Checked(object sender, RoutedEventArgs e) {
            DisableButton();
        }
    }
}
