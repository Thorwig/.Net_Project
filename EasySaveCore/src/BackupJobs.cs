using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace EasySave {
    /// <summary>  
    ///  The BackupJobs class is used to get informations about the current saved backup jobs.
    /// </summary> 
    public class BackupJobs {
		/// <summary> 
		/// This constructor method create an object backupJobs from the class BackupJobs which is a Singleton. It will be used by all the other classe who wants to interract with the classe BackupJobs.
		/// </summary> 
		public static BackupJobs backupJobs = new BackupJobs();
		private string fileName = "..\\..\\..\\..\\EasySaveCore\\assets\\system\\BackupJobs.json";

		/// <summary> 
		/// This is the private constructor of the BackupJobs class. It don't allows the other class to instanciate BackupJobs object.
		/// </summary> 
		private BackupJobs() { }

		public string GetfileName() {
			return fileName;
		}

		public int GetcountBackupJobs() {
			if (File.Exists(GetfileName()) && File.ReadAllText(GetfileName()).Length > 0) {
				JArray currentJsonArray = JArray.Parse(File.ReadAllText(GetfileName()));
				return currentJsonArray.Count;
			}
			else {
				return 0;
            }
		}

		public string[] GetArrayBackupJobName () {
			var backupJobsList = new List<string>();
			JArray backupJson = JArray.Parse(File.ReadAllText(GetfileName()));
			for (int loop = 0; loop < GetcountBackupJobs(); loop++) {
                backupJobsList.Add(backupJson[loop]["Name"].ToString());
			}
			string[] backupJobsArray = backupJobsList.ToArray();
			return backupJobsArray;
        }

		public string[] GetArrayBackupJobType() {
			var backupJobsList = new List<string>();
			JArray backupJson = JArray.Parse(File.ReadAllText(GetfileName()));
			for (int loop = 0; loop < GetcountBackupJobs(); loop++) {
				backupJobsList.Add(backupJson[loop]["Type"].ToString());
			}
			string[] backupJobsArray = backupJobsList.ToArray();
			return backupJobsArray;
		}

		public string[] GetArrayBackupJobSource() {
			var backupJobsList = new List<string>();
			JArray backupJson = JArray.Parse(File.ReadAllText(GetfileName()));
			for (int loop = 0; loop < GetcountBackupJobs(); loop++) {
				backupJobsList.Add(backupJson[loop]["SourcePath"].ToString());
			}
			string[] backupJobsArray = backupJobsList.ToArray();
			return backupJobsArray;
		}

		public string[] GetArrayBackupJobDestination() {
			var backupJobsList = new List<string>();
			JArray backupJson = JArray.Parse(File.ReadAllText(GetfileName()));
			for (int loop = 0; loop < GetcountBackupJobs(); loop++) {
				backupJobsList.Add(backupJson[loop]["DestinationPath"].ToString());
			}
			string[] backupJobsArray = backupJobsList.ToArray();
			return backupJobsArray;
		}

		public string[] GetArrayBackupJobEncrypted() {
			var backupJobsList = new List<string>();
			JArray backupJson = JArray.Parse(File.ReadAllText(GetfileName()));
			for (int loop = 0; loop < GetcountBackupJobs(); loop++) {
				backupJobsList.Add(backupJson[loop]["IsEncrypted"].ToString());
			}
			string[] backupJobsArray = backupJobsList.ToArray();
			return backupJobsArray;
		}

		public void LaunchThisBackupJob(int id) {
			LaunchBackupJob.launchBackupJob.LaunchThisBackupJob(id);
		}
	}
}
