using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace JIAOFENG.Practices.Logic.Log
{
    public abstract class LogTarget : ILogTarget
    {
        public void LogError(string title, string action, string targetCode, string targetName, int categoryID, string categoryName, int userID, string userName)
        {
            WriteEntry(title,action,targetCode,targetName, EventType.Error, categoryID,categoryName, userID, userName);
        }
        public void LogInformation(string title, string action, string targetCode, string targetName, int categoryID, string categoryName, int userID, string userName)
        {
            WriteEntry(title,action, targetCode, targetName, EventType.Information, categoryID, categoryName, userID, userName);
        }
        public void LogSuccessAudit(string title, string action, string targetCode, string targetName, int categoryID, string categoryName, int userID, string userName)
        {
            WriteEntry(title,action, targetCode, targetName, EventType.SuccessAudit, categoryID, categoryName, userID, userName);
        }
        public void LogFailureAudit(string title, string action, string targetCode, string targetName, int categoryID, string categoryName, int userID, string userName)
        {
            WriteEntry(title,action, targetCode, targetName, EventType.FailureAudit, categoryID, categoryName, userID, userName);
        }
        public void LogWarning(string title, string action, string targetCode, string targetName, int categoryID, string categoryName, int userID, string userName)
        {
            WriteEntry(title, action, targetCode, targetName, EventType.Warning, categoryID, categoryName, userID, userName);
        }
        public bool Verbose { get; set; }
        public void LogDebug(string title, string action, string targetCode, string targetName, int categoryID, string categoryName, int userID, string userName)
        {
            if (Verbose)
            {
                WriteEntry(title, action, targetCode, targetName, EventType.Debug, categoryID, categoryName, userID, userName);
            }
        }
        protected abstract void WriteEntry(string title, string action, string targetCode, string targetName, EventType logEntryType, int categoryID, string categoryName, int userID, string userName);
        public abstract List<LogData> GetLogs(string targetCode, EventType? logEntryType, int? categoryID, int? userID, DateTime? dtStart, DateTime? dtEnd);
        
    }
}
