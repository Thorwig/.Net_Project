namespace EasySave {
    /// <summary>  
    ///  The CreatedBackupJobSelect class is used to show all the saved backup jobs's names to the user (view) and let him chose one (to replace).
    /// </summary> 
    public class CreatedBackupJobSelect {
		/// <summary> 
		/// This constructor method create an object createdBackupJobSelect from the class CreatedBackupJobSelect which is a Singleton. It will be used by all the other classe who wants to interract with the classe CreatedBackupJobSelect.
		/// </summary> 
		public static CreatedBackupJobSelect createdBackupJobSelect = new CreatedBackupJobSelect();

		/// <summary> 
		/// This is the private constructor of the CreatedBackupJobSelect class. It don't allows the other class to instanciate CreatedBackupJobSelect object.
		/// </summary> 
		private CreatedBackupJobSelect() { }

		public void StartBackup(int id) {
			BackupJobs.backupJobs.LaunchThisBackupJob(id);
		}

		public string[] GetArrayOfExistingBackupJobs() {
			return BackupJobs.backupJobs.GetArrayBackupJobName();
		}
	}
}
