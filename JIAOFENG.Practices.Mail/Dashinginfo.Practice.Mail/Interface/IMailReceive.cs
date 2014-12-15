using System;
using JIAOFENG.Practices.Mail.Pop3;
namespace JIAOFENG.Practices.Mail
{
    public interface IMailReceive
    {
        bool ApopSupported { get; }
        void Authenticate(string username, string password);
        void Authenticate(string username, string password, AuthenticationMethod authenticationMethod);
        System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> Capabilities();
        void Connect(System.IO.Stream stream);
        void Connect(string hostname, int port, bool useSsl);
        void Connect(string hostname, int port, bool useSsl, int receiveTimeout, int sendTimeout, System.Net.Security.RemoteCertificateValidationCallback certificateValidator);
        bool Connected { get; }
        void DeleteAllMessages();
        void DeleteMessage(int messageNumber);
        void Disconnect();
        JIAOFENG.Practices.Mail.Pop3.Mime.Message GetMessage(int messageNumber);
        byte[] GetMessageAsBytes(int messageNumber);
        int GetMessageCount();
        JIAOFENG.Practices.Mail.Pop3.Mime.Header.MessageHeader GetMessageHeaders(int messageNumber);
        int GetMessageSize(int messageNumber);
        System.Collections.Generic.List<int> GetMessageSizes();
        string GetMessageUid(int messageNumber);
        System.Collections.Generic.List<string> GetMessageUids();
        void NoOperation();
        void Reset();
    }
}
