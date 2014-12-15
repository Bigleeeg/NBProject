using JIAOFENG.Practices.Logic.File;
using JIAOFENG.Practices.Logic.Log;
using JIAOFENG.Practices.Logic.Mail;
using JIAOFENG.Practices.Mail;
using JIAOFENG.Practices.Mail.Pop3.Exceptions;
using JIAOFENG.Practices.Mail.Pop3.Mime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.PushService
{
    public class MailService : PushService
    {
        public MailService(int failRetryIntervalPow, int failMostNum, ClientConfig cc) : base(failRetryIntervalPow, failMostNum, cc)
        {

        }
        public override void Push()
        {
            if (this.Client.Mail != null && this.Client.Mail.Enabled)
            {
                if (this.Client.Mail.SendConfiguration != null && this.Client.Mail.SendConfiguration.Enabled)
                {
                    SendMail();
                }
                if (this.Client.Mail.ReceiveConfiguration != null && this.Client.Mail.ReceiveConfiguration.Enabled)
                {
                    ReceiveMail();
                }
            }
        }
        private void SendMail()
        {
            // 发送失败后特殊处理，每失败一次记录最后一次发送时间，然后失败重试时间从最后一次发送时间递增，算法如下：
            //（发送次数*mailConf.RestFailRetryBase*mailConf.FailRetryIntervalPow）

            //设置MailManager的当前数据库连接
            MailManager.Database = JIAOFENG.Practices.Database.DatabaseFactory.CreateDatabase(this.Client.Provider.ProviderType, this.Client.Provider.ConnectionString);

            List<OutMailMessage> list = MailManager.GetWaitingSendMails(this.FailRetryIntervalPow, this.FailMostNum);
            MailSmtpClient smtpClient = new MailSmtpClient(this.Client.Mail.SendConfiguration.SmtpServer, this.Client.Mail.SendConfiguration.MailAccount, this.Client.Mail.SendConfiguration.MailPassword);

            foreach (OutMailMessage message in list)
            {
                //Dictionary<string, Stream> dictemp = new Dictionary<string, Stream>();
                try
                {
                    smtpClient.SendMailMessage(string.IsNullOrWhiteSpace(message.Sender) ? this.Client.Mail.SendConfiguration.DefaultSender : message.Sender,
                    string.IsNullOrWhiteSpace(message.MailFrom) ? this.Client.Mail.SendConfiguration.MailAccount : message.Sender,
                    null,
                    string.IsNullOrWhiteSpace(message.MailTo) ? new string[] { } : message.MailTo.Split(';'),
                    string.IsNullOrWhiteSpace(message.MailCC) ? new string[] { } : message.MailCC.Split(';'),
                    string.IsNullOrWhiteSpace(message.MailBcc) ? new string[] { } : message.MailBcc.Split(';'),
                    message.Subject,
                    message.Body,
                    true,
                    message.Attachments);

                    //更新发送状态
                    MailManager.UpdataMailStatus(message.MailID, OutMailStatus.Success);
                }
                catch (Exception ex)
                {
                    //更新失败次数
                    MailManager.UpdateSendCount(message.MailID);

                    if (message.SendCount >= this.FailMostNum - 1)
                    {
                        //失败，根据this.Client.ConnectionString来修改
                        MailManager.UpdataMailStatus(message.MailID, OutMailStatus.Fail);
                    }

                    //应该根据rc.ConnectionString写Log。。todo
                    LogManager.LogError("邮件发送失败", ex.Message, message.Subject, message.MailTo, Constant.LogCategoryID, Constant.LogCategoryName);
                }
            }
        }

        private void ReceiveMail()
        {
            IMailReceive pop3Client = MailReceiveFactory.GetMailReceiver(this.Client.Mail.ReceiveConfiguration.ReceiveAssembly, this.Client.Mail.ReceiveConfiguration.ReceiveType);
            try
            {
                if (pop3Client.Connected)
                    pop3Client.Disconnect();
                pop3Client.Connect(this.Client.Mail.ReceiveConfiguration.Host, this.Client.Mail.ReceiveConfiguration.Port, this.Client.Mail.ReceiveConfiguration.RequireSSL);
                pop3Client.Authenticate(this.Client.Mail.ReceiveConfiguration.MailAccount, this.Client.Mail.ReceiveConfiguration.MailPassword);
                int count = pop3Client.GetMessageCount();
                int success = 0;
                int fail = 0;
                for (int i = count; i >= 1; i -= 1)
                {
                    try
                    {
                        Message message = pop3Client.GetMessage(i);
                        MailMessage mm = message.ToMailMessage();

                        //To-do 注意这里需要添加方法，把获取到的信息存储在this.Client.ConnectionString指向的Server中的库
                        InMailMessage mrdb = new InMailMessage();
                        mrdb.MailReceiver = this.Client.Mail.ReceiveConfiguration.MailAccount;
                        mrdb.MailSenderAddress = mm.From.ToString();
                        mrdb.MailTo = string.Join(";", mm.To.ToString().ToArray());
                        mrdb.MailCC = string.Join(";", mm.CC.ToString().ToArray());
                        mrdb.MailBCC = string.Join(";", mm.Bcc.ToString().ToArray());
                        mrdb.Subject = mm.Subject;
                        mrdb.Body = mm.Body;
                        if (mm.Attachments.Count() > 0)
                        {
                            foreach (Attachment att in mm.Attachments)
                            {
                                FileStorage fileStorage = new LocalDBFile();
                                fileStorage.FileCode = Guid.NewGuid().ToString();
                                fileStorage.FileName = att.Name;
                                fileStorage.ExtensionName = "";
                                fileStorage.Context = JIAOFENG.Practices.Library.Utility.NetStreamUtility.StreamToBytes(att.ContentStream);
                                fileStorage.SavePath = "";
                                fileStorage.FileType = "";

                                mrdb.Attachments.FileStorages.Add(fileStorage);
                            }
                        }
                        MailReceiveManager.AddReceivedMail(mrdb, this.Client.Provider.ProviderType, this.Client.Provider.ConnectionString);

                        success++;
                    }
                    catch (Exception e)
                    {
                        fail++;
                        LogManager.LogError("邮件新增失败", e.Message, this.Client.Mail.ReceiveConfiguration.MailAccount, this.Client.Mail.ReceiveConfiguration.MailAccount, Constant.LogCategoryID, Constant.LogCategoryName);
                    }
                }
                if (fail > 0)
                {
                    string exception = "Since some of the emails were not parsed correctly (exceptions were thrown)\r\n" +
                                     "please consider sending your log file to the developer for fixing.\r\n" +
                                     "If you are able to include any extra information, please do so.";
                    LogManager.LogError("邮件新增失败", exception, this.Client.Mail.ReceiveConfiguration.MailAccount, this.Client.Mail.ReceiveConfiguration.MailAccount, Constant.LogCategoryID, Constant.LogCategoryName);
                }
            }
            catch (InvalidLoginException ie)
            {
                //"The server did not accept the user credentials!", "POP3 Server Authentication"
                LogManager.LogError("邮件新增失败", ie.Message, this.Client.Mail.ReceiveConfiguration.MailAccount, this.Client.Mail.ReceiveConfiguration.MailAccount, Constant.LogCategoryID, Constant.LogCategoryName);
            }
            catch (PopServerNotFoundException pfe)
            {
                //"The server could not be found", "POP3 Retrieval"
                LogManager.LogError("邮件新增失败", pfe.Message, this.Client.Mail.ReceiveConfiguration.MailAccount, this.Client.Mail.ReceiveConfiguration.MailAccount, Constant.LogCategoryID, Constant.LogCategoryName);
            }
            catch (PopServerLockedException pse)
            {
                //MessageBox.Show(this, "The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?", "POP3 Account Locked"
                LogManager.LogError("邮件新增失败", pse.Message, this.Client.Mail.ReceiveConfiguration.MailAccount, this.Client.Mail.ReceiveConfiguration.MailAccount, Constant.LogCategoryID, Constant.LogCategoryName);
            }
            catch (LoginDelayException le)
            {
                //"Login not allowed. Server enforces delay between logins. Have you connected recently?", "POP3 Account Login Delay"
                LogManager.LogError("邮件新增失败", le.Message, this.Client.Mail.ReceiveConfiguration.MailAccount, this.Client.Mail.ReceiveConfiguration.MailAccount, Constant.LogCategoryID, Constant.LogCategoryName);
            }
            catch (Exception ex)
            {
                //"Error occurred retrieving mail. " + e.Message, "POP3 Retrieval"
                LogManager.LogError("邮件新增失败", ex.Message, this.Client.Mail.ReceiveConfiguration.MailAccount, this.Client.Mail.ReceiveConfiguration.MailAccount, Constant.LogCategoryID, Constant.LogCategoryName);
            }
            finally
            {
                pop3Client.Disconnect();
            }
        }
    }
}
