using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using EasySave.Layouts;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using EasySaveGUI;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Data;

namespace EasySave.View {
    /// <summary>
    /// Logique d'interaction pour SettingsView.xaml
    /// </summary>
    /// 

    public partial class SettingsView : Page {
        public static SettingsView settingsView = new SettingsView();
        private ResourceDictionary dict = new ResourceDictionary();
        private MainWindow mainWindow = null;
        private readonly string fileName = "..\\..\\..\\..\\EasySaveCore\\assets\\system\\";
        private Language language = new Language();
        private bool XML, JSON;
        private string logFormat, minimumLargeFileSize, minimumLargeFileType;

        public SettingsView() {
            InitializeComponent();
            SetLanguageDictionary();
            SetAllWidget();
            DisableButton();
        }

        public void SetMainWindow(MainWindow mainWindowObject) {
            mainWindow = mainWindowObject;
        }

        private void SetSoftwareDatagrid() {
            List<SoftwareDatagrid> soft = new List<SoftwareDatagrid>();
            soft.Add(new SoftwareDatagrid() { Software = GetSoftware() });
            SofwareExceptionsDataGrid.Items.Add(soft);
        }

        private void SetPriorityDatagrid() {
            List<SoftwareDatagrid> soft = new List<SoftwareDatagrid>();
            soft.Add(new SoftwareDatagrid() { Software = GetPriority() });
            PriorityFilesDataGrid.Items.Add(soft);
        }

        private void SetExtensionsDatagrid() {
            List<ExtensionsDataGrid> soft = new List<ExtensionsDataGrid>();
            soft.Add(new ExtensionsDataGrid() { Extensions = GetExtensions() });
            ExtensionsToEncryptDataGrid.Items.Add(soft);
        }

        private string GetSoftware() {
            return language.GetSoftware();
        }

        private string GetPriority() {
            return language.GetPriority();
        }

        private string GetExtensions() {
            return language.GetExtensions();
        }

        private void SaveSoftware() {
            File.WriteAllText(fileName + "SoftwareExceptions.json", JsonConvert.SerializeObject(SofwareExceptionsTextBox.Text));
        }

        private void SavePriority() {
            File.WriteAllText(fileName + "PriorityFile.json", JsonConvert.SerializeObject(PriorityFilesTextBox.Text));
        }

        private void SaveExtensions() {
            File.WriteAllText(fileName + "ExtensionsToEncrypt.json", JsonConvert.SerializeObject(ExtensionsToEncryptTextBox.Text));
        }

        private void SetAllWidget() {
            SetLanguageComboBox();
            SetLogFormat();
            SetMinimumLargeFileSize();
            SetLargeFileType();
            SetSoftwareDatagrid();
            SetExtensionsDatagrid();
            SetPriorityDatagrid();
        }

        private void SetMinimumLargeFileSize() {
            MinimunSizeOfLargeFilesTextBox.Text = GetMinimumLargeFileSize();
        }

        private void SetLargeFileType() {
            if (GetLargeFileType() == "Go") {
                MinimunSizeOfLargeFilesComboBox.SelectedIndex = 0;
            }
            else if (GetLargeFileType() == "Mo") {
                MinimunSizeOfLargeFilesComboBox.SelectedIndex = 1;
            }
            else {
                MinimunSizeOfLargeFilesComboBox.SelectedIndex = 0;
            }
        }

        private string GetMinimumLargeFileSize() {
            return language.GetLargeFileSize();
        }

        private string GetLargeFileType() {
            return language.GetLargeFileType();
        }

        private void SetLogFormat() {
            switch (GetLogFormat()) {
                case "XML and JSON":
                    XMLCheckBox.IsChecked = true;
                    JSONCheckBox.IsChecked = true;
                    break;
                case "XML":
                    XMLCheckBox.IsChecked = true;
                    JSONCheckBox.IsChecked = false;
                    break;
                case "JSON":
                    XMLCheckBox.IsChecked = false;
                    JSONCheckBox.IsChecked = true;
                    break;
                case "none":
                    XMLCheckBox.IsChecked = false;
                    JSONCheckBox.IsChecked = false;
                    break;
                default:
                    XMLCheckBox.IsChecked = true;
                    JSONCheckBox.IsChecked = true;
                    break;
            }
        }

        private string GetLogFormat() {
            return language.GetLogFormat();
        }

        private void SetLanguageComboBox() {
            switch (language.GetLang()) {
                case "en-En":
                    LanguageComboBox.SelectedIndex = 0;
                    break;
                case "fr-Fr":
                    LanguageComboBox.SelectedIndex = 1;
                    break;
                default:
                    LanguageComboBox.SelectedIndex = 0;
                    break;
            }
        }

        public void SetLanguageDictionary() {
            dict.Source = new Uri(".\\Properties\\Dictionary-" + language.GetLang() + ".xaml", UriKind.Relative);
            this.Resources.MergedDictionaries.Add(dict);
        }

        private void SetAllDictionaries() {
            SetLanguageDictionary();
            ManageBackupJobsView.manageBackupJobsView.SetLanguageDictionary();
            RunBackupJobView.runBackupJobView.SetLanguageDictionary();
            NewBackupJobView.newBackupJobView.SetLanguageDictionary();
            RunBackupBtnView.runBackupBtnView.SetLanguageDictionary();
            SettingsBtnView.settingsBtnView.SetLanguageDictionary();
            ManageBackupBtnView.manageBackupBtnView.SetLanguageDictionary();
            NewBackupBtnView.newBackupBtnView.SetLanguageDictionary();
            if (mainWindow != null) {
                mainWindow.SetLanguageDictionary();
            }
        }

        private void SaveLanguage() {
            switch (LanguageComboBox.SelectedIndex) {
                case 0:
                    language.ChangeLanguageTo("en-En");
                    break;
                case 1:
                    language.ChangeLanguageTo("fr-Fr");
                    break;
                default:
                    language.ChangeLanguageTo("en-En");
                    break;
            }
        }

        private void SaveMinimumLargeFileSize() {
            minimumLargeFileSize = MinimunSizeOfLargeFilesTextBox.Text;
            string temp = "";

            for (int i = 0; i < minimumLargeFileSize.Length; i ++) {
                if (minimumLargeFileSize[i] != ' ') {
                    temp += minimumLargeFileSize[i];
                }
            }
            minimumLargeFileSize = temp;

            JObject jsonContent = new JObject(new JProperty("MinimumLargeFileSize", minimumLargeFileSize));

            string jsonString = jsonContent.ToString();
            File.WriteAllText(fileName + "LargeFileSize.json", jsonString);
        }

        private void SaveLargeFileType() {
            minimumLargeFileType = MinimunSizeOfLargeFilesComboBox.Text;

            JObject jsonContent = new JObject(new JProperty("MinimumLargeFileType", minimumLargeFileType));

            string jsonString = jsonContent.ToString();
            File.WriteAllText(fileName + "LargeFileType.json", jsonString);
        }

        private void SaveLogFormat() {
            XML = XMLCheckBox.IsChecked ?? false;
            JSON = JSONCheckBox.IsChecked ?? false;

            if (XML && JSON) {
                logFormat = "XML and JSON";
                JObject jsonContent = new JObject(new JProperty("LogFormat", logFormat));

                string jsonString = jsonContent.ToString();
                File.WriteAllText(fileName + "LogFormat.json", jsonString);
            }
            else if (XML) {
                logFormat = "XML";
                JObject jsonContent = new JObject(new JProperty("LogFormat", logFormat));

                string jsonString = jsonContent.ToString();
                File.WriteAllText(fileName + "LogFormat.json", jsonString);
            }
            else if (JSON) {
                logFormat = "JSON";
                JObject jsonContent = new JObject(new JProperty("LogFormat", logFormat));

                string jsonString = jsonContent.ToString();
                File.WriteAllText(fileName + "LogFormat.json", jsonString);
            }
            else {
                logFormat = "none";
                JObject jsonContent = new JObject(new JProperty("LogFormat", logFormat));

                string jsonString = jsonContent.ToString();
                File.WriteAllText(fileName + "LogFormat.json", jsonString);
            }
        }

        private void SaveAllSettings() {
            SaveLanguage();
            SaveLogFormat();
            SaveMinimumLargeFileSize();
            SaveLargeFileType();
        }

        private void MinimunSizeOfLargeFilesTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            DisableButton();
        }

        private void SoftwareExceptionsAddButton_Click(object sender, RoutedEventArgs e) {
            SofwareExceptionsDataGrid.Items.RemoveAt(0);
            List<SoftwareDatagrid> soft = new List<SoftwareDatagrid>();
            soft.Add(new SoftwareDatagrid() { Software = SofwareExceptionsTextBox.Text });
            SofwareExceptionsDataGrid.Items.Add(soft);
            SaveSoftware();
            SofwareExceptionsTextBox.Text = "";
        }

        private void PriorityFilesAddButton_Click(object sender, RoutedEventArgs e) {
            PriorityFilesDataGrid.Items.RemoveAt(0);
            List<SoftwareDatagrid> extension = new List<SoftwareDatagrid>();
            extension.Add(new SoftwareDatagrid() { Software = PriorityFilesTextBox.Text });
            PriorityFilesDataGrid.Items.Add(extension);
            SavePriority();
            PriorityFilesTextBox.Text = "";
        }

        private void ExtensionsToEncryptAddButton_Click(object sender, RoutedEventArgs e) {
            ExtensionsToEncryptDataGrid.Items.RemoveAt(0);
            List<ExtensionsDataGrid> extension = new List<ExtensionsDataGrid>();
            extension.Add(new ExtensionsDataGrid() { Extensions = ExtensionsToEncryptTextBox.Text });
            ExtensionsToEncryptDataGrid.Items.Add(extension);
            SaveExtensions();
            ExtensionsToEncryptTextBox.Text = "";
        }

        private void SaveParametersButton_Click(object sender, RoutedEventArgs e) {
            SaveAllSettings();
            SetAllDictionaries();
        }

        private void DisableButton() {
            bool isNumeric = int.TryParse(MinimunSizeOfLargeFilesTextBox.Text, out _);
            if(MinimunSizeOfLargeFilesTextBox.Text != null && isNumeric) {
                SaveParametersButton.IsEnabled = true;
            }
            else {
                SaveParametersButton.IsEnabled = false;
            }
        }
    }
}
