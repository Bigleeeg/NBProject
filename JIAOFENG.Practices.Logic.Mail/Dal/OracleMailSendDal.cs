using JIAOFENG.Practices.Logic.Mail.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Mail
{
    internal class OracleMailSendDal : MailPushDal
    {
        public OracleMailSendDal(JIAOFENG.Practices.Database.Database db)
            : base(db)
        {

        }
        #region add
        public override OutMailMessageEntity Add(OutMailMessageEntity entity)
        {
            string sql = "INSERT INTO MailSend (Sender,MailFrom,MailTo,MailCC,MailBcc,Subject,Body,IsBodyHtml,BodyEncoding,Priority,Status,SendCount,LastTrySendTime,FilePackageID,CreateUserName,CreateTime,CreateBy,UpdateTime,UpdateBy) VALUES (@Sender,@From,@To,@CC,@Bcc,@Subject,@Body,@IsBodyHtml,@BodyEncoding,@Priority,@Status,@SendCount,@LastTrySendTime,@FilePackageID,@CreateUserName,@CreateTime,@CreateBy,@UpdateTime,@UpdateBy);select seq_MailSend.CURRVAL from dual;";
            List<OracleParameter> OracleParameterList = new List<OracleParameter>();
            OracleParameterList.Add(new OracleParameter("@Sender", entity.Sender == null ? (object)DBNull.Value : entity.Sender));
            OracleParameterList.Add(new OracleParameter("@From", entity.MailFrom == null ? (object)DBNull.Value : entity.MailFrom));
            OracleParameterList.Add(new OracleParameter("@To", entity.MailTo));
            OracleParameterList.Add(new OracleParameter("@CC", entity.MailCC == null ? (object)DBNull.Value : entity.MailCC));
            OracleParameterList.Add(new OracleParameter("@Bcc", entity.MailBcc == null ? (object)DBNull.Value : entity.MailBcc));
            OracleParameterList.Add(new OracleParameter("@Subject", entity.Subject));
            OracleParameterList.Add(new OracleParameter("@Body", entity.Body == null ? (object)DBNull.Value : entity.Body));
            OracleParameterList.Add(new OracleParameter("@IsBodyHtml", entity.IsBodyHtml));
            OracleParameterList.Add(new OracleParameter("@BodyEncoding", entity.BodyEncoding));
            OracleParameterList.Add(new OracleParameter("@Priority", entity.Priority));
            OracleParameterList.Add(new OracleParameter("@Status", entity.Status));
            OracleParameterList.Add(new OracleParameter("@SendCount", entity.SendCount));
            OracleParameterList.Add(new OracleParameter("@LastTrySendTime", entity.LastTrySendTime == null ? (object)DBNull.Value : entity.LastTrySendTime));
            OracleParameterList.Add(new OracleParameter("@FilePackageID", entity.FilePackageID == null ? (object)DBNull.Value : entity.FilePackageID));
            OracleParameterList.Add(new OracleParameter("@CreateUserName", entity.CreateUserName));
            OracleParameterList.Add(new OracleParameter("@CreateTime", entity.CreateTime));
            OracleParameterList.Add(new OracleParameter("@CreateBy", entity.CreateBy));
            OracleParameterList.Add(new OracleParameter("@UpdateTime", entity.UpdateTime));
            OracleParameterList.Add(new OracleParameter("@UpdateBy", entity.UpdateBy));

            int id = Convert.ToInt32(database.ExecuteScalar(CommandType.Text, sql, OracleParameterList.ToArray()));
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
            const string dateTimeBarFormat = "or (SendCount={0} and to_number(sysdate-LastTrySendTime)*1440 >= {1})";

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
            string strSql = "Update MailSend SET LastTrySendTime=sysdate, Status=" + (int)status + " WHERE MailID=" + mailID;
            database.ExecuteNonQuery(strSql);
        }
        public override void UpdateSendCount(int mailID)
        {
            string strSql = "Update MailSend SET LastTrySendTime=sysdate, SendCount=SendCount+1 WHERE MailID=" + mailID;
            database.ExecuteNonQuery(strSql);
        }
        #endregion
    }
}
