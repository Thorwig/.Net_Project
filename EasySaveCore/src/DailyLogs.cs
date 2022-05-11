using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace EasySave {
    /// <summary>  
    ///  The DailyLogs class is used to write daily logs, which are set to stay forever.
    /// </summary> 
    public class DailyLogs {
		/// <summary> 
		/// This constructor method create an object dailyLogs from the class DailyLogs which is a Singleton. It will be used by all the other classe who wants to interract with the classe DailyLogs.
		/// </summary> 
		public static DailyLogs dailyLogs = new DailyLogs();

		/// <summary> 
		/// This is the private constructor of the DailyLogs class. It don't allows the other class to instanciate DailyLogs object.
		/// </summary> 
		private DailyLogs() { }

		private string filePath = "..\\..\\..\\..\\EasySaveCore\\assets\\logs\\", fileName;

		public void WriteDailyLog(string[] informations) {

			JObject jsonContent = new JObject(new JProperty("Timestamp", informations[0]),
											  new JProperty("Name", informations[1]),
											  new JProperty("Source Path", informations[2]),
											  new JProperty("Destination Path", informations[3]),
											  new JProperty("Files size", informations[4]),
											  new JProperty("Save duration", informations[5]),
											  new JProperty("Encryption duration", informations[6]));

			string jsonString = "[\n";

			if (!File.Exists(filePath + informations[0] + ".json")) {
				jsonString += jsonContent.ToString();
				jsonString += "\n]";
			}
			else {
				JArray currentJsonArray = JArray.Parse(File.ReadAllText(filePath + informations[0] + ".json"));
				currentJsonArray.Add(jsonContent);
				jsonString = currentJsonArray.ToString();
			}

			using (StreamWriter newFile = File.CreateText(filePath + informations[0] + ".json")) {
				newFile.Write(jsonString);
			}
			// File.WriteAllText(filePath + informations[0] + ".json", jsonString);
			fileName = filePath + informations[0];
			WriteDailyLogXML();
		}

		private void WriteDailyLogXML() {
			string jsonLogContent = File.ReadAllText(fileName + ".json");
			string xmlFile = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(jsonLogContent), new XmlDictionaryReaderQuotas())).ToString();
			using (StreamWriter newFile = File.CreateText(fileName + ".xml")) {
				newFile.Write(xmlFile);
			}
		}
	}
}
