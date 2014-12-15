using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using JIAOFENG.Practices.Library.FileIO;

namespace JIAOFENG.Practices.Logic.Log
{
    public class FileAdapter : LogTarget
    {
        public string logFileFullName { get; private set; }
        private FileOperator fileIO;
        public FileAdapter():this(null)
        {
        }
        public FileAdapter(string logFileFullName)
        {
            if (string.IsNullOrWhiteSpace(logFileFullName))
            {
                logFileFullName = Path.Combine(System.Environment.CurrentDirectory, "Log\\log.txt");
            }
            this.logFileFullName = logFileFullName;
            FileInfo file = new FileInfo(this.logFileFullName);
            this.fileIO = new FileOperator(file);
        }
        protected override void WriteEntry(string title, string action, string targetCode, string targetName, EventType logEntryType, int categoryID, string categoryName, int userID, string userName)
        {
            const string stringFormat = "{0}!|!{1}!|!{2}!|!{3}!|!{4}!|!{5}!|!{6}|!{7}|!{8}|!{9}";
            string data = string.Format(stringFormat, title, action, targetCode, targetName, (int)logEntryType, categoryID, categoryName, userID, userName, DateTime.Now.ToString());
            fileIO.WriteLine(data);
        }

        public override List<LogData> GetLogs(string targetCode, EventType? logEntryType, int? categoryID, int? userID, DateTime? dtStart, DateTime? dtEnd)
        {
            List<LogData> logs = new List<LogData>();
            return logs;
        }
    }
}
