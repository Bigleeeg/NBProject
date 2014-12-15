using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JIAOFENG.Practices.Logic.Wechat.Request.Event
{
    public class LocationEvent : BaseEvent
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Precision { get; set; }
        public static LocationEvent Parse(XmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            XmlElement rootElement = doc.DocumentElement;
            LocationEvent messageEvent = new LocationEvent();
            BaseEvent.Parse(doc, messageEvent);

            messageEvent.Latitude = rootElement.SelectSingleNode("PicUrl").InnerText;
            messageEvent.Longitude = rootElement.SelectSingleNode("Longitude").InnerText;
            messageEvent.Precision = rootElement.SelectSingleNode("Precision").InnerText;

            return messageEvent;
        }
    }
}
