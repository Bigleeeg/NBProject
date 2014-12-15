using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Log
{
    public class OracleDatabaseAdapter : DatabaseAdapter
    {
        public OracleDatabaseAdapter(JIAOFENG.Practices.Database.Database db)
            : base(db)
        {

        }
        protected override void WriteEntry(string title, string action, string targetCode, string targetName, EventType eventType, int categoryID, string categoryName, int userID, string userName)
        {
            string sql = @"INSERT INTO Log (Title,Action,TargetCode,TargetName ,EventTypeID,CategoryID,CategoryName,CreateBy,CreateUserName,CreateTime)
                                    VALUES (@Title,@Action,@TargetCode,@TargetName,@EventTypeID,@CategoryID,@CategoryName,@CreateBy,@CreateUserName,@CreateTime)";

            List<OracleParameter> commandParameters = new List<OracleParameter>();
            commandParameters.Add(new OracleParameter("@Title", title));
            commandParameters.Add(new OracleParameter("@Action", action));
            commandParameters.Add(new OracleParameter("@TargetCode", targetCode));
            commandParameters.Add(new OracleParameter("@TargetName", targetName));
            commandParameters.Add(new OracleParameter("@EventTypeID", (int)eventType));
            commandParameters.Add(new OracleParameter("@CategoryID", categoryID));
            commandParameters.Add(new OracleParameter("@CategoryName", categoryName));
            commandParameters.Add(new OracleParameter("@CreateBy", userID));
            commandParameters.Add(new OracleParameter("@CreateUserName", userName));
            commandParameters.Add(new OracleParameter("@CreateTime", DateTime.Now));

            this.database.ExecuteNonQuery(CommandType.Text, sql, commandParameters.ToArray());
        }

        public override List<LogData> GetLogs(string targetCode, EventType? logEntryType, int? categoryID, int? userID, DateTime? dtStart, DateTime? dtEnd)
        {
            List<LogData> logs = new List<LogData>();
            string sqlCommand = @"select * from Log";
            string where = string.Empty;
            List<OracleParameter> commandParameters = new List<OracleParameter>();
            if (!string.IsNullOrEmpty(targetCode))
            {
                where += " and TargetCode='@TargetCode'";
                commandParameters.Add(new OracleParameter("@TargetCode", targetCode));
            }
            if (logEntryType.HasValue)
            {
                where += " and EventTypeID=@EventTypeID";
                commandParameters.Add(new OracleParameter("@EventTypeID", (int)logEntryType.Value));
            }
            if (categoryID.HasValue)
            {
                where += " and categoryID=@categoryID";
                commandParameters.Add(new OracleParameter("@categoryID", categoryID.Value));
            }
            if (userID.HasValue)
            {
                where += " and CreateBy=@CreateBy";
                commandParameters.Add(new OracleParameter("@CreateBy", userID.Value));
            }
            if (dtStart.HasValue)
            {
                where += " and CreateTime>=@CreateTime";
                commandParameters.Add(new OracleParameter("@CreateTime", dtStart.Value));
            }
            if (dtEnd.HasValue)
            {
                where += " and CreateTime<=@CreateTime";
                commandParameters.Add(new OracleParameter("@CreateTime", dtEnd.Value));
            }
            if (where != string.Empty)
            {
                sqlCommand = sqlCommand + " where " + where.Substring(4);
            }
            sqlCommand += " order by CreateTime desc";
            DataSet ds = this.database.ExecuteDataSet(CommandType.Text, sqlCommand, commandParameters.ToArray());

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                LogData data = new LogData();
                data.LogID = (int)dr["LogID"];
                data.Title = dr["Title"].ToString();
                data.Action = dr["Action"].ToString();
                data.TargetCode = dr["TargetCode"].ToString();
                data.TargetName = dr["TargetName"].ToString();
                data.CategoryID = (int)dr["CategoryID"];
                data.CategoryName = dr["CategoryName"].ToString();
                data.CreateBy = (int)dr["CreateBy"];
                data.CreateUserName = dr["UserName"].ToString();
                data.EventTypeID = (int)dr["EventTypeID"];
                data.EventTypeName = ((EventType)data.EventTypeID).ToString();
                data.CreateTime = (DateTime)dr["CreateTime"];
                logs.Add(data);
            }
            return logs;
        }
    }
}
