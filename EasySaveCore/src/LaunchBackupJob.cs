using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace EasySave {
    /// <summary>  
    ///  The LaunchBackupJob class is used to run saved backup jobs (either one or all of them in sequence).
    /// </summary> 
    public class LaunchBackupJob {
		/// <summary> 
		/// This constructor method create an object launchBackupJob from the class LaunchBackupJob which is a Singleton. It will be used by all the other classe who wants to interract with the classe LaunchBackupJob.
		/// </summary> 
		public static LaunchBackupJob launchBackupJob = new LaunchBackupJob();
		private int totalSizeFiles, currentIndexFile, numberOfFiles;
		private string extensionToEncrypt;
		public int Pourcentage;
		public int FilesNumber = 0;
		private bool _runThread = true;
		private bool _pauseThread = false;
		private int currentFile;
		private DateTime startTimer;
		private string currentSourcePath, currentDestPath, encryption, encryptionTimer, cryptoSoftFileName = "..\\..\\..\\..\\CryptoSoft\\bin\\Release\\net5.0\\CryptoSoft.exe";

		/// <summary> 
		/// This is the private constructor of the LaunchBackupJob class. It don't allows the other class to instanciate LaunchBackupJob object.
		/// </summary> 
		private LaunchBackupJob () { }

		public void SetExtensionsToEncrypt(string extension) {
			extensionToEncrypt = extension;
        }

		private void CountNumberOfFiles(string path) {
			if (Directory.Exists(path)) {
				string[] files = Directory.GetFiles(path);
				FilesNumber += files.Count();
				if (Directory.GetDirectories(path) != null) {
					string[] folders = Directory.GetDirectories(path);
					for (int loop = 0; loop < folders.Length; loop++) {
						CountNumberOfFiles(folders[loop]);
					}
				}
			}
		}

		public void StartDifferentialBackup(Newtonsoft.Json.Linq.JToken parameters) {
			Thread.Sleep(200);
			try {
				Stopwatch timer = new Stopwatch();
				string fileName, destFile;
				if (Directory.Exists(parameters["SourcePath"].ToString())) {
					string[] files = Directory.GetFiles(parameters["SourcePath"].ToString());
					foreach (string s in files) {
						if (_runThread) {
							CheckIfThreadIsPaused();
							fileName = Path.GetFileName(s);
							destFile = Path.Combine(parameters["DestinationPath"].ToString(), fileName);
							currentFile++;
							Pourcentage = currentFile;
							if (!File.Exists(destFile) && File.GetLastWriteTimeUtc(destFile) != File.GetLastWriteTimeUtc(s)) {
								File.Copy(s, destFile, true);
								if (encryption == "Yes" && destFile.Substring(destFile.LastIndexOf('.') + 1) == extensionToEncrypt) {
									timer.Start();
									string[] encryptionParameters = { destFile, "010" };
									Process.Start(cryptoSoftFileName, encryptionParameters);
									timer.Stop();
								}
								currentIndexFile++;
								if (currentIndexFile != numberOfFiles) {
									string[] dataForStateLogs = { parameters["Name"].ToString(), DateTime.Now.ToString(), "Active", numberOfFiles.ToString(), totalSizeFiles.ToString(), (numberOfFiles - currentIndexFile).ToString(), currentSourcePath, currentDestPath };
									LogsWriter.logsWriter.WriteStateLog(dataForStateLogs);
								}
								else {
									string[] dataForStateLogs = { parameters["Name"].ToString(), DateTime.Now.ToString(), "Inactive" };
									LogsWriter.logsWriter.WriteStateLog(dataForStateLogs);
								}
							}
						}
					}
					if (Directory.GetDirectories(parameters["SourcePath"].ToString()) != null) {
						string[] folders = Directory.GetDirectories(parameters["SourcePath"].ToString());
						for (int loop = 0; loop < folders.Length; loop++) {
							parameters["SourcePath"] = folders[loop];
							string[] folderNameArray = folders[loop].Split('\\');
							string folderName = folderNameArray[^1];
							Directory.CreateDirectory(parameters["DestinationPath"].ToString() + "\\" + folderName);
							parameters["DestinationPath"] += "\\" + folderName;
							StartDifferentialBackup(parameters);
						}
					}
				}
				encryptionTimer = timer.Elapsed.ToString();
			}
			catch (Exception) {
				string[] dataForDailyLogsIfError = {DateTime.Now.ToString("dd-MM-yyyy").Replace("/", "-"), parameters["Name"].ToString(), currentSourcePath, currentDestPath, totalSizeFiles.ToString(), (-1).ToString(), encryptionTimer };
				LogsWriter.logsWriter.WriteDailyLog(dataForDailyLogsIfError);
			}
			string[] dataForDailyLogs = {DateTime.Now.ToString("dd-MM-yyyy").Replace("/", "-"), parameters["Name"].ToString(), currentSourcePath, currentDestPath, totalSizeFiles.ToString(), (DateTime.Now - startTimer).ToString(), encryptionTimer };
			LogsWriter.logsWriter.WriteDailyLog(dataForDailyLogs);
			currentFile = 0;
			FilesNumber = 0;
		}

		private void CheckIfThreadIsPaused() {
			while(_pauseThread) {
				Thread.Sleep(200);
            }
        }

		public void ResumeBackup() {
			_pauseThread = false;
        }

		public void PauseBackup() {
			_pauseThread = true;
        }

		public void StopBackup() {
			_runThread = false;
        }

		public void SetRunBackupTrue() {
			_runThread = true;
        }

		public void StartFullBackup(Newtonsoft.Json.Linq.JToken parameters) {
			Thread.Sleep(200);
			try {
				Stopwatch timer = new Stopwatch();
				string fileName, destFile;
				if (Directory.Exists(parameters["SourcePath"].ToString())) {
					string[] files = Directory.GetFiles(parameters["SourcePath"].ToString());
					foreach (string s in files) {
						if (_runThread) {
							CheckIfThreadIsPaused();
							fileName = Path.GetFileName(s);
							destFile = Path.Combine(parameters["DestinationPath"].ToString(), fileName);
							currentFile++;
							Pourcentage = currentFile;
							File.Copy(s, destFile, true);
							if (encryption == "Yes" && destFile.Substring(destFile.LastIndexOf('.') + 1) == extensionToEncrypt) {
								timer.Start();
								string[] encryptionParameters = { destFile, "010" };
								Process.Start(cryptoSoftFileName, encryptionParameters);
								timer.Stop();
							}
							currentIndexFile++;
							if (currentIndexFile != numberOfFiles) {
								string[] dataForStateLogs = { parameters["Name"].ToString(), DateTime.Now.ToString(), "Active", numberOfFiles.ToString(), totalSizeFiles.ToString(), (numberOfFiles - currentIndexFile).ToString(), currentSourcePath, currentDestPath };
								LogsWriter.logsWriter.WriteStateLog(dataForStateLogs);
							}
							else {
								string[] dataForStateLogs = { parameters["Name"].ToString(), DateTime.Now.ToString(), "Inactive" };
								LogsWriter.logsWriter.WriteStateLog(dataForStateLogs);
							}
						}
					}
					if (Directory.GetDirectories(parameters["SourcePath"].ToString()) != null)
					{
						string[] folders = Directory.GetDirectories(parameters["SourcePath"].ToString());
						for (int loop = 0; loop < folders.Length; loop++)
						{
							parameters["SourcePath"] = folders[loop];
							string[] folderNameArray = folders[loop].Split('\\');
							string folderName = folderNameArray[^1];
							Directory.CreateDirectory(parameters["DestinationPath"].ToString() + "\\" + folderName);
							parameters["DestinationPath"] += "\\" + folderName;
							StartFullBackup(parameters);
						}
					}
				}
				encryptionTimer = timer.Elapsed.ToString();
			}
			catch (Exception)
			{
				string[] dataForDailyLogsIfError = { DateTime.Now.ToString("dd-MM-yyyy").Replace("/", "-"), parameters["Name"].ToString(), currentSourcePath, currentDestPath, totalSizeFiles.ToString(), (-1).ToString(), encryptionTimer };
				LogsWriter.logsWriter.WriteDailyLog(dataForDailyLogsIfError);
			}
			string[] dataForDailyLogs = { DateTime.Now.ToString("dd-MM-yyyy").Replace("/", "-"), parameters["Name"].ToString(), currentSourcePath, currentDestPath, totalSizeFiles.ToString(), (DateTime.Now - startTimer).ToString(), encryptionTimer };
			LogsWriter.logsWriter.WriteDailyLog(dataForDailyLogs);
			currentFile = 0;
			FilesNumber = 0;
		}

		public void LaunchThisBackupJob(int id) {
			JArray currentJsonArray = JArray.Parse(File.ReadAllText(BackupJobs.backupJobs.GetfileName()));
			DirectoryInfo di = new DirectoryInfo(currentJsonArray[id]["SourcePath"].ToString());
			totalSizeFiles = (int)di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
			numberOfFiles = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Count();
			currentIndexFile = 0;
			startTimer = DateTime.Now;
			currentDestPath = currentJsonArray[id]["DestinationPath"].ToString();
			currentSourcePath = currentJsonArray[id]["SourcePath"].ToString();
			encryption = currentJsonArray[id]["IsEncrypted"].ToString();
			if (currentJsonArray[id]["Type"].ToString() == "Differential") {
				CountNumberOfFiles(currentSourcePath);
				StartDifferentialBackup(currentJsonArray[id]);
			}
			else {
				CountNumberOfFiles(currentSourcePath);
				StartFullBackup(currentJsonArray[id]);
			}
		}

		public void LaunchAllBackupJobs() {
			for (int loop = 0; loop < BackupJobs.backupJobs.GetcountBackupJobs(); loop++) {
				LaunchThisBackupJob(loop);
            }
        }
	}
}
