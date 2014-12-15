using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Log
{
    public class SqlServerDatabaseAdapter : DatabaseAdapter
    {
        public SqlServerDatabaseAdapter(JIAOFENG.Practices.Database.Database db)
            : base(db)
        {

        }
        protected override void WriteEntry(string title, string action, string targetCode, string targetName, EventType eventType, int categoryID, string categoryName, int userID, string userName)
        {
            string sql = @"INSERT INTO Log (Title,Action,TargetCode,TargetName ,EventTypeID,CategoryID,CategoryName,CreateBy,CreateUserName,CreateTime)
                                    VALUES (@Title,@Action,@TargetCode,@TargetName,@EventTypeID,@CategoryID,@CategoryName,@CreateBy,@CreateUserName,@CreateTime)";

            List<SqlParameter> commandParameters = new List<SqlParameter>();
            commandParameters.Add(new SqlParameter("@Title", title));
            commandParameters.Add(new SqlParameter("@Action", action));
            commandParameters.Add(new SqlParameter("@TargetCode", targetCode));
            commandParameters.Add(new SqlParameter("@TargetName", targetName));
            commandParameters.Add(new SqlParameter("@EventTypeID", (int)eventType));
            commandParameters.Add(new SqlParameter("@CategoryID", categoryID));
            commandParameters.Add(new SqlParameter("@CategoryName", categoryName));
            commandParameters.Add(new SqlParameter("@CreateBy", userID));
            commandParameters.Add(new SqlParameter("@CreateUserName", userName));
            commandParameters.Add(new SqlParameter("@CreateTime", DateTime.Now));

            this.database.ExecuteNonQuery(CommandType.Text, sql, commandParameters.ToArray());
        }

        public override List<LogData> GetLogs(string targetCode, EventType? logEntryType, int? categoryID, int? userID, DateTime? dtStart, DateTime? dtEnd)
        {
            List<LogData> logs = new List<LogData>();
            string sqlCommand = @"select * from Log";
            string where = string.Empty;
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(targetCode))
            {
                where += " and TargetCode='@TargetCode'";
                commandParameters.Add(new SqlParameter("@TargetCode", targetCode));
            }
            if (logEntryType.HasValue)
            {
                where += " and EventTypeID=@EventTypeID";
                commandParameters.Add(new SqlParameter("@EventTypeID", (int)logEntryType.Value));
            }
            if (categoryID.HasValue)
            {
                where += " and categoryID=@categoryID";
                commandParameters.Add(new SqlParameter("@categoryID", categoryID.Value));
            }
            if (userID.HasValue)
            {
                where += " and CreateBy=@CreateBy";
                commandParameters.Add(new SqlParameter("@CreateBy", userID.Value));
            }
            if (dtStart.HasValue)
            {
                where += " and CreateTime>=@CreateTime";
                commandParameters.Add(new SqlParameter("@CreateTime", dtStart.Value));
            }
            if (dtEnd.HasValue)
            {
                where += " and CreateTime<=@CreateTime";
                commandParameters.Add(new SqlParameter("@CreateTime", dtEnd.Value));
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
