namespace EasySave {
    /// <summary>  
    ///  The SequencialBackupJob class is used to run all the backup jobs in sequence.
    /// </summary> 
    public class SequencialBackupJob {
		/// <summary> 
		/// This constructor method create an object sequencialBackupJob from the class SequencialBackupJob which is a Singleton. It will be used by all the other classe who wants to interract with the classe SequencialBackupJob.
		/// </summary> 
		public static SequencialBackupJob sequencialBackupJob = new SequencialBackupJob();

		/// <summary> 
		/// This is the private constructor of the SequencialBackupJob class. It don't allows the other class to instanciate SequencialBackupJob object.
		/// </summary> 
		private SequencialBackupJob() { }

		public void LaunchAllBackupSequentially() {
			LaunchBackupJob.launchBackupJob.LaunchAllBackupJobs();
		}
	}
}
