using JIAOFENG.Practices.Logic.Mail.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Mail
{
    internal class MySQLMailSendDal : MailPushDal
    {
        public MySQLMailSendDal(JIAOFENG.Practices.Database.Database db)
            : base(db)
        {

        }
        #region add
        public override OutMailMessageEntity Add(OutMailMessageEntity entity)
        {
            string sql = "INSERT INTO MailSend (Sender,MailFrom,MailTo,MailCC,MailBcc,Subject,Body,IsBodyHtml,BodyEncoding,Priority,Status,SendCount,LastTrySendTime,FilePackageID,CreateUserName,CreateTime,CreateBy,UpdateTime,UpdateBy) VALUES (@Sender,@From,@To,@CC,@Bcc,@Subject,@Body,@IsBodyHtml,@BodyEncoding,@Priority,@Status,@SendCount,@LastTrySendTime,@FilePackageID,@CreateUserName,@CreateTime,@CreateBy,@UpdateTime,@UpdateBy);select last_insert_id();";
            List<MySqlParameter> MySqlParameterList = new List<MySqlParameter>();
            MySqlParameterList.Add(new MySqlParameter("@Sender", entity.Sender == null ? (object)DBNull.Value : entity.Sender));
            MySqlParameterList.Add(new MySqlParameter("@From", entity.MailFrom == null ? (object)DBNull.Value : entity.MailFrom));
            MySqlParameterList.Add(new MySqlParameter("@To", entity.MailTo));
            MySqlParameterList.Add(new MySqlParameter("@CC", entity.MailCC == null ? (object)DBNull.Value : entity.MailCC));
            MySqlParameterList.Add(new MySqlParameter("@Bcc", entity.MailBcc == null ? (object)DBNull.Value : entity.MailBcc));
            MySqlParameterList.Add(new MySqlParameter("@Subject", entity.Subject));
            MySqlParameterList.Add(new MySqlParameter("@Body", entity.Body == null ? (object)DBNull.Value : entity.Body));
            MySqlParameterList.Add(new MySqlParameter("@IsBodyHtml", entity.IsBodyHtml));
            MySqlParameterList.Add(new MySqlParameter("@BodyEncoding", entity.BodyEncoding));
            MySqlParameterList.Add(new MySqlParameter("@Priority", entity.Priority));
            MySqlParameterList.Add(new MySqlParameter("@Status", entity.Status));
            MySqlParameterList.Add(new MySqlParameter("@SendCount", entity.SendCount));
            MySqlParameterList.Add(new MySqlParameter("@LastTrySendTime", entity.LastTrySendTime == null ? (object)DBNull.Value : entity.LastTrySendTime));
            MySqlParameterList.Add(new MySqlParameter("@FilePackageID", entity.FilePackageID == null ? (object)DBNull.Value : entity.FilePackageID));
            MySqlParameterList.Add(new MySqlParameter("@CreateUserName", entity.CreateUserName));
            MySqlParameterList.Add(new MySqlParameter("@CreateTime", entity.CreateTime));
            MySqlParameterList.Add(new MySqlParameter("@CreateBy", entity.CreateBy));
            MySqlParameterList.Add(new MySqlParameter("@UpdateTime", entity.UpdateTime));
            MySqlParameterList.Add(new MySqlParameter("@UpdateBy", entity.UpdateBy));

            int id = Convert.ToInt32(database.ExecuteScalar(CommandType.Text, sql, MySqlParameterList.ToArray()));
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
            const string dateTimeBarFormat = "or (SendCount={0} and DATE_SUB(now(),INTERVAL {1} MINUTE) >= LastTrySendTime)";

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
            string strSql = "Update MailSend SET LastTrySendTime=now(), Status=" + (int)status + " WHERE MailID=" + mailID;
            database.ExecuteNonQuery(strSql);
        }
        public override void UpdateSendCount(int mailID)
        {
            string strSql = "Update MailSend SET LastTrySendTime=now(), SendCount=SendCount+1 WHERE MailID=" + mailID;
            database.ExecuteNonQuery(strSql);
        }
        #endregion
    }
}
