using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EasySave {
    /// <summary>  
    ///  The BackupJobCreate class is used to create backup jobs and writes them down in a JSON file whose index all the user's backup jobs.
    /// </summary> 
    public class BackupJobCreate {
		/// <summary> 
		/// This constructor method create an object backupJobCreate from the class BackupJobCreate which is a Singleton. It will be used by all the other classe who wants to interract with the classe BackupJobCreate.
		/// </summary> 
		public static BackupJobCreate backupJobCreate = new BackupJobCreate();

		/// <summary> 
		/// This is the private constructor of the BackupJobCreate class. It don't allows the other class to instanciate BackupJobCreate object.
		/// </summary> 
		private BackupJobCreate() { }

		public void CreateBackupJob(string[] parameters) {
			JObject jsonContent = new JObject(new JProperty("Type", parameters[0]),
											  new JProperty("SourcePath", parameters[1]),
											  new JProperty("DestinationPath", parameters[2]),
											  new JProperty("Name", parameters[3]),
											  new JProperty("IsEncrypted", parameters[4]));

			string jsonString = "[\n";

			if (BackupJobs.backupJobs.GetcountBackupJobs() == 0) {
				jsonString += jsonContent.ToString();
				jsonString += "\n]";
			}
			else {
				JArray currentJsonArray = JArray.Parse(File.ReadAllText(BackupJobs.backupJobs.GetfileName()));
				currentJsonArray.Add(jsonContent);
				jsonString = currentJsonArray.ToString();
			}

			File.WriteAllText(BackupJobs.backupJobs.GetfileName(), jsonString);
		}

		public void DeleteBackupJob(int backupJobId) {
			List<string> linesList = File.ReadAllLines(BackupJobs.backupJobs.GetfileName()).ToList();
			for (int j = 0; j < 7; j++) {
				linesList.RemoveAt(1 + (7 * backupJobId));
			}
			File.WriteAllLines(BackupJobs.backupJobs.GetfileName(), linesList.ToArray());
		}
	}
}
