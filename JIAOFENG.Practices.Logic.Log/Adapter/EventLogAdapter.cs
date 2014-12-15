using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace JIAOFENG.Practices.Logic.Log
{
    /// <summary>
    /// Summary description for EventLog.
    /// </summary>
    public class EventLogAdapter : LogTarget
    {
        private EventLog log;//For logging to the event log

        public EventLogAdapter(string eventLogSourceName, string eventLogFileName)
        {
            if (!EventLog.SourceExists(eventLogSourceName))
            {
                EventLog.CreateEventSource(eventLogSourceName, eventLogFileName);
            }

            log = new EventLog();
            ((System.ComponentModel.ISupportInitialize)log).BeginInit();
            ((System.ComponentModel.ISupportInitialize)log).EndInit();
            log.Source = eventLogSourceName;
            log.Log = eventLogFileName;
        }

        protected override void WriteEntry(string title, string action, string targetCode, string targetName, EventType logEntryType, int categoryID, string categoryName, int userID, string userName)   
        {
            log.WriteEntry(action, GetEventLogEntryType(logEntryType), userID, (short)categoryID);
        }

        public override List<LogData> GetLogs(string targetCode, EventType? logEntryType, int? categoryID, int? userID, DateTime? dtStart, DateTime? dtEnd)
        {
            List<LogData> logs = new List<LogData>();
            return logs;
        }

        #region helper methods
        // translate EventType to EventLogEntryType
        private static EventLogEntryType GetEventLogEntryType(EventType eventClassification)
        {
            EventLogEntryType ret;

            switch (eventClassification)
            {
                case EventType.Error:
                    ret = EventLogEntryType.Error;
                    break;
                case EventType.Debug:           // fall through
                case EventType.Information:
                    ret = EventLogEntryType.Information;
                    break;
                case EventType.Warning:
                    ret = EventLogEntryType.Warning;
                    break;
                case EventType.SuccessAudit:
                    ret = EventLogEntryType.SuccessAudit;
                    break;
                case EventType.FailureAudit:
                    ret = EventLogEntryType.FailureAudit;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("eventClassification", eventClassification, "Unrecognized EventType member.");
            }

            return (ret);
        }
        #endregion
    }
}
