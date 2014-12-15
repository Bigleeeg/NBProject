using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JIAOFENG.Practices.Mail.Pop3.Mime;
using JIAOFENG.Practices.Mail.Pop3.Mime.Header;
using JIAOFENG.Practices.Mail;
using JIAOFENG.Practices.Mail.Pop3;

namespace JIAOFENG.Practices.Mail
{
    public class IMAPReceive : MailReceive
    {
        public override void Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public override void Authenticate(string username, string password, AuthenticationMethod authenticationMethod)
        {
            throw new NotImplementedException();
        }

        public override void Connect(System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }

        public override void Connect(string hostname, int port, bool useSsl)
        {
            throw new NotImplementedException();
        }

        public override void Connect(string hostname, int port, bool useSsl, int receiveTimeout, int sendTimeout, System.Net.Security.RemoteCertificateValidationCallback certificateValidator)
        {
            throw new NotImplementedException();
        }

        public override void DeleteAllMessages()
        {
            throw new NotImplementedException();
        }

        public override void DeleteMessage(int messageNumber)
        {
            throw new NotImplementedException();
        }

        public override void Disconnect()
        {
            throw new NotImplementedException();
        }

        public override Message GetMessage(int messageNumber)
        {
            throw new NotImplementedException();
        }

        public override byte[] GetMessageAsBytes(int messageNumber)
        {
            throw new NotImplementedException();
        }

        public override int GetMessageCount()
        {
            throw new NotImplementedException();
        }

        public override MessageHeader GetMessageHeaders(int messageNumber)
        {
            throw new NotImplementedException();
        }

        public override int GetMessageSize(int messageNumber)
        {
            throw new NotImplementedException();
        }

        public override List<int> GetMessageSizes()
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, List<string>> Capabilities()
        {
            throw new NotImplementedException();
        }

        public override string GetMessageUid(int messageNumber)
        {
            throw new NotImplementedException();
        }

        public override List<string> GetMessageUids()
        {
            throw new NotImplementedException();
        }

        public override void NoOperation()
        {
            throw new NotImplementedException();
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
