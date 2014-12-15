using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Dashinginfo.Practices.Library.Database;

namespace DashingMailService
{
    public class MailReceiveDB
    {
        public int MailReceiveID { get; set; }
        public int MailID { get; set; }
        public string MailReceiver { get; set; }
        public string MailSendTime { get; set; }
        public string MailSenderAddress { get; set; }
        public string MailToAddress { get; set; }
        public string MailCC { get; set; }
        public string MailBCC { get; set; }
        public string MailSubject { get; set; }
        public string MailContent { get; set; }

        public Dictionary<string, byte[]> MailAttachment { get; set; }

        public static bool AddReceiveMailItem(MailReceiveDB msd)
        {
            bool result = false;
            const string sqlInsertReceive = @"INSERT INTO [MailReceive]
                                               ([MailID]
                                               ,[MailReceiver]
                                               ,[MailSendTime]
                                               ,[MailSenderAddress]
                                               ,[MailToAddress]
                                               ,[MailCC]
                                               ,[MailBCC]
                                               ,[MailSubject]
                                               ,[MailContent])
                                         VALUES
                                               (@MailID
                                               ,@MailReceiver
                                               ,@MailSendTime
                                               ,@MailSenderAddress
                                               ,@MailToAddress
                                               ,@MailCC
                                               ,@MailBCC
                                               ,@MailSubject
                                               ,@MailContent); SELECT SCOPE_IDENTITY();";
            StringBuilder sbAttachMents = new StringBuilder();
            List<SqlParameter> listPara = new List<SqlParameter>();
            int i = 0;
            foreach (KeyValuePair<string, byte[]> kv in msd.MailAttachment)
            {
                sbAttachMents.Append("INSERT INTO [dbo].[MailReceiveAttachment] ([MailReceiveID],[MailReceiveAttachment],[AttachmentType])");
                sbAttachMents.Append("VALUES(");
                sbAttachMents.Append("{0}"); sbAttachMents.Append(",");
                sbAttachMents.Append("@kValue" + i); sbAttachMents.Append(",");
                sbAttachMents.Append("@kKey" + i);
                sbAttachMents.Append(");");
                listPara.Add(new SqlParameter("@kValue" + i, kv.Value));
                listPara.Add(new SqlParameter("@kKey" + i, kv.Key));
                i++;
            }

            SqlDatabase db = SqlDatabasePool.Singleton.BorrowDBConnection();
            SqlTransaction tran = db.Connection.BeginTransaction();
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] { 
                  new SqlParameter("@MailID", msd.MailID),
                  new SqlParameter("@MailReceiver", msd.MailReceiver),
                  new SqlParameter("@MailSendTime", msd.MailSendTime),
                  new SqlParameter("@MailSenderAddress", msd.MailSenderAddress),
                  new SqlParameter("@MailToAddress", msd.MailToAddress),
                  new SqlParameter("@MailCC", msd.MailCC),
                  new SqlParameter("@MailBCC", msd.MailBCC),
                  new SqlParameter("@MailSubject", msd.MailSubject),
                  new SqlParameter("@MailContent", msd.MailContent)
                };
                int mailReceiveID = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlInsertReceive, tran, sqlParameters));
                msd.MailReceiveID = mailReceiveID;
                if (!string.IsNullOrEmpty(sbAttachMents.ToString()))
                {
                    //db.ExecuteNonQuery(string.Format(sbAttachMents.ToString(), mailSenderID.ToString()), tran);
                    db.ExecuteNonQuery(CommandType.Text, string.Format(sbAttachMents.ToString(), mailReceiveID.ToString()), tran, listPara.ToArray());
                }
                tran.Commit();
                result = true;
            }
            catch
            {
                tran.Rollback();
                result = false;
            }
            tran.Dispose();
            SqlDatabasePool.Singleton.ReturnDBConnection(db);
            return result;
        }

        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
    }
}

