using EasySave.View;
using Newtonsoft.Json.Linq;
using System.IO;

namespace EasySave {
    public class Language {
        private readonly string fileNameLanguage = "..\\..\\..\\..\\EasySaveCore\\assets\\system\\Lang.json";
        private readonly string fileNameLogFormat = "..\\..\\..\\..\\EasySaveCore\\assets\\system\\LogFormat.json";
        private readonly string fileNameLargeFileSize = "..\\..\\..\\..\\EasySaveCore\\assets\\system\\LargeFileSize.json";
        private readonly string fileNameLargeFileType = "..\\..\\..\\..\\EasySaveCore\\assets\\system\\LargeFileType.json";
        private readonly string fileNameSoftware = "..\\..\\..\\..\\EasySaveCore\\assets\\system\\SoftwareExceptions";
        private readonly string fileNameExtensions = "..\\..\\..\\..\\EasySaveCore\\assets\\system\\ExtensionsToEncrypt.json";
        private readonly string fileNamePriority = "..\\..\\..\\..\\EasySaveCore\\assets\\system\\PriorityFile.json";
        private string lang, minimumLargeFileSize, minimumLargeFileType, logFormat, software, extensions, priority;

        public string Lang { get; set; }
        public string MinimumLargeFileSize { get; set; }
        public string SoftwareExceptions { get; set; }
        public string LogFormat { get; set; }

        public string GetLang() {
            if (!File.Exists(fileNameLanguage)) {
                return "en-En";
            }
            else {
                lang = System.Text.Json.JsonSerializer.Deserialize<Language>
                                                             (File.ReadAllText(fileNameLanguage))
                                                             .Lang;
                if (lang != null) {
                    return lang;
                }
                else {
                    return "en-En";
                }
            }
        }

        public string GetSoftware() {
            if (!File.Exists(fileNameSoftware + ".json")) {
                return "";
            }
            else {
                return File.ReadAllText(fileNameSoftware + ".json").Replace("\"", "");
            }
        }

        public string GetPriority() {
            if (!File.Exists(fileNamePriority)) {
                return "";
            }
            else {
                return File.ReadAllText(fileNamePriority).Replace("\"", "");
            }

        }

        public string GetExtensions() {
            if (!File.Exists(fileNameExtensions)) {
                return "";
            }
            else {
                extensions = File.ReadAllText(fileNameExtensions).Replace(".", "").Replace("\"", "");
                return extensions;
            }
        }

        public string GetLargeFileType() {
            if (!File.Exists(fileNameLargeFileType)) {
                return "Go";
            }
            else {
                minimumLargeFileSize = System.Text.Json.JsonSerializer.Deserialize<Language>
                                                             (File.ReadAllText(fileNameLargeFileSize))
                                                             .MinimumLargeFileSize;
                return minimumLargeFileSize ?? "Go";
            }
        }

        public string GetLargeFileSize() {
            if (!File.Exists(fileNameLargeFileSize)) {
                return "1";
            }
            else {
                minimumLargeFileSize = System.Text.Json.JsonSerializer.Deserialize<Language>
                                                             (File.ReadAllText(fileNameLargeFileSize))
                                                             .MinimumLargeFileSize;
                return minimumLargeFileSize ?? "1";
            }
        }

        public string GetLogFormat() {
            if (!File.Exists(fileNameLogFormat)) {
                return "XML and JSON";
            }
            else {
                logFormat = System.Text.Json.JsonSerializer.Deserialize<Language>
                                                             (File.ReadAllText(fileNameLogFormat)).LogFormat;
                return logFormat ?? "XML and JSON";
            }
        }

        public void ChangeLanguageTo(string lang) {
            JObject jsonContent = new JObject(new JProperty("Lang", lang));

            string jsonString = jsonContent.ToString();
            File.WriteAllText(fileNameLanguage, jsonString);
        }
    }
}
