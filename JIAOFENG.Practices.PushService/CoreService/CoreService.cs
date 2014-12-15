using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using JIAOFENG.Practices.Logic.Log;
using JIAOFENG.Practices.Logic.Mail;
using JIAOFENG.Practices.Mail;
using JIAOFENG.Practices.Mail.Pop3.Exceptions;
using JIAOFENG.Practices.Mail.Pop3.Mime;
using System.Net.Mail;
using System.IO;
using JIAOFENG.Practices.Logic.File;

namespace JIAOFENG.Practices.PushService
{
    public partial class CoreService : ServiceBase
    {
        Configuration pushConfig;
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

                pushConfig = Configuration.Load("Config.xml");

                timer = new System.Timers.Timer();
                timer.Interval = pushConfig.TimerInterval * 60 * 1000;
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
            pushConfig = Configuration.Load("Config.xml");
            timer_Elapsed(null, null);
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //这里是总体逻辑
            //根据获取的配置信息循环读取数据库信息，及发送及收取邮件信息
            //然后根据SendReceive的值判断发送还是收取，还是两者都做或者两都都不做
            //然后针对每一个Account的信息，发送和接收相应的邮件到相应的数据库中
            //MailSendManager.WriteLog("timer_Elapsed", "进入成功!");
            foreach (ClientConfig client in pushConfig.Clients)
            {
                try
                {
                    LogManager.LogDebug("PushService", client.ToString(), "timer_Elapsed", "timer_Elapsed");
                    //手机App
                    if (client.AppPush != null && client.AppPush.Enabled)
                    {
                        IPushService service = new AppPushService(pushConfig.FailRetryIntervalPow, pushConfig.FailMostNum, client);
                        service.Push();
                    }
                    //微信
                    if (client.Wechat != null && client.Wechat.Enabled)
                    {
                        IPushService service = new WechatPushService(pushConfig.FailRetryIntervalPow, pushConfig.FailMostNum, client);
                        service.Push();
                    }                   
                    //邮件
                    if (client.Mail != null && client.Mail.Enabled)
                    {
                        IPushService service = new MailService(pushConfig.FailRetryIntervalPow, pushConfig.FailMostNum, client);
                        service.Push();
                    }
                }
                catch (Exception ex)
                {
                    //MailSendManager.WriteLog("ServerConf sc", "进入异常处理");
                    LogManager.LogError("PushService", ex.Message, "timer_Elapsed", "timer_Elapsed", Constant.LogCategoryID, Constant.LogCategoryName);//根据rc.ConnectionString写Log
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
