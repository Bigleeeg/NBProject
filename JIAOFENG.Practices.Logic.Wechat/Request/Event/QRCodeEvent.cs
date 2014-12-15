using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JIAOFENG.Practices.Logic.Wechat.Request.Event
{
    public class QRCodeEvent : BaseEvent
    {
        public string EventKey { get; set; }
        public string Ticket { get; set; }
        public static QRCodeEvent Parse(XmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            XmlElement rootElement = doc.DocumentElement;
            QRCodeEvent messageEvent = new QRCodeEvent();
            BaseEvent.Parse(doc, messageEvent);

            messageEvent.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;
            messageEvent.Ticket = rootElement.SelectSingleNode("Ticket").InnerText;

            return messageEvent;
        }
    }
}
