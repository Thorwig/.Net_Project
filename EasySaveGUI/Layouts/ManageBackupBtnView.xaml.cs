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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySave.Layouts
{
    /// <summary>
    /// Logique d'interaction pour ManageBackupBtnView.xaml
    /// </summary>
    public partial class ManageBackupBtnView : Page {
        public static ManageBackupBtnView manageBackupBtnView = new ManageBackupBtnView();
        private Language language = new Language();
        private ResourceDictionary dict = new ResourceDictionary();

        private ManageBackupBtnView() {
            InitializeComponent();
            SetLanguageDictionary();
        }

        public void SetLanguageDictionary() {
            dict.Source = new Uri(".\\Properties\\Dictionary-" + language.GetLang() + ".xaml", UriKind.Relative);
            this.Resources.MergedDictionaries.Add(dict);
        }
    }
}
