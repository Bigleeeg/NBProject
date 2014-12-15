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
using Dashinginfo.Practices.Mail;
using Dashinginfo.Practices.Mail.Pop3.Exceptions;
using Dashinginfo.Practices.Mail.Pop3.Mime;
using System.Net.Mail;
using System.IO;

namespace DashingMailService
{
    public partial class MailService : ServiceBase
    {
        MailConfiguration mailConfig;
        System.Timers.Timer timer;
        public MailService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                mailConfig = MailConfiguration.Load("MailConfig.xml");

                timer = new System.Timers.Timer();
                timer.Interval = mailConfig.TimerInterval * 60 * 1000;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                timer.Start();
            }
            catch (Exception ex)
            {
                //to do日志
                //LogManager.LogError("邮件参数获取失败", ex.Message, "邮件服务", "邮件服务", Configuration.LogCategoryID, Configuration.LogCategoryName);
                return;
            }
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //这里是总体逻辑
            //根据获取的配置信息循环读取数据库信息，及发送及收取邮件信息
            //然后根据SendReceive的值判断发送还是收取，还是两者都做或者两都都不做
            //然后针对每一个Account的信息，发送和接收相应的邮件到相应的数据库中
            //OutMailDB.WriteLog("timer_Elapsed", "进入成功!");
            foreach (ClientConfig client in mailConfig.Clients)
            {
                try
                {
                    //OutMailDB.WriteLog("ServerConf sc", "目前只连接了一个数据库");
                    SendMail(client);
                    //OutMailDB.WriteLog("ServerConf sc", "一分钟的时间间隔，执行一次完毕");
                }
                catch (Exception ex)
                {
                    //OutMailDB.WriteLog("ServerConf sc", "进入异常处理");
                    LogManager.LogError("邮件发送失败", ex.Message, "邮件服务", "邮件服务", Configuration.LogCategoryID, Configuration.LogCategoryName);//根据rc.ConnectionString写Log
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
            if (sc.SendConfiguration.Enabled)
            {
                MailSmtpClient msc = new MailSmtpClient(sc.SendConfiguration.SmtpServer, sc.SendConfiguration.MailAccount, sc.SendConfiguration.MailPassword);
                DataSet ds = OutMailDB.GetMailByStatus(OutMailStatus.UnSend, mailConfig.FailRetryIntervalPow, mailConfig.FailMostNum, sc.Provider.ConnectionString);//根据当前的数据库链接  查看未发送邮件的信息
                List<string> senderIDs = OutMailDB.GetSenderIDsByDataTable(ds.Tables[0]);
                if (sc.SendConfiguration.LogConfiguration.Enabled)
                {
                    OutMailDB.WriteLog("SendMail", "进入SendMail,要发送" + ds.Tables[0].Rows.Count + "封邮件", sc.SendConfiguration.LogConfiguration.Url);
                    OutMailDB.WriteLog("SendMail", "要发送邮件SendMailIDs:" + string.Join(",", senderIDs), sc.SendConfiguration.LogConfiguration.Url);
                    OutMailDB.WriteLog("SendMail", "数据库连接串为:" + sc.Provider.ConnectionString, sc.SendConfiguration.LogConfiguration.Url);
                }
                foreach (string strID in senderIDs)
                {
                    DataRow[] drs = ds.Tables[0].Select("MailID=" + strID);
                    Dictionary<string, Stream> dictemp = new Dictionary<string, Stream>();
                    try
                    {
                        if(sc.SendConfiguration.LogConfiguration.Enabled)
                            OutMailDB.WriteLog("SendMail", "要发送邮件SendMailID:" + strID + ";ToUser:" + string.Join(",", drs[0]["To"].ToString().Split(';')), sc.SendConfiguration.LogConfiguration.Url);
                        string[] mailToAddress = null;
                        string[] mailCC = null;
                        string mailSender = string.Empty;
                        string mailFrom = string.Empty;
                        if (!string.IsNullOrEmpty(drs[0]["To"].ToString()))
                        {
                            mailToAddress = drs[0]["To"].ToString().Split(';');
                        }
                        else
                        {
                            mailToAddress = new string[] { };
                        }
                        if (!string.IsNullOrEmpty(drs[0]["CC"].ToString()))
                        {
                            mailCC = drs[0]["CC"].ToString().Split(';');
                        }
                        else
                        {
                            mailCC = new string[] { };
                        }
                        if (!string.IsNullOrEmpty(drs[0]["Sender"].ToString()))
                        {
                            mailSender = drs[0]["Sender"].ToString();
                        }
                        else
                        {
                            mailSender = sc.SendConfiguration.DefaultSender;
                        }
                        if (!string.IsNullOrEmpty(drs[0]["From"].ToString()))
                        {
                            mailFrom = drs[0]["From"].ToString();
                        }
                        else
                        {
                            mailFrom = sc.SendConfiguration.MailAccount;
                        }
                        msc.SendMailMessage(mailSender,
                        mailFrom,
                        null,
                        mailToAddress,
                        mailCC,
                        null,
                        drs[0]["Subject"].ToString(),
                        drs[0]["Body"].ToString(),
                        OutMailDB.GetAttachMents(drs),
                        true);
                        if (sc.SendConfiguration.LogConfiguration.Enabled)
                            OutMailDB.WriteLog("SendMailMessageOnTime", "this.SmtpClient.Send Success", sc.SendConfiguration.LogConfiguration.Url);
                        //成功，根据rc.ConnectionString来修改
                        OutMailDB.UpdataMailStatus((int)drs[0]["MailID"], OutMailStatus.Success, sc.Provider.ConnectionString);
                    }
                    catch (Exception ex)
                    {
                        if (sc.SendConfiguration.LogConfiguration.Enabled)
                            OutMailDB.WriteLog("SendMail#####Exception", "发送给:" + drs[0]["To"].ToString() + "发送邮件SendMailID:" + strID + ";Ex:" + ex.Message, sc.SendConfiguration.LogConfiguration.Url);
                        if (int.Parse(drs[0]["SendCount"].ToString()) >= mailConfig.FailMostNum - 1)
                        {
                            //失败，根据rc.ConnectionString来修改
                            OutMailDB.UpdataMailStatus((int)drs[0]["MailID"], OutMailStatus.Fail, sc.Provider.ConnectionString);
                            //根据rc.ConnectionString写Log
                        }
                        //失败 根据rc.ConnectionString来修改
                        OutMailDB.UpdateSendCount((int)drs[0]["MailID"], sc.Provider.ConnectionString);
                    }
                }
            }
        }

        private void ReceiveMail(ClientConfig rc)
        {
            if (rc.ReceiveConfiguration.Enabled)
            {
                IMailReceive pop3Client = MailReceiveFactory.GetMailReceiver(rc.ReceiveConfiguration.ReceiveAssembly, rc.ReceiveConfiguration.ReceiveType);
                try
                {
                    if (pop3Client.Connected)
                        pop3Client.Disconnect();
                    pop3Client.Connect(rc.ReceiveConfiguration.Host, rc.ReceiveConfiguration.Port, rc.ReceiveConfiguration.RequireSSL);
                    pop3Client.Authenticate(rc.ReceiveConfiguration.MailAccount, rc.ReceiveConfiguration.MailPassword);
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
                            MailReceiveDB mrdb = new MailReceiveDB();
                            mrdb.MailReceiver = rc.ReceiveConfiguration.MailAccount;
                            mrdb.MailSenderAddress = mm.From.ToString();
                            mrdb.MailToAddress = string.Join(";", mm.To.ToString().ToArray());
                            mrdb.MailCC = string.Join(";", mm.CC.ToString().ToArray());
                            mrdb.MailBCC = string.Join(";", mm.Bcc.ToString().ToArray());
                            mrdb.MailSubject = mm.Subject;
                            mrdb.MailContent = mm.Body;
                            mrdb.MailAttachment = new Dictionary<string, byte[]>();
                            if (mm.Attachments.Count() > 0)
                            {
                                foreach (Attachment att in mm.Attachments)
                                {
                                    mrdb.MailAttachment.Add(att.Name, MailReceiveDB.StreamToBytes(att.ContentStream));
                                }
                            }
                            MailReceiveDB.AddReceiveMailItem(mrdb);
                            //end
                            success++;
                        }
                        catch (Exception e)
                        {
                            fail++;
                            LogManager.LogError("邮件新增失败", e.Message, rc.ReceiveConfiguration.MailAccount, rc.ReceiveConfiguration.MailAccount, Configuration.LogCategoryID, Configuration.LogCategoryName);
                        }
                    }
                    if (fail > 0)
                    {
                        string exception = "Since some of the emails were not parsed correctly (exceptions were thrown)\r\n" +
                                         "please consider sending your log file to the developer for fixing.\r\n" +
                                         "If you are able to include any extra information, please do so.";
                        LogManager.LogError("邮件新增失败", exception, rc.ReceiveConfiguration.MailAccount, rc.ReceiveConfiguration.MailAccount, Configuration.LogCategoryID, Configuration.LogCategoryName);
                    }
                }
                catch (InvalidLoginException ie)
                {
                    //"The server did not accept the user credentials!", "POP3 Server Authentication"
                    LogManager.LogError("邮件新增失败",ie.Message , rc.ReceiveConfiguration.MailAccount, rc.ReceiveConfiguration.MailAccount, Configuration.LogCategoryID, Configuration.LogCategoryName);
                }
                catch (PopServerNotFoundException pfe)
                {
                    //"The server could not be found", "POP3 Retrieval"
                    LogManager.LogError("邮件新增失败", pfe.Message, rc.ReceiveConfiguration.MailAccount, rc.ReceiveConfiguration.MailAccount, Configuration.LogCategoryID, Configuration.LogCategoryName);
                }
                catch (PopServerLockedException pse)
                {
                    //MessageBox.Show(this, "The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?", "POP3 Account Locked"
                    LogManager.LogError("邮件新增失败", pse.Message, rc.ReceiveConfiguration.MailAccount, rc.ReceiveConfiguration.MailAccount, Configuration.LogCategoryID, Configuration.LogCategoryName);
                }
                catch (LoginDelayException le)
                {
                    //"Login not allowed. Server enforces delay between logins. Have you connected recently?", "POP3 Account Login Delay"
                    LogManager.LogError("邮件新增失败", le.Message, rc.ReceiveConfiguration.MailAccount, rc.ReceiveConfiguration.MailAccount, Configuration.LogCategoryID, Configuration.LogCategoryName);
                }
                catch (Exception ex)
                {
                   //"Error occurred retrieving mail. " + e.Message, "POP3 Retrieval"
                    LogManager.LogError("邮件新增失败", ex.Message, rc.ReceiveConfiguration.MailAccount, rc.ReceiveConfiguration.MailAccount, Configuration.LogCategoryID, Configuration.LogCategoryName);
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
