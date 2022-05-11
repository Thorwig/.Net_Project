using System;
using System.Windows;
using System.Windows.Controls;

namespace EasySave {
    /// <summary>
    /// Logique d'interaction pour NewBackupBtnView.xaml
    /// </summary>
    public partial class NewBackupBtnView : Page {
        public static NewBackupBtnView newBackupBtnView = new NewBackupBtnView();
        private Language language = new Language();
        private ResourceDictionary dict = new ResourceDictionary();

        private NewBackupBtnView() {
            InitializeComponent();
            SetLanguageDictionary();
        }

        public void SetLanguageDictionary() {
            dict.Source = new Uri(".\\Properties\\Dictionary-" + language.GetLang() + ".xaml", UriKind.Relative);
            this.Resources.MergedDictionaries.Add(dict);
        }
    }
}
