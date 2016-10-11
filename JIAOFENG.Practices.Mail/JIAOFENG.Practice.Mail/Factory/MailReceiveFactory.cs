using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace JIAOFENG.Practices.Mail
{
    public class MailReceiveFactory
    {
        public static IMailReceive GetMailReceiver(string assembly, string receiver)
        {
            Assembly mail = Assembly.Load(assembly);
            return (IMailReceive)mail.CreateInstance(receiver);
        }
    }
}
