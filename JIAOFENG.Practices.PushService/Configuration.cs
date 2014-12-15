using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using JIAOFENG.Practices.Database;

namespace JIAOFENG.Practices.PushService
{
    public class Configuration
    {
        public int TimerInterval { get; set; }
        //<!--失败重试次数-->
        public int FailMostNum { get; set; }
        public int FailRetryIntervalPow { get; set; }

        public List<ClientConfig> Clients { get; set; }
        private Configuration() 
        { 
        }
        public static Configuration Load(string configFileName)
        { 
            Configuration config = new Configuration();

            string configFileFullName = Path.Combine(GetConfigXMLPath(), configFileName);
            XElement xe = XElement.Load(configFileFullName);
            config.TimerInterval = int.Parse(xe.Element("TimerInterval").Attribute("value").Value);
            config.FailMostNum = int.Parse(xe.Element("FailMostNum").Attribute("value").Value);
            config.FailRetryIntervalPow = int.Parse(xe.Element("FailRetryIntervalPow").Attribute("value").Value);

            config.Clients = new List<ClientConfig>();

            foreach (XElement x in xe.Element("Clients").Elements("Client"))
            {
                ClientConfig client = new ClientConfig();

                //Provider
                XElement provider = x.Element("Provider");
                client.Provider = new DatabaseProvider((DatabaseProviderType)Enum.Parse(typeof(DatabaseProviderType), provider.Attribute("providerType").Value), provider.Attribute("connectionString").Value);

                if (x.Element("Mail") != null)
                {
                    XElement mail = x.Element("Mail");
                    MailConfig mailConfig = new MailConfig();
                    client.Mail  = mailConfig;
                    client.Mail.Enabled = bool.Parse(mail.Attribute("enabled").Value);

                    //Send                    
                    XElement send = mail.Element("Send");
                    if (send != null)
                    {
                        SendConfiguration sendConf = new SendConfiguration();

                        sendConf.Enabled = bool.Parse(send.Attribute("enabled").Value);

                        sendConf.SmtpServer = send.Element("SmtpServer").Attribute("value").Value;
                        sendConf.MailAccount = send.Element("MailAccount").Attribute("value").Value;
                        sendConf.MailPassword = send.Element("MailPassword").Attribute("value").Value;
                        sendConf.DefaultSender = send.Element("DefaultSender").Attribute("value").Value;

                        mailConfig.SendConfiguration = sendConf;
                    }                   

                    //Receive
                    XElement receive = mail.Element("Receive");
                    if (receive != null)
                    {
                        ReceiveConfiguration rc = new ReceiveConfiguration();
                        rc.Enabled = bool.Parse(receive.Attribute("enabled").Value);
                        rc.Host = receive.Element("MailHost").Attribute("value").Value;
                        rc.Port = int.Parse(receive.Element("MailPort").Attribute("value").Value);
                        rc.RequireSSL = bool.Parse(receive.Element("RequireSSL").Attribute("value").Value);
                        rc.MailAccount = receive.Element("MailAccount").Attribute("value").Value;
                        rc.MailPassword = receive.Element("MailPassword").Attribute("value").Value;
                        rc.ReceiveType = receive.Element("MailReceive").Attribute("value").Value;
                        rc.ReceiveAssembly = receive.Element("MailAssembly").Attribute("value").Value;
                        mailConfig.ReceiveConfiguration = rc;
                    }
                }
                if (x.Element("WechatPush") != null)
                {
                    XElement wechat = x.Element("WechatPush");
                    WechatConfig wechatConfig = new WechatConfig();
                    client.Wechat = wechatConfig;
                    wechatConfig.Enabled = bool.Parse(wechat.Attribute("enabled").Value);
                    wechatConfig.TemplateMessageHost = wechat.Element("TemplateMessageHost").Attribute("value").Value;
                    wechatConfig.ClientAccessTokenUrl = wechat.Element("ClientAccessTokenUrl").Attribute("value").Value;
                    wechatConfig.WechatAppId = wechat.Element("WechatAppId").Attribute("value").Value;
                    wechatConfig.AppSecret = wechat.Element("AppSecret").Attribute("value").Value;
                }
                if (x.Element("AppPush") != null)
                {
                    XElement appPush = x.Element("AppPush");
                    AppPushConfig appConfig = new AppPushConfig();
                    client.AppPush = appConfig;
                    appConfig.Enabled = bool.Parse(appPush.Attribute("enabled").Value);

                    appConfig.MasterSecret = appPush.Element("MasterSecret").Attribute("value").Value;
                    appConfig.AppId = appPush.Element("AppId").Attribute("value").Value;
                    appConfig.AppKey = appPush.Element("AppKey").Attribute("value").Value;
                    appConfig.Host = appPush.Element("Host").Attribute("value").Value;
                }
                config.Clients.Add(client);
            }
            return config;
        }

        /// <summary>
        /// path
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private static string GetConfigXMLPath()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            FileInfo fi = new FileInfo(path);
            return fi.Directory.ToString();
        }
    }

    public class ClientConfig
    {
        public DatabaseProvider Provider;

        public MailConfig Mail;
        public WechatConfig Wechat;
        public AppPushConfig AppPush;
    }
    public class MailConfig
    {
        public bool Enabled = false; 

        public SendConfiguration SendConfiguration;
        public ReceiveConfiguration ReceiveConfiguration;
    }
    public class WechatConfig
    {
        public bool Enabled = false;

        public string TemplateMessageHost;
        public string ClientAccessTokenUrl;
        public string WechatAppId;
        public string AppSecret;
    }
    public class AppPushConfig
    {
        public bool Enabled = false;

        public string MasterSecret;
        public string Host;
        public string AppId;
        public string AppKey;
    }
    public class SendConfiguration
    {
        public bool Enabled = false;

        public string SmtpServer;
        public string MailAccount;
        public string MailPassword;
        public string DefaultSender;

    }

    public class ReceiveConfiguration
    {
        public bool Enabled = false;

        public string Host;
        public int Port;
        public bool RequireSSL;
        public string MailAccount;
        public string MailPassword;

        public string ReceiveType;
        public string ReceiveAssembly;
    }

    public class LogConf
    {
        public bool Enabled = false;
        public string Url;
    }
}
