using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JIAOFENG.Practices.Logic.Wechat.Request.Event
{
    public class SubscribeEvent : BaseEvent
    {
        public static SubscribeEvent Parse(XmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            XmlElement rootElement = doc.DocumentElement;
            SubscribeEvent messageEvent = new SubscribeEvent();
            BaseEvent.Parse(doc, messageEvent);

            return messageEvent;
        }
    }
}
