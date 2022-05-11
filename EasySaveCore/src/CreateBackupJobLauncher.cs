using System;
using System.IO;

namespace EasySave {
    /// <summary>  
    ///  The CreateBackupJobLauncher class is used to transmit the informations from the user (view) to the BackupJobCreate class.
    ///  It will also overwrite a backup job in the JSON file to replace it if the user already has five saved.
    /// </summary> 
    public class CreateBackupJobLauncher {
		/// <summary> 
		/// This constructor method create an object createBackupJobLauncher from the class CreateBackupJobLauncher which is a Singleton. It will be used by all the other classe who wants to interract with the classe CreateBackupJobLauncher.
		/// </summary> 
		public static CreateBackupJobLauncher createBackupJobLauncher = new CreateBackupJobLauncher();

		/// <summary> 
		/// This is the private constructor of the CreateBackupJobLauncher class. It don't allows the other class to instanciate CreateBackupJobLauncher object.
		/// </summary>
        ///  
		private CreateBackupJobLauncher() { }

		public void CreateBackupJob(string[] parameters) {
			BackupJobCreate.backupJobCreate.CreateBackupJob(parameters);
		}
/*
        private void ReplaceBackupJob(string[] parameters) {
            try {
                int id = 0;// Int32.Parse(CommandController.commandController.SelectBackupJobToDelete(BackupJobs.backupJobs.GetArrayBackupJob()))-1;
                string[] jsonString = File.ReadAllLines(BackupJobs.backupJobs.GetfileName());
                string newString = "";
                for (int loop = 0; loop < 2 + 6 * id; loop++) {
                    newString += jsonString[loop].ToString() + "\n";
                }
                string sourcePath = parameters[1];
                string goodFormatSourcePath = sourcePath.Replace("\\", "\\\\");
                string destPath = parameters[2];
                string goodFormatDestPath = destPath.Replace("\\", "\\\\");
                newString += "    \"Type\": " + "\"" + parameters[0] + "\"," + "\n";
                newString += "    \"SourcePath\": " + "\"" + goodFormatSourcePath + "\"," + "\n";
                newString += "    \"DestinationPath\": " + "\"" + goodFormatDestPath + "\"," + "\n";
                newString += "    \"Name\": " + "\"" + parameters[3] + "\"" + "\n";
                for (int loop = 2 + 6 * id + 4; loop < jsonString.Length; loop++) {
                    newString += jsonString[loop].ToString() + "\n";
                }
                using (StreamWriter newFile = File.CreateText(BackupJobs.backupJobs.GetfileName())) {
                    newFile.Write(newString);
                }
                LaunchBackupJob.launchBackupJob.LaunchThisBackupJob(id);
            }
            catch (FormatException) {
                // CommandController.commandController.RequestFatalError(1);
            }
        }*/
    }
}
