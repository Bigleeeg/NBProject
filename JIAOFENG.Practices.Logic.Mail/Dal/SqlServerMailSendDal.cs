using JIAOFENG.Practices.Logic.Mail.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Mail
{
    internal class SqlServerMailSendDal : MailPushDal
    {
        public SqlServerMailSendDal(JIAOFENG.Practices.Database.Database db) : base(db)
        {

        }

        #region add
        public override OutMailMessageEntity Add(OutMailMessageEntity entity)
        {
            string sql = "INSERT INTO MailSend (Sender,MailFrom,MailTo,MailCC,MailBcc,Subject,Body,IsBodyHtml,BodyEncoding,Priority,Status,SendCount,LastTrySendTime,FilePackageID,CreateUserName,CreateTime,CreateBy,UpdateTime,UpdateBy) VALUES (@Sender,@From,@To,@CC,@Bcc,@Subject,@Body,@IsBodyHtml,@BodyEncoding,@Priority,@Status,@SendCount,@LastTrySendTime,@FilePackageID,@CreateUserName,@CreateTime,@CreateBy,@UpdateTime,@UpdateBy);SELECT SCOPE_IDENTITY();";
            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            sqlParameterList.Add(new SqlParameter("@Sender", entity.Sender == null ? (object)DBNull.Value : entity.Sender));
            sqlParameterList.Add(new SqlParameter("@From", entity.MailFrom == null ? (object)DBNull.Value : entity.MailFrom));
            sqlParameterList.Add(new SqlParameter("@To", entity.MailTo));
            sqlParameterList.Add(new SqlParameter("@CC", entity.MailCC == null ? (object)DBNull.Value : entity.MailCC));
            sqlParameterList.Add(new SqlParameter("@Bcc", entity.MailBcc == null ? (object)DBNull.Value : entity.MailBcc));
            sqlParameterList.Add(new SqlParameter("@Subject", entity.Subject));
            sqlParameterList.Add(new SqlParameter("@Body", entity.Body == null ? (object)DBNull.Value : entity.Body));
            sqlParameterList.Add(new SqlParameter("@IsBodyHtml", entity.IsBodyHtml));
            sqlParameterList.Add(new SqlParameter("@BodyEncoding", entity.BodyEncoding));
            sqlParameterList.Add(new SqlParameter("@Priority", entity.Priority));
            sqlParameterList.Add(new SqlParameter("@Status", entity.Status));
            sqlParameterList.Add(new SqlParameter("@SendCount", entity.SendCount));
            sqlParameterList.Add(new SqlParameter("@LastTrySendTime", entity.LastTrySendTime == null ? (object)DBNull.Value : entity.LastTrySendTime));
            sqlParameterList.Add(new SqlParameter("@FilePackageID", entity.FilePackageID == null ? (object)DBNull.Value : entity.FilePackageID));
            sqlParameterList.Add(new SqlParameter("@CreateUserName", entity.CreateUserName));
            sqlParameterList.Add(new SqlParameter("@CreateTime", entity.CreateTime));
            sqlParameterList.Add(new SqlParameter("@CreateBy", entity.CreateBy));
            sqlParameterList.Add(new SqlParameter("@UpdateTime", entity.UpdateTime));
            sqlParameterList.Add(new SqlParameter("@UpdateBy", entity.UpdateBy));

            int id = Convert.ToInt32(database.ExecuteScalar(CommandType.Text, sql, sqlParameterList.ToArray()));
            if (id > 0)
            {
                entity.MailID = id;
                return entity;
            }
            return null;
        }
        #endregion 

        #region search
        public override List<OutMailMessageEntity> GetWaitingSendMails(int failInterval, int failMostNum)
        {
            const string dateTimeBarFormat = "or ([SendCount]={0} and DATEDIFF(n,LastTrySendTime,GetDate())>={1})";

            string dateTimeBar = string.Empty;
            for (int i = 1; i < failMostNum; i++)
            {
                dateTimeBar += string.Format(dateTimeBarFormat, i, Math.Pow(failInterval, i));
            }
            string strSql = @"SELECT * FROM MailSend WHERE Status =" + (int)OutMailStatus.UnSend + " and (SendCount = 0 {0})";
            strSql = string.Format(strSql, dateTimeBar);
            DataSet ds = database.ExecuteDataSet(strSql);
            List<OutMailMessageEntity> list = DataTableToList(ds.Tables[0]);
            return list;
        }
        #endregion


        #region update
        public override void UpdataMailStatus(int mailID, OutMailStatus status)
        {
            string strSql = "Update MailSend SET LastTrySendTime=getdate(), Status=" + (int)status + " WHERE MailID=" + mailID;
            database.ExecuteNonQuery(strSql);
        }
        public override void UpdateSendCount(int mailID)
        {
            string strSql = "Update MailSend SET LastTrySendTime=getdate(), SendCount=SendCount+1 WHERE MailID=" + mailID;
            database.ExecuteNonQuery(strSql);
        }
        #endregion
    }
}
