using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Data; 
using System.Data.Common;
using System.Data.SqlClient; 
using JIAOFENG.Practices.Database; 
using JIAOFENG.Practices.Library.Common;
using JIAOFENG.Practices.Logic.Mail;
using JIAOFENG.Practices.Logic.Mail.Entity;

namespace JIAOFENG.Practices.Logic.Mail
{
    internal abstract class MailPushDal : IMailPushDal
    {
        protected JIAOFENG.Practices.Database.Database database = null;
        public MailPushDal(JIAOFENG.Practices.Database.Database db)
        {
            this.database = db;
        }

        #region add
        public virtual OutMailMessageEntity Add(OutMailMessageEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region search
        public virtual List<OutMailMessageEntity> GetWaitingSendMails(int failInterval, int failMostNum)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region delete
        public void Delete(string ids)
        {
            string sql = "DELETE FROM MailSend WHERE MailID IN (" + ids + ")";
            database.ExecuteNonQuery(sql);
        }

		public void Delete(int id)
		{
            string sql = "DELETE FROM MailSend WHERE MailID = " + id;
            database.ExecuteNonQuery(sql);
		}

        public void Delete(List<int> ids)
		{
			Delete(string.Join(",", ids));
		}

        public void Delete(int[] ids)
		{
			Delete(string.Join(",", ids));
		}
        #endregion

        #region update
        public virtual void UpdataMailStatus(int mailID, OutMailStatus status)
        {
            throw new NotImplementedException();
        }
        public virtual void UpdateSendCount(int mailID)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region private method
        private static OutMailMessageEntity DataRowToEntity(DataRow dr)
        {
            OutMailMessageEntity entity = new OutMailMessageEntity();
            if (dr != null)
            {
                if (dr["MailID"] != DBNull.Value) { entity.MailID = Convert.ToInt32(dr["MailID"]); }
                if (dr["Sender"] != DBNull.Value) { entity.Sender = Convert.ToString(dr["Sender"]); }
                if (dr["MailFrom"] != DBNull.Value) { entity.MailFrom = Convert.ToString(dr["From"]); }
                if (dr["MailTo"] != DBNull.Value) { entity.MailTo = Convert.ToString(dr["To"]); }
                if (dr["MailCC"] != DBNull.Value) { entity.MailCC = Convert.ToString(dr["CC"]); }
                if (dr["MailBcc"] != DBNull.Value) { entity.MailBcc = Convert.ToString(dr["Bcc"]); }
                if (dr["Subject"] != DBNull.Value) { entity.Subject = Convert.ToString(dr["Subject"]); }
                if (dr["Body"] != DBNull.Value) { entity.Body = Convert.ToString(dr["Body"]); }
                if (dr["IsBodyHtml"] != DBNull.Value) { entity.IsBodyHtml = Convert.ToBoolean(dr["IsBodyHtml"]); }
                if (dr["BodyEncoding"] != DBNull.Value) { entity.BodyEncoding = Convert.ToString(dr["BodyEncoding"]); }
                if (dr["Priority"] != DBNull.Value) { entity.Priority = Convert.ToInt32(dr["Priority"]); }
                if (dr["Status"] != DBNull.Value) { entity.Status = Convert.ToInt32(dr["Status"]); }
                if (dr["SendCount"] != DBNull.Value) { entity.SendCount = Convert.ToInt32(dr["SendCount"]); }
                if (dr["LastTrySendTime"] != DBNull.Value) { entity.LastTrySendTime = Convert.ToDateTime(dr["LastTrySendTime"]); }
                if (dr["FilePackageID"] != DBNull.Value) { entity.FilePackageID = Convert.ToInt32(dr["FilePackageID"]); }
                if (dr["CreateUserName"] != DBNull.Value) { entity.CreateUserName = Convert.ToString(dr["CreateUserName"]); }
                if (dr["CreateTime"] != DBNull.Value) { entity.CreateTime = Convert.ToDateTime(dr["CreateTime"]); }
                if (dr["CreateBy"] != DBNull.Value) { entity.CreateBy = Convert.ToInt32(dr["CreateBy"]); }
                if (dr["UpdateTime"] != DBNull.Value) { entity.UpdateTime = Convert.ToDateTime(dr["UpdateTime"]); }
                if (dr["UpdateBy"] != DBNull.Value) { entity.UpdateBy = Convert.ToInt32(dr["UpdateBy"]); }
            }
            return entity;
        }
        /// <summary>
        /// 转换数据行到实体对象
        /// </summary>
        protected static void DataRowToEntity(OutMailMessageEntity entity, DataRow dr)
        {
            if (dr != null)
            {
                entity = DataRowToEntity(dr);
            }
        }
        protected static List<OutMailMessageEntity> DataTableToList(DataTable dt)
        {
            List<OutMailMessageEntity> list = new List<OutMailMessageEntity>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(DataRowToEntity(dr));
            }
            return list;
        }
        #endregion
    }
}
