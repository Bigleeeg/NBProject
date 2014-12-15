using System;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;

namespace JIAOFENG.Practices.Logic.Log
{
    /// <summary>
    /// Summary description for ATSServerLogManager.
    /// </summary>
    public static class LogManager
    {
        private const int DefaultUserID = 0;
        private const string DefaultUserName = "System";
        private const int DefaultCategoryID = 0;
        private const string DefaultCategoryName = "Unclassified";
        
        private static List<ILogTarget> logTargets = new List<ILogTarget>();
        static LogManager()
        {
            LogSettingsSectionHandler section = (LogSettingsSectionHandler)ConfigurationManager.GetSection(Configuration.LogSectionName);
            foreach (AdapterElement element in section.Adapters)
            {
                logTargets.Add(element.CreateLogTarget());
            }
        }
 
        public static void Add(ILogTarget log)
        {
            logTargets.Add(log);
        }
        public static void LogError(string title, string action, string targetCode, string targetName, int categoryID = DefaultCategoryID, string categoryName = DefaultCategoryName, int userID = DefaultUserID, string userName = DefaultUserName)
        {
            foreach (ILogTarget log in logTargets)
            {
                log.LogError(title, action, targetCode, targetName, categoryID, categoryName, userID, userName);
            }
        }

        public static void LogSuccessAudit(string title, string action, string targetCode, string targetName, int categoryID = DefaultCategoryID, string categoryName = DefaultCategoryName, int userID = DefaultUserID, string userName = DefaultUserName)
        {
            foreach (ILogTarget log in logTargets)
            {
                log.LogSuccessAudit(title,action, targetCode, targetName, categoryID, categoryName, userID, userName);
            }
        }

        public static void LogFailureAudit(string title, string action, string targetCode, string targetName, int categoryID = DefaultCategoryID, string categoryName = DefaultCategoryName, int userID = DefaultUserID, string userName = DefaultUserName)
        {
            foreach (ILogTarget log in logTargets)
            {
                log.LogFailureAudit(title, action, targetCode, targetName, categoryID, categoryName, userID, userName);
            }
        }

        public static void LogWarning(string title, string action, string targetCode, string targetName, int categoryID = DefaultCategoryID, string categoryName = DefaultCategoryName, int userID = DefaultUserID, string userName = DefaultUserName)
        {
            foreach (ILogTarget log in logTargets)
            {
                log.LogWarning(title, action, targetCode, targetName, categoryID, categoryName, userID, userName);
            }
        }

        public static void LogInformation(string title, string action, string targetCode, string targetName, int categoryID = DefaultCategoryID, string categoryName = DefaultCategoryName, int userID = DefaultUserID, string userName = DefaultUserName)
        {
            foreach (ILogTarget log in logTargets)
            {
                log.LogInformation(title, action, targetCode, targetName, categoryID, categoryName, userID, userName);
            }
        }

        public static void LogDebug(string title, string action, string targetCode, string targetName, int categoryID = DefaultCategoryID, string categoryName = DefaultCategoryName, int userID = DefaultUserID, string userName = DefaultUserName)
        {
            foreach (ILogTarget log in logTargets)
            {
                log.LogDebug(title, action, targetCode, targetName, categoryID, categoryName, userID, userName);
            }
        }

        public static List<LogData> GetLogs(string targetCode, EventType? logEntryType, int? categoryID, int? userID, DateTime? dtStart, DateTime? dtEnd)
        {
            List<LogData> logDatas = new List<LogData>();
            if (logTargets.Count > 0)
            {
                logDatas = logTargets[0].GetLogs(targetCode, logEntryType, categoryID, userID, dtStart, dtEnd);
            }
            return logDatas;
        }
    }
}
