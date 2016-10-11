using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using Dashinginfo.Practices.Database;

namespace DashingMailService
{
    public class MailConfiguration
    {
        public int TimerInterval { get; set; }
        //<!--失败重试次数-->
        public int FailMostNum { get; set; }
        public int FailRetryIntervalPow { get; set; }

        public List<ClientConfig> Clients { get; set; }
        private MailConfiguration() 
        { 
        }
        public static MailConfiguration Load(string configFileName)
        { 
            MailConfiguration config = new MailConfiguration();

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

                //Send
                SendConfiguration sendConf = new SendConfiguration();
                XElement send = x.Element("Send");
                sendConf.Enabled = bool.Parse(send.Attribute("enabled").Value);

                XElement log = send.Element("Loging");
                LogConf lc = new LogConf();
                if (log != null)
                {
                    lc.Enabled = bool.Parse(log.Attribute("enabled").Value);
                    lc.Url = log.Attribute("url").Value; 
                }
                sendConf.LogConfiguration = lc;

                sendConf.SmtpServer = send.Element("SmtpServer").Attribute("value").Value;
                sendConf.MailAccount = send.Element("MailAccount").Attribute("value").Value;
                sendConf.MailPassword = send.Element("MailPassword").Attribute("value").Value;
                sendConf.DefaultSender = send.Element("DefaultSender").Attribute("value").Value;

                client.SendConfiguration = sendConf;

                //Receive
                XElement receive = x.Element("Receive");
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
                    client.ReceiveConfiguration = rc;
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

        public SendConfiguration SendConfiguration;
        public ReceiveConfiguration ReceiveConfiguration;
    }

    public class SendConfiguration
    {
        public bool Enabled = false;

        public string SmtpServer;
        public string MailAccount;
        public string MailPassword;
        public string DefaultSender;

        public LogConf LogConfiguration;
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
