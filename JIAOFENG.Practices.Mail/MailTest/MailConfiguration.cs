using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using System.IO;

namespace MailTest.TestApplication
{
    public class MailConfiguration
    {
        public int TimerInterval { get; set; }
        public int FailMostNum { get; set; }
        public int FailRetryIntervalPow { get; set; }
        //public int RestFailRetryBase { get; set; }

        public List<ServerConf> ServerConfiguration { get; set; }
        private MailConfiguration()
        {
        }
        public static MailConfiguration Load(string configFileFullName)
        {
            //XElement xe = XElement.Load(GetConfigXMLPath()+"\\MailConfig.xml");
            MailConfiguration config = new MailConfiguration();
            XElement xe = XElement.Load(configFileFullName);
            config.TimerInterval = int.Parse(xe.Element("TimerInterval").Attribute("value").Value);
            config.FailMostNum = int.Parse(xe.Element("FailMostNum").Attribute("value").Value);
            config.FailRetryIntervalPow = int.Parse(xe.Element("FailRetryIntervalPow").Attribute("value").Value);
            //config.RestFailRetryBase = int.Parse(xe.Element("FailRetryInterval").Attribute("value").Value.Split(',')[1]);

            config.ServerConfiguration = new List<ServerConf>();

            foreach (XElement x in xe.Element("Clients").Elements("Client"))
            {
                ServerConf sc = new ServerConf();

                XElement provider = x.Element("Provider");
                sc.Provider = new Provider((ProviderType)Enum.Parse(typeof(ProviderType), provider.Attribute("providerType").Value), provider.Attribute("connectionString").Value);


                SendConf sendConf = new SendConf();
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
                sc.SendConfiguration = sendConf;

                ReceiveConf rc = new ReceiveConf();
                XElement receive = x.Element("Receive");
                rc.Enabled = bool.Parse(receive.Attribute("enabled").Value);
                rc.Host = receive.Element("MailHost").Attribute("value").Value;
                rc.Port = int.Parse(receive.Element("MailPort").Attribute("value").Value);
                rc.RequireSSL = bool.Parse(receive.Element("RequireSSL").Attribute("value").Value);
                rc.MailAccount = receive.Element("MailAccount").Attribute("value").Value;
                rc.MailPassword = receive.Element("MailPassword").Attribute("value").Value;
                rc.ReceiveType = receive.Element("MailReceive").Attribute("value").Value;
                rc.ReceiveAssembly = receive.Element("MailAssembly").Attribute("value").Value;
                sc.ReceiveConfiguration = rc;

                config.ServerConfiguration.Add(sc);
            }
            return config;
        }

        /// <summary>
        /// path
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static string GetConfigXMLPath()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            FileInfo fi = new FileInfo(path);
            return fi.Directory.ToString();
        }
    }

    public class ServerConf
    {
        public Provider Provider;

        public SendConf SendConfiguration;
        public ReceiveConf ReceiveConfiguration;
    }

    public class SendConf
    {
        public bool Enabled = false;


        public string SmtpServer;
        public string MailAccount;
        public string MailPassword;
        public string DefaultSender;

        public LogConf LogConfiguration;
    }

    public class ReceiveConf
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
