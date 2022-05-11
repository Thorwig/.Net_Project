namespace EasySave {
    /// <summary>  
    ///  The LogsWriter class is used to Write state and daily logs.
    /// </summary> 
    public class LogsWriter {
		/// <summary> 
		/// This constructor method create an object logsWriter from the class LogsWriter which is a Singleton. It will be used by all the other classe who wants to interract with the classe LogsWriter.
		/// </summary> 
		public static LogsWriter logsWriter = new LogsWriter();
		private string logsFolderName = "..\\..\\..\\..\\EasySaveCore\\assets\\logs\\";

		public string GetlogsFileName() {
			return logsFolderName;
        }

		/// <summary> 
		/// This is the private constructor of the LogsWriter class. It don't allows the other class to instanciate LogsWriter object.
		/// </summary> 
		private LogsWriter() { }

		public void WriteStateLog(string[] informations) {
			StateLogs.stateLogs.WriteStateLog(informations);
		}

		public void WriteDailyLog(string[] informations) {
			DailyLogs.dailyLogs.WriteDailyLog(informations);
		}
	}
}
