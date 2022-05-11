using Newtonsoft.Json;
using System.IO;
using System.Xml.Linq;

namespace EasySave {
    /// <summary>  
    ///  The StateLogs class is used to write state log, which is set to be updated each time a file is saved. This is a buffer log.
    /// </summary> 
    public class StateLogs {
		/// <summary> 
		/// This constructor method create an object stateLogs from the class StateLogs which is a Singleton. It will be used by all the other classe who wants to interract with the classe StateLogs.
		/// </summary> 
		public static StateLogs stateLogs = new StateLogs();

		/// <summary> 
		/// This is the private constructor of the StateLogs class. It don't allows the other class to instanciate StateLogs object.
		/// </summary> 
		private StateLogs() { }

		private string fileName;

		public void WriteStateLog(string[] informations) {
			string fileContent = "{\n";
			if (informations.Length == 3) {
				fileContent += "   \"Name\": " + "\"" + informations[0] + "\",\n" +
								 "   \"Timestamp\": " + "\"" + informations[1] + "\",\n" +
								 "   \"Backup state\": " + "\"" + informations[2] + "\"\n";

			}
			else {
				fileContent += "   \"Name\": " + "\"" + informations[0] + "\",\n" +
								 "   \"Timestamp\": " + "\"" + informations[1] + "\",\n" +
								 "   \"Backup state\": " + "\"" + informations[2] + "\",\n" +
								 "   \"Number of files\": " + "\"" + informations[3] + "\",\n" +
								 "   \"Files size\": " + "\"" + informations[4] + "\",\n" +
								 "   \"Number of files left\": " + "\"" + informations[5] + "\",\n" +
								 "   \"Source path\": " + "\"" + informations[6].Replace("\\", "\\\\") + "\",\n" +
								 "   \"Destination path\": " + "\"" + informations[7].Replace("\\", "\\\\") + "\"\n";
			}
			fileContent += "}";
			fileName = LogsWriter.logsWriter.GetlogsFileName();

			using (StreamWriter newFile = File.CreateText(fileName + "State log.json")) {
				newFile.Write(fileContent.ToString());
			}
			WriteStateLogXml();
		}

		private void WriteStateLogXml() {
			string jsonLogContent = File.ReadAllText(fileName + "State log.json");
			XNode node = JsonConvert.DeserializeXNode(jsonLogContent, "Root");
			using (StreamWriter newFile = File.CreateText(fileName + "State log.xml")) {
				newFile.Write(node.ToString());
			}
		}
	}
}
