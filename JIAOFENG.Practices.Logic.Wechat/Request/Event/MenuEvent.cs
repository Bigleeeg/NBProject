using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JIAOFENG.Practices.Logic.Wechat.Request.Event
{
    public class MenuEvent : BaseEvent
    {
        public string EventKey { get; set; }
        public static MenuEvent Parse(XmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            XmlElement rootElement = doc.DocumentElement;
            MenuEvent messageEvent = new MenuEvent();
            BaseEvent.Parse(doc, messageEvent);

            messageEvent.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;

            return messageEvent;
        }
    }
}
