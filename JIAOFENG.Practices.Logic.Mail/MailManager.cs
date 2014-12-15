using JIAOFENG.Practices.Database;
using JIAOFENG.Practices.Library.Common;
using JIAOFENG.Practices.Logic.File;
using JIAOFENG.Practices.Logic.Mail.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace JIAOFENG.Practices.Logic.Mail
{
    public static class MailManager
    {
        static MailManager()
        {
            Database = DatabaseFactory.CreateDatabase();
        }
        public static JIAOFENG.Practices.Database.Database Database { get; set; }
        //internal static DatabaseProviderType DatabaseProviderType { get; set; }
        //internal static string ConnectionString { get; set; }

        #region search
        public static List<OutMailMessage> GetWaitingSendMails(int failInterval, int failMostNum)
        {
            List<OutMailMessageEntity> entityList = GetMailSendDal().GetWaitingSendMails(failInterval, failMostNum);
            List<OutMailMessage> list = new List<OutMailMessage>();
            foreach (OutMailMessageEntity messageEntity in entityList)
            {
                OutMailMessage message = new OutMailMessage();
                message.CopyFrom(messageEntity);
                if (messageEntity.FilePackageID.HasValue)
                {
                    message.Attachments = FilePackage.GetEntityById(messageEntity.FilePackageID.Value);
                }
                list.Add(message);
            }
            
            return list;
        }
        #endregion

        #region update
        public static void UpdataMailStatus(int mailID, OutMailStatus status)
        {
            GetMailSendDal().UpdataMailStatus(mailID, status);
        }
        public static void UpdateSendCount(int mailID)
        {
            GetMailSendDal().UpdateSendCount(mailID);
        }
        #endregion

        #region SendMail/Add
        public static void SendMail(string to, string subject, string body)
        {
            SendMail(to, null, subject, body, null);
        }
        public static void SendMail(string[] to, string subject, string body)
        {
            SendMail(to, null, subject, body, null);
        }

        public static void SendMail(string to, string cc, string subject, string body)
        {
            SendMail(to, cc, subject, body, null);
        }
        public static void SendMail(string[] to, string[] cc, string subject, string body)
        {
            SendMail(to, cc, subject, body, null);
        }

        public static void SendMail(string to, string cc, string subject, string body, Dictionary<string, byte[]> attachments)
        {
            SendMail(to, cc, string.Empty, subject, body, attachments);
        }

        public static void SendMail(string[] to, string[] cc, string subject, string body, Dictionary<string, byte[]> attachments)
        {
            string strTo = string.Empty;
            if (to != null)
            {
                strTo = string.Join(";", to);
            }

            string strCC = string.Empty;
            if (cc != null)
            {
                strCC = string.Join(";", cc);
            }
            SendMail(strTo, strCC, subject, body, attachments);
        }

        /// <summary>
        /// 省略发送者和创建者
        /// </summary>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        public static void SendMail(string to, string cc, string bcc, string subject, string body, Dictionary<string, byte[]> attachments)
        {
            SendMail(to, cc, bcc, subject, body, attachments, 0, string.Empty);
        }

        /// <summary>
        /// 省略发送者
        /// </summary>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="attachments"></param>
        /// <param name="createUserID"></param>
        /// <param name="createUserName"></param>
        /// <returns></returns>
        public static void SendMail(string to, string cc, string bcc, string subject, string body, Dictionary<string, byte[]> attachments, int createUserID, string createUserName)
        {
            SendMail(string.Empty, string.Empty, to, cc, bcc, subject, body, attachments, createUserID, createUserName);
        }

        public static void SendMail(string sender, string senderMail, string to, string cc, string bcc, string subject, string body, Dictionary<string, byte[]> attachments, int createUserID, string createUserName)
        {
            SendMail(sender, senderMail, to, cc, bcc, subject, body, attachments, true, 1, createUserID, createUserName);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="sender">发件人姓名</param>
        /// <param name="senderMail">发件人地址</param>
        /// <param name="to">多个收件人地址</param>
        /// <param name="cc">多个抄送人地址</param>
        /// <param name="bcc">多个密件抄送人地址</param>
        /// <param name="subject">标题</param>
        /// <param name="attachment">多个附件地址</param>
        /// <param name="body">内容</param>
        /// <param name="createUserID"><创建者ID/param>
        /// <param name="createUserName">创建者Name</param>
        /// <returns>如果为null则“待发送”，否则出现错误</returns>
        public static void SendMail(string sender, string senderMail, string[] to, string[] cc, string[] bcc, string subject, string body, Dictionary<string, byte[]> attachments, bool isBodyHtml, int priority, int createUserID, string createUserName)
        {
            string strTo = string.Empty;
            if (to != null)
            {
                strTo = string.Join(";", to);
            }

            string strCC = string.Empty;
            if (cc != null)
            {
                strCC = string.Join(";", cc);
            }

            string strBCC = string.Empty;
            if (bcc != null)
            {
                strBCC = string.Join(";", bcc);
            }
            SendMail(sender, senderMail, strTo, strCC, strBCC, subject, body, attachments, isBodyHtml, priority, createUserID, createUserName);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="sender">发件人姓名</param>
        /// <param name="senderMail">发件人地址</param>
        /// <param name="to">多个收件人地址</param>
        /// <param name="cc">多个抄送人地址</param>
        /// <param name="bcc">多个密件抄送人地址</param>
        /// <param name="subject">标题</param>
        /// <param name="body">内容</param>
        /// <param name="attachments">多个附件地址</param>
        /// <param name="isBodyHtml">邮件内容格式</param>
        /// <param name="priority">邮件优先级</param>
        /// <param name="createUserID"><创建者ID/param>
        /// <param name="createUserName">创建者Name</param>
        /// <returns></returns>
        public static void SendMail(string sender, string senderMail, string to, string cc, string bcc, string subject, string body, Dictionary<string, byte[]> attachments, bool isBodyHtml, int priority, int createUserID, string createUserName)
        {
            if (string.IsNullOrWhiteSpace(to))
            {
                throw new CommonException(JIAOFENG.Practices.Resources.Resource.Exception_Mail_RecipientAddress_Emption);
            }
            int? filePackageID = null;
            if (attachments != null)
            {
                FilePackage package = new FilePackage();
                foreach (KeyValuePair<string, byte[]> kv in attachments)
                {
                    FileStorage fileStorage = new LocalDBFile();
                    fileStorage.FileCode = Guid.NewGuid().ToString();
                    fileStorage.FileName = kv.Key;
                    fileStorage.ExtensionName = "";
                    fileStorage.Context = kv.Value;
                    fileStorage.SavePath = "";
                    fileStorage.FileType = "";
                    package.FileStorages.Add(fileStorage);
                }
                package.Save();
                filePackageID = package.FilePackageID;
            }

            SendMail(sender, senderMail, to, cc, bcc, subject, body, filePackageID, isBodyHtml, priority, createUserID, createUserName);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="sender">发件人姓名</param>
        /// <param name="senderMail">发件人地址</param>
        /// <param name="to">多个收件人地址</param>
        /// <param name="cc">多个抄送人地址</param>
        /// <param name="bcc">多个密件抄送人地址</param>
        /// <param name="subject">标题</param>
        /// <param name="body">内容</param>
        /// <param name="attachments">多个附件地址</param>
        /// <param name="isBodyHtml">邮件内容格式</param>
        /// <param name="priority">邮件优先级</param>
        /// <param name="createUserID"><创建者ID/param>
        /// <param name="createUserName">创建者Name</param>
        /// <returns></returns>
        public static void SendMail(string sender, string senderMail, string to, string cc, string bcc, string subject, string body, int? filePackageID, bool isBodyHtml, int priority, int createUserID, string createUserName)
        {
            if (string.IsNullOrWhiteSpace(to))
            {
                throw new CommonException(JIAOFENG.Practices.Resources.Resource.Exception_Mail_RecipientAddress_Emption);
            }

            //主表部分
            OutMailMessageEntity message = new OutMailMessageEntity();
            message.Sender = sender;
            message.MailFrom = senderMail;
            message.MailTo = to;
            message.MailCC = cc;
            message.MailBcc = bcc;
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isBodyHtml;
            message.BodyEncoding = "utf-8";
            message.Priority = priority;
            message.SendCount = 0;
            message.Status = (int)OutMailStatus.UnSend;
            message.LastTrySendTime = null;
            message.FilePackageID = filePackageID;
            message.CreateUserName = createUserName;
            message.CreateBy = createUserID;
            message.CreateTime = DateTime.Now;
            
            message = GetMailSendDal().Add(message);
        }
        #endregion

        #region private method
        internal static IMailPushDal GetMailSendDal()
        {
            IMailPushDal dal = null;
            switch (Database.DatabaseProviderType)
            {
                case DatabaseProviderType.SqlServer:
                    dal = new SqlServerMailSendDal(Database);
                    break;
                case DatabaseProviderType.Oracle:
                    dal = new OracleMailSendDal(Database);
                    break;
                case DatabaseProviderType.MySQL:
                    dal = new MySQLMailSendDal(Database);
                    break;
                default:
                    dal = new SqlServerMailSendDal(Database);
                    break;
            }
            return dal;
        }
        #endregion
    }
}
