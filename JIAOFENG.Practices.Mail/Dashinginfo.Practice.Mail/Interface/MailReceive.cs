using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Security;
using JIAOFENG.Practices.Mail.Pop3.Mime;
using JIAOFENG.Practices.Mail.Pop3;
using JIAOFENG.Practices.Mail.Pop3.Mime.Header;

namespace JIAOFENG.Practices.Mail
{
    public abstract class MailReceive : JIAOFENG.Practices.Mail.IMailReceive
    {
        public bool Connected { get; set; }
        public bool ApopSupported { get; set; }

        public abstract Dictionary<string, List<string>> Capabilities();
        public abstract string GetMessageUid(int messageNumber);
        public abstract List<string> GetMessageUids();
        public abstract void NoOperation();
        public abstract void Reset();
        public abstract void Authenticate(string username, string password);
        public abstract void Authenticate(string username, string password, AuthenticationMethod authenticationMethod);
        public abstract void Connect(System.IO.Stream stream);
        public abstract void Connect(string hostname, int port, bool useSsl);
        public abstract void Connect(string hostname, int port, bool useSsl, int receiveTimeout, int sendTimeout, RemoteCertificateValidationCallback certificateValidator);
        public abstract void DeleteAllMessages();
        public abstract void DeleteMessage(int messageNumber);
        public abstract void Disconnect();
        public abstract Message GetMessage(int messageNumber);
        public abstract byte[] GetMessageAsBytes(int messageNumber);
        public abstract int GetMessageCount();
        public abstract MessageHeader GetMessageHeaders(int messageNumber);
        public abstract int GetMessageSize(int messageNumber);
        public abstract List<int> GetMessageSizes();
    }
}
