using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using Dashinginfo.Practices.Database;

namespace MailTest.TestApplication
{
    public class MailSenderDB
    {
        public static DataSet GetMailByStatus(MailStatus status, int firstFailInterval, int restBase, int failMostNum, string connectingString)
        {

            string strSql = @"SELECT [MailSenderID]
                                      ,[MailSender]
                                      ,[MailSenderAddress]
                                      ,[MailToAddress]
                                      ,[MailCC]
                                      ,[MailBCC]
                                      ,[MailSubject]
                                      ,[MailContent]
                                      ,[MailSendTime]
                                      ,[MailCreateUserName]
                                      ,[MailCreateUserID]
                                      ,[MailStatus]
                                      ,ISNULL([MailSendCount],0) AS MailSendCount
                                      ,LastSendTime
                                      ,MailSendAttachmentID
                                      ,MailSendID
                                      ,MailSendAttachment
                                      ,AttachmentType
                                      FROM MailSend LEFT OUTER JOIN MailSendAttachment on MailSend.MailSenderID = MailSendAttachment.MailSendID
                                      WHERE MailStatus =" + (int)status + " and" +
                                                          "(" +
                                                              "([MailSendCount] is null) or " +
                                                              "([MailSendCount]=1 and DATEDIFF(n,LastSendTime,GetDate())>=" + firstFailInterval + ") or " +
                                                              "([MailSendCount]>=2 and [MailSendCount]<" + failMostNum + " and DATEDIFF(n,LastSendTime,GetDate())>=" + firstFailInterval + "*" + restBase + "*[MailSendCount]" + "-" + firstFailInterval + "*" + restBase + ")" +
                                                          ")";
            using (SqlDatabase db = new SqlDatabase(connectingString))
            {
                return db.ExecuteDataSet(strSql);
            }
        }

        internal static bool UpdataMailStatus(int mailSenderID, MailStatus status, string connectingString)
        {
            string strSql = string.Empty;
            if (status == MailStatus.Success)
            {
                strSql = @"Update MailSend SET LastSendTime=getdate(),MailSendTime=getdate(), MailStatus=" + (int)status + " WHERE MailSenderID=" + mailSenderID;
            }
            else if (status == MailStatus.Fail)
            {
                strSql = @"Update MailSend SET LastSendTime=getdate() MailStatus=" + (int)status + " WHERE MailSenderID=" + mailSenderID;
            }
            else
            { }
            using (SqlDatabase db = new SqlDatabase(connectingString))
            {
                if (db.ExecuteNonQuery(strSql) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool UpdateSendCount(int mailSenderID, string connectingString)
        {
            string strSql = @"Update MailSender SET LastSendTime=getdate(), MailSendCount=ISNULL(MailSendCount,0)+1 WHERE MailSenderID=" + mailSenderID;
            using (SqlDatabase db = new SqlDatabase(connectingString))
            {
                if (db.ExecuteNonQuery(strSql) > 0)
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 获得邮件的ID集合 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<string> GetSenderIDsByDataTable(DataTable dt)
        {
            List<string> listSenderIDs = new List<string>();
            List<object> dataObjs = dt.Select().GroupBy(o => o["MailSenderID"]).Select(p => p.Key).ToList();
            if (dataObjs.Count() > 0)
            {
                foreach (object tm in dataObjs)
                {
                    listSenderIDs.Add(tm.ToString());
                }
            }
            return listSenderIDs;
        }
        /// <summary>
        /// 获得某一封邮件的附件集合
        /// </summary>
        /// <param name="drs"></param>
        /// <returns></returns>
        public static Dictionary<string, Stream> GetAttachMents(DataRow[] drs)
        {
            Dictionary<string, Stream> dicAttachs = new Dictionary<string, Stream>();
            foreach (DataRow dr in drs)
            {
                if (dr["MailSendAttachment"] != DBNull.Value && dr["AttachmentType"] != DBNull.Value)
                {
                    byte[] bytes = (byte[])dr["MailSendAttachment"];
                    dicAttachs.Add(dr["AttachmentType"].ToString(), BytesToStream((byte[])dr["MailSendAttachment"]));
                }
            }
            return dicAttachs;
        }
        /// <summary>
        /// 字节数值转换成流
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static Stream BytesToStream(byte[] value)
        {
            Stream stream = new MemoryStream(value);
            return stream;
        }
    }
}
