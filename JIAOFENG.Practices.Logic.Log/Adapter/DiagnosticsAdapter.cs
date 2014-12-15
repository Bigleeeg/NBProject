using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace JIAOFENG.Practices.Logic.Log
{
    public class DiagnosticsAdapter : LogTarget
    {
        protected override void WriteEntry(string title, string action, string targetCode, string targetName, EventType logEntryType, int categoryID, string categoryName, int userID, string userName)
        {
            System.Diagnostics.Trace.WriteLine("ACTION: " + action);
        }

        public override List<LogData> GetLogs(string targetCode, EventType? logEntryType, int? categoryID, int? userID, DateTime? dtStart, DateTime? dtEnd)
        {
            List<LogData> logs = new List<LogData>();
            return logs;
        }
    }
}
