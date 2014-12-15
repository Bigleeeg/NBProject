using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JIAOFENG.Practices.Logic.Log
{
    public interface ILogTarget
    {
        void LogError(string title, string action, string targetCode, string targetName, int categoryID, string categoryName, int userID, string userName);
        void LogInformation(string title, string action, string targetCode, string targetName, int categoryID, string categoryName, int userID, string userName);
        void LogSuccessAudit(string title, string action, string targetCode, string targetName, int categoryID, string categoryName, int userID, string userName);
        void LogFailureAudit(string title, string action, string targetCode, string targetName, int categoryID, string categoryName, int userID, string userName);
        void LogWarning(string title, string action, string targetCode, string targetName, int categoryID, string categoryName, int userID, string userName);
        bool Verbose { get; set; }
        void LogDebug(string title, string action, string targetCode, string targetName, int categoryID, string categoryName, int userID, string userName);
        List<LogData> GetLogs(string targetCode, EventType? logEntryType, int? categoryID, int? userID, DateTime? dtStart, DateTime? dtEnd);
    }
}
