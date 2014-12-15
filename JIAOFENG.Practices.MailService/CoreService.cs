using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.Threading;
using Dashinginfo.Practices.Database;
using Dashinginfo.Practices.Logic.Log;
using Dashinginfo.Practices.Logic.Mail;
using Dashinginfo.Practices.Mail;
using Dashinginfo.Practices.Mail.Pop3.Exceptions;
using Dashinginfo.Practices.Mail.Pop3.Mime;
using System.Net.Mail;
using System.IO;
using Dashinginfo.Practices.Logic.Mail.Entity;
using Dashinginfo.Practices.Logic.File;

namespace DashingMailService
{
    public partial class CoreService : ServiceBase
    {
        Configuration mailConfig;
        System.Timers.Timer timer;
        public CoreService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                LogManager.LogDebug("PushService", "Try to start PushService...", "OnStart", "OnStart");

                mailConfig = Configuration.Load("Config.xml");

                timer = new System.Timers.Timer();
                timer.Interval = mailConfig.TimerInterval * 60 * 1000;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                timer.Start();

                LogManager.LogInformation("PushService", "PushService is running.", "OnStart", "OnStart");
            }
            catch (Exception ex)
            {
                LogManager.LogError("PushService", ex.Message, "OnStart", "OnStart", Constant.LogCategoryID, Constant.LogCategoryName);
                return;
            }        
        }
        public void Start(string[] args)
        {
            mailConfig = Configuration.Load("Config.xml");
            timer_Elapsed(null, null);
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //这里是总体逻辑
            //根据获取的配置信息循环读取数据库信息，及发送及收取邮件信息
            //然后根据SendReceive的值判断发送还是收取，还是两者都做或者两都都不做
            //然后针对每一个Account的信息，发送和接收相应的邮件到相应的数据库中
            //MailSendManager.WriteLog("timer_Elapsed", "进入成功!");
            foreach (ClientConfig client in mailConfig.Clients)
            {
                try
                {
                    LogManager.LogDebug("PushService", client.ToString(), "timer_Elapsed", "timer_Elapsed");
                    SendMail(client);
                }
                catch (Exception ex)
                {
                    //MailSendManager.WriteLog("ServerConf sc", "进入异常处理");
                    LogManager.LogError("PushService", ex.Message, "timer_Elapsed", "timer_Elapsed", Constant.LogCategoryID, Constant.LogCategoryName);//根据rc.ConnectionString写Log
                }
                #region 接受邮件 暂时去掉
                //try
                //{
                //    ReceiveMail(sc);
                //}
                //catch (Exception ex)
                //{
                //    LogManager.LogError("邮件获取失败", ex.Message, "邮件服务", "邮件服务", Configuration.LogCategoryID, Configuration.LogCategoryName);//根据rc.ConnectionString写Log
                //}
                #endregion
            }
        }

        private void SendMail(ClientConfig sc)
        {
            // 注意这里需要添加逻辑处理，处理如下
            // 发送失败后特殊处理，每失败一次记录最后一次发送时间，然后失败重试时间从最后一次发送时间递增，算法如下：
            //（发送次数*mailConf.RestFailRetryBase*mailConf.FailRetryIntervalPow）
            if (sc.Mail.SendConfiguration.Enabled)
            {
                MailManager.Database = Dashinginfo.Practices.Database.DatabaseFactory.CreateDatabase(sc.Provider.ProviderType, sc.Provider.ConnectionString);//设置MailManager的当前数据库连接
                List<OutMailMessage> list = MailManager.GetWaitingSendMails(mailConfig.FailRetryIntervalPow, mailConfig.FailMostNum);
                MailSmtpClient smtpClient = new MailSmtpClient(sc.Mail.SendConfiguration.SmtpServer, sc.Mail.SendConfiguration.MailAccount, sc.Mail.SendConfiguration.MailPassword);
                foreach (OutMailMessage message in list)
                {
                    Dictionary<string, Stream> dictemp = new Dictionary<string, Stream>();
                    try
                    {
                        smtpClient.SendMailMessage(string.IsNullOrWhiteSpace(message.Sender) ? sc.Mail.SendConfiguration.DefaultSender : message.Sender,
                        string.IsNullOrWhiteSpace(message.MailFrom) ? sc.Mail.SendConfiguration.MailAccount : message.Sender,
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

                        if (message.SendCount >= mailConfig.FailMostNum - 1)
                        {
                            //失败，根据rc.ConnectionString来修改
                            MailManager.UpdataMailStatus(message.MailID, OutMailStatus.Fail);
                        }
                        LogManager.LogError("邮件发送失败", ex.Message, message.Subject, message.MailTo, Constant.LogCategoryID, Constant.LogCategoryName);//根据rc.ConnectionString写Log
                    }
                }
            }
        }

        private void ReceiveMail(ClientConfig rc)
        {
            if (rc.Mail.ReceiveConfiguration.Enabled)
            {
                IMailReceive pop3Client = MailReceiveFactory.GetMailReceiver(rc.Mail.ReceiveConfiguration.ReceiveAssembly, rc.Mail.ReceiveConfiguration.ReceiveType);
                try
                {
                    if (pop3Client.Connected)
                        pop3Client.Disconnect();
                    pop3Client.Connect(rc.Mail.ReceiveConfiguration.Host, rc.Mail.ReceiveConfiguration.Port, rc.Mail.ReceiveConfiguration.RequireSSL);
                    pop3Client.Authenticate(rc.Mail.ReceiveConfiguration.MailAccount, rc.Mail.ReceiveConfiguration.MailPassword);
                    int count = pop3Client.GetMessageCount();
                    int success = 0;
                    int fail = 0;
                    for (int i = count; i >= 1; i -= 1)
                    {
                        try
                        {
                            Message message = pop3Client.GetMessage(i);
                            MailMessage mm = message.ToMailMessage();
                            
                            //To-do 注意这里需要添加方法，把获取到的信息存储在rc.ConnectionString指向的Server中的库
                            InMailMessage mrdb = new InMailMessage();
                            mrdb.MailReceiver = rc.Mail.ReceiveConfiguration.MailAccount;
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
                                    fileStorage.Context = Dashinginfo.Practices.Library.Utility.NetStreamUtility.StreamToBytes(att.ContentStream);
                                    fileStorage.SavePath = "";
                                    fileStorage.FileType = "";

                                    mrdb.Attachments.FileStorages.Add(fileStorage);
                                }
                            }
                            MailReceiveManager.AddReceivedMail(mrdb, rc.Provider.ProviderType, rc.Provider.ConnectionString);

                            success++;
                        }
                        catch (Exception e)
                        {
                            fail++;
                            LogManager.LogError("邮件新增失败", e.Message, rc.Mail.ReceiveConfiguration.MailAccount, rc.Mail.ReceiveConfiguration.MailAccount, Constant.LogCategoryID, Constant.LogCategoryName);
                        }
                    }
                    if (fail > 0)
                    {
                        string exception = "Since some of the emails were not parsed correctly (exceptions were thrown)\r\n" +
                                         "please consider sending your log file to the developer for fixing.\r\n" +
                                         "If you are able to include any extra information, please do so.";
                        LogManager.LogError("邮件新增失败", exception, rc.Mail.ReceiveConfiguration.MailAccount, rc.Mail.ReceiveConfiguration.MailAccount, Constant.LogCategoryID, Constant.LogCategoryName);
                    }
                }
                catch (InvalidLoginException ie)
                {
                    //"The server did not accept the user credentials!", "POP3 Server Authentication"
                    LogManager.LogError("邮件新增失败",ie.Message , rc.Mail.ReceiveConfiguration.MailAccount, rc.Mail.ReceiveConfiguration.MailAccount, Constant.LogCategoryID, Constant.LogCategoryName);
                }
                catch (PopServerNotFoundException pfe)
                {
                    //"The server could not be found", "POP3 Retrieval"
                    LogManager.LogError("邮件新增失败", pfe.Message, rc.Mail.ReceiveConfiguration.MailAccount, rc.Mail.ReceiveConfiguration.MailAccount, Constant.LogCategoryID, Constant.LogCategoryName);
                }
                catch (PopServerLockedException pse)
                {
                    //MessageBox.Show(this, "The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?", "POP3 Account Locked"
                    LogManager.LogError("邮件新增失败", pse.Message, rc.Mail.ReceiveConfiguration.MailAccount, rc.Mail.ReceiveConfiguration.MailAccount, Constant.LogCategoryID, Constant.LogCategoryName);
                }
                catch (LoginDelayException le)
                {
                    //"Login not allowed. Server enforces delay between logins. Have you connected recently?", "POP3 Account Login Delay"
                    LogManager.LogError("邮件新增失败", le.Message, rc.Mail.ReceiveConfiguration.MailAccount, rc.Mail.ReceiveConfiguration.MailAccount, Constant.LogCategoryID, Constant.LogCategoryName);
                }
                catch (Exception ex)
                {
                   //"Error occurred retrieving mail. " + e.Message, "POP3 Retrieval"
                    LogManager.LogError("邮件新增失败", ex.Message, rc.Mail.ReceiveConfiguration.MailAccount, rc.Mail.ReceiveConfiguration.MailAccount, Constant.LogCategoryID, Constant.LogCategoryName);
                }
                finally
                {
                    pop3Client.Disconnect();
                }

            }
        }

        protected override void OnStop()
        {
            this.timer.Stop();
            this.timer.Dispose();
        }
    }
}
